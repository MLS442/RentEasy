<<<<<<< HEAD
﻿using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RentEasyAPI.Data;
using RentEasyAPI.DTOs;
=======
﻿using Azure.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RentEasyAPI.Data;
>>>>>>> dd70261a832a6b9debdf33cc0cc3bf7464cfcc16
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
<<<<<<< HEAD
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
=======
        public AuthService(RentEasyContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<int?> Register(Landlord landlord, string password)
        {
            if (await UserExists(landlord.Email))
>>>>>>> dd70261a832a6b9debdf33cc0cc3bf7464cfcc16
            {
                return null;
            }

<<<<<<< HEAD
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
=======
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
>>>>>>> dd70261a832a6b9debdf33cc0cc3bf7464cfcc16
            {
                return null;
            }
            else
            {
<<<<<<< HEAD
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
=======
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
>>>>>>> dd70261a832a6b9debdf33cc0cc3bf7464cfcc16
            {
                return null;
            }

<<<<<<< HEAD
            return user;
=======
            return landlord;
>>>>>>> dd70261a832a6b9debdf33cc0cc3bf7464cfcc16
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

<<<<<<< HEAD
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
=======
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
>>>>>>> dd70261a832a6b9debdf33cc0cc3bf7464cfcc16
            {
                return true;
            }

            return false;
        }

<<<<<<< HEAD
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
=======
        private string CreateToken(Landlord landlord)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, landlord.Email),
                new Claim(ClaimTypes.NameIdentifier, landlord.LandlordId.ToString()),
                new Claim(ClaimTypes.Name, landlord.FullName),
                new Claim(ClaimTypes.Role, landlord.Role)
>>>>>>> dd70261a832a6b9debdf33cc0cc3bf7464cfcc16
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
<<<<<<< HEAD
            );
=======
            ); 
>>>>>>> dd70261a832a6b9debdf33cc0cc3bf7464cfcc16

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }
}
