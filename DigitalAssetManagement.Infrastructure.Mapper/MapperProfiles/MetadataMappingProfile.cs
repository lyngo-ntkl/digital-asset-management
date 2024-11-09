using AutoMapper;
using DigitalAssetManagement.Entities.Enums;
using DigitalAssetManagement.Infrastructure.Mapper.Converter;
using DigitalAssetManagement.UseCases.Files.Create;
using DigitalAssetManagement.UseCases.Folders;

namespace DigitalAssetManagement.Infrastructure.Mapper.MapperProfiles
{
    public class MetadataMappingProfile : Profile
    {
        public MetadataMappingProfile()
        {
            CreateMap<Entities.DomainEntities.Metadata, PostgreSQL.DatabaseContext.Metadata>().ReverseMap();
            CreateMap<MetadataType, string>()
                .ConvertUsing(metadataType => metadataType.ToString());
            CreateMap<Entities.DomainEntities.Metadata, MetadataResponse>();
            CreateMap<Entities.DomainEntities.Metadata, FolderDetailResponse>()
                .ForMember(dto => dto.Children, opt => opt.MapFrom<FolderChildrenResolver>())
                .ForAllMembers(opt => opt.Condition((src, dest, value) => value != null));
            CreateMap<FileCreationRequest, Entities.DomainEntities.Metadata>()
                .ForMember(entity => entity.ParentId, opt => opt.MapFrom(dto => dto.ParentId))
                .ForMember(entity => entity.Name, opt => opt.MapFrom(dto => dto.FileName))
                .ForAllMembers(opt => opt.Condition((src, dest, value) => value != null));
        }
    }
}
