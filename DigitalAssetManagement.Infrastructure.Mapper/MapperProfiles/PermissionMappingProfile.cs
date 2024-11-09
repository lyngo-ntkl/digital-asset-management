using AutoMapper;
using DigitalAssetManagement.UseCases.Permissions;

namespace DigitalAssetManagement.Infrastructure.Mapper.MapperProfiles
{
    public partial class PermissionMappingProfile : Profile
    {
        public PermissionMappingProfile()
        {
            CreateMap<Entities.DomainEntities.Permission, PostgreSQL.DatabaseContext.Permission>()
                .ReverseMap();
            CreateMap<Entities.DomainEntities.Permission, PermissionResponse>()
                .ForAllMembers(config => config.Condition((src, dest, srcVal) => srcVal != null));
        }
    }
}
