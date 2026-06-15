using Forever.Api.DTOs.User;

namespace Forever.Api.Interfaces.AuthService
{
    public interface IAuthService
    {
        string Register(RegisterRequestDto request);
        LoginResponseDto Login(LoginRequestDto login);

        LoginResponseDto RefreshToken(RefreshTokenRequestDTO request);
        string Logout(string refreshToken);

    }
}
