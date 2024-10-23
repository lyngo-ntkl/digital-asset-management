using DigitalAssetManagement.Application.Common.Requests;
using DigitalAssetManagement.Application.Dtos.Requests;
using DigitalAssetManagement.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;

namespace DigitalAssetManagement.API.Controllers
{
    [Route("/v1/api/files")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly FileService _fileService;

        public FilesController(IAuthorizationService authorizationService, FileService fileService)
        {
            _authorizationService = authorizationService;
            _fileService = fileService;
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
        public async Task AddPermission([FromRoute] int id, PermissionRequestDto request)
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

        [HttpPatch("move/{fileId}")]
        public async Task MoveFile([FromRoute] int fileId, [FromQuery] [Required] int newParentId)
        {
            await _authorizationService.AuthorizeAsync(User, new ResourceBasedPermissionCheckingRequestDto { ParentId = fileId }, "Admin");
            await _authorizationService.AuthorizeAsync(User, new ResourceBasedPermissionCheckingRequestDto { ParentId = newParentId }, "Admin");
            await _fileService.MoveFile(fileId, newParentId);
        }
    }
}
