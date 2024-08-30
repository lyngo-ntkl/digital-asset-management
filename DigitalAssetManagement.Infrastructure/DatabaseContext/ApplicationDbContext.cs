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
            });

            modelBuilder.Entity<Drive>(options =>
            {
                options.HasIndex(drive => new { drive.DriverName, drive.OwnerId})
                    .IsUnique(true);
            });

            modelBuilder.Entity<Folder>(options =>
            {
                options.ToTable(table => table
                    .HasCheckConstraint("CK_Folders_Parent", $"(\"{nameof(Folder.ParentFolderId)}\" IS NOT NULL AND \"{nameof(Folder.ParentDriveId)}\" IS NULL) OR (\"{nameof(Folder.ParentFolderId)}\" IS NULL AND \"{nameof(Folder.ParentDriveId)}\" IS NOT NULL)"));
                options.HasIndex(folder => new { folder.FolderName, folder.ParentDriveId, folder.ParentFolderId })
                    .IsUnique(true)
                    .AreNullsDistinct(false);
            });

            modelBuilder.Entity<Domain.Entities.File>(options =>
            {
                options.ToTable(table => table
                    .HasCheckConstraint("CK_Files_Parent", $"(\"{nameof(Domain.Entities.File.ParentFolderId)}\" IS NOT NULL AND \"{nameof(Domain.Entities.File.ParentDriveId)}\" IS NULL) OR (\"{nameof(Domain.Entities.File.ParentFolderId)}\" IS NULL AND \"{nameof(Domain.Entities.File.ParentDriveId)}\" IS NOT NULL)"));
                options.HasIndex(file => new {file.FileName, file.ParentDriveId, file.ParentFolderId})
                    .IsUnique(true)
                    .AreNullsDistinct(false);
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
