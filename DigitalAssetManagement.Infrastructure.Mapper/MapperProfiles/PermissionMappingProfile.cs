using AutoMapper;
using DigitalAssetManagement.Infrastructure.PostgreSQL.DatabaseContext;
using DigitalAssetManagement.UseCases.Permissions;

namespace DigitalAssetManagement.Infrastructure.Mapper.MapperProfiles
{
    public partial class PermissionMappingProfile : Profile
    {
        public PermissionMappingProfile()
        {
            CreateMap<Permission, PermissionResponse>()
                .ForAllMembers(config => config.Condition((src, dest, srcVal) => srcVal != null));
        }
    }
}
