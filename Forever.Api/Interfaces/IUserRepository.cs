using Forever.Api.Models;

namespace Forever.Api.Interfaces
{
    public interface IUserRepository
    {
        Users GetUserByEmail(string email);
    }
}
