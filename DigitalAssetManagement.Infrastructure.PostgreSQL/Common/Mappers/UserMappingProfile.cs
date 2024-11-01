using AutoMapper;

namespace DigitalAssetManagement.Infrastructure.Common.Mappers
{
    public partial class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<Entities.DomainEntities.User, PostgreSQL.DatabaseContext.User>();
            CreateMap<PostgreSQL.DatabaseContext.User, Entities.DomainEntities.User>();
            CreateMap<User, UserResponseDto>()
                .ForAllMembers(options => options.Condition((src, dest, srcVal) => srcVal != null));
        }
    }
}
