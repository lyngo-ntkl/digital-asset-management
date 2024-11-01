using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using DigitalAssetManagement.Infrastructure.Common.Exceptions;
using DigitalAssetManagement.UseCases.Repositories;
using DigitalAssetManagement.Entities.Enums;

namespace DigitalAssetManagement.Infrastructure.Common.AuthorizationHandler
{
    public class ResourceBasedAuthorizationHandler : AuthorizationHandler<CustomAuthorizationRequirement, ResourceBasedPermissionCheckingRequest>
    {
        private readonly UserRepository _userRepository;
        private readonly PermissionRepository _permissionRepository;

        public ResourceBasedAuthorizationHandler(UserRepository userRepository, PermissionRepository permissionRepository)
        {
            _userRepository = userRepository;
            _permissionRepository = permissionRepository;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomAuthorizationRequirement requirement, ResourceBasedPermissionCheckingRequest resource)
        {
            var userId = await CheckAuthorizationAsync(context);
            await CheckResourceBasedPermissionAsync(userId, resource.Id, requirement.Role);

            context.Succeed(requirement);
        }

        private async Task<int> CheckAuthorizationAsync(AuthorizationHandlerContext context)
        {
            if (!int.TryParse(context.User.FindFirstValue(ClaimTypes.Sid)!, out int loginUserId) &&
                !await _userRepository.ExistByIdAsync(loginUserId))
            {
                throw new UnauthorizedException();
            }
            return loginUserId;
        }

        private async Task CheckResourceBasedPermissionAsync(int userId, int resourceId, Role role)
        {
            if (
                (role == Role.Admin && ! await IsAdmin(userId, resourceId)) ||
                (role == Role.Contributor && !await CanContribute(userId, resourceId)) ||
                (role == Role.Reader && !await CanRead(userId, resourceId))
            )
            {
                throw new ForbiddenException();
            }
        }

        private async Task<bool> IsAdmin(int userId, int metadataId)
        {
            var role = await _permissionRepository.GetRoleByUserIdAndMetadataId(userId, metadataId);
            return role == Role.Admin;
        }

        private async Task<bool> CanContribute(int userId, int metadataId)
        {
            var role = await _permissionRepository.GetRoleByUserIdAndMetadataId(userId, metadataId);
            return role != null && role != Role.Reader;
        }

        private async Task<bool> CanRead(int userId, int metadataId)
        {
            var role = await _permissionRepository.GetRoleByUserIdAndMetadataId(userId, metadataId);
            return role != null;
        }
    }
}
