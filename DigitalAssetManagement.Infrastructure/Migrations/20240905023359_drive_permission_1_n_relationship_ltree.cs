using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalAssetManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class drive_permission_1_n_relationship_ltree : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Permissions_UserId_FileId_FolderId",
                table: "Permissions");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Permissions_Asset",
                table: "Permissions");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:role", "admin,contributor,reader")
                .Annotation("Npgsql:PostgresExtension:ltree", ",,")
                .OldAnnotation("Npgsql:Enum:role", "admin,contributor,reader");

            migrationBuilder.AddColumn<int>(
                name: "DriveId",
                table: "Permissions",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HierarchicalPath",
                table: "Folders",
                type: "ltree",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HierarchicalPath",
                table: "Files",
                type: "ltree",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HierarchicalPath",
                table: "Drives",
                type: "ltree",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_DriveId",
                table: "Permissions",
                column: "DriveId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Permissions_Drives_DriveId",
                table: "Permissions",
                column: "DriveId",
                principalTable: "Drives",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permissions_Drives_DriveId",
                table: "Permissions");

            migrationBuilder.DropIndex(
                name: "IX_Permissions_DriveId",
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
                name: "HierarchicalPath",
                table: "Folders");

            migrationBuilder.DropColumn(
                name: "HierarchicalPath",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "HierarchicalPath",
                table: "Drives");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:role", "admin,contributor,reader")
                .OldAnnotation("Npgsql:Enum:role", "admin,contributor,reader")
                .OldAnnotation("Npgsql:PostgresExtension:ltree", ",,");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_UserId_FileId_FolderId",
                table: "Permissions",
                columns: new[] { "UserId", "FileId", "FolderId" },
                unique: true)
                .Annotation("Npgsql:NullsDistinct", false);

            migrationBuilder.AddCheckConstraint(
                name: "CK_Permissions_Asset",
                table: "Permissions",
                sql: "(\"FolderId\" IS NOT NULL AND \"FileId\" IS NULL) OR (\"FolderId\" IS NULL AND \"FileId\" IS NOT NULL)");
        }
    }
}
