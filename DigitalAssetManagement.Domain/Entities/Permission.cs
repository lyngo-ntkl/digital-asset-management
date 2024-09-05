using DigitalAssetManagement.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalAssetManagement.Domain.Entities
{
    public class Permission: BaseEntity
    {
        public required Role Role { get; set; }
        public required int UserId { get; set; }
        public int? DriveId { get; set; }
        public int? FileId { get; set; }
        public int? FolderId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User? User { get; set; }
        [ForeignKey(nameof(DriveId))]
        public virtual Drive? Drive { get; set; }
        [ForeignKey(nameof(FileId))]
        public virtual File? File { get; set; }
        [ForeignKey(nameof(FolderId))]
        public virtual Folder? Folder { get; set; }
    }
}
