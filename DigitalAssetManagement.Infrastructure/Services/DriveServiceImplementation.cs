﻿
using AutoMapper;
using DigitalAssetManagement.Application.Dtos.Responses.Folders;
using DigitalAssetManagement.Application.Services;
using DigitalAssetManagement.Domain.Entities;
using DigitalAssetManagement.Infrastructure.Common;

namespace DigitalAssetManagement.Infrastructure.Services
{
    public class DriveServiceImplementation: DriveService
    {
        private readonly SystemFolderHelper _systemFolderHelper;
        private readonly MetadataService _metadataService;
        private readonly PermissionService _permissionService;
        private readonly IMapper _mapper;

        public DriveServiceImplementation(
            SystemFolderHelper systemFolderHelper,
            MetadataService metadataService,
            PermissionService permissionService,
            IMapper mapper)
        {
            _systemFolderHelper = systemFolderHelper;
            _metadataService = metadataService;
            _permissionService = permissionService;
            _mapper = mapper;
        }
        public async Task AddNewDrive(int ownerId, string driveName)
        {
            _systemFolderHelper.AddFolder(ownerId.ToString(), out string absolutePath);
            var metadata = await _metadataService.AddDrive(driveName, absolutePath, ownerId);
            await _permissionService.Add(new Permission
            {
                UserId = ownerId,
                MetadataId = metadata.Id,
                Role = Domain.Enums.Role.Admin
            });
        }

        public async Task<FolderDetailResponseDto> GetLoginUserDrive()
        {
            var loginUserDrive = await _metadataService.GetLoginUserDriveMetadata();
            return _mapper.Map<FolderDetailResponseDto>(loginUserDrive);
        }
    }
}
