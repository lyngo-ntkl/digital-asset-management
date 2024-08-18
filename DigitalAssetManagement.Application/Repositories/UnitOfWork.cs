namespace DigitalAssetManagement.Application.Repositories
{
    public interface UnitOfWork
    {
        int Save();
        Task<int> SaveAsync();
        UserRepository UserRepository { get; }
        DriveRepository DriveRepository { get; }
        FileRepository FileRepository { get; }
        FolderRepository FolderRepository { get; }
    }
}
