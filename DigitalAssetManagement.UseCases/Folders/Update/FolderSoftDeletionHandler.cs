using DigitalAssetManagement.UseCases.Common.Exceptions;
using DigitalAssetManagement.UseCases.Common;
using Microsoft.Extensions.Configuration;
using DigitalAssetManagement.UseCases.Folders.Delete;

namespace DigitalAssetManagement.UseCases.Folders.Update
{
    public class FolderSoftDeletionHandler(MetadataPermissionUnitOfWork metadataUnitOfWork, FolderDeletion folderDeletion, Scheduler scheduler, IConfiguration configuration): FolderSoftDeletion
    {
        private readonly MetadataPermissionUnitOfWork _unitOfWork = metadataUnitOfWork;
        private readonly FolderDeletion _folderDeletion = folderDeletion;
        private readonly Scheduler _scheduler = scheduler;
        private readonly IConfiguration _configuration = configuration;

        public async Task DeleteFolderSoftly(int folderId)
        {
            if (!await _unitOfWork.MetadataRepository.ExistByIdAndTypeAsync(folderId, Entities.Enums.MetadataType.Folder))
            {
                throw new NotFoundException(ExceptionMessage.FolderNotFound);
            }
            await _unitOfWork.MetadataRepository.UpdateIsDeletedByIdAsync(folderId);
            await _unitOfWork.MetadataRepository.UpdateIsDeletedByParentIdAsync(folderId);

            _scheduler.ScheduleAfterTimeInterval(
                () => _folderDeletion.DeleteFolder(folderId),
                TimeSpan.FromDays(int.Parse(_configuration["schedule:deletedWaitDays"]!))
            );
        }

    }
}
