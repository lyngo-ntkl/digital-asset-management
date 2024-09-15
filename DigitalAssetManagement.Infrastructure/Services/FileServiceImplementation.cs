using AutoMapper;
using DigitalAssetManagement.Application.Common;
using DigitalAssetManagement.Application.Dtos.Requests.Files;
using DigitalAssetManagement.Application.Dtos.Responses.Files;
using DigitalAssetManagement.Application.Exceptions;
using DigitalAssetManagement.Application.Repositories;
using DigitalAssetManagement.Application.Services;
using DigitalAssetManagement.Domain.Entities;
using DigitalAssetManagement.Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace DigitalAssetManagement.Infrastructure.Services
{
    public class FileServiceImplementation: FileService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly FileHelper _fileHelper;
        private readonly PermissionService _permissionService;

        public FileServiceImplementation(UnitOfWork unitOfWork, IMapper mapper, FileHelper fileHelper, PermissionService permissionService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileHelper = fileHelper;
            _permissionService = permissionService;
        }

        public async Task<FileResponseDto> UploadFile(FileUploadRequestDto request)
        {
            // TODO: refactor, custom authorization attribute
            if (! await _permissionService.HasPermissionLoginUser(Domain.Enums.Role.Contributor, request.DriveId ?? request.FolderId!.Value, request.DriveId != null ? typeof(Drive) : typeof(Folder))) {
                throw new ForbiddenException(ExceptionMessage.UnallowedPermissionCreation);
            }

            // TODO: refactor
            LTree? parentPath = null;
            if (request.DriveId != null)
            {
                var drive = await _unitOfWork.DriveRepository.GetByIdAsync(request.DriveId.Value);
                if (drive == null)
                {
                    throw new NotFoundException(ExceptionMessage.DriveNotFound);
                }
                parentPath = drive.HierarchicalPath;
            }
            if (request.FolderId != null)
            {
                var folder = await _unitOfWork.FolderRepository.GetByIdAsync(request.FolderId.Value);
                if (folder == null)
                {
                    throw new NotFoundException(ExceptionMessage.FolderNotFound);
                }
                parentPath = folder.HierarchicalPath;
            }

            var filePath = _fileHelper.UploadFile(request.File.OpenReadStream(), request.File.FileName, parentPath.ToString()!);
            // TODO: remove FileContent, just keep LTree
            Domain.Entities.File file = new Domain.Entities.File
            {
                FileContent = filePath,
                FileName = request.File.FileName,
                HierarchicalPath = new LTree(Regex.Replace(filePath, "/", "."))
            };
            if (request.DriveId != null)
            {
                file.ParentDriveId = request.DriveId.Value;
            }
            if (request.FolderId != null)
            {
                file.ParentFolderId = request.FolderId.Value;
            }

            file = await _unitOfWork.FileRepository.InsertAsync(file);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<FileResponseDto>(file);
        }
    }
}
