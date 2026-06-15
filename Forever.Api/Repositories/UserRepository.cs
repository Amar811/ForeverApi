using AuthDemo.Api.Data;
using AuthDemo.Api.Models;
using Forever.Api.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Runtime.Serialization;

namespace Forever.Api.Repositories
{
    public class UserRepository:IUserRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public UserRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public Users GetUserByEmail(string email)
        {
            return _applicationDbContext.Users
                          .FirstOrDefault(x => x.Email == email);

        }
    }
}
