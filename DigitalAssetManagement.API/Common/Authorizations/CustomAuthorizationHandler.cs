using DigitalAssetManagement.Application.Common.Requests;
using DigitalAssetManagement.Application.Exceptions;
using DigitalAssetManagement.Application.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace DigitalAssetManagement.API.Common.Authorizations
{
    public class CustomAuthorizationHandler : AuthorizationHandler<CustomAuthorizationRequirement, MetadataParentRequestDto>
    {
        private readonly UserService _userService;
        private readonly PermissionService _permissionService;

        public CustomAuthorizationHandler(UserService userService, PermissionService permissionService)
        {
            _userService = userService;
            _permissionService = permissionService;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomAuthorizationRequirement requirement, MetadataParentRequestDto resource)
        {
            var loginUserId = int.Parse(context.User.FindFirstValue(ClaimTypes.Sid)!);
            var loginUser = await _userService.Get(loginUserId);
            if (loginUser == null)
            {
                throw new ForbiddenException();
            }

            if (!await _permissionService.HasPermission(requirement.Role, loginUserId, resource.ParentId))
            {
                throw new ForbiddenException();
            }

            context.Succeed(requirement);
        }
    }
}
