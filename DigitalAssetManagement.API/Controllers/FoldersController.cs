using DigitalAssetManagement.Application.Dtos.Requests;
using DigitalAssetManagement.Application.Dtos.Requests.Folders;
using DigitalAssetManagement.Application.Dtos.Responses.Folders;
using DigitalAssetManagement.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace DigitalAssetManagement.API.Controllers
{
    [ApiController]
    [Route("/v1/api/folders")]
    public class FoldersController : ControllerBase
    {
        private readonly FolderService _folderService;
        private readonly PermissionService _permissionService;

        public FoldersController(FolderService folderService, PermissionService permissionService)
        {
            _folderService = folderService;
            _permissionService = permissionService;
        }

        [HttpGet("{id}")]
        public async Task<FolderDetailResponseDto> Get([FromRoute] int id)
        {
            return await _folderService.Get(id);
        }

        [HttpPost]
        [ProducesResponseType<FolderDetailResponseDto>(StatusCodes.Status201Created)]
        public async Task<ActionResult<FolderDetailResponseDto>> Create([FromBody] FolderCreationRequestDto request)
        {
            return await _folderService.Create(request);
        }

        [HttpPatch("{id}/movement")]
        public async Task<FolderDetailResponseDto> MoveFolder([FromRoute] int id, [FromBody] FolderMovementRequestDto request)
        {
            return await _folderService.MoveFolder(id, request);
        }

        [HttpPost("{id}/permissions")]
        public async Task<ActionResult> AddPermission([FromRoute] int id, [FromForm] PermissionRequestDto request)
        {
            await _permissionService.CreateFolderPermission(id, request);
            return new CreatedResult();
        }
    }
}