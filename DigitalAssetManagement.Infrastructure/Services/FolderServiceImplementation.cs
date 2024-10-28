using AutoMapper;
using DigitalAssetManagement.Application.Common;
using DigitalAssetManagement.Application.Dtos.Requests;
using DigitalAssetManagement.Application.Dtos.Requests.Folders;
using DigitalAssetManagement.Application.Dtos.Responses.Folders;
using DigitalAssetManagement.Application.Common.Exceptions;
using DigitalAssetManagement.Application.Repositories;
using DigitalAssetManagement.Application.Services;
using DigitalAssetManagement.Domain.Entities;
using DigitalAssetManagement.Domain.Enums;
using DigitalAssetManagement.Infrastructure.Common;
using Hangfire;
using Microsoft.Extensions.Configuration;
using DigitalAssetManagement.Infrastructure.PostgreSQL.DatabaseContext;

namespace DigitalAssetManagement.Infrastructure.Services
{
    public class FolderServiceImplementation : FolderService
    {
        private readonly IMapper _mapper;
        private readonly JwtHelper _jwtHelper;
        private readonly SystemFolderHelper _systemFolderHelper;
        private readonly MetadataService _metadataService;
        private readonly PermissionService _permissionService;
        private readonly UserService _userService;
        private readonly UnitOfWork _unitOfWork;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IConfiguration _configuration;

        public FolderServiceImplementation(
            IMapper mapper, 
            JwtHelper jwtHelper,
            SystemFolderHelper systemFolderHelper, 
            MetadataService metadataService, 
            PermissionService permissionService,
            UserService userService,
            UnitOfWork unitOfWork,
            IBackgroundJobClient backgroundJobClient,
            IConfiguration configuration)
        {
            _mapper = mapper;
            _jwtHelper = jwtHelper;
            _systemFolderHelper = systemFolderHelper;
            _metadataService = metadataService;
            _permissionService = permissionService;
            _userService = userService;
            _unitOfWork = unitOfWork;
            _backgroundJobClient = backgroundJobClient;
            _configuration = configuration;
        }

        public async Task DeleteFolderSoftly(int id)
        {
            if (! await _metadataService.IsFolderExist(id))
            {
                throw new NotFoundException(ExceptionMessage.FileNotFound);
            }

            await _unitOfWork.MetadataRepository.BatchUpdateAsync(
                m => m.SetProperty(x => x.IsDeleted, true), 
                m => m.ParentMetadataId == id || m.Id == id
            );

            _backgroundJobClient.Schedule(
                () => DeleteFolder(id), 
                TimeSpan.FromDays(int.Parse(_configuration["schedule:deletedWaitDays"]!))
            );
        }
    }
}
