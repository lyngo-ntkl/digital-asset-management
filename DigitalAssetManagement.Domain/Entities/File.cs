using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalAssetManagement.Domain.Entities
{
    public class File: BaseEntity
    {
        public required string FileName { get; set; }
        // TODO: decide whether it's a url to access file, a file content itself (byte[])
        public required string FileContent { get; set; }
        public int? ParentFolderId { get; set; }
        public int? DriveId { get; set; }
        [ForeignKey(nameof(ParentFolderId))]
        public virtual Folder? ParentFolder { get; set; }
        [ForeignKey(nameof(DriveId))]
        public virtual Drive? Drive { get; set; }
        public virtual ICollection<Permission> Permissions { get; set; } = null!;
    }
}
