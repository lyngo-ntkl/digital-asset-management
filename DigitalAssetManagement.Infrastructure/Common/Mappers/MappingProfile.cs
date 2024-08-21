using AutoMapper;
using DigitalAssetManagement.Application.Dtos.Requests.Users;
using DigitalAssetManagement.Domain.Entities;

namespace DigitalAssetManagement.Infrastructure.Common.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<EmailPasswordRegistrationRequest, User>()
                .AfterMap<PasswordMappingAction>()
                .ForAllMembers(options => options.Condition((src, dest, srcVal) => srcVal != null));
        }
    }
}
