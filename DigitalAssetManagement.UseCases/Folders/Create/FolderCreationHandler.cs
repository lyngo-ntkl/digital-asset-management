using DigitalAssetManagement.Entities.DomainEntities;
using DigitalAssetManagement.UseCases.Common;
using DigitalAssetManagement.Entities.Enums;
using DigitalAssetManagement.UseCases.Repositories;
using DigitalAssetManagement.UseCases.Common.Exceptions;

namespace DigitalAssetManagement.UseCases.Folders.Create
{
    public class FolderCreationHandler(
        MetadataRepository metadataRepository,
        PermissionRepository permissionRepository,
        Mapper mapper,
        JwtHelper jwtHelper,
        SystemFolderHelper folderHelper) : FolderCreation
    {
        private readonly MetadataRepository _metadataRepository = metadataRepository;
        private readonly PermissionRepository _permissionRepository = permissionRepository;
        private readonly Mapper _mapper = mapper;
        private readonly JwtHelper _jwtHelper = jwtHelper;
        private readonly SystemFolderHelper _systemFolderHelper = folderHelper;

        public async Task<FolderDetailResponse> AddFolder(FolderCreationRequest request)
        {
            var loginUserId = int.Parse(_jwtHelper.ExtractSidFromAuthorizationHeader()!);
            var folder = await AddFolderAsync(request.ParentId, request.Name, loginUserId);
            await DuplicateParentPermissionsAsync(folder.Id, request.ParentId);
            return _mapper.Map<FolderDetailResponse>(folder);
        }

        public async Task<Metadata> AddFolderAsync(int parentId, string folderName, int ownerId)
        {
            Metadata parent = await GetParentMetadataAsync(parentId);
            var absolutePath = AbsolutePathCreationHelper.CreateAbsolutePath(folderName, parent.AbsolutePath);
            var folder = new Metadata
            {
                Type = MetadataType.Folder,
                AbsolutePath = absolutePath,
                Name = folderName,
                OwnerId = ownerId,
                ParentMetadataId = parentId
            };
            folder = await _metadataRepository.AddAsync(folder);
            _systemFolderHelper.AddFolder(absolutePath);
            return folder;
        }

        public async Task<Metadata> GetParentMetadataAsync(int id)
        {
            var metadata = await _metadataRepository.GetByIdAsync(id);

            if (metadata == null
                || metadata.Type != MetadataType.Folder && metadata.Type != MetadataType.Drive)
            {
                throw new NotFoundException(ExceptionMessage.FolderNotFound);
            }

            return metadata;
        }

        public async Task DuplicateParentPermissionsAsync(int childId, int parentId)
        {
            var parentPermissions = _permissionRepository.GetByMetadataIdNoTracking(parentId);
            foreach (var permission in parentPermissions)
            {
                permission.Id = 0;
                permission.MetadataId = childId;
            }
            await _permissionRepository.AddRangeAsync(parentPermissions);
        }
    }
}
