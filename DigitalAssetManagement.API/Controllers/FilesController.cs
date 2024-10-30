using DigitalAssetManagement.Application.Common.Requests;
using DigitalAssetManagement.Application.Dtos.Requests;
using DigitalAssetManagement.Application.Services;
using DigitalAssetManagement.UseCases.Files.Create;
using DigitalAssetManagement.UseCases.Files.Update;
using DigitalAssetManagement.UseCases.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System.Net.Mime;

namespace DigitalAssetManagement.API.Controllers
{
    [Route("/v1/api/files")]
    [ApiController]
    public class FilesController(
        FileCreation fileCreation,
        MoveFile moveFile,
        IAuthorizationService authorizationService, 
        FileService fileService) : ControllerBase
    {
        private readonly FileCreation _fileCreation = fileCreation;
        private readonly MoveFile _moveFile = moveFile;
        private readonly IAuthorizationService _authorizationService = authorizationService;
        private readonly FileService _fileService = fileService;

        [HttpPost("metadata")]
        [Authorize]
        public async Task<int> AddFileMetadata(FileCreationRequest request)
        {
            await _authorizationService.AuthorizeAsync(User, request, "Contributor");
            return await _fileCreation.AddFileMetadataAsync(request);
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Multipart.FormData)]
        [Authorize]
        public async Task<IActionResult> AddFiles(MultipleFilesUploadRequestDto request)
        {
            await _authorizationService.AuthorizeAsync(User, request, "Contributor");
            await _fileService.AddFiles(request);
            return Created();
        }

        [HttpPost("{id}/permissions")]
        public async Task AddPermission([FromRoute] int id, PermissionCreationRequest request)
        {
            await _authorizationService.AuthorizeAsync(
                User,
                new ResourceBasedPermissionCheckingRequestDto { ParentId = id },
                "Admin"
            );
            await _fileService.AddFilePermission(id, request);
        }

        [HttpDelete("{id}")]
        public async Task DeleteFile([FromRoute] int id)
        {
            await _authorizationService.AuthorizeAsync(
                User,
                new ResourceBasedPermissionCheckingRequestDto { ParentId = id },
                "Contributor"
            );
            await _fileService.DeleteFile(id);
        }

        [HttpDelete("soft-deletion/{id}")]
        public async Task DeleteFileSoftly([FromRoute] int id)
        {
            await _authorizationService.AuthorizeAsync(
                User,
                new ResourceBasedPermissionCheckingRequestDto { ParentId = id },
                "Contributor"
            );
            await _fileService.DeleteFileSoftly(id);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFile([FromRoute] int id)
        {
            await _authorizationService.AuthorizeAsync(User,
                new ResourceBasedPermissionCheckingRequestDto { ParentId = id },
                "Reader"
            );
            var fileResponse = await _fileService.GetFile(id);
            new FileExtensionContentTypeProvider().TryGetContentType(fileResponse.FileName, out var contentType);
            return File(fileResponse.FileContent, contentType!, fileResponse.FileName);
        }

        [HttpPatch("move")]
        public async Task MoveFile([FromBody] MoveFileRequest request)
        {
            await _authorizationService.AuthorizeAsync(User, new ResourceBasedPermissionCheckingRequestDto { ParentId = request.FileId }, "Admin");
            await _authorizationService.AuthorizeAsync(User, new ResourceBasedPermissionCheckingRequestDto { ParentId = request.NewParentId }, "Admin");
            await _moveFile.MoveFile(request);
        }
    }
}
