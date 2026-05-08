using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RentEasyAPI.Data;
using RentEasyAPI.DTOs;
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
        private readonly IMapper _mapper;
        public AuthService(RentEasyContext context, IConfiguration configuration, IMapper mapper)
        {
            _context = context;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<int?> Register(UserRegisterDto request)
        {
            if (request.Role != "Landlord" && request.Role != "Tenant")
            {
                return null;
            }

            var user = _mapper.Map<User>(request);

            if(request.Role == "Landlord")
            {
                var landlord =_mapper.Map<Landlord>(request);
                user.Landlord = landlord;
            }
            else if(request.Role == "Tenant")
            {
                var tenant = _mapper.Map<Tenant>(request);
                user.Tenant = tenant;
            }

            if (await UserExists(request.Email))
            {
                return null;
            }

            var hashedPassword = new PasswordHasher<User>()
                .HashPassword(user, request.Password);

            user.PasswordHash = hashedPassword;
   
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user.UserId;
        }

        public async Task<TokenResponse?> Login(UserLoginDto request)
        {
            User user = await _context.Users.Include(u => u.Landlord).Include(u => u.Tenant)
                .FirstOrDefaultAsync(l => l.Email.ToLower().Equals(request.Email.ToLower()));

            if(user == null)
            {
                return null;
            }
            else if (new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, request.Password)
                == PasswordVerificationResult.Failed)
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

        public async Task<TokenResponse?> RefreshTokens(UserRefreshTokenRequestDto request)
        {
            var user = await ValidateRefreshToken(request.UserId, request.RefreshToken);
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