﻿using DigitalAssetManagement.Application.Dtos.Responses.Folders;

namespace DigitalAssetManagement.Application.Services
{
    public interface DriveService
    {
        Task AddNewDrive(int userId);
        //Task<FolderResponseDto> GetLoginUserDrive();
    }
}
