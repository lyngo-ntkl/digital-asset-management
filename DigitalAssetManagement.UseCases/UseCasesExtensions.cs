using DigitalAssetManagement.UseCases.Files.Create;
using DigitalAssetManagement.UseCases.Files.Delete;
using DigitalAssetManagement.UseCases.Files.Read;
using DigitalAssetManagement.UseCases.Files.Update;
using DigitalAssetManagement.UseCases.Folders.Create;
using DigitalAssetManagement.UseCases.Folders.Delete;
using DigitalAssetManagement.UseCases.Folders.Read;
using DigitalAssetManagement.UseCases.Folders.Update;
using DigitalAssetManagement.UseCases.Permissions.Create;
using DigitalAssetManagement.UseCases.UnitOfWork;
using DigitalAssetManagement.UseCases.Users.Create;
using DigitalAssetManagement.UseCases.Users.Login;
using DigitalAssetManagement.UseCases.Users.Read;
using Microsoft.Extensions.DependencyInjection;

namespace DigitalAssetManagement.UseCases
{
    public static class UseCasesExtensions
    {
        public static void AddUseCases(this IServiceCollection services)
        {
            services.AddScoped<UserRegistration, UserRegistrationHandler>();
            services.AddScoped<LoginByEmailPassword, LoginByEmailPasswordHandler>();
            services.AddScoped<GetUsers, GetUsersHandler>();

            services.AddScoped<FileDeletion, FileDeletionHandler>();
            services.AddScoped<GetFile, GetFileHandler>();
            services.AddScoped<FileSoftDeletion, FileSoftDeletionHandler>();
            services.AddScoped<MoveFile, MoveFileHandler>();
            services.AddScoped<FileCreation, FileCreationHandler>();
            services.AddHostedService<FileContentTransferBackgroundService>();

            services.AddScoped<FolderCreation, FolderCreationHandler>();
            services.AddScoped<FolderDeletion, FolderDeletionHandler>();
            services.AddScoped<IGetDrive, GetDriveHandler>();
            services.AddScoped<GetFolder, GetFolderHandler>();
            services.AddScoped<FolderNameModification, FolderNameModificationHandler>();
            services.AddScoped<MoveFolderToTrash, MoveFolderToTrashHandler>();
            services.AddScoped<MoveFolder, MoveFolderHandler>();

            services.AddScoped<FilePermissionCreation, FilePermissionCreationHandler>();
            services.AddScoped<FolderPermissionCreation, FolderPermissionCreationHandler>();

            services.AddScoped<IMetadataPermissionUnitOfWork, MetadataPermissionUnitOfWorkImplementation>();
        }
    }
}
