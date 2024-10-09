using AutoMapper;
using DigitalAssetManagement.Application.Common;
using DigitalAssetManagement.Application.Dtos.Requests;
using DigitalAssetManagement.Application.Dtos.Requests.Folders;
using DigitalAssetManagement.Application.Dtos.Responses.Folders;
using DigitalAssetManagement.Application.Exceptions;
using DigitalAssetManagement.Application.Repositories;
using DigitalAssetManagement.Application.Services;
using DigitalAssetManagement.Domain.Entities;
using DigitalAssetManagement.Domain.Enums;
using DigitalAssetManagement.Infrastructure.Common;
using Hangfire;
using Microsoft.Extensions.Configuration;

namespace DigitalAssetManagement.Infrastructure.Services
{
    public class FolderServiceImplementation : FolderService
    {
        private readonly IMapper _mapper;
        private readonly JwtHelper _jwtHelper;
        private readonly SystemFolderHelper _systemFolderHelper;
        private readonly MetadataService _metadataService;
        private readonly PermissionService _permissionService;
        private readonly UserService _userService;
        private readonly UnitOfWork _unitOfWork;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IConfiguration _configuration;

        public FolderServiceImplementation(
            IMapper mapper, 
            JwtHelper jwtHelper,
            SystemFolderHelper systemFolderHelper, 
            MetadataService metadataService, 
            PermissionService permissionService,
            UserService userService,
            UnitOfWork unitOfWork,
            IBackgroundJobClient backgroundJobClient,
            IConfiguration configuration)
        {
            _mapper = mapper;
            _jwtHelper = jwtHelper;
            _systemFolderHelper = systemFolderHelper;
            _metadataService = metadataService;
            _permissionService = permissionService;
            _userService = userService;
            _unitOfWork = unitOfWork;
            _backgroundJobClient = backgroundJobClient;
            _configuration = configuration;
        }

        public async Task<FolderDetailResponseDto> AddNewFolder(FolderCreationRequestDto request)
        {
            var loginUserId = int.Parse(_jwtHelper.ExtractSidFromAuthorizationHeader()!);
            Metadata parentMetadata = await _metadataService.GetFolderOrDriveMetadataById(request.ParentId);

            _systemFolderHelper.AddFolder(request.Name, parentMetadata.AbsolutePath, out string newFolderAbsolutePath);

            var newFolderMetadata = new Metadata
            {
                Name = request.Name,
                AbsolutePath = newFolderAbsolutePath,
                MetadataType = MetadataType.Folder,
                OwnerId = loginUserId,
                ParentMetadataId = request.ParentId
            };
            newFolderMetadata = await _metadataService.Add(newFolderMetadata);

            await _permissionService.DuplicatePermissions(newFolderMetadata.Id, parentMetadata.Id);
            return _mapper.Map<FolderDetailResponseDto>(newFolderMetadata);
        }

        public async Task AddFolderPermission(int folderId, PermissionRequestDto request)
        {
            var folderMetadata = await _metadataService.GetFolderMetadataById(folderId);

            var user = await _userService.GetByEmail(request.Email);

            // TODO: refactor
            await _permissionService.AddFolderPermission(folderMetadata.AbsolutePath, user.Id, request.Role);
        }

        public async Task DeleteFolder(int id)
        {
            Metadata metadata = await _metadataService.GetFolderMetadataById(id);
            _systemFolderHelper.DeleteFolder(metadata.AbsolutePath);
            await _metadataService.DeleteMetadata(metadata);
        }

        public async Task DeleteFolderSoftly(int id)
        {
            if (! await _metadataService.IsFolderExist(id))
            {
                throw new NotFoundException(ExceptionMessage.FileNotFound);
            }

            await _unitOfWork.MetadataRepository.BatchUpdateAsync(
                m => m.SetProperty(x => x.IsDeleted, true), 
                m => m.ParentMetadataId == id || m.Id == id
            );

            _backgroundJobClient.Schedule(
                () => DeleteFolder(id), 
                TimeSpan.FromDays(int.Parse(_configuration["schedule:deletedWaitDays"]!))
            );
        }

        public async Task<FolderDetailResponseDto> Get(int id)
        {
            var folder = await _unitOfWork.MetadataRepository.GetByIdAsync(id, $"{nameof(Metadata.ChildrenMetadata)},{nameof(Metadata.Permissions)}");
            if (folder == null || folder.MetadataType != MetadataType.Folder)
            {
                throw new NotFoundException(ExceptionMessage.MetadataNotFound);
            }
            return _mapper.Map<FolderDetailResponseDto>(folder);
        }


        public async Task MoveFolder(int folderId, int newParentId)
        {
            var folder = await _metadataService.GetFolderMetadataById(folderId);
            var newParent = await _metadataService.GetFolderOrDriveMetadataById(newParentId);
            
            var newAbsolutePath = _systemFolderHelper.MoveFolder(folder.AbsolutePath, newParent.AbsolutePath);

            folder.ParentMetadataId = newParentId;
            await _metadataService.Update(folder);

            await _metadataService.UpdateFolderAbsolutePathAsync(folder.AbsolutePath, newAbsolutePath);

            var folderAndChildrenMetadataIds = new List<int> { folderId };
            folderAndChildrenMetadataIds.AddRange(_unitOfWork.MetadataRepository.GetPropertyValue(m => m.Id, m => m.ParentMetadataId == folderId));
            
            await _permissionService.DeletePermissonsByMetadataIds(folderAndChildrenMetadataIds);
            await _permissionService.DuplicatePermissions(folderAndChildrenMetadataIds, newParentId);
        }
    }
}
