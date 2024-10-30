using AutoMapper;
using DigitalAssetManagement.Application.Common;
using DigitalAssetManagement.Application.Dtos.Requests;
using DigitalAssetManagement.Application.Dtos.Responses;
using DigitalAssetManagement.Application.Common.Exceptions;
using DigitalAssetManagement.Application.Services;
using DigitalAssetManagement.Domain.Entities;
using DigitalAssetManagement.Infrastructure.Common;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using DigitalAssetManagement.Infrastructure.PostgreSQL.DatabaseContext;
using DigitalAssetManagement.UseCases.Permissions.Create;

namespace DigitalAssetManagement.Infrastructure.Services
{
    public class FileServiceImplementation: FileService
    {
        private readonly IMapper _mapper;
        private readonly JwtHelper _jwtHelper;
        private readonly SystemFileHelper _systemFileHelper;
        private readonly MetadataService _metadataService;
        private readonly PermissionService _permissionService;
        private readonly UserService _userService;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IConfiguration _configuration;

        public FileServiceImplementation(
            IMapper mapper,
            JwtHelper jwtHelper,
            SystemFileHelper systemFileHelper,
            MetadataService metadataService,
            PermissionService permissionService,
            UserService userService,
            IBackgroundJobClient backgroundJobClient,
            IConfiguration configuration)
        {
            _mapper = mapper;
            _jwtHelper = jwtHelper;
            _systemFileHelper = systemFileHelper;
            _metadataService = metadataService;
            _permissionService = permissionService;
            _userService = userService;
            _backgroundJobClient = backgroundJobClient;
            _configuration = configuration;
        }

        public async Task DeleteFile(int fileId)
        {
            var deletedFileMetadata = await _metadataService.GetFileMetadataById(fileId);
            _systemFileHelper.DeleteFile(deletedFileMetadata.AbsolutePath);
            await _metadataService.DeleteMetadata(deletedFileMetadata);
        }

        public async Task DeleteFileSoftly(int fileId)
        {
            var file = await _metadataService.GetFileMetadataById(fileId);
            file.IsDeleted = true;
            await _metadataService.Update(file);

            _backgroundJobClient.Schedule(() => DeleteFile(fileId), TimeSpan.FromDays(int.Parse(_configuration["schedule:deletedWaitDays"]!)));
        }

        public async Task<FileResponseDto> GetFile(int fileId)
        {
            var file = await _metadataService.GetFileMetadataById(fileId);
            var fileBytes = await _systemFileHelper.GetFile(file.AbsolutePath);
            return new FileResponseDto
            {
                FileContent = fileBytes,
                FileName = file.Name
            };
        }
    }
}
