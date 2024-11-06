using DigitalAssetManagement.UseCases.Common.Exceptions;
using DigitalAssetManagement.UseCases.Common;
using Microsoft.Extensions.Configuration;
using DigitalAssetManagement.UseCases.Folders.Delete;
using DigitalAssetManagement.UseCases.UnitOfWork;

namespace DigitalAssetManagement.UseCases.Folders.Update
{
    public class MoveFolderToTrashHandler(
        IMetadataPermissionUnitOfWork metadataUnitOfWork, 
        FolderDeletion folderDeletion, 
        IScheduler scheduler, 
        IConfiguration configuration): MoveFolderToTrash
    {
        private readonly IMetadataPermissionUnitOfWork _unitOfWork = metadataUnitOfWork;
        private readonly FolderDeletion _folderDeletion = folderDeletion;
        private readonly IScheduler _scheduler = scheduler;
        private readonly IConfiguration _configuration = configuration;

        public async Task MoveFolderToTrashAsync(int folderId)
        {
            if (!await _unitOfWork.MetadataRepository.ExistByIdAndTypeAsync(folderId, Entities.Enums.MetadataType.Folder))
            {
                throw new NotFoundException(ExceptionMessage.FolderNotFound);
            }
            await _unitOfWork.MetadataRepository.UpdateIsDeletedByIdAsync(folderId);
            await _unitOfWork.MetadataRepository.UpdateIsDeletedByParentIdAsync(folderId);

            ScheduleDeleteAfterTime(folderId);
        }

        private void ScheduleDeleteAfterTime(int folderId)
        {
            _scheduler.ScheduleAfterTimeInterval(
                () => _folderDeletion.DeleteFolderAsync(folderId),
                TimeSpan.FromDays(int.Parse(_configuration["schedule:deletedWaitDays"]!))
            );
        }
    }
}
