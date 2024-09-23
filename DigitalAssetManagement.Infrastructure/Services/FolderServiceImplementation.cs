using AutoMapper;
using DigitalAssetManagement.Application.Common;
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
        private readonly SystemFolderHelper _systemFolderHelper;
        private readonly MetadataService _metadataService;
        private readonly UserService _userService;
        private readonly PermissionService _permissionService;

        public FolderServiceImplementation(IMapper mapper, SystemFolderHelper systemFolderHelper, MetadataService metadataService, UserService userService, PermissionService permissionService)
        {
            _mapper = mapper;
            _systemFolderHelper = systemFolderHelper;
            _metadataService = metadataService;
            _userService = userService;
            _permissionService = permissionService;
        }

        // done
        public async Task<FolderDetailResponseDto> AddNewFolder(FolderCreationRequestDto request)
        {
            // TODO: customize authorization & remove this part
            User loginUser = await _userService.GetLoginUserAsync();
            //if (!await _permissionService.HasPermission(Role.Contributor, userId: loginUser.Id!.Value, request.ParentId))
            //{
            //    throw new ForbiddenException(ExceptionMessage.UnallowedModification);
            //}

            Metadata parentMetadata;
            if (request.ParentId != null)
            {
                parentMetadata = await _metadataService.GetById(request.ParentId.Value);
            }
            else
            {
                parentMetadata = await _metadataService.GetUserDrive(loginUser.Id!.Value);
            }
            _systemFolderHelper.AddFolder(request.Name, parentMetadata.AbsolutePath, out string newFolderAbsolutePath);

            var newFolderMetadata = new Metadata
            {
                Name = request.Name,
                AbsolutePath = newFolderAbsolutePath,
                MetadataType = MetadataType.Folder,
                OwnerId = loginUser.Id!.Value
            };
            newFolderMetadata = await _metadataService.Add(newFolderMetadata);

            await _permissionService.DuplicatePermissions(newFolderMetadata.Id!.Value, parentMetadata.Id!.Value);

            return _mapper.Map<FolderDetailResponseDto>(parentMetadata);
        }
    }
}
