using Cidco.Middleware.Application.Contracts.Infrastructure;
using Cidco.Middleware.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Cidco.Middleware.Infrastructure.Token
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GetToken(Apiuser aPIUsers)
        {
            if (aPIUsers.Email != null && aPIUsers.Password != null)
            {
                var claims = new[] {
                new Claim("Email", aPIUsers.Email)
                };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    _configuration["JwtSettings:Issuer"],
                    _configuration["JwtSettings:Audience"],
                    claims,
                    expires: DateTime.UtcNow.AddHours(3),
                    signingCredentials: signIn);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            else
            {
                return "Invalid credentials";
            }
        }
    }
}
