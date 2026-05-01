using RentEasyAPI.Models;
using RentEasyAPI.Responses;
using RentEasyAPI.DTOs;

namespace RentEasyAPI.Services
{
    public interface IAuthService
    {
        Task<int?> Register(UserRegisterDto request);
        Task<TokenResponse?> Login(string email, string password);
        Task<bool> UserExists(string email);
        Task<TokenResponse?> RefreshTokens(User user);
    }
}
