﻿using DigitalAssetManagement.Application.Common.Requests;
using DigitalAssetManagement.Application.Dtos.Requests.Folders;
using DigitalAssetManagement.Application.Dtos.Responses.Folders;
using DigitalAssetManagement.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mime;

namespace DigitalAssetManagement.API.Controllers
{
    [ApiController]
    [Route("/v1/api/folders")]
    public class FoldersController : ControllerBase
    {
        private readonly FolderService _folderService;
        private readonly IAuthorizationService _authorizationService;

        public FoldersController(FolderService folderService, IAuthorizationService authorizationService)
        {
            _folderService = folderService;
            _authorizationService = authorizationService;
        }

        //[HttpGet("{id}")]
        //public async Task<FolderDetailResponseDto> Get([FromRoute] int id)
        //{
        //    return await _folderService.Get(id);
        //}

        [HttpPost]
        [ProducesResponseType<FolderDetailResponseDto>(StatusCodes.Status201Created)]
        [Authorize]
        public async Task<ActionResult<FolderDetailResponseDto>> CreateFolder([FromBody] FolderCreationRequestDto request)
        {
            await _authorizationService.AuthorizeAsync(User, request, "Contributor");
            return await _folderService.AddNewFolder(request);
        }

        //[HttpPatch("{id}")]
        //public async Task<FolderDetailResponseDto> Update([FromRoute] int id, [FromBody] FolderModificationRequestDto request)
        //{
        //    return await _folderService.Update(id, request);
        //}

        //[HttpPatch("{id}/movement")]
        //public async Task<FolderDetailResponseDto> MoveFolder([FromRoute] int id, [FromBody] FolderMovementRequestDto request)
        //{
        //    return await _folderService.MoveFolder(id, request);
        //}

        //[HttpPost("{id}/permissions")]
        //public async Task<ActionResult> AddPermission([FromRoute] int id, [FromBody] PermissionRequestDto request)
        //{
        //    await _permissionService.CreateFolderPermission(id, request);
        //    return new CreatedResult();
        //}

        //[HttpPatch("{id}/trash")]
        //public async Task MoveToTrash([FromRoute] int id)
        //{
        //    await _folderService.MoveToTrash(id);
        //}

        [HttpDelete("{id}")]
        [Authorize]
        public async Task DeleteFolder([FromRoute] int id)
        {
            await _authorizationService.AuthorizeAsync(
                User, 
                new MetadataParentRequestDto { ParentId = id}, 
                "Contributor"
            );
            await _folderService.DeleteFolder(id);
        }
    }
}