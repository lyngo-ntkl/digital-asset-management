using AutoMapper;
using DigitalAssetManagement.Application.Dtos.Requests;
using DigitalAssetManagement.Application.Dtos.Responses.Folders;
using DigitalAssetManagement.Domain.Enums;
using DigitalAssetManagement.Infrastructure.PostgreSQL.DatabaseContext;

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
                .ForMember(dto => dto.Children, opt => opt.MapFrom(entity => entity.Children))
                .ForAllMembers(opt => opt.Condition((src, dest, value) => value != null));
            CreateMap<FileCreationRequest, Metadata>()
                .ForMember(entity => entity.ParentId, opt => opt.MapFrom(dto => dto.ParentId))
                .ForMember(entity => entity.Name, opt => opt.MapFrom(dto => dto.FileName))
                .ForAllMembers(opt => opt.Condition((src, dest, value) => value != null));
        }
    }
}
