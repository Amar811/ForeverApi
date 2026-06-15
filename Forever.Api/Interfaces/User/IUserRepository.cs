using Forever.Api.Models.User;

namespace Forever.Api.Interfaces.User
{
    public interface IUserRepository
    {
        Users GetUserByEmail(string email);
    }
}
