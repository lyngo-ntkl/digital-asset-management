using AutoMapper;
using DigitalAssetManagement.Entities.Enums;
using DigitalAssetManagement.Infrastructure.PostgreSQL.DatabaseContext;
using DigitalAssetManagement.UseCases.Files.Create;
using DigitalAssetManagement.UseCases.Folders;

namespace DigitalAssetManagement.Infrastructure.Mapper.MapperProfiles
{
    public class MetadataMappingProfile : Profile
    {
        public MetadataMappingProfile()
        {
            CreateMap<MetadataType, string>()
                .ConvertUsing(metadataType => metadataType.ToString());
            CreateMap<Metadata, MetadataResponse>();
            CreateMap<Metadata, FolderDetailResponse>()
                .ForMember(dto => dto.Children, opt => opt.MapFrom(entity => entity.Children))
                .ForAllMembers(opt => opt.Condition((src, dest, value) => value != null));
            CreateMap<FileCreationRequest, Metadata>()
                .ForMember(entity => entity.ParentId, opt => opt.MapFrom(dto => dto.ParentId))
                .ForMember(entity => entity.Name, opt => opt.MapFrom(dto => dto.FileName))
                .ForAllMembers(opt => opt.Condition((src, dest, value) => value != null));
        }
    }
}
