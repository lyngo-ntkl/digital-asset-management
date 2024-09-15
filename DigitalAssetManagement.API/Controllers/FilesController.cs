using DigitalAssetManagement.Application.Dtos.Requests.Files;
using DigitalAssetManagement.Application.Dtos.Responses.Files;
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
        private readonly FileService _fileService;

        public FilesController(FileService fileService)
        {
            _fileService = fileService;
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Multipart.FormData)]
        [ProducesResponseType(typeof(FileResponseDto), 201)]
        public async Task<ActionResult<FileResponseDto>> UploadFile(FileUploadRequestDto request)
        {
            return await _fileService.UploadFile(request);
        }
    }
}
