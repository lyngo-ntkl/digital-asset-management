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

        //[HttpGet]
        //public async Task<FolderResponseDto> GetDriveOwnedByLoginUser()
        //{
        //    return await _driveService.GetLoginUserDrive();
        //}

        //[HttpGet("{id}")]
        //public async Task<DriveDetailsResponseDto> GetById([FromRoute] int id)
        //{
        //    return await _driveService.GetById(id);
        //}

        //[HttpPost]
        //public async Task<DriveDetailsResponseDto> Create([FromBody] DriveRequestDto request)
        //{
        //    return await _driveService.Create(request);
        //}

        //[HttpPatch("{id}")]
        //public async Task<DriveDetailsResponseDto> Update([FromRoute] int id, [FromBody] DriveRequestDto request)
        //{
        //    return await _driveService.Update(id, request);
        //}

        //[HttpPatch("{id}/trash")]
        //public async Task MoveToTrash([FromRoute] int id)
        //{
        //    await _driveService.MoveToTrash(id);
        //}

        //[HttpDelete("{id}")]
        //public async Task Delete([FromRoute] int id)
        //{
        //    await _driveService.Delete(id);
        //}
    }
}
