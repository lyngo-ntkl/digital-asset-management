using AutoMapper;
using DigitalAssetManagement.Application.Dtos.Requests.Drives;
using DigitalAssetManagement.Application.Dtos.Responses.Drives;
using DigitalAssetManagement.Domain.Entities;

namespace DigitalAssetManagement.Infrastructure.Common.Mappers
{
    public partial class DriveMappingProfile: Profile
    {
        public DriveMappingProfile()
        {
            CreateMap<DriveRequestDto, Drive>()
                .ForAllMembers(config => config.Condition((src, dest, srcVal) => srcVal != null));
            CreateMap<Drive, DriveResponseDto>()
                .ForAllMembers(config => config.Condition((src, dest, srcVal) => srcVal != null));
            CreateMap<Drive, DriveDetailsResponseDto>()
                .ForAllMembers(config => config.Condition((src, dest, srcVal) => srcVal != null));
        }
    }
}
