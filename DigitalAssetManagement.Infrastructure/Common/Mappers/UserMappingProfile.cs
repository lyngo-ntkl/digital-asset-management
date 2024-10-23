using AutoMapper;
using DigitalAssetManagement.Application.Dtos.Requests;
using DigitalAssetManagement.Application.Dtos.Requests.Users;
using DigitalAssetManagement.Application.Dtos.Responses;
using DigitalAssetManagement.Application.Dtos.Responses.Users;
using DigitalAssetManagement.Domain.Entities;

namespace DigitalAssetManagement.Infrastructure.Common.Mappers
{
    public partial class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<EmailPasswordRegistrationRequest, User>()
                .AfterMap<PasswordMappingAction>()
                .ForAllMembers(options => options.Condition((src, dest, srcVal) => srcVal != null));
            CreateMap<User, UserResponseDto>()
                .ForAllMembers(options => options.Condition((src, dest, srcVal) => srcVal != null));
        }
    }
}
