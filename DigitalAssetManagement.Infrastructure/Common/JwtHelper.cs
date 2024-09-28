using DigitalAssetManagement.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Text;

namespace DigitalAssetManagement.Infrastructure.Common
{
    public interface JwtHelper
    {
        string? ExtractJwtFromAuthorizationHeader();
        string? ExtractSidFromAuthorizationHeader();
        string GenerateAccessToken(User user);
        IEnumerable<Claim> GetClaims(string jwt);
        string? GetSid(string jwt);
    }

    public class JwtHelperImplementation : JwtHelper
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public JwtHelperImplementation(
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public string? ExtractJwtFromAuthorizationHeader()
        {
            var authorizationHeader = _httpContextAccessor.HttpContext!.Request.Headers.FirstOrDefault(header => header.Key == "Authorization").Value;
            return authorizationHeader.ToString()!.Split("Bearer ", StringSplitOptions.RemoveEmptyEntries)[0];
        }

        public string? ExtractSidFromAuthorizationHeader()
        {
            var jwt = ExtractJwtFromAuthorizationHeader();
            return GetSid(jwt!);
        }

        public string GenerateAccessToken(User user)
        {
            Claim[] claims = [
                new Claim(ClaimTypes.Sid, user.Id.ToString()!),
            ];
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwt:key"]!));
            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                SigningCredentials = credential,
                IssuedAt = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddDays(int.Parse(_configuration["jwt:expirationDays"]!)),
                Issuer = _configuration["jwt:issuer"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public IEnumerable<Claim> GetClaims(string jwt)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadJwtToken(jwt);
            return token.Claims;
        }

        public string? GetSid(string jwt)
        {
            return GetClaims(jwt).FirstOrDefault(claim => claim.Type == ClaimTypes.Sid)?.Value;
        }
    }
}
