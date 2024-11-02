using AutoMapper;
using DigitalAssetManagement.Application.Dtos.Responses;
using DigitalAssetManagement.Domain.Entities;
using DigitalAssetManagement.Infrastructure.PostgreSQL.DatabaseContext;

namespace DigitalAssetManagement.Infrastructure.Mapper.MapperProfiles
{
    public partial class PermissionMappingProfile : Profile
    {
        public PermissionMappingProfile()
        {
            CreateMap<Permission, PermissionResponseDto>()
                .ForAllMembers(config => config.Condition((src, dest, srcVal) => srcVal != null));
        }
    }
}
