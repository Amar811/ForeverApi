using AuthDemo.Api.DTOs;
using Forever.Api.DTOs;

namespace Forever.Api.Interfaces
{
    public interface IAuthService
    {
        string Register(RegisterRequestDto request);
        LoginResponseDto Login(LoginRequestDto login);

        LoginResponseDto RefreshToken(RefreshTokenRequestDTO request);
        string Logout(string refreshToken);

    }
}
