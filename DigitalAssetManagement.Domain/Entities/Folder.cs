using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalAssetManagement.Domain.Entities
{
    public class Folder: BaseEntity
    {
        public required string FolderName { get; set; }
        public int? ParentFolderId { get; set; }
        public int? DriveId { get; set; }

        [ForeignKey(nameof(ParentFolderId))]
        public virtual Folder? ParentFolder { get; set; }
        [ForeignKey(nameof(DriveId))]
        public virtual Drive? Drive { get; set; }
        public virtual ICollection<File> Files { get; set; } = null!;
        public virtual ICollection<Permission> Permissions { get; set; } = null!;
    }
}
