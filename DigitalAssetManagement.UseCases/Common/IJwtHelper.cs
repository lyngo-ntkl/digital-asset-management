using DigitalAssetManagement.Entities.DomainEntities;
using System.Security.Claims;

namespace DigitalAssetManagement.UseCases.Common
{
    public interface IJwtHelper
    {
        string? ExtractJwtFromAuthorizationHeader();
        string? ExtractSidFromAuthorizationHeader();
        string GenerateAccessToken(User user);
        IEnumerable<Claim> GetClaims(string jwt);
        string? GetSid(string jwt);
    }
}
