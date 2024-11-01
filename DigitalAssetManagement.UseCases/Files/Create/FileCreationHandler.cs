using DigitalAssetManagement.UseCases.Common;
using DigitalAssetManagement.Entities.DomainEntities;
using DigitalAssetManagement.Entities.Enums;
using DigitalAssetManagement.Application.Dtos.Requests;
using Newtonsoft.Json;
using System.Text;
using DigitalAssetManagement.Application.Services;
using Microsoft.Extensions.Caching.Memory;
using System.IO;
using DigitalAssetManagement.UseCases.Common.Exceptions;

namespace DigitalAssetManagement.UseCases.Files.Create
{
    public class FileCreationHandler(MetadataPermissionUnitOfWork unitOfWork, JwtHelper jwtHelper, SystemFileHelper fileHelper): FileCreation
    {
        private readonly MetadataPermissionUnitOfWork _unitOfWork = unitOfWork;
        private readonly JwtHelper _jwtHelper = jwtHelper;
        private readonly SystemFileHelper _fileHelper = fileHelper;
        private readonly IMemoryCache _fileChunkTracker;

        public async Task<int> AddFileMetadataAsync(FileCreationRequest request)
        {
            int fileId = await AddMetadataAsync(request.ParentId, request.FileName);
            await DuplicatePermissionsAsync(fileId, request.ParentId);
            return fileId;
        }

        private async Task<int> AddMetadataAsync(int parentId, string fileName)
        {
            var parent = await GetParentMetadataAsync(parentId);
            var loginUserId = int.Parse(_jwtHelper.ExtractSidFromAuthorizationHeader()!);
            var metadata = new Metadata
            {
                AbsolutePath = AbsolutePathCreationHelper.CreateAbsolutePath(fileName, parent.AbsolutePath),
                Type = MetadataType.File,
                Name = fileName,
                OwnerId = loginUserId
            };
            metadata = await _unitOfWork.MetadataRepository.AddAsync(metadata);
            return metadata.Id;
        }

        private async Task<Metadata> GetParentMetadataAsync(int id)
        {
            var metadata = await _unitOfWork.MetadataRepository.GetByIdAsync(id);
            if (metadata == null ||
                (metadata.Type != MetadataType.Folder && metadata.Type != MetadataType.Drive))
            {
                throw new NotFoundException(ExceptionMessage.FolderNotFound);
            }
            return metadata;
        }

        private async Task DuplicatePermissionsAsync(int fileId, int parentId)
        {
            var parentPermissions = _unitOfWork.PermissionRepository.GetByMetadataIdNoTracking(parentId);
            var childPermissions = new List<Permission>(parentPermissions.Count);
            foreach (var permissions in parentPermissions)
            {
                permissions.Id = 0;
                permissions.MetadataId = fileId;
                childPermissions.Add(permissions);
            }
            await _unitOfWork.PermissionRepository.AddRangeAsync(childPermissions);
        }

        public async Task ProcessFileChunkUploadAsync(FileChunkUploadRequest request)
        {
            if (request.TotalChunk == 1)
            {
                await UploadFileHavingSingleChunkAsync(request);
            }
            else
            {
                await UploadFileHavingMultipleChunkAsync(request);
            }
        }

        private async Task UploadFileHavingMultipleChunkAsync(FileChunkUploadRequest request)
        {
            var fileChunkPath = AbsolutePathCreationHelper.CreateAbsolutePath($"{request.FileId}.{request.ChunkNumber}.part");
            _fileHelper.AddFile(request.Data, fileChunkPath);
            await TrackUploadedFileChunk(request, request.FileId, request.TotalChunk, request.ChunkNumber, fileChunkPath);
        }

        private async Task TrackUploadedFileChunk(FileChunkUploadRequest fileChunk, int fileId, int totalChunk, int chunkNumber, string path)
        {
            FileChunkUploadTracking? fileChunkTracking;
            if (_fileChunkTracker.TryGetValue(fileChunk.FileId, out fileChunkTracking))
            {
                if (fileChunkTracking?.ArrivedChunks.Count == fileChunk.TotalChunk - 1)
                {
                    fileChunkTracking.ArrivedChunks.Add(fileChunk.ChunkNumber, path);
                    await MergeFileChunk(fileChunk.FileId, fileChunkTracking.ArrivedChunks);
                    _fileChunkTracker.Remove(fileChunk.FileId);
                }
                else
                {
                    var arrivedChunks = fileChunkTracking?.ArrivedChunks;
                    arrivedChunks.Add(fileChunk.ChunkNumber, path);
                    _fileChunkTracker.Set(fileChunk.FileId, fileChunkTracking);
                }
            }
            else
            {
                _fileChunkTracker.Set(
                    fileChunk.FileId,
                    new FileChunkUploadTracking
                    {
                        TotalChunk = fileChunk.TotalChunk,
                        ArrivedChunks = new Dictionary<int, string>
                    {
                                {fileChunk.ChunkNumber, path}
                            }
                    }
                );
            }
        }

        private async Task MergeFileChunk(int fileId, Dictionary<int, string> arrivedChunk)
        {
            var file = await GetFileMetadataById(fileId);
            var fileChunkPaths = arrivedChunk.OrderBy(pair => pair.Key).ToDictionary().Values.ToList();
            _fileHelper.MergeFile(file.AbsolutePath, fileChunkPaths);
        }

        private async Task UploadFileHavingSingleChunkAsync(FileChunkUploadRequest request)
        {
            var file = await GetFileMetadataById(request.FileId);
            _fileHelper.AddFile(request.Data, file.AbsolutePath);
        }

        private async Task<Metadata> GetFileMetadataById(int id)
        {
            var metadata = await _unitOfWork.MetadataRepository.GetByIdAsync(id);
            if (metadata == null || metadata.Type != MetadataType.File)
            {
                throw new NotFoundException(ExceptionMessage.MetadataNotFound);
            }
            return metadata;
        }
    }
}
