using AutoMapper;
using DigitalAssetManagement.Application.Dtos.Requests;
using DigitalAssetManagement.Application.Dtos.Responses.Folders;
using DigitalAssetManagement.Application.Services;
using DigitalAssetManagement.Domain.Entities;
using DigitalAssetManagement.Infrastructure.Common;

namespace DigitalAssetManagement.Infrastructure.Services
{
    public class FileServiceImplementation: FileService
    {
        private const int FileBufferSize = 2048;
        private readonly IMapper _mapper;
        private readonly JwtHelper _jwtHelper;
        private readonly SystemFileHelper _systemFileHelper;
        private readonly MetadataService _metadataService;
        private readonly PermissionService _permissionService;

        public FileServiceImplementation(
            IMapper mapper,
            JwtHelper jwtHelper,
            SystemFileHelper systemFileHelper,
            MetadataService metadataService,
            PermissionService permissionService)
        {
            _mapper = mapper;
            _jwtHelper = jwtHelper;
            _systemFileHelper = systemFileHelper;
            _metadataService = metadataService;
            _permissionService = permissionService;
        }

        public async Task AddFiles(MultipleFilesUploadRequestDto request)
        {
            var loginUserId = int.Parse(_jwtHelper.ExtractSidFromAuthorizationHeader()!);
            List<Metadata> createdfileMetadataList = new List<Metadata>(request.Files.Count);
            var parentMetadata = await _metadataService.GetById(request.ParentId);
            foreach (var file in request.Files)
            {
                var fileAbsolutePath = _systemFileHelper.AddFile(
                    file.OpenReadStream(), 
                    file.FileName, 
                    parentMetadata.AbsolutePath
                );
                var fileMetadata = new Metadata { 
                    Name = file.Name, 
                    AbsolutePath = fileAbsolutePath, 
                    MetadataType = Domain.Enums.MetadataType.File,
                    OwnerId = loginUserId
                };
                createdfileMetadataList.Add(fileMetadata);
            }
            await _metadataService.AddRange(createdfileMetadataList);
        }
    }
}
