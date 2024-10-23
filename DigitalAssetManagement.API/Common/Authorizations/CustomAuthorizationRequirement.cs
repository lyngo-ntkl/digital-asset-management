using DigitalAssetManagement.Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace DigitalAssetManagement.API.Common.Authorizations
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
