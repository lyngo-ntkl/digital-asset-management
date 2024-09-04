using AutoMapper;
using DigitalAssetManagement.Application.Dtos.Requests.Folders;
using DigitalAssetManagement.Application.Dtos.Responses.Folders;
using DigitalAssetManagement.Domain.Entities;

namespace DigitalAssetManagement.Infrastructure.Common.Mappers
{
    public partial class FolderMappingProfile: Profile
    {
        public FolderMappingProfile()
        {
            CreateMap<Folder, FolderDetailResponseDto>()
                .ForAllMembers(config => config.Condition((src, dest, srcVal) => srcVal != null));
            CreateMap<Folder, FolderResponseDto>()
                .ForAllMembers(config => config.Condition((src, dest, srcVal) => srcVal != null));
            CreateMap<FolderCreationRequestDto, Folder>()
                .ForAllMembers(config => config.Condition((src, dest, srcVal) => srcVal != null));
            CreateMap<FolderMovementRequestDto, Folder>();
            CreateMap<FolderModificationRequestDto, Folder>()
                .ForAllMembers(config => config.Condition((src, dest, srcVal) => srcVal != null));
        }
    }
}
