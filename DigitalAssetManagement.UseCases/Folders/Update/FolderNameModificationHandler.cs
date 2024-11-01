using DigitalAssetManagement.UseCases.Common.Exceptions;
using DigitalAssetManagement.UseCases.Repositories;
using DigitalAssetManagement.Entities.DomainEntities;
using DigitalAssetManagement.UseCases.Common;

namespace DigitalAssetManagement.UseCases.Folders.Update
{
    public class FolderNameModificationHandler(MetadataRepository metadataRepository, Mapper mapper) : FolderNameModification
    {
        private readonly MetadataRepository _metadataRepository = metadataRepository;
        private readonly Mapper _mapper = mapper;

        public async Task<FolderDetailResponse> UpdateName(MetadataNameModificationRequest request)
        {
            var folder = await GetFolderAsync(request.Id);
            folder.Name = request.NewName;
            folder.AbsolutePath = AbsolutePathCreationHelper.ChangeName(request.NewName, folder.AbsolutePath);
            await _metadataRepository.UpdateAsync(folder);

            await _metadataRepository.UpdateAbsolutePathByIdsAsync(folder.Children!.Select(m => m.Id).ToList(), folder.AbsolutePath);

            return _mapper.Map<FolderDetailResponse>(folder);
        }

        private async Task<Metadata> GetFolderAsync(int id)
        {
            var metadata = await _metadataRepository.GetByIdAsync(id);
            if (metadata == null || metadata.Type != Entities.Enums.MetadataType.Folder)
            {
                throw new NotFoundException(ExceptionMessage.MetadataNotFound);
            }
            return metadata;
        }
    }
}
