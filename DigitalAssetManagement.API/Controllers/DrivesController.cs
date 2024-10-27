using DigitalAssetManagement.Application.Common.Requests;
using DigitalAssetManagement.Application.Services;
using DigitalAssetManagement.UseCases.Folders;
using DigitalAssetManagement.UseCases.Folders.Read;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitalAssetManagement.API.Controllers
{
    [Route("v1/api/drives")]
    [ApiController]
    public class DrivesController(GetDrive getDrive, IAuthorizationService authorizationService) : ControllerBase
    {
        private readonly GetDrive _getDrive = getDrive;
        private readonly IAuthorizationService _authorizationService = authorizationService;

        [HttpGet("my-drive")]
        [Authorize]
        public async Task<FolderDetailResponse> GetLoginUserDrive()
        {
            var myDrive = await _getDrive.GetDrive();
            await _authorizationService.AuthorizeAsync(
                User,
                new ResourceBasedPermissionCheckingRequestDto { ParentId = myDrive.Id },
                "Admin"
            );
            return myDrive;
        }
    }
}
