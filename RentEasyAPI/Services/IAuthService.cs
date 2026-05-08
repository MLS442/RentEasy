using RentEasyAPI.Models;
using RentEasyAPI.Responses;
using RentEasyAPI.DTOs;


namespace RentEasyAPI.Services
{
    public interface IAuthService
    {
        Task<int?> Register(UserRegisterDto request);
        Task<TokenResponse?> Login(UserLoginDto request);
        Task<bool> UserExists(string email);
        Task<TokenResponse?> RefreshTokens(UserRefreshTokenRequestDto request);
    }
}
