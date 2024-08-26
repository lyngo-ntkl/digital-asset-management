using DigitalAssetManagement.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DigitalAssetManagement.Infrastructure.Common
{
    public interface JwtHelper
    {
        string GenerateAccessToken(User user);
    }

    public class JwtHelperImplementation : JwtHelper
    {
        private readonly IConfiguration _configuration;

        public JwtHelperImplementation(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateAccessToken(User user)
        {
            Claim[] claims = [
                new Claim(ClaimTypes.Sid, user.Id.ToString()),
            ];
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwt:key"]!));
            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                SigningCredentials = credential,
                IssuedAt = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddMonths(1),
                Issuer = _configuration["jwt:issuer"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
