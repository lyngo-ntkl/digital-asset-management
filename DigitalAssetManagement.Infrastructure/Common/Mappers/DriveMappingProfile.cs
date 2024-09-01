﻿using AutoMapper;
using DigitalAssetManagement.Application.Dtos.Responses;
using DigitalAssetManagement.Domain.Entities;

namespace DigitalAssetManagement.Infrastructure.Common.Mappers
{
    public partial class DriveMappingProfile: Profile
    {
        public DriveMappingProfile()
        {
            CreateMap<Drive, DriveResponseDto>()
                .ForAllMembers(config => config.Condition((src, dest, srcVal) => srcVal != null));
        }
    }
}