
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

        public DriveServiceImplementation(
            SystemFolderHelper systemFolderHelper,
            MetadataService metadataService,
            PermissionService permissionService)
        {
            _systemFolderHelper = systemFolderHelper;
            _metadataService = metadataService;
            _permissionService = permissionService;
        }
        public async Task AddNewDrive(int userId)
        {
            _systemFolderHelper.AddFolder(userId.ToString(), out string absolutePath);
            var metadata = await _metadataService.AddDrive(userId.ToString(), absolutePath, userId);
            await _permissionService.Add(new Permission
            {
                UserId = userId,
                MetadataId = metadata.Id!.Value,
                Role = Domain.Enums.Role.Admin
            });
        }

        //public async Task<FolderResponseDto> GetLoginUserDrive()
        //{
        //    _metadataService.
        //}
    }
}
