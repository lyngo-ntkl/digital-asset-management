﻿using DigitalAssetManagement.Application.Dtos.Requests;
using DigitalAssetManagement.Application.Dtos.Responses;
using DigitalAssetManagement.Domain.Enums;

namespace DigitalAssetManagement.Application.Services
{
    public interface PermissionService
    {
        Task CreateFolderPermission(int fileIdOrFolderId, PermissionRequestDto request);
        Task CreatePermission(int userId, int assetId, int? folderId, int? driveId, bool isFile = true);
        Task<bool> HasReaderPermission(int userId, int fileIdOrFolderIdOrDriveId, bool isFile = false, bool isDrive = false);
        Task<bool> HasAdminPermission(int userId, int fileIdOrDriveIdOrFolderId, bool isFile = false, bool isDrive = false);
        Task<bool> HasModifiedPermission(int userId, int fileIdOrDriveIdOrFolderId, bool isFile = false, bool isDrive = false);
    }
}