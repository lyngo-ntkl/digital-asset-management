﻿using DigitalAssetManagement.Application.Dtos.Requests;
using DigitalAssetManagement.Application.Dtos.Requests.Folders;
using DigitalAssetManagement.Application.Dtos.Responses.Folders;

namespace DigitalAssetManagement.Application.Services
{
    public interface FolderService
    {
        
        Task AddFolderPermission(int folderId, PermissionRequest request);
        //Task<FolderDetailResponseDto> Update(int id, FolderModificationRequestDto request);
        Task<FolderDetailResponseDto> Get(int id);
        Task DeleteFolder(int id);
        Task DeleteFolderSoftly(int id);
        Task MoveFolder(int folderId, int newParentId);
    }
}
