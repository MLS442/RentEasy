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

        public async Task<int?> Register(Landlord landlord, string password)
        {
            if (await UserExists(landlord.Email))
            {
                return null;
            }

            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            landlord.PasswordHash = passwordHash;
            landlord.PasswordSalt = passwordSalt;
            landlord.Role = "Landlord";

            await _context.Landlords.AddAsync(landlord);
            await _context.SaveChangesAsync();

            return landlord.LandlordId;
        }

        public async Task<TokenResponse?> Login(string email, string password)
        {
            Landlord landlord = await _context.Landlords
                .FirstOrDefaultAsync(l => l.Email.ToLower().Equals(email.ToLower()));

            if(landlord == null)
            {
                return null;
            }
            else if (!VerifyPasswordHash(password, landlord.PasswordHash, landlord.PasswordSalt))
            {
                return null;
            }
            else
            {
                return await CreateTokenResponse(landlord);
            }
        }

        private async Task<TokenResponse> CreateTokenResponse(Landlord landlord)
        {
            return new TokenResponse
            {
                AccessToken = CreateToken(landlord),
                RefreshToken = await GenerateAndSaveRefreshToken(landlord)
            };
        }

        public async Task<TokenResponse?> RefreshTokens(Landlord landlord)
        {
            landlord = await ValidateRefreshToken(landlord.LandlordId, landlord.RefreshToken);
            if (landlord is null)
                return null;

            return await CreateTokenResponse(landlord);
        }
        private async Task<Landlord?> ValidateRefreshToken(int landlordId, string refreshToken)
        {
            var landlord = await _context.Landlords.FindAsync(landlordId);

            if (landlord is null || landlord.RefreshToken != refreshToken 
                ||landlord.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return null;
            }

            return landlord;
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private async Task<string> GenerateAndSaveRefreshToken(Landlord landlord)
        {
            var refreshToken = GenerateRefreshToken();
            landlord.RefreshToken = refreshToken;
            landlord.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
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
            if(await _context.Landlords.AnyAsync(l => l.Email.ToLower() == email.ToLower()))
            {
                return true;
            }

            return false;
        }

        private string CreateToken(Landlord landlord)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, landlord.Email),
                new Claim(ClaimTypes.NameIdentifier, landlord.LandlordId.ToString()),
                new Claim(ClaimTypes.Name, landlord.FullName),
                new Claim(ClaimTypes.Role, landlord.Role)
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
