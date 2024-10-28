﻿using DigitalAssetManagement.Domain.Entities;
using DigitalAssetManagement.Domain.Enums;

namespace DigitalAssetManagement.Application.Services
{
    public interface MetadataService
    {
        Task<Metadata> Add(Metadata metadata);
        Task<Metadata> Add(string name, string absolutePath, int ownerId, MetadataType type);
        Task AddRange(ICollection<Metadata> metadata);
        Task DeleteMetadata(Metadata metadata);
        Task<Metadata> GetById(int id);
        Task<ICollection<Metadata>> GetByAbsolutePathStartsWith(string absolutePath);
        Task<Metadata> GetFileMetadataById(int id);
        Task<Metadata> GetFolderOrDriveMetadataByIdAsync(int id);
        Task<bool> IsFileExist(int id);
        Task Update(Metadata metadata);
    }
}
