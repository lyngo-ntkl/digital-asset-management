using AutoMapper;
using DigitalAssetManagement.Application.Common;
using DigitalAssetManagement.Application.Dtos.Requests;
using DigitalAssetManagement.Application.Exceptions;
using DigitalAssetManagement.Application.Services;
using DigitalAssetManagement.Domain.Entities;
using DigitalAssetManagement.Infrastructure.Common;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace DigitalAssetManagement.Infrastructure.Services
{
    public class FileServiceImplementation: FileService
    {
        private readonly IMapper _mapper;
        private readonly JwtHelper _jwtHelper;
        private readonly SystemFileHelper _systemFileHelper;
        private readonly MetadataService _metadataService;
        private readonly PermissionService _permissionService;
        private readonly UserService _userService;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IConfiguration _configuration;

        public FileServiceImplementation(
            IMapper mapper,
            JwtHelper jwtHelper,
            SystemFileHelper systemFileHelper,
            MetadataService metadataService,
            PermissionService permissionService,
            UserService userService,
            IBackgroundJobClient backgroundJobClient,
            IConfiguration configuration)
        {
            _mapper = mapper;
            _jwtHelper = jwtHelper;
            _systemFileHelper = systemFileHelper;
            _metadataService = metadataService;
            _permissionService = permissionService;
            _userService = userService;
            _backgroundJobClient = backgroundJobClient;
            _configuration = configuration;
        }

        public async Task AddFile(IFormFile file, Metadata parentMetadata, int ownerId)
        {
            var fileAbsolutePath = _systemFileHelper.AddFile(
                file.OpenReadStream(),
                file.FileName,
                parentMetadata.AbsolutePath
            );
            var fileMetadata = new Metadata
            {
                Name = file.FileName,
                AbsolutePath = fileAbsolutePath,
                MetadataType = Domain.Enums.MetadataType.File,
                OwnerId = ownerId,
                ParentMetadataId = parentMetadata.Id
            };
            fileMetadata = await _metadataService.Add(fileMetadata);
            //await _permissionService.DuplicatePermissions(fileMetadata.Id!.Value, parentMetadata.Id!.Value);
            await _permissionService.DuplicatePermissions(fileMetadata.Id, parentMetadata.Id);
        }

        public async Task AddFiles(MultipleFilesUploadRequestDto request)
        {
            var loginUserId = int.Parse(_jwtHelper.ExtractSidFromAuthorizationHeader()!);
            var parentMetadata = await _metadataService.GetFolderOrDriveMetadataById(request.ParentId);

            foreach ( var file in request.Files )
            {
                await AddFile(file, parentMetadata, loginUserId);
            }
        }

        public async Task AddFilePermission(int fileId, PermissionRequestDto request)
        {
            if (! await _metadataService.IsFileExist(fileId))
            {
                throw new NotFoundException(ExceptionMessage.FileNotFound);
            }

            var user = await _userService.GetByEmail(request.Email);

            var permission = await _permissionService.GetPermissionByUserIdAndMetadataId(user.Id, fileId);
            if (permission != null)
            {
                permission.Role = request.Role;
                await _permissionService.UpdatePermission(permission);
            }
            else
            {
                permission = new Permission
                {
                    MetadataId = fileId,
                    UserId = user.Id,
                    Role = request.Role,
                };
                await _permissionService.Add(permission);
            }
        }

        public async Task DeleteFile(int fileId)
        {
            var deletedFileMetadata = await _metadataService.GetFileMetadataById(fileId);
            _systemFileHelper.DeleteFile(deletedFileMetadata.AbsolutePath);
            await _metadataService.DeleteMetadata(deletedFileMetadata);
        }

        public async Task DeleteFileSoftly(int fileId)
        {
            var file = await _metadataService.GetFileMetadataById(fileId);
            file.IsDeleted = true;
            await _metadataService.Update(file);

            _backgroundJobClient.Schedule(() => DeleteFile(fileId), TimeSpan.FromDays(int.Parse(_configuration["schedule:deletedWaitDays"]!)));
        }

        public async Task MoveFile(int fileId, int newParentId)
        {
            var fileMetadata = await _metadataService.GetFileMetadataById(fileId);
            var newParentMetadata = await _metadataService.GetFolderOrDriveMetadataById(newParentId);

            var newFileAbsolutePath = _systemFileHelper.MoveFile(fileMetadata.AbsolutePath, newParentMetadata.AbsolutePath);

            fileMetadata.ParentMetadataId = newParentId;
            fileMetadata.AbsolutePath = newFileAbsolutePath;
            await _metadataService.Update(fileMetadata);

            await _permissionService.AddPermissionsWithDifferentUsers(fileId, newParentId);
        }
    }
}
