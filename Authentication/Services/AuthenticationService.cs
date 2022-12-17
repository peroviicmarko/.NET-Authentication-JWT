using Authentication.DTOs.User;
using Authentication.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Authentication.Services
{
    public class AuthenticationService
    {
        private IConfiguration _configuration;
        public AuthenticationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<User> GetUsers()
        {
            List<User> users = new()
           {
               new User() { Username = "user01", Password = "123", Role = Common.Enum.UserRole.Client, UserId = Guid.NewGuid() },
               new User() { Username = "user02", Password = "123123", Role = Common.Enum.UserRole.Client, UserId = Guid.NewGuid() },
               new User() { Username = "user03", Password = "123456", Role = Common.Enum.UserRole.Client, UserId = Guid.NewGuid() },
               new User() { Username = "admin", Password = "123456", Role = Common.Enum.UserRole.Admin, UserId = Guid.NewGuid() },
           };

            return users;
        }

        public User Authenticate(UserLogInRequest request)
        {
            var user = GetUsers().FirstOrDefault(u => u.Username.ToLower().Equals(request.Username.ToLower()) && u.Password.Equals(request.Password));
            return user;
        }

        public string GenerateJwt(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secretKey = Encoding.ASCII.GetBytes(_configuration["AppSettings:SecretKey"]);

            Claim[] claimes = new Claim[]
            {
                new Claim("userId", user.UserId.ToString()),
                new Claim("username", user.Username),
                new Claim("roleId", user.Role.ToString()),
            };

            ClaimsIdentity identity = new(claimes);

            DateTime expireTime = DateTime.Now.AddDays(1);

            SymmetricSecurityKey key = new(secretKey);

            SigningCredentials signingCredentials = new(key, SecurityAlgorithms.HmacSha256);

            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Subject = identity,
                Expires = expireTime,
                SigningCredentials = signingCredentials
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwt = tokenHandler.WriteToken(token);

            return jwt;
        }
    }
}
