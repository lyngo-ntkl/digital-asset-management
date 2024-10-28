using DigitalAssetManagement.Application.Common.Requests;
using DigitalAssetManagement.Application.Services;
using DigitalAssetManagement.UseCases.Folders;
using DigitalAssetManagement.UseCases.Folders.Create;
using DigitalAssetManagement.UseCases.Folders.Delete;
using DigitalAssetManagement.UseCases.Folders.Read;
using DigitalAssetManagement.UseCases.Folders.Update;
using DigitalAssetManagement.UseCases.Permissions.Create;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DigitalAssetManagement.API.Controllers
{
    [ApiController]
    [Route("/v1/api/folders")]
    public class FoldersController(
        FolderCreation folderCreation, 
        FolderPermissionCreation folderPermissionCreation,
        GetFolder getFolder,
        MoveFolder moveFolder,
        FolderDeletion folderDeletion,
        FolderService folderService,
        IAuthorizationService authorizationService) : ControllerBase
    {
        private readonly FolderCreation _folderCreation = folderCreation;
        private readonly FolderPermissionCreation _folderPermissionCreation = folderPermissionCreation;
        private readonly GetFolder _getFolder = getFolder;
        private readonly MoveFolder _moveFolder = moveFolder;
        private readonly FolderDeletion _folderDeletion = folderDeletion;
        private readonly FolderService _folderService = folderService;
        private readonly IAuthorizationService _authorizationService = authorizationService;

        [HttpPost]
        [ProducesResponseType<FolderDetailResponse>(StatusCodes.Status201Created)]
        [Authorize]
        public async Task<ActionResult<FolderDetailResponse>> AddFolder([FromBody] FolderCreationRequest request)
        {
            await _authorizationService.AuthorizeAsync(User, request, "Contributor");
            return await _folderCreation.AddFolder(request);
        }

        [HttpPost("permissions")]
        public async Task AddFolderPermission([FromBody] PermissionCreationRequest request)
        {
            await _authorizationService.AuthorizeAsync(
                User,
                new ResourceBasedPermissionCheckingRequestDto { ParentId = request.MetadataId },
                "Admin"
            );
            await _folderPermissionCreation.AddFolderPermission(request);
        }

        //[HttpPatch("{id}")]
        //public async Task<FolderDetailResponseDto> Update([FromRoute] int id, [FromBody] FolderModificationRequestDto request)
        //{
        //    return await _folderService.Update(id, request);
        //}

        [HttpDelete("{id}")]
        [Authorize]
        public async Task DeleteFolder([FromRoute] int id)
        {
            await _authorizationService.AuthorizeAsync(
                User, 
                new ResourceBasedPermissionCheckingRequestDto { ParentId = id}, 
                "Contributor"
            );
            await _folderDeletion.DeleteFolder(id);
        }

        [HttpDelete("soft-deletion/{id}")]
        [Authorize]
        public async Task DeleteFolderSoftly([FromRoute] int id)
        {
            await _authorizationService.AuthorizeAsync(
                User,
                new ResourceBasedPermissionCheckingRequestDto { ParentId = id },
                "Contributor"
            );
            await _folderService.DeleteFolderSoftly(id);
        }

        [HttpGet("{id}")]
        public async Task<FolderDetailResponse> Get([FromRoute] int id)
        {
            await _authorizationService.AuthorizeAsync(User, new ResourceBasedPermissionCheckingRequestDto { ParentId = id }, "Reader");
            return await _getFolder.GetFolder(id);
        }

        [HttpPatch("move")]
        public async Task MoveFolder([FromBody] MoveFolderRequest request)
        {
            await _authorizationService.AuthorizeAsync(User, new ResourceBasedPermissionCheckingRequestDto { ParentId = request.FolderId }, "Admin");
            await _authorizationService.AuthorizeAsync(User, new ResourceBasedPermissionCheckingRequestDto { ParentId = request.NewParentId }, "Admin");
            await _moveFolder.MoveFolder(request);
        }

    }
}