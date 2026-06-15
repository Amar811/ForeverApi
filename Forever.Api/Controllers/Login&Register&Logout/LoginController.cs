using Forever.Api.DTOs.User;
using Forever.Api.Interfaces.AuthService;
using Forever.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Forever.Api.Controllers.Login_Register_Logout
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IAuthService _authservice;
        public LoginController(IAuthService authService)
        {
            _authservice = authService;
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterRequestDto request)
        {
            var response = _authservice.Register(request);
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto login)
        {
            var response = _authservice.Login(login);

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
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequestDTO request)
        {
            var response = _authservice.RefreshToken(request);

            if (string.IsNullOrEmpty(response.Token))
            {
                return Unauthorized(response);
            }

            return Ok(response);
        }


        [HttpPost("logout")]
        public IActionResult Logout(RefreshTokenRequestDTO request)
        {
            var result = _authservice.Logout(request.RefreshToken);

            return Ok(result);
        }



    }
}
