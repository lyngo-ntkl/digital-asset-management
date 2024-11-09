using DigitalAssetManagement.Entities.DomainEntities;
using DigitalAssetManagement.UseCases.Common;
using DigitalAssetManagement.Entities.Enums;
using DigitalAssetManagement.UseCases.Common.Exceptions;
using DigitalAssetManagement.UseCases.UnitOfWork;

namespace DigitalAssetManagement.UseCases.Folders.Create
{
    public class FolderCreationHandler(
        IMetadataPermissionUnitOfWork unitOfWork,
        IMapper mapper,
        IJwtHelper jwtHelper,
        ISystemFolderHelper folderHelper) : FolderCreation
    {
        private readonly IMetadataPermissionUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IJwtHelper _jwtHelper = jwtHelper;
        private readonly ISystemFolderHelper _systemFolderHelper = folderHelper;

        public async Task<FolderDetailResponse> AddFolderAsync(FolderCreationRequest request)
        {
            var loginUserId = int.Parse(_jwtHelper.ExtractSidFromAuthorizationHeader()!);

            var folder = await AddFolderAsync(request.ParentId, request.Name, loginUserId);
            await DuplicateParentPermissionsAsync(folder.Id, request.ParentId);

            return _mapper.Map<FolderDetailResponse>(folder);
        }

        private async Task<Metadata> AddFolderAsync(int parentId, string folderName, int ownerId)
        {
            Metadata parent = await GetParentMetadataAsync(parentId);
            var absolutePath = AbsolutePathCreationHelper.CreateAbsolutePath(folderName, parent.AbsolutePath);
            var folder = await AddFolderMetadataAsync(absolutePath, folderName, ownerId, parentId);
            AddPhysicalFolder(absolutePath);
            return folder;
        }

        private async Task<Metadata> AddFolderMetadataAsync(string absolutePath, string folderName, int ownerId, int parentId)
        {
            var folder = new Metadata
            {
                Type = MetadataType.Folder,
                AbsolutePath = absolutePath,
                Name = folderName,
                OwnerId = ownerId,
                ParentId = parentId
            };
            folder = await _unitOfWork.MetadataRepository.AddAsync(folder);
            return folder;
        }

        private void AddPhysicalFolder(string absolutePath)
        {
            _systemFolderHelper.AddFolder(absolutePath);
        }

        private async Task<Metadata> GetParentMetadataAsync(int id)
        {
            var metadata = await _unitOfWork.MetadataRepository.GetByIdAsync(id);

            if (metadata == null
                || metadata.Type != MetadataType.Folder && metadata.Type != MetadataType.Drive)
            {
                throw new NotFoundException(ExceptionMessage.FolderNotFound);
            }

            return metadata;
        }

        private async Task DuplicateParentPermissionsAsync(int childId, int parentId)
        {
            var parentPermissions = _unitOfWork.PermissionRepository.GetByMetadataIdNoTracking(parentId);
            foreach (var permission in parentPermissions)
            {
                permission.Id = 0;
                permission.MetadataId = childId;
            }
            await _unitOfWork.PermissionRepository.AddRangeAsync(parentPermissions);
        }
    }
}
