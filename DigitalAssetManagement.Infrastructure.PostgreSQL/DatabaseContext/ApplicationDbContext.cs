using DigitalAssetManagement.Entities.Enums;
using Microsoft.EntityFrameworkCore;

namespace DigitalAssetManagement.Infrastructure.PostgreSQL.DatabaseContext
{
    public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
    {
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Metadata> Metadata { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<PermissionRequest> PermissionRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresEnum<Role>();
            modelBuilder.HasPostgresEnum<MetadataType>();

            modelBuilder.Entity<User>(options =>
            {
                options.HasIndex(user => user.Email)
                    .IsUnique(true);
                options.HasMany(user => user.Metadata)
                    .WithOne(metadata => metadata.Owner)
                    .OnDelete(DeleteBehavior.Cascade);
                options.HasMany(user => user.Permissions)
                    .WithOne(permission => permission.User)
                    .OnDelete(DeleteBehavior.Cascade);
                options.HasMany(user => user.PermissionRequests)
                    .WithOne(permissionRequest => permissionRequest.User)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Metadata>(options =>
            {
                options.HasIndex(metadata => metadata.AbsolutePath)
                    .IsUnique(true);
                options.HasMany(metadata => metadata.Permissions)
                    .WithOne(permission => permission.Metadata)
                    .OnDelete(DeleteBehavior.Cascade);
                options.HasMany(metadata => metadata.Children)
                    .WithOne(metadata => metadata.ParentMetadata)
                    .OnDelete(DeleteBehavior.Cascade);
                options.HasMany(metadata => metadata.PermissionRequests)
                    .WithOne(permissionRequest => permissionRequest.Metadata)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Permission>(options =>
            {
                options.HasIndex(permission => new { permission.UserId, permission.MetadataId })
                    .IsUnique(true);
            });

            modelBuilder.Entity<PermissionRequest>(options =>
            {
                options.HasIndex(permission => new { permission.UserId, permission.MetadataId })
                    .IsUnique(true);
            });
        }
    }
}
