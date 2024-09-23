using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DigitalAssetManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class redesign_database : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permissions_Drives_DriveId",
                table: "Permissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Permissions_Files_FileId",
                table: "Permissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Permissions_Folders_FolderId",
                table: "Permissions");

            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.DropTable(
                name: "Folders");

            migrationBuilder.DropTable(
                name: "Drives");

            migrationBuilder.DropIndex(
                name: "IX_Permissions_DriveId",
                table: "Permissions");

            migrationBuilder.DropIndex(
                name: "IX_Permissions_FileId",
                table: "Permissions");

            migrationBuilder.DropIndex(
                name: "IX_Permissions_FolderId",
                table: "Permissions");

            migrationBuilder.DropIndex(
                name: "IX_Permissions_UserId_FileId_FolderId_DriveId",
                table: "Permissions");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Permissions_Asset",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "DriveId",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "FileId",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "FolderId",
                table: "Permissions");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:metadata_type", "file,folder,user_drive")
                .Annotation("Npgsql:Enum:role", "admin,contributor,reader")
                .OldAnnotation("Npgsql:Enum:role", "admin,contributor,reader")
                .OldAnnotation("Npgsql:PostgresExtension:ltree", ",,");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedDate",
                table: "Users",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Users",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedDate",
                table: "Permissions",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Permissions",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MetadataId",
                table: "Permissions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Metadata",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    MetadataType = table.Column<int>(type: "integer", nullable: false),
                    AbsolutePath = table.Column<string>(type: "text", nullable: false),
                    OwnerId = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Metadata", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Metadata_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_MetadataId",
                table: "Permissions",
                column: "MetadataId");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_UserId",
                table: "Permissions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Metadata_AbsolutePath",
                table: "Metadata",
                column: "AbsolutePath",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Metadata_OwnerId",
                table: "Metadata",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Permissions_Metadata_MetadataId",
                table: "Permissions",
                column: "MetadataId",
                principalTable: "Metadata",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permissions_Metadata_MetadataId",
                table: "Permissions");

            migrationBuilder.DropTable(
                name: "Metadata");

            migrationBuilder.DropIndex(
                name: "IX_Permissions_MetadataId",
                table: "Permissions");

            migrationBuilder.DropIndex(
                name: "IX_Permissions_UserId",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "MetadataId",
                table: "Permissions");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:role", "admin,contributor,reader")
                .Annotation("Npgsql:PostgresExtension:ltree", ",,")
                .OldAnnotation("Npgsql:Enum:metadata_type", "file,folder,user_drive")
                .OldAnnotation("Npgsql:Enum:role", "admin,contributor,reader");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedDate",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedDate",
                table: "Permissions",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Permissions",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<int>(
                name: "DriveId",
                table: "Permissions",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FileId",
                table: "Permissions",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FolderId",
                table: "Permissions",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Drives",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OwnerId = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DriveName = table.Column<string>(type: "text", nullable: false),
                    HierarchicalPath = table.Column<string>(type: "ltree", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drives", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Drives_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Folders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ParentDriveId = table.Column<int>(type: "integer", nullable: true),
                    ParentFolderId = table.Column<int>(type: "integer", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FolderName = table.Column<string>(type: "text", nullable: false),
                    HierarchicalPath = table.Column<string>(type: "ltree", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Folders", x => x.Id);
                    table.CheckConstraint("CK_Folders_Parent", "(\"ParentFolderId\" IS NOT NULL AND \"ParentDriveId\" IS NULL) OR (\"ParentFolderId\" IS NULL AND \"ParentDriveId\" IS NOT NULL)");
                    table.ForeignKey(
                        name: "FK_Folders_Drives_ParentDriveId",
                        column: x => x.ParentDriveId,
                        principalTable: "Drives",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Folders_Folders_ParentFolderId",
                        column: x => x.ParentFolderId,
                        principalTable: "Folders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ParentDriveId = table.Column<int>(type: "integer", nullable: true),
                    ParentFolderId = table.Column<int>(type: "integer", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FileContent = table.Column<string>(type: "text", nullable: false),
                    FileName = table.Column<string>(type: "text", nullable: false),
                    HierarchicalPath = table.Column<string>(type: "ltree", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.Id);
                    table.CheckConstraint("CK_Files_Parent", "(\"ParentFolderId\" IS NOT NULL AND \"ParentDriveId\" IS NULL) OR (\"ParentFolderId\" IS NULL AND \"ParentDriveId\" IS NOT NULL)");
                    table.ForeignKey(
                        name: "FK_Files_Drives_ParentDriveId",
                        column: x => x.ParentDriveId,
                        principalTable: "Drives",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Files_Folders_ParentFolderId",
                        column: x => x.ParentFolderId,
                        principalTable: "Folders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_DriveId",
                table: "Permissions",
                column: "DriveId");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_FileId",
                table: "Permissions",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_FolderId",
                table: "Permissions",
                column: "FolderId");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_UserId_FileId_FolderId_DriveId",
                table: "Permissions",
                columns: new[] { "UserId", "FileId", "FolderId", "DriveId" },
                unique: true)
                .Annotation("Npgsql:NullsDistinct", false);

            migrationBuilder.AddCheckConstraint(
                name: "CK_Permissions_Asset",
                table: "Permissions",
                sql: "(\"FolderId\" IS NOT NULL AND \"FileId\" IS NULL AND \"DriveId\" IS NULL) OR (\"FolderId\" IS NULL AND \"FileId\" IS NOT NULL AND \"DriveId\" IS NULL) OR (\"FolderId\" IS NULL AND \"FileId\" IS NULL AND \"DriveId\" IS NOT NULL)");

            migrationBuilder.CreateIndex(
                name: "IX_Drives_DriveName_OwnerId",
                table: "Drives",
                columns: new[] { "DriveName", "OwnerId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Drives_OwnerId",
                table: "Drives",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Files_FileName_ParentDriveId_ParentFolderId",
                table: "Files",
                columns: new[] { "FileName", "ParentDriveId", "ParentFolderId" },
                unique: true)
                .Annotation("Npgsql:NullsDistinct", false);

            migrationBuilder.CreateIndex(
                name: "IX_Files_ParentDriveId",
                table: "Files",
                column: "ParentDriveId");

            migrationBuilder.CreateIndex(
                name: "IX_Files_ParentFolderId",
                table: "Files",
                column: "ParentFolderId");

            migrationBuilder.CreateIndex(
                name: "IX_Folders_FolderName_ParentDriveId_ParentFolderId",
                table: "Folders",
                columns: new[] { "FolderName", "ParentDriveId", "ParentFolderId" },
                unique: true)
                .Annotation("Npgsql:NullsDistinct", false);

            migrationBuilder.CreateIndex(
                name: "IX_Folders_ParentDriveId",
                table: "Folders",
                column: "ParentDriveId");

            migrationBuilder.CreateIndex(
                name: "IX_Folders_ParentFolderId",
                table: "Folders",
                column: "ParentFolderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Permissions_Drives_DriveId",
                table: "Permissions",
                column: "DriveId",
                principalTable: "Drives",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Permissions_Files_FileId",
                table: "Permissions",
                column: "FileId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Permissions_Folders_FolderId",
                table: "Permissions",
                column: "FolderId",
                principalTable: "Folders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
