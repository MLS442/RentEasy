using RentEasyAPI.Models;
using RentEasyAPI.Responses;
<<<<<<< HEAD
using RentEasyAPI.DTOs;
=======
>>>>>>> dd70261a832a6b9debdf33cc0cc3bf7464cfcc16

namespace RentEasyAPI.Services
{
    public interface IAuthService
    {
<<<<<<< HEAD
        Task<int?> Register(UserRegisterDto request);
        Task<TokenResponse?> Login(UserLoginDto request);
        Task<bool> UserExists(string email);
        Task<TokenResponse?> RefreshTokens(User user);
=======
        Task<int?> Register(Landlord landlord, string password);
        Task<TokenResponse?> Login(string email, string password);
        Task<bool> UserExists(string email);
        Task<TokenResponse?> RefreshTokens(Landlord landlord);
>>>>>>> dd70261a832a6b9debdf33cc0cc3bf7464cfcc16
    }
}
