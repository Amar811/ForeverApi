using Forever.Api.Models.User;

namespace Forever.Api.Interfaces
{
    public interface IJwtHelper
    {
        string GenerateToken(Users user);
        public string GenerateRefreshToken();
    }
}
