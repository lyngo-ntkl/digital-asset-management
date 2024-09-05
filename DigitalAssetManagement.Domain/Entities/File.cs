using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalAssetManagement.Domain.Entities
{
    public class File: BaseEntity
    {
        public required string FileName { get; set; }
        // TODO: decide whether it's a url to access file, a file content itself (byte[])
        public required string FileContent { get; set; }
        public LTree? HierarchicalPath { get; set; }
        public int? ParentFolderId { get; set; }
        public int? ParentDriveId { get; set; }
        [ForeignKey(nameof(ParentFolderId))]
        public virtual Folder? ParentFolder { get; set; }
        [ForeignKey(nameof(ParentDriveId))]
        public virtual Drive? ParentDrive { get; set; }
        public virtual ICollection<Permission> Permissions { get; set; } = null!;
    }
}
