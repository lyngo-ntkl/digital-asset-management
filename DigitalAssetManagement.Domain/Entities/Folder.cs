using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalAssetManagement.Domain.Entities
{
    public class Folder: BaseEntity
    {
        public required string FolderName { get; set; }
        public LTree? HierarchicalPath { get; set; }
        public int? ParentFolderId { get; set; }
        public int? ParentDriveId { get; set; }

        [ForeignKey(nameof(ParentFolderId))]
        public virtual Folder? ParentFolder { get; set; }
        [ForeignKey(nameof(ParentDriveId))]
        public virtual Drive? ParentDrive { get; set; }
        public virtual ICollection<File> Files { get; set; } = null!;
        public virtual ICollection<Folder> SubFolders { get; set; } = null!;
        public virtual ICollection<Permission> Permissions { get; set; } = null!;
    }
}
