using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalAssetManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class rename_properties_add_is_delete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Drives_Users_UserId",
                table: "Drives");

            migrationBuilder.DropForeignKey(
                name: "FK_Files_Drives_DriveId",
                table: "Files");

            migrationBuilder.DropForeignKey(
                name: "FK_Folders_Drives_DriveId",
                table: "Folders");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Folders_Parent",
                table: "Folders");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Files_Parent",
                table: "Files");

            migrationBuilder.RenameColumn(
                name: "DriveId",
                table: "Folders",
                newName: "ParentDriveId");

            migrationBuilder.RenameIndex(
                name: "IX_Folders_FolderName_DriveId_ParentFolderId",
                table: "Folders",
                newName: "IX_Folders_FolderName_ParentDriveId_ParentFolderId");

            migrationBuilder.RenameIndex(
                name: "IX_Folders_DriveId",
                table: "Folders",
                newName: "IX_Folders_ParentDriveId");

            migrationBuilder.RenameColumn(
                name: "DriveId",
                table: "Files",
                newName: "ParentDriveId");

            migrationBuilder.RenameIndex(
                name: "IX_Files_FileName_DriveId_ParentFolderId",
                table: "Files",
                newName: "IX_Files_FileName_ParentDriveId_ParentFolderId");

            migrationBuilder.RenameIndex(
                name: "IX_Files_DriveId",
                table: "Files",
                newName: "IX_Files_ParentDriveId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Drives",
                newName: "OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Drives_UserId",
                table: "Drives",
                newName: "IX_Drives_OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Drives_DriverName_UserId",
                table: "Drives",
                newName: "IX_Drives_DriverName_OwnerId");

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

            migrationBuilder.AddColumn<byte[]>(
                name: "Avatar",
                table: "Users",
                type: "bytea",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

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

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Permissions",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedDate",
                table: "Folders",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Folders",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Folders",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedDate",
                table: "Files",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Files",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Files",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedDate",
                table: "Drives",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Drives",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Drives",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddCheckConstraint(
                name: "CK_Folders_Parent",
                table: "Folders",
                sql: "(\"ParentFolderId\" IS NOT NULL AND \"ParentDriveId\" IS NULL) OR (\"ParentFolderId\" IS NULL AND \"ParentDriveId\" IS NOT NULL)");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Files_Parent",
                table: "Files",
                sql: "(\"ParentFolderId\" IS NOT NULL AND \"ParentDriveId\" IS NULL) OR (\"ParentFolderId\" IS NULL AND \"ParentDriveId\" IS NOT NULL)");

            migrationBuilder.AddForeignKey(
                name: "FK_Drives_Users_OwnerId",
                table: "Drives",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Drives_ParentDriveId",
                table: "Files",
                column: "ParentDriveId",
                principalTable: "Drives",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Folders_Drives_ParentDriveId",
                table: "Folders",
                column: "ParentDriveId",
                principalTable: "Drives",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Drives_Users_OwnerId",
                table: "Drives");

            migrationBuilder.DropForeignKey(
                name: "FK_Files_Drives_ParentDriveId",
                table: "Files");

            migrationBuilder.DropForeignKey(
                name: "FK_Folders_Drives_ParentDriveId",
                table: "Folders");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Folders_Parent",
                table: "Folders");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Files_Parent",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "Avatar",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Folders");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Drives");

            migrationBuilder.RenameColumn(
                name: "ParentDriveId",
                table: "Folders",
                newName: "DriveId");

            migrationBuilder.RenameIndex(
                name: "IX_Folders_ParentDriveId",
                table: "Folders",
                newName: "IX_Folders_DriveId");

            migrationBuilder.RenameIndex(
                name: "IX_Folders_FolderName_ParentDriveId_ParentFolderId",
                table: "Folders",
                newName: "IX_Folders_FolderName_DriveId_ParentFolderId");

            migrationBuilder.RenameColumn(
                name: "ParentDriveId",
                table: "Files",
                newName: "DriveId");

            migrationBuilder.RenameIndex(
                name: "IX_Files_ParentDriveId",
                table: "Files",
                newName: "IX_Files_DriveId");

            migrationBuilder.RenameIndex(
                name: "IX_Files_FileName_ParentDriveId_ParentFolderId",
                table: "Files",
                newName: "IX_Files_FileName_DriveId_ParentFolderId");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "Drives",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Drives_OwnerId",
                table: "Drives",
                newName: "IX_Drives_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Drives_DriverName_OwnerId",
                table: "Drives",
                newName: "IX_Drives_DriverName_UserId");

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

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedDate",
                table: "Folders",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Folders",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedDate",
                table: "Files",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Files",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedDate",
                table: "Drives",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Drives",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AddCheckConstraint(
                name: "CK_Folders_Parent",
                table: "Folders",
                sql: "(\"ParentFolderId\" IS NOT NULL AND \"DriveId\" IS NULL) OR (\"ParentFolderId\" IS NULL AND \"DriveId\" IS NOT NULL)");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Files_Parent",
                table: "Files",
                sql: "(\"ParentFolderId\" IS NOT NULL AND \"DriveId\" IS NULL) OR (\"ParentFolderId\" IS NULL AND \"DriveId\" IS NOT NULL)");

            migrationBuilder.AddForeignKey(
                name: "FK_Drives_Users_UserId",
                table: "Drives",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Drives_DriveId",
                table: "Files",
                column: "DriveId",
                principalTable: "Drives",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Folders_Drives_DriveId",
                table: "Folders",
                column: "DriveId",
                principalTable: "Drives",
                principalColumn: "Id");
        }
    }
}
