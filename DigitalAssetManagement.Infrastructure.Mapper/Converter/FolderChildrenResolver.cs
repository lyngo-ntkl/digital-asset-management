using AutoMapper;
using DigitalAssetManagement.Entities.DomainEntities;
using DigitalAssetManagement.UseCases.Folders;
using DigitalAssetManagement.UseCases.Repositories;

namespace DigitalAssetManagement.Infrastructure.Mapper.Converter
{
    public class FolderChildrenResolver(IMetadataRepository metadataRepository, IMapper mapper): IValueResolver<Metadata, FolderDetailResponse, ICollection<MetadataResponse>>
    {
        private readonly IMetadataRepository _metadataRepository = metadataRepository;
        private readonly IMapper _mapper = mapper;

        public ICollection<MetadataResponse> Resolve(Metadata source, FolderDetailResponse destination, ICollection<MetadataResponse> destMember, ResolutionContext context)
        {
            var children = _metadataRepository.GetByParentIdAndNotIsDeleted(source.Id);
            return _mapper.Map<ICollection<MetadataResponse>>(children);
        }
    }
}
