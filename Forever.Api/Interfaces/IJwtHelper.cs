using AuthDemo.Api.Models;

namespace Forever.Api.Interfaces
{
    public interface IJwtHelper
    {
        string GenerateToken(Users user);
        public string GenerateRefreshToken();
    }
}
