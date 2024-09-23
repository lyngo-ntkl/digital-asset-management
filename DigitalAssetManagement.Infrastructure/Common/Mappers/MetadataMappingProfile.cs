using AutoMapper;
using DigitalAssetManagement.Application.Dtos.Responses.Folders;
using DigitalAssetManagement.Domain.Entities;

namespace DigitalAssetManagement.Infrastructure.Common.Mappers
{
    public class MetadataMappingProfile: Profile
    {
        public MetadataMappingProfile()
        {
            CreateMap<Metadata, FolderDetailResponseDto>();
        }
    }
}
