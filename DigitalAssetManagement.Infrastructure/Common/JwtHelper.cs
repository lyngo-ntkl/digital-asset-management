using DigitalAssetManagement.Domain.Entities;
using System.Security.Claims;

namespace DigitalAssetManagement.Infrastructure.Common
{
    public interface JwtHelper
    {
        string GenerateAccessToken(User user);
    }

    public class JwtHelperImplementation : JwtHelper
    {
        public string GenerateAccessToken(User user)
        {
            Claim[] claims = [
                new Claim(ClaimTypes.Sid, user.Id.ToString()),
            ];
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims);
            
            throw new NotImplementedException();
        }
    }
}
