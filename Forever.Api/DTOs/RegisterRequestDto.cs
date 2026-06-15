namespace Forever.Api.DTOs
{
    public class RegisterRequestDto
    {
        //public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PashwordHash { get; set; }
    }
}
