using DataAccess;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class UserService
    {
        private readonly SportContext _context;
        private readonly IConfiguration _configuration;

        public UserService(SportContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public Task<string> Authenticate(string login, string password)
        {
            var user = GetUser(login, password);

            if (user == null)
                return Task.FromResult<string>(null);

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();

            var consumer = _configuration["JwtSettings:Consumer"];
            var issuer = _configuration["JwtSettings:Issuer"];
            var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:SecretKey"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email)
                }),
                Issuer = issuer,
                Audience = consumer,
                Expires = DateTime.UtcNow.AddYears(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Task.FromResult(tokenHandler.WriteToken(token));
        }

        private User GetUser(string login, string password)
        {
            var users = new List<User>
            {
                new User
                {
                    Id = 1,
                    Login = "username",
                    Email = "domo.ddr@gmail.com",
                    Password = "12345"
                }
            };

            return users.SingleOrDefault(u => u.Login == login
                                    && u.Password == password);
        }
    }
}
