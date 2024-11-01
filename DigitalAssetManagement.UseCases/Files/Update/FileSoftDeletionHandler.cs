using DigitalAssetManagement.UseCases.Common;
using DigitalAssetManagement.UseCases.Files.Delete;
using DigitalAssetManagement.UseCases.Repositories;
using Microsoft.Extensions.Configuration;

namespace DigitalAssetManagement.UseCases.Files.Update
{
    public class FileSoftDeletionHandler(MetadataRepository metadataRepository, FileDeletion fileDeletion, Scheduler scheduler, IConfiguration configuration): FileSoftDeletion
    {
        private readonly MetadataRepository _metadataRepository = metadataRepository;
        private readonly FileDeletion _fileDeletion = fileDeletion;
        private readonly Scheduler _scheduler = scheduler;
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
