using DigitalAssetManagement.UseCases.Common;
using DigitalAssetManagement.UseCases.Files.Delete;
using DigitalAssetManagement.UseCases.Repositories;
using Microsoft.Extensions.Configuration;

namespace DigitalAssetManagement.UseCases.Files.Update
{
    public class FileSoftDeletionHandler(IMetadataRepository metadataRepository, FileDeletion fileDeletion, IScheduler scheduler, IConfiguration configuration): FileSoftDeletion
    {
        private readonly IMetadataRepository _metadataRepository = metadataRepository;
        private readonly FileDeletion _fileDeletion = fileDeletion;
        private readonly IScheduler _scheduler = scheduler;
        private readonly IConfiguration _configuration = configuration;

        public async Task DeleteFileSoftlyAsync(int fileId)
        {
            await UpdateFileMetadataIsDeletedAsync(fileId);
            _scheduler.ScheduleAfterTimeInterval(
                () => _fileDeletion.DeleteFileAsync(fileId), 
                TimeSpan.FromDays(int.Parse(_configuration["schedule:deletedWaitDays"]!))
            );
        }

        private async Task UpdateFileMetadataIsDeletedAsync(int fileId)
        {
            var metadata = await _metadataRepository.GetByIdAsync(fileId);
            if (metadata == null || metadata.Type != Entities.Enums.MetadataType.File)
            {
                throw new Exception();
            }

            metadata.IsDeleted = true;
            await _metadataRepository.UpdateAsync(metadata);
        }
    }
}
