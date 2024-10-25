using DigitalAssetManagement.Application.Common;
using DigitalAssetManagement.Application.Common.Requests;
using DigitalAssetManagement.Application.Common.Exceptions;
using DigitalAssetManagement.Application.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace DigitalAssetManagement.API.Common.Authorizations
{
    public class CustomAuthorizationHandler : AuthorizationHandler<CustomAuthorizationRequirement, ResourceBasedPermissionCheckingRequestDto>
    {
        private readonly UserService _userService;
        private readonly PermissionService _permissionService;

        public CustomAuthorizationHandler(UserService userService, PermissionService permissionService)
        {
            _userService = userService;
            _permissionService = permissionService;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomAuthorizationRequirement requirement, ResourceBasedPermissionCheckingRequestDto resource)
        {
            var loginUserId = int.Parse(context.User.FindFirstValue(ClaimTypes.Sid)!);
            var loginUser = await _userService.GetById(loginUserId);
            if (loginUser == null)
            {
                throw new UnauthorizedException();
            }

            if (!await _permissionService.HasPermission(requirement.Role, loginUserId, resource.ParentId))
            {
                throw new ForbiddenException(ExceptionMessage.NoPermission);
            }

            context.Succeed(requirement);
        }
    }
}
