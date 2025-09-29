using System.Security.Claims;
using System.Text;
using UserService.DTOs;
using UserService.Interfaces;
using UserService.Models;
using static UserService.Models.Role;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using BCrypt.Net;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace FiapCloudGamesAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _config;
        public AuthService(IConfiguration config) => _config = config;

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string hash, string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }

        public string GenerateToken(User user)
        {
            var key = _config["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key not configured");
            var issuer = _config["Jwt:Issuer"] ?? "FiapCloud";
            var audience = _config["Jwt:Audience"] ?? "FiapCloudClients";
            var keyBytes = Encoding.UTF8.GetBytes(key);

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role ?? "User"),
            new Claim("name", user.Name)
        };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(4),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
