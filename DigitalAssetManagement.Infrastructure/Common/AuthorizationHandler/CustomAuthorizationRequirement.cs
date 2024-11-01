using DigitalAssetManagement.Entities.Enums;
using Microsoft.AspNetCore.Authorization;

namespace DigitalAssetManagement.Infrastructure.Common.AuthorizationHandler
{
    public class CustomAuthorizationRequirement: IAuthorizationRequirement
    {
        public Role Role { get; }

        public CustomAuthorizationRequirement(Role role)
        {
            Role = role;
        }
    }
}
