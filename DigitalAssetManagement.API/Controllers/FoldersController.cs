﻿using DigitalAssetManagement.Application.Common.Requests;
using DigitalAssetManagement.Application.Dtos.Requests;
using DigitalAssetManagement.Application.Dtos.Requests.Folders;
using DigitalAssetManagement.Application.Dtos.Responses.Folders;
using DigitalAssetManagement.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
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

        [HttpPost]
        [ProducesResponseType<FolderDetailResponseDto>(StatusCodes.Status201Created)]
        [Authorize]
        public async Task<ActionResult<FolderDetailResponseDto>> AddFolder([FromBody] FolderCreationRequestDto request)
        {
            await _authorizationService.AuthorizeAsync(User, request, "Contributor");
            return await _folderService.AddNewFolder(request);
        }

        [HttpPost("{id}/permissions")]
        public async Task AddFolderPermission([FromRoute] int id, [FromBody] PermissionRequestDto request)
        {
            await _authorizationService.AuthorizeAsync(
                User,
                new ResourceBasedPermissionCheckingRequestDto { ParentId = id },
                "Admin"
            );
            await _folderService.AddFolderPermission(id, request);
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
            await _folderService.DeleteFolder(id);
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
        public async Task<FolderDetailResponseDto> Get([FromRoute] int id)
        {
            await _authorizationService.AuthorizeAsync(User, new ResourceBasedPermissionCheckingRequestDto { ParentId = id }, "Reader");
            return await _folderService.Get(id);
        }

        [HttpPatch("move/{folderId}")]
        public async Task MoveFolder([FromRoute] int folderId, [FromQuery][Required] int newParentId)
        {
            await _authorizationService.AuthorizeAsync(User, new ResourceBasedPermissionCheckingRequestDto { ParentId = folderId }, "Admin");
            await _authorizationService.AuthorizeAsync(User, new ResourceBasedPermissionCheckingRequestDto { ParentId = newParentId }, "Admin");
            await _folderService.MoveFolder(folderId, newParentId);
        }

    }
}