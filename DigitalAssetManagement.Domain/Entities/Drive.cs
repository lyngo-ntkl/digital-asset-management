using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalAssetManagement.Domain.Entities
{
    public class Drive: BaseEntity
    {
        // TODO: rename to DriveName
        public required string DriverName { get; set; }

        public required int OwnerId { get; set; }
        [ForeignKey(nameof(OwnerId))]
        public virtual User? Owner { get; set; }

        public virtual ICollection<Folder> Folders { get; set; } = null!;
        public virtual ICollection<File> Files { get; set; } = null!;
    }
}
