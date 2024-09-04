using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalAssetManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class cascade_delete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_Drives_ParentDriveId",
                table: "Files");

            migrationBuilder.DropForeignKey(
                name: "FK_Files_Folders_ParentFolderId",
                table: "Files");

            migrationBuilder.DropForeignKey(
                name: "FK_Folders_Drives_ParentDriveId",
                table: "Folders");

            migrationBuilder.DropForeignKey(
                name: "FK_Folders_Folders_ParentFolderId",
                table: "Folders");

            migrationBuilder.DropForeignKey(
                name: "FK_Permissions_Files_FileId",
                table: "Permissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Permissions_Folders_FolderId",
                table: "Permissions");

            migrationBuilder.RenameColumn(
                name: "DriverName",
                table: "Drives",
                newName: "DriveName");

            migrationBuilder.RenameIndex(
                name: "IX_Drives_DriverName_OwnerId",
                table: "Drives",
                newName: "IX_Drives_DriveName_OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Drives_ParentDriveId",
                table: "Files",
                column: "ParentDriveId",
                principalTable: "Drives",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Folders_ParentFolderId",
                table: "Files",
                column: "ParentFolderId",
                principalTable: "Folders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Folders_Drives_ParentDriveId",
                table: "Folders",
                column: "ParentDriveId",
                principalTable: "Drives",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Folders_Folders_ParentFolderId",
                table: "Folders",
                column: "ParentFolderId",
                principalTable: "Folders",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_Drives_ParentDriveId",
                table: "Files");

            migrationBuilder.DropForeignKey(
                name: "FK_Files_Folders_ParentFolderId",
                table: "Files");

            migrationBuilder.DropForeignKey(
                name: "FK_Folders_Drives_ParentDriveId",
                table: "Folders");

            migrationBuilder.DropForeignKey(
                name: "FK_Folders_Folders_ParentFolderId",
                table: "Folders");

            migrationBuilder.DropForeignKey(
                name: "FK_Permissions_Files_FileId",
                table: "Permissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Permissions_Folders_FolderId",
                table: "Permissions");

            migrationBuilder.RenameColumn(
                name: "DriveName",
                table: "Drives",
                newName: "DriverName");

            migrationBuilder.RenameIndex(
                name: "IX_Drives_DriveName_OwnerId",
                table: "Drives",
                newName: "IX_Drives_DriverName_OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Drives_ParentDriveId",
                table: "Files",
                column: "ParentDriveId",
                principalTable: "Drives",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Folders_ParentFolderId",
                table: "Files",
                column: "ParentFolderId",
                principalTable: "Folders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Folders_Drives_ParentDriveId",
                table: "Folders",
                column: "ParentDriveId",
                principalTable: "Drives",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Folders_Folders_ParentFolderId",
                table: "Folders",
                column: "ParentFolderId",
                principalTable: "Folders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Permissions_Files_FileId",
                table: "Permissions",
                column: "FileId",
                principalTable: "Files",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Permissions_Folders_FolderId",
                table: "Permissions",
                column: "FolderId",
                principalTable: "Folders",
                principalColumn: "Id");
        }
    }
}
