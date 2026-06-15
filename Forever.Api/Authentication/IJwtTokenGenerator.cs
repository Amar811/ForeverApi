using Forever.Api.Models.User;

namespace Forever.Api.Authentication
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(Users user);
        public string GenerateRefreshToken();
    }
}
