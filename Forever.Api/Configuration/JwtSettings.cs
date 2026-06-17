namespace Forever.Api.Configuration
{
    public class JwtSettings
    {
        public string Key { get; set; }

        public string Issuer { get; set; }

        public string Audience { get; set; }

        // Access token lifetime in minutes
        public int ExpiryMinutes { get; set; }

        // Refresh token lifetime in days
        public int RefreshTokenExpiryDays { get; set; }
    }
}
