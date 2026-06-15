using System.ComponentModel.DataAnnotations;

namespace Forever.Api.Models.User
{
    public class Users
    {
        [Key]
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PashwordHash { get; set; }
        
        public string Role { get; set; }
    }
}
