using DigitalAssetManagement.Domain.Entities;
using DigitalAssetManagement.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace DigitalAssetManagement.Infrastructure.DatabaseContext
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Drive> Drives { get; set; }
        public virtual DbSet<Folder> Folders { get; set; }
        public virtual DbSet<Domain.Entities.File> Files { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresEnum<Role>();

            modelBuilder.Entity<User>(options =>
            {
                options.HasIndex(user => user.Email)
                    .IsUnique(true);
                options.HasMany(user => user.Drives)
                    .WithOne(drive => drive.Owner)
                    .OnDelete(DeleteBehavior.Cascade);
                options.HasMany(user => user.Permissions)
                    .WithOne(permission => permission.User)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Drive>(options =>
            {
                options.HasIndex(drive => new { drive.DriveName, drive.OwnerId})
                    .IsUnique(true);
                options.HasMany(drive => drive.Files)
                    .WithOne(file => file.ParentDrive)
                    .OnDelete(DeleteBehavior.Cascade);
                options.HasMany(drive => drive.Folders)
                    .WithOne(folder => folder.ParentDrive)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Folder>(options =>
            {
                options.ToTable(table => table
                    .HasCheckConstraint("CK_Folders_Parent", $"(\"{nameof(Folder.ParentFolderId)}\" IS NOT NULL AND \"{nameof(Folder.ParentDriveId)}\" IS NULL) OR (\"{nameof(Folder.ParentFolderId)}\" IS NULL AND \"{nameof(Folder.ParentDriveId)}\" IS NOT NULL)"));
                options.HasIndex(folder => new { folder.FolderName, folder.ParentDriveId, folder.ParentFolderId })
                    .IsUnique(true)
                    .AreNullsDistinct(false);
                options.HasMany(folder => folder.SubFolders)
                    .WithOne(folder => folder.ParentFolder)
                    .OnDelete(DeleteBehavior.Cascade);
                options.HasMany(folder => folder.Permissions)
                    .WithOne(permission => permission.Folder)
                    .OnDelete(DeleteBehavior.Cascade);
                options.HasMany(folder => folder.Files)
                    .WithOne(file => file.ParentFolder)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Domain.Entities.File>(options =>
            {
                options.ToTable(table => table
                    .HasCheckConstraint("CK_Files_Parent", $"(\"{nameof(Domain.Entities.File.ParentFolderId)}\" IS NOT NULL AND \"{nameof(Domain.Entities.File.ParentDriveId)}\" IS NULL) OR (\"{nameof(Domain.Entities.File.ParentFolderId)}\" IS NULL AND \"{nameof(Domain.Entities.File.ParentDriveId)}\" IS NOT NULL)"));
                options.HasIndex(file => new {file.FileName, file.ParentDriveId, file.ParentFolderId})
                    .IsUnique(true)
                    .AreNullsDistinct(false);
                options.HasMany(file => file.Permissions)
                    .WithOne(permission => permission.File)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Permission>(options =>
            {
                options.HasIndex(permission => new { permission.UserId, permission.FileId, permission.FolderId })
                    .IsUnique(true)
                    .AreNullsDistinct(false);
                options.ToTable(table => table
                    .HasCheckConstraint("CK_Permissions_Asset", $"(\"{nameof(Permission.FolderId)}\" IS NOT NULL AND \"{nameof(Permission.FileId)}\" IS NULL) OR (\"{nameof(Permission.FolderId)}\" IS NULL AND \"{nameof(Permission.FileId)}\" IS NOT NULL)"));
            });
        }
    }
}
