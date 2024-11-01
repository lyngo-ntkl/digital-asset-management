using DigitalAssetManagement.UseCases.Common.Exceptions;
using DigitalAssetManagement.Entities.Enums;
using DigitalAssetManagement.UseCases.Common;
using DigitalAssetManagement.UseCases.Repositories;

namespace DigitalAssetManagement.UseCases.Folders.Read
{
    public class GetFolderHandler(MetadataRepository metadataRepository, Mapper mapper) : GetFolder
    {
        private readonly MetadataRepository _metadataRepository = metadataRepository;
        private readonly Mapper _mapper = mapper;

        public async Task<FolderDetailResponse> GetFolder(int id)
        {
            var folder = await _metadataRepository.GetByIdAsync(id);
            if (folder == null || folder.Type != MetadataType.Folder)
            {
                throw new NotFoundException(ExceptionMessage.MetadataNotFound);
            }
            return _mapper.Map<FolderDetailResponse>(folder);
        }
    }
}
