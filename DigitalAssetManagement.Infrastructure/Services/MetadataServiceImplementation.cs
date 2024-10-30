using DigitalAssetManagement.Application.Common;
using DigitalAssetManagement.Application.Common.Exceptions;
using DigitalAssetManagement.Application.Repositories;
using DigitalAssetManagement.Application.Services;
using DigitalAssetManagement.Domain.Entities;
using DigitalAssetManagement.Domain.Enums;
using DigitalAssetManagement.Infrastructure.Common;
using DigitalAssetManagement.Infrastructure.PostgreSQL.DatabaseContext;

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

        public async Task AddRange(ICollection<Metadata> metadata)
        {
            await _unitOfWork.MetadataRepository.BatchAddAsync(metadata);
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

        public async Task<bool> IsFileExist(int id)
        {
            return await _unitOfWork.MetadataRepository.ExistByConditionAsync(m => m.Id == id && m.MetadataType == MetadataType.File);
        }

        public async Task Update(Metadata metadata)
        {
            _unitOfWork.MetadataRepository.Update(metadata);
            await _unitOfWork.SaveAsync();
        }
    }
}
