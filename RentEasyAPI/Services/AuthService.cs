using Azure.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RentEasyAPI.Data;
using RentEasyAPI.Models;
using RentEasyAPI.Responses;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace RentEasyAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly RentEasyContext _context;
        private readonly IConfiguration _configuration;
        public AuthService(RentEasyContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<int?> Register(User user, string password)
        {
            if (await UserExists(user.Email))
            {
                return null;
            }

            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
    
            if(user.Role != "Landlord" && user.Role != "Tenant")
            {
                return null;
            }

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user.UserId;
        }

        public async Task<TokenResponse?> Login(string email, string password)
        {
            User user = await _context.Users.Include(u => u.Landlord).Include(u => u.Tenant)
                .FirstOrDefaultAsync(l => l.Email.ToLower().Equals(email.ToLower()));

            if(user == null)
            {
                return null;
            }
            else if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                return null;
            }
            else
            {
                return await CreateTokenResponse(user);
            }
        }

        private async Task<TokenResponse> CreateTokenResponse(User user)
        {
            return new TokenResponse
            {
                AccessToken = CreateToken(user),
                RefreshToken = await GenerateAndSaveRefreshToken(user)
            };
        }

        public async Task<TokenResponse?> RefreshTokens(User user)
        {
            user = await ValidateRefreshToken(user.UserId, user.RefreshToken);
            if (user is null)
                return null;

            return await CreateTokenResponse(user);
        }
        private async Task<User?> ValidateRefreshToken(int userId, string refreshToken)
        {
            var user = await _context.Users.Include(u => u.Landlord).Include(u => u.Tenant).FirstOrDefaultAsync(u => u.UserId ==userId);

            if (user is null || user.RefreshToken != refreshToken 
                ||user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return null;
            }

            return user;
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private async Task<string> GenerateAndSaveRefreshToken(User user)
        {
            var refreshToken = GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _context.SaveChangesAsync();
            return refreshToken;
        }



        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public async Task<bool> UserExists(string email)
        {
            if(await _context.Users.AnyAsync(u => u.Email.ToLower() == email.ToLower()))
            {
                return true;
            }

            return false;
        }

        private string CreateToken(User user)
        {
            string userName;

            if (user.Role == "Landlord")
            {
                userName = user.Landlord.FullName;
            }
            else
            {
                userName = user.Tenant.FullName;
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration.GetValue<string>("AppSettings:Token")!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: _configuration.GetValue<string>("AppSettings:Issuer"),
                audience: _configuration.GetValue<string>("AppSettings:Audience"),
                claims: claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }
}
