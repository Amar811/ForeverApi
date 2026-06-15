using AuthDemo.Api.Data;
using Forever.Api.Authentication;
using Forever.Api.DTOs.User;
using Forever.Api.Helpers;
using Forever.Api.Interfaces.AuthService;
using Forever.Api.Interfaces.User;
using Forever.Api.Models;
using Forever.Api.Models.RefreshToken;
using Forever.Api.Models.User;

namespace Forever.Api.Services.AuthService
{
    public class AuthService:IAuthService
    {
        private readonly IUserRepository _userRepository;
        //private readonly IJwtHelper _jwtHelper;
        private readonly IJwtTokenGenerator _jwttokengenerator;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
             IUserRepository userRepository, 
             //IJwtHelper jwtHelper,
             IJwtTokenGenerator jwttokengenerator,
             ApplicationDbContext context,
             ILogger<AuthService> logger
            )
        {
            _userRepository = userRepository;
            //_jwtHelper = jwtHelper;
            _jwttokengenerator = jwttokengenerator;
            _context = context;
            _logger = logger;
        }


        public string Register(RegisterRequestDto request)
        {
            _logger.LogInformation("Registration attempt started for Email: {Email}", request.Email);
            var existing = _userRepository.GetUserByEmail(request.Email);

            if (existing != null)
            {
                _logger.LogWarning("Registration failed. Email already exists: {Email}", request.Email);
                return "Email already exists";
            }

            var user = new Users
            {
                Username = request.Username,
                Email = request.Email,
                PashwordHash = request.PashwordHash,
                //PashwordHash = BCrypt.Net.BCrypt.HashPassword(request.PashwordHash),
                Role = "User"
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            _logger.LogInformation("User registered successfully: {Email}", request.Email);
            return "Registration Successful";
        }

        public  LoginResponseDto Login(LoginRequestDto login)
        {
            _logger.LogInformation("Login attempt started for Email: {Email}", login.Email);
            var user =  _userRepository.GetUserByEmail(login.Email);

            if(user == null)
            {
                _logger.LogWarning("Login failed. User not found for Email: {Email}", login.Email);

                return new LoginResponseDto
                {
                    Message = "User not found"
                };
            }

            if(user.PashwordHash != login.PashwordHash)
            {
                _logger.LogWarning("Login failed. Invalid password for Email: {Email}", login.Email);

                return new LoginResponseDto
                {
                    Message = "Invalid Password"
                };
            }

            //var token = _jwtHelper.GenerateToken(user);
            var token = _jwttokengenerator.GenerateToken(user);

            //return new LoginResponseDto
            //{
            //    Token = token,
            //    Message = "Login SuccessFul",
            //    Username = user.Username,
            //    Email = user.Email,
            //    Role = user.Role
            //};

            _logger.LogInformation("Access token generated for UserId: {UserId}", user.UserId);
            //var refreshToken = _jwtHelper.GenerateRefreshToken();
            var refreshToken = _jwttokengenerator.GenerateRefreshToken();

            var refreshTokenEntity = new RefreshToken
            { 
                UserId = user.UserId,
                Token = refreshToken,
                ExpiryDate = DateTime.Now.AddDays(1),
                IsRevoked = false,
                CreatedDate = DateTime.Now,
            };

            _context.RefreshTokens.Add(refreshTokenEntity);
            _context.SaveChanges();

            _logger.LogInformation("Refresh token created and saved for UserId: {UserId}", user.UserId);
            return new LoginResponseDto
            {
                Token = token,
                RefreshToken = refreshToken,
                Message = "Login Successful",
                Username = user.Username,
                Email = user.Email,
                Role = user.Role
            };

        }


        public LoginResponseDto RefreshToken(RefreshTokenRequestDTO request)
        {
            _logger.LogInformation("Refresh token request received");
            var storedToken = _context.RefreshTokens.FirstOrDefault(x => x.Token == request.RefreshToken
              && !x.IsRevoked && x.ExpiryDate > DateTime.Now
            );

            if(storedToken == null)
            {
                _logger.LogWarning("Invalid or expired refresh token used");
                return new LoginResponseDto
                {
                    Message = "Invalid Refresh Token"
                };
            }

            var user = _context.Users.FirstOrDefault(x => x.UserId == storedToken.UserId);

            if (user == null)
            {
                _logger.LogWarning("User not found for refresh token request");

                return new LoginResponseDto
                {
                    Message = "User not found"
                };
            }

            //var newAccessToken = _jwtHelper.GenerateToken(user);
            var newAccessToken = _jwttokengenerator.GenerateToken(user);

            _logger.LogInformation("New access token generated for UserId: {UserId}", user.UserId);
            return new LoginResponseDto
            {
                Token = newAccessToken,
                RefreshToken = request.RefreshToken,
                Message = "New access token generated",
                Username = user.Username,
                Role = user.Role
            };

        }

        public string Logout(string refreshToken)
        {
            _logger.LogInformation("Logout request started");
            var token = _context.RefreshTokens.FirstOrDefault(x => x.Token ==  refreshToken);

            if (token == null)
            {
                _logger.LogWarning("Logout failed. Refresh token not found");
                return "Refresh token not found";
            }

            token.IsRevoked = true;
            _context.SaveChanges();

            _logger.LogInformation("Logout successful. Refresh token revoked");
            return "Logout successful";
        }

    }
}
