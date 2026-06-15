using Forever.Api.Configuration;
using Forever.Api.Models.User;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using JwtConstants = Forever.Api.Configuration.JwtConstants;

namespace Forever.Api.Authentication
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly JwtSettings _jwtSettings;
        public JwtTokenGenerator(IOptions<JwtSettings> jwtOption)
        {
            _jwtSettings = jwtOption.Value;
        }

       

        public string GenerateToken(Users user)
        {
            var claims = new List<Claim>
            {
                new Claim(
                      JwtConstants.UserId,
                      user.UserId.ToString()
                ),

                new Claim(
                    JwtConstants.Email,
                    user.Email.ToString()
                ),

                new Claim (
                    ClaimTypes.Role,
                    user.Role
                ),
            };


            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    _jwtSettings.Key
            ));

            var credentials =
           new SigningCredentials(
           key,
           SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
              issuer: _jwtSettings.Issuer,
              audience: _jwtSettings.Audience,
              claims: claims,
              expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
              signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            return Guid.NewGuid().ToString();
        }

    }
}
