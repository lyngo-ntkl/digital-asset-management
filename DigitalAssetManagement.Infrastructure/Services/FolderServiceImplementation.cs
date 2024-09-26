using AutoMapper;
using DigitalAssetManagement.Application.Dtos.Requests.Folders;
using DigitalAssetManagement.Application.Dtos.Responses.Folders;
using DigitalAssetManagement.Application.Services;
using DigitalAssetManagement.Domain.Entities;
using DigitalAssetManagement.Domain.Enums;
using DigitalAssetManagement.Infrastructure.Common;
using Microsoft.AspNetCore.Authorization;

namespace DigitalAssetManagement.Infrastructure.Services
{
    public class FolderServiceImplementation : FolderService
    {
        private readonly IMapper _mapper;
        private readonly JwtHelper _jwtHelper;
        private readonly SystemFolderHelper _systemFolderHelper;
        private readonly MetadataService _metadataService;
        private readonly PermissionService _permissionService;

        public FolderServiceImplementation(
            IMapper mapper, 
            JwtHelper jwtHelper,
            SystemFolderHelper systemFolderHelper, 
            MetadataService metadataService, 
            PermissionService permissionService)
        {
            _mapper = mapper;
            _jwtHelper = jwtHelper;
            _systemFolderHelper = systemFolderHelper;
            _metadataService = metadataService;
            _permissionService = permissionService;
        }

        public async Task<FolderDetailResponseDto> AddNewFolder(FolderCreationRequestDto request)
        {
            var loginUserId = int.Parse(_jwtHelper.ExtractSidFromAuthorizationHeader()!);
            Metadata parentMetadata = await _metadataService.GetById(request.ParentId);

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

            await _permissionService.DuplicatePermissions(newFolderMetadata.Id!.Value, parentMetadata.Id!.Value);

            return _mapper.Map<FolderDetailResponseDto>(newFolderMetadata);
        }

        public async Task DeleteFolder(int id)
        {
            Metadata metadata = await _metadataService.GetFolderMetadataById(id);
            _systemFolderHelper.DeleteFolder(metadata.AbsolutePath);
            await _metadataService.DeleteMetadata(metadata);
        }
    }
}
