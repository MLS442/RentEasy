using RentEasyAPI.Models;
using RentEasyAPI.Responses;

namespace RentEasyAPI.Services
{
    public interface IAuthService
    {
        Task<int?> Register(User user, string password);
        Task<TokenResponse?> Login(string email, string password);
        Task<bool> UserExists(string email);
        Task<TokenResponse?> RefreshTokens(User user);
    }
}
