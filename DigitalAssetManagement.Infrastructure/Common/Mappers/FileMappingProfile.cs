using AutoMapper;
using DigitalAssetManagement.Application.Dtos.Responses;

namespace DigitalAssetManagement.Infrastructure.Common.Mappers
{
    public partial class FileMappingProfile: Profile
    {
        public FileMappingProfile()
        {
            CreateMap<Domain.Entities.File, FileResponseDto>()
                .ForAllMembers(config => config.Condition((src, dest, srcVal) => srcVal != null));
        }
    }
}
