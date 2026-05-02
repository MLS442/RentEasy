using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentEasyAPI.DTOs;
using RentEasyAPI.Models;
using RentEasyAPI.Responses;
using RentEasyAPI.Services;

namespace RentEasyAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController (IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<int?>> Register(UserRegisterDto request)
        {
            var result = await _authService.Register(request);

            if (result == null)
            {
                return BadRequest("User already exists");
            }

            return Ok(result);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<TokenResponse?>> Login(UserLoginDto request)
        {
            var loginRequest = await _authService.Login(request.Email, request.Password);

            if(loginRequest == null)
            {
                return BadRequest("Email or Password is wrong");
            }

            return Ok(loginRequest);
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<TokenResponse>> RefreshToken(UserRefreshTokenRequestDto request)
        {
            var tokenRefresh = await _authService.RefreshTokens(
                new User { UserId = request.UserId, RefreshToken = request.RefreshToken});
            if (tokenRefresh is null || tokenRefresh.AccessToken is null || tokenRefresh.RefreshToken is null)
                return Unauthorized("Invalid refresh token.");

            return Ok(tokenRefresh);
        }

        [Authorize]
        [HttpGet]
        public IActionResult AuthenticatedOnlyEndpoint()
        {
            return Ok("You are authenticated!");
        }

        [Authorize(Roles ="Landlord")]
        [HttpGet("landlord-only")]
        public IActionResult LandlordOnlyEndpoint()
        {
            return Ok("You are a landlord!");
        }
    }
}
