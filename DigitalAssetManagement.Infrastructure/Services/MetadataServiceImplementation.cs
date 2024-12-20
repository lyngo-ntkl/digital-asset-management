﻿using DigitalAssetManagement.Application.Common;
using DigitalAssetManagement.Application.Exceptions;
using DigitalAssetManagement.Application.Repositories;
using DigitalAssetManagement.Application.Services;
using DigitalAssetManagement.Domain.Entities;
using DigitalAssetManagement.Domain.Enums;
using DigitalAssetManagement.Infrastructure.Common;

namespace DigitalAssetManagement.Infrastructure.Services
{
    public class MetadataServiceImplementation : MetadataService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly JwtHelper _jwtHelper;

        public MetadataServiceImplementation(UnitOfWork unitOfWork, JwtHelper jwtHelper)
        {
            _unitOfWork = unitOfWork;
            _jwtHelper = jwtHelper;
        }

        public async Task<Metadata> Add(Metadata metadata)
        {
            metadata = await _unitOfWork.MetadataRepository.AddAsync(metadata);
            await _unitOfWork.SaveAsync();
            return metadata;
        }

        public async Task<Metadata> Add(string name, string absolutePath, int ownerId, MetadataType type)
        {
            var newMetadata = new Metadata
            {
                Name = name,
                AbsolutePath = absolutePath,
                MetadataType = type,
                OwnerId = ownerId
            };
            newMetadata = await _unitOfWork.MetadataRepository.AddAsync(newMetadata);
            await _unitOfWork.SaveAsync();
            return newMetadata;
        }

        public async Task<Metadata> AddDrive(string name, string absolutePath, int ownerId)
        {
            var newDriveMetadata = new Metadata
            {
                Name = name,
                AbsolutePath = absolutePath,
                MetadataType = MetadataType.UserDrive,
                OwnerId = ownerId
            };
            newDriveMetadata = await _unitOfWork.MetadataRepository.AddAsync(newDriveMetadata);
            await _unitOfWork.SaveAsync();
            return newDriveMetadata;
        }

        public async Task AddRange(ICollection<Metadata> metadata)
        {
            await _unitOfWork.MetadataRepository.BatchAddAsync(metadata);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteMetadata(Metadata metadata)
        {
            _unitOfWork.MetadataRepository.Delete(metadata);
            await _unitOfWork.SaveAsync();
        }

        public async Task<Metadata> GetById(int id)
        {
            var metadata = await _unitOfWork.MetadataRepository.GetByIdAsync(id);
            if (metadata == null)
            {
                throw new NotFoundException(ExceptionMessage.MetadataNotFound);
            }
            return metadata;
        }

        public async Task<ICollection<Metadata>> GetByAbsolutePathStartsWith(string absolutePath)
        {
            return await _unitOfWork.MetadataRepository.GetAllAsync(
                m => m.AbsolutePath.StartsWith(absolutePath)
            );
        }

        public async Task<Metadata> GetFileMetadataById(int id)
        {
            var metadata = await _unitOfWork.MetadataRepository.GetByIdAsync(id);
            if (metadata == null || metadata.MetadataType != MetadataType.File)
            {
                throw new NotFoundException(ExceptionMessage.MetadataNotFound);
            }
            return metadata;
        }

        public async Task<Metadata> GetFolderMetadataById(int id)
        {
            var metadata = await _unitOfWork.MetadataRepository.GetByIdAsync(id);
            if (metadata == null || metadata.MetadataType != MetadataType.Folder)
            {
                throw new NotFoundException(ExceptionMessage.MetadataNotFound);
            }
            return metadata;
        }

        public async Task<Metadata> GetFolderOrDriveMetadataById(int id)
        {
            var metadata = await _unitOfWork.MetadataRepository.GetByIdAsync(id);
            if (metadata == null || (metadata.MetadataType != MetadataType.Folder && metadata.MetadataType != MetadataType.UserDrive))
            {
                throw new NotFoundException(ExceptionMessage.FolderNotFound);
            }
            return metadata;
        }

        public async Task<Metadata?> GetLoginUserDriveMetadata()
        {
            var loginUserId = int.Parse(_jwtHelper.ExtractSidFromAuthorizationHeader()!);
            var driveMetadata = await _unitOfWork.MetadataRepository.GetAllAsync(
                filter: m => m.OwnerId == loginUserId && m.MetadataType == MetadataType.UserDrive,
                includedProperties: $"{nameof(Metadata.ChildrenMetadata)}"
            );
            return driveMetadata.FirstOrDefault();
        }

        public async Task<bool> IsFileExist(int id)
        {
            return await _unitOfWork.MetadataRepository.ExistByConditionAsync(m => m.Id == id && m.MetadataType == MetadataType.File);
        }

        public async Task<bool> IsFolderExist(int id)
        {
            return await _unitOfWork.MetadataRepository.ExistByConditionAsync(m => m.Id == id && m.MetadataType == MetadataType.Folder);
        }

        public async Task Update(Metadata metadata)
        {
            _unitOfWork.MetadataRepository.Update(metadata);
            await _unitOfWork.SaveAsync();
        }

        public async Task<int> UpdateFolderAbsolutePathAsync(string oldFolderAbsolutePath, string newFolderAbsolutePath)
        {
            return await _unitOfWork.MetadataRepository.BatchUpdateAsync(
                m => m.SetProperty(
                    x => x.AbsolutePath,
                    x => $"{newFolderAbsolutePath}/{x.Name}"
                ),
                filter: m => m.AbsolutePath.StartsWith(oldFolderAbsolutePath)
            );
        }
    }
}
