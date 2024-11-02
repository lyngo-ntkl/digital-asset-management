using AutoMapper;

namespace DigitalAssetManagement.Infrastructure.Mapper.MapperProfiles
{
    public partial class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<Entities.DomainEntities.User, PostgreSQL.DatabaseContext.User>();
            CreateMap<PostgreSQL.DatabaseContext.User, Entities.DomainEntities.User>();
            CreateMap<Entities.DomainEntities.User, UseCases.Users.Read.UserResponse>()
                .ForAllMembers(options => options.Condition((src, dest, srcVal) => srcVal != null));
        }
    }
}
