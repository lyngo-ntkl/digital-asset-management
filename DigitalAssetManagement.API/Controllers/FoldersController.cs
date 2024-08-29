using DigitalAssetManagement.Application.Dtos.Requests;
using DigitalAssetManagement.Application.Dtos.Responses;
using DigitalAssetManagement.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace DigitalAssetManagement.API.Controllers
{
    [ApiController]
    [Route("/v1/api/folders")]
    public class FoldersController : ControllerBase
    {
        private readonly FolderService _folderService;

        public FoldersController(FolderService folderService)
        {
            _folderService = folderService;
        }

        [HttpPost]
        public async Task<FolderDetailResponseDto> Create(FolderCreationRequestDto request)
        {
            return await _folderService.Create(request);
        }
    }
}