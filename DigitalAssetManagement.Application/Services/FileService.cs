﻿using DigitalAssetManagement.Application.Dtos.Requests;

namespace DigitalAssetManagement.Application.Services
{
    public interface FileService
    {
        Task AddFiles(MultipleFilesUploadRequestDto request);
        Task AddFilePermission(int fileId, PermissionRequestDto request);
        Task DeleteFile(int fileId);
        Task DeleteFileSoftly(int fileId);
        Task MoveFile(int fileId, int newParentId);
    }
}
