using AutoMapper;
using DigitalAssetManagement.Application.Dtos.Responses.Folders;
using DigitalAssetManagement.Domain.Entities;
using DigitalAssetManagement.Domain.Enums;

namespace DigitalAssetManagement.Infrastructure.Common.Mappers
{
    public class MetadataMappingProfile: Profile
    {
        public MetadataMappingProfile()
        {
            CreateMap<MetadataType, string>()
                .ConvertUsing(metadataType => metadataType.ToString());
            CreateMap<Metadata, MetadataResponseDto>();
            CreateMap<Metadata, FolderDetailResponseDto>()
                .ForMember(dto => dto.Children, opt => opt.MapFrom(entity => entity.ChildrenMetadata))
                .ForAllMembers(opt => opt.Condition((src, dest, value) => value != null));
        }
    }
}
