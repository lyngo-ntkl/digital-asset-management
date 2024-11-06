using DigitalAssetManagement.Infrastructure.Common.AuthorizationHandler;
using DigitalAssetManagement.UseCases.Folders;
using DigitalAssetManagement.UseCases.Folders.Create;
using DigitalAssetManagement.UseCases.Folders.Delete;
using DigitalAssetManagement.UseCases.Folders.Read;
using DigitalAssetManagement.UseCases.Folders.Update;
using DigitalAssetManagement.UseCases.Permissions.Create;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitalAssetManagement.Infrastructure.Controllers
{
    [ApiController]
    [Route("/v1/api/folders")]
    public class FoldersController(
        FolderCreation folderCreation,
        FolderPermissionCreation folderPermissionCreation,
        GetFolder getFolder,
        MoveFolder moveFolder,
        MoveFolderToTrash folderSoftDeletion,
        FolderDeletion folderDeletion,
        FolderNameModification folderNameModification,
        IAuthorizationService authorizationService) : ControllerBase
    {
        private readonly FolderCreation _folderCreation = folderCreation;
        private readonly FolderPermissionCreation _folderPermissionCreation = folderPermissionCreation;
        private readonly GetFolder _getFolder = getFolder;
        private readonly MoveFolder _moveFolder = moveFolder;
        private readonly MoveFolderToTrash _moveFolderToTrash = folderSoftDeletion;
        private readonly FolderDeletion _folderDeletion = folderDeletion;
        private readonly FolderNameModification _folderNameModification = folderNameModification;
        private readonly IAuthorizationService _authorizationService = authorizationService;

        [HttpPost]
        [ProducesResponseType<FolderDetailResponse>(StatusCodes.Status201Created)]
        [Authorize]
        public async Task<ActionResult<FolderDetailResponse>> AddFolder([FromBody] FolderCreationRequest request)
        {
            await _authorizationService.AuthorizeAsync(
                User,
                new ResourceBasedPermissionCheckingRequest { Id = request.ParentId },
                "Contributor");
            return await _folderCreation.AddFolderAsync(request);
        }

        [HttpPost("permissions")]
        public async Task AddFolderPermission([FromBody] PermissionCreationRequest request)
        {
            await _authorizationService.AuthorizeAsync(
                User,
                new ResourceBasedPermissionCheckingRequest { Id = request.MetadataId },
                "Admin"
            );
            await _folderPermissionCreation.AddFolderPermissionAsync(request);
        }

        [HttpPatch("name-modification")]
        public async Task<FolderDetailResponse> RenameFolderAsync([FromBody] MetadataNameModificationRequest request)
        {
            await _authorizationService.AuthorizeAsync(
                User,
                new ResourceBasedPermissionCheckingRequest { Id = request.Id },
                "Contributor"
            );
            return await _folderNameModification.RenameFolderAsync(request);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task DeleteFolder([FromRoute] int id)
        {
            await _authorizationService.AuthorizeAsync(
                User,
                new ResourceBasedPermissionCheckingRequest { Id = id },
                "Contributor"
            );
            await _folderDeletion.DeleteFolderAsync(id);
        }

        [HttpDelete("soft-deletion/{id}")]
        [Authorize]
        public async Task MoveFolderToTrash([FromRoute] int id)
        {
            await _authorizationService.AuthorizeAsync(
                User,
                new ResourceBasedPermissionCheckingRequest { Id = id },
                "Contributor"
            );
            await _moveFolderToTrash.MoveFolderToTrashAsync(id);
        }

        [HttpGet("{id}")]
        public async Task<FolderDetailResponse> Get([FromRoute] int id)
        {
            await _authorizationService.AuthorizeAsync(User, new ResourceBasedPermissionCheckingRequest { Id = id }, "Reader");
            return await _getFolder.GetFolder(id);
        }

        [HttpPatch("move")]
        public async Task MoveFolder([FromBody] MoveFolderRequest request)
        {
            await _authorizationService.AuthorizeAsync(User, new ResourceBasedPermissionCheckingRequest { Id = request.FolderId }, "Admin");
            await _authorizationService.AuthorizeAsync(User, new ResourceBasedPermissionCheckingRequest { Id = request.NewParentId }, "Admin");
            await _moveFolder.MoveFolder(request);
        }

    }
}