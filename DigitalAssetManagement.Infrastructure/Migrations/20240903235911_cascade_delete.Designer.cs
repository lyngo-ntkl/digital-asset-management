﻿// <auto-generated />
using System;
using DigitalAssetManagement.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DigitalAssetManagement.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240903235911_cascade_delete")]
    partial class cascade_delete
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresEnum(modelBuilder, "role", new[] { "admin", "contributor", "reader" });
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("DigitalAssetManagement.Domain.Entities.Drive", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int?>("Id"));

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("DriveName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("OwnerId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.HasIndex("DriveName", "OwnerId")
                        .IsUnique();

                    b.ToTable("Drives");
                });

            modelBuilder.Entity("DigitalAssetManagement.Domain.Entities.File", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int?>("Id"));

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("FileContent")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int?>("ParentDriveId")
                        .HasColumnType("integer");

                    b.Property<int?>("ParentFolderId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ParentDriveId");

                    b.HasIndex("ParentFolderId");

                    b.HasIndex("FileName", "ParentDriveId", "ParentFolderId")
                        .IsUnique();

                    NpgsqlIndexBuilderExtensions.AreNullsDistinct(b.HasIndex("FileName", "ParentDriveId", "ParentFolderId"), false);

                    b.ToTable("Files", t =>
                        {
                            t.HasCheckConstraint("CK_Files_Parent", "(\"ParentFolderId\" IS NOT NULL AND \"ParentDriveId\" IS NULL) OR (\"ParentFolderId\" IS NULL AND \"ParentDriveId\" IS NOT NULL)");
                        });
                });

            modelBuilder.Entity("DigitalAssetManagement.Domain.Entities.Folder", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int?>("Id"));

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("FolderName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int?>("ParentDriveId")
                        .HasColumnType("integer");

                    b.Property<int?>("ParentFolderId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ParentDriveId");

                    b.HasIndex("ParentFolderId");

                    b.HasIndex("FolderName", "ParentDriveId", "ParentFolderId")
                        .IsUnique();

                    NpgsqlIndexBuilderExtensions.AreNullsDistinct(b.HasIndex("FolderName", "ParentDriveId", "ParentFolderId"), false);

                    b.ToTable("Folders", t =>
                        {
                            t.HasCheckConstraint("CK_Folders_Parent", "(\"ParentFolderId\" IS NOT NULL AND \"ParentDriveId\" IS NULL) OR (\"ParentFolderId\" IS NULL AND \"ParentDriveId\" IS NOT NULL)");
                        });
                });

            modelBuilder.Entity("DigitalAssetManagement.Domain.Entities.Permission", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int?>("Id"));

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int?>("FileId")
                        .HasColumnType("integer");

                    b.Property<int?>("FolderId")
                        .HasColumnType("integer");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Role")
                        .HasColumnType("integer");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("FileId");

                    b.HasIndex("FolderId");

                    b.HasIndex("UserId", "FileId", "FolderId")
                        .IsUnique();

                    NpgsqlIndexBuilderExtensions.AreNullsDistinct(b.HasIndex("UserId", "FileId", "FolderId"), false);

                    b.ToTable("Permissions", t =>
                        {
                            t.HasCheckConstraint("CK_Permissions_Asset", "(\"FolderId\" IS NOT NULL AND \"FileId\" IS NULL) OR (\"FolderId\" IS NULL AND \"FileId\" IS NOT NULL)");
                        });
                });

            modelBuilder.Entity("DigitalAssetManagement.Domain.Entities.User", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int?>("Id"));

                    b.Property<byte[]>("Avatar")
                        .HasColumnType("bytea");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("DigitalAssetManagement.Domain.Entities.Drive", b =>
                {
                    b.HasOne("DigitalAssetManagement.Domain.Entities.User", "Owner")
                        .WithMany("Drives")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("DigitalAssetManagement.Domain.Entities.File", b =>
                {
                    b.HasOne("DigitalAssetManagement.Domain.Entities.Drive", "ParentDrive")
                        .WithMany("Files")
                        .HasForeignKey("ParentDriveId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DigitalAssetManagement.Domain.Entities.Folder", "ParentFolder")
                        .WithMany("Files")
                        .HasForeignKey("ParentFolderId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("ParentDrive");

                    b.Navigation("ParentFolder");
                });

            modelBuilder.Entity("DigitalAssetManagement.Domain.Entities.Folder", b =>
                {
                    b.HasOne("DigitalAssetManagement.Domain.Entities.Drive", "ParentDrive")
                        .WithMany("Folders")
                        .HasForeignKey("ParentDriveId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DigitalAssetManagement.Domain.Entities.Folder", "ParentFolder")
                        .WithMany("SubFolders")
                        .HasForeignKey("ParentFolderId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("ParentDrive");

                    b.Navigation("ParentFolder");
                });

            modelBuilder.Entity("DigitalAssetManagement.Domain.Entities.Permission", b =>
                {
                    b.HasOne("DigitalAssetManagement.Domain.Entities.File", "File")
                        .WithMany("Permissions")
                        .HasForeignKey("FileId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DigitalAssetManagement.Domain.Entities.Folder", "Folder")
                        .WithMany("Permissions")
                        .HasForeignKey("FolderId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DigitalAssetManagement.Domain.Entities.User", "User")
                        .WithMany("Permissions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("File");

                    b.Navigation("Folder");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DigitalAssetManagement.Domain.Entities.Drive", b =>
                {
                    b.Navigation("Files");

                    b.Navigation("Folders");
                });

            modelBuilder.Entity("DigitalAssetManagement.Domain.Entities.File", b =>
                {
                    b.Navigation("Permissions");
                });

            modelBuilder.Entity("DigitalAssetManagement.Domain.Entities.Folder", b =>
                {
                    b.Navigation("Files");

                    b.Navigation("Permissions");

                    b.Navigation("SubFolders");
                });

            modelBuilder.Entity("DigitalAssetManagement.Domain.Entities.User", b =>
                {
                    b.Navigation("Drives");

                    b.Navigation("Permissions");
                });
#pragma warning restore 612, 618
        }
    }
}
