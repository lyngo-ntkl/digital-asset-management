using DigitalAssetManagement.Infrastructure.Common.AuthorizationHandler;
using DigitalAssetManagement.UseCases.Files.Create;
using DigitalAssetManagement.UseCases.Files.Delete;
using DigitalAssetManagement.UseCases.Files.Read;
using DigitalAssetManagement.UseCases.Files.Update;
using DigitalAssetManagement.UseCases.Permissions.Create;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace DigitalAssetManagement.Infrastructure.Controllers
{
    [Route("/v1/api/files")]
    [ApiController]
    public class FilesController(
        FileCreation fileCreation,
        FilePermissionCreation filePermissionCreation,
        GetFile getFile,
        MoveFile moveFile,
        FileSoftDeletion fileSoftDeletion,
        FileDeletion fileDeletion,
        IAuthorizationService authorizationService) : ControllerBase
    {
        private readonly FileCreation _fileCreation = fileCreation;
        private readonly FilePermissionCreation _filePermissionCreation = filePermissionCreation;
        private readonly GetFile _getFile = getFile;
        private readonly MoveFile _moveFile = moveFile;
        private readonly FileSoftDeletion _fileSoftDeletion = fileSoftDeletion;
        private readonly FileDeletion _fileDeletion = fileDeletion;
        private readonly IAuthorizationService _authorizationService = authorizationService;

        [HttpPost("metadata")]
        [Authorize]
        public async Task<int> AddFileMetadata(FileCreationRequest request)
        {
            await _authorizationService.AuthorizeAsync(
                User,
                new ResourceBasedPermissionCheckingRequest { Id = request.ParentId },
                "Contributor");
            return await _fileCreation.AddFileMetadataAsync(request);
        }

        [HttpPost("permissions")]
        public async Task AddPermission(PermissionCreationRequest request)
        {
            await _authorizationService.AuthorizeAsync(
                User,
                new ResourceBasedPermissionCheckingRequest { Id = request.MetadataId },
                "Admin"
            );
            await _filePermissionCreation.AddOrUpdateFilePermissionAsync(request);
        }

        [HttpDelete("{id}")]
        public async Task DeleteFile([FromRoute] int id)
        {
            await _authorizationService.AuthorizeAsync(
                User,
                new ResourceBasedPermissionCheckingRequest { Id = id },
                "Contributor"
            );
            await _fileDeletion.DeleteFileAsync(id);
        }

        [HttpDelete("soft-deletion/{id}")]
        public async Task DeleteFileSoftly([FromRoute] int id)
        {
            await _authorizationService.AuthorizeAsync(
                User,
                new ResourceBasedPermissionCheckingRequest { Id = id },
                "Contributor"
            );
            await _fileSoftDeletion.DeleteFileSoftlyAsync(id);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFile([FromRoute] int id)
        {
            await _authorizationService.AuthorizeAsync(User,
                new ResourceBasedPermissionCheckingRequest { Id = id },
                "Reader"
            );
            var fileResponse = await _getFile.GetFileContentAsync(id);
            new FileExtensionContentTypeProvider().TryGetContentType(fileResponse.FileName, out var contentType);
            return File(fileResponse.FileContent, contentType!, fileResponse.FileName);
        }

        [HttpPatch("move")]
        public async Task MoveFile([FromBody] MoveFileRequest request)
        {
            await _authorizationService.AuthorizeAsync(User, new ResourceBasedPermissionCheckingRequest { Id = request.FileId }, "Admin");
            await _authorizationService.AuthorizeAsync(User, new ResourceBasedPermissionCheckingRequest { Id = request.NewParentId }, "Admin");
            await _moveFile.MoveFile(request);
        }
    }
}
