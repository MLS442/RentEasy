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
        public async Task<ActionResult<int?>> Register(LandlordRegisterDto request)
        {
            var newUser = await _authService.Register(
                new Landlord { Email = request.Email, FullName = request.FullName }, request.Password);  

            if (newUser == null)
            {
                return BadRequest("User already exists");
            }

            return Ok(newUser);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<TokenResponse?>> Login(LandlordLoginDto request)
        {
            var loginRequest = await _authService.Login(request.Email, request.Password);

            if(loginRequest == null)
            {
                return BadRequest("Email or Password is wrong");
            }

            return Ok(loginRequest);
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<TokenResponse>> RefreshToken(LandlordRefreshTokenRequestDto request)
        {
            var tokenRefresh = await _authService.RefreshTokens(
                new Landlord { LandlordId = request.LandlordId, RefreshToken = request.RefreshToken});
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
