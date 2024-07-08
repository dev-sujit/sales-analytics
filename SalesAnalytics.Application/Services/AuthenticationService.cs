using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SalesAnalytics.Core.Entities;
using SalesAnalytics.Core.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SalesAnalytics.API.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IConfiguration _configuration;
        private readonly ISalesDbContext _context;

        public AuthenticationService(IConfiguration configuration, ISalesDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public async Task<JwtTokenResponse> Authenticate(UserLoginModel loginModel)
        {
            // This is where you'd validate the user's credentials against your data source
            // For simplicity, let's assume the user is valid
            //if (loginModel.Username != "test" || loginModel.Password != "password")
            //{
            //    return null;
            //}

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, loginModel.Username),
                new Claim(ClaimTypes.Role, "User")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtTokenResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo
            };
        }

        public async Task<bool> Register(UserLoginModel registrationModel)
        {
            try
            {
                if (await _context.Users.AnyAsync(u => u.Username == registrationModel.Username))
                    return false;

                var user = new User
                {
                    Username = registrationModel.Username,
                    PasswordHash = CreatePasswordHash(registrationModel.Password)
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync(CancellationToken.None);

                return true;
            }
            catch (Exception ex) { 
                throw ex;
            }
        }

        private string CreatePasswordHash(string password)
        {
            using (var hmac = new HMACSHA512())
            {
                return Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
            }
        }
        private bool VerifyPasswordHash(string password, string storedHash)
        {
            using (var hmac = new HMACSHA512())
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(computedHash) == storedHash;
            }
        }
    }
}
