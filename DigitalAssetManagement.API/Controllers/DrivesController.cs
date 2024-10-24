using DigitalAssetManagement.Application.Common.Requests;
using DigitalAssetManagement.Application.Dtos.Responses.Folders;
using DigitalAssetManagement.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitalAssetManagement.API.Controllers
{
    [Route("v1/api/drives")]
    [ApiController]
    public class DrivesController : ControllerBase
    {
        private readonly DriveService _driveService;
        private readonly IAuthorizationService _authorizationService;

        public DrivesController(DriveService driveService, IAuthorizationService authorizationService)
        {
            _driveService = driveService;
            _authorizationService = authorizationService;
        }

        [HttpGet("my-drive")]
        [Authorize]
        public async Task<FolderDetailResponseDto> GetLoginUserDrive()
        {
            var myDrive = await _driveService.GetLoginUserDrive();
            await _authorizationService.AuthorizeAsync(
                User,
                new ResourceBasedPermissionCheckingRequestDto { ParentId = myDrive.Id },
                "Admin"
            );
            return myDrive;
        }
    }
}
