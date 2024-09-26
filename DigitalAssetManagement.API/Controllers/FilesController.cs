using DigitalAssetManagement.Application.Dtos.Requests;
using DigitalAssetManagement.Application.Dtos.Responses.Folders;
using DigitalAssetManagement.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    }
}
