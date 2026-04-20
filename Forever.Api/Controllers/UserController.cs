using AuthDemo.Api.Data;
using AuthDemo.Api.DTOs;
using AuthDemo.Api.Models;
using Forever.Api.DTOs;
using Forever.Api.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Forever.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAuthService _authService;
         

        public UserController(IAuthService authService)
        {
            _authService = authService;
        }


        [HttpPost("register")]
        public IActionResult Register(RegisterRequestDto request)
        {
            var response = _authService.Register(request);
            return Ok(response);
        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequestDto request)
        {
            var response =  _authService.Login(request);

            if (string.IsNullOrEmpty(response.Token))
            {
                return Unauthorized(response);
            }

            return Ok(new ApiResponseDto<LoginResponseDto>
            {

                Success = true,
                Message = "Login successful",
                Data = response
            });
            //return Ok(response);
        }

        [HttpPost("refresh-token")]
        public IActionResult RefreshToken(RefreshTokenRequestDTO request)
        {
            var response = _authService.RefreshToken(request);

            if (string.IsNullOrEmpty(response.Token))
            {
                return Unauthorized(response);
            }

            return Ok(response);
        }

        [HttpPost("logout")]
        public IActionResult Logout(RefreshTokenRequestDTO request)
        {
            var result = _authService.Logout(request.RefreshToken);

            return Ok(result);
        }




    }
}
