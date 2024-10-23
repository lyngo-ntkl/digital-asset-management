using AutoMapper;
using DigitalAssetManagement.Application.Dtos.Responses;
using DigitalAssetManagement.Domain.Entities;

namespace DigitalAssetManagement.Infrastructure.Common.Mappers
{
    public partial class PermissionMappingProfile: Profile
    {
        public PermissionMappingProfile()
        {
            CreateMap<Permission, PermissionResponseDto>()
                .ForAllMembers(config => config.Condition((src, dest, srcVal) => srcVal != null));
        }
    }
}
