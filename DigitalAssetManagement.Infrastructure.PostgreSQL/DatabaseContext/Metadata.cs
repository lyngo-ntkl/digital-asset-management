using DigitalAssetManagement.Entities.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalAssetManagement.Infrastructure.PostgreSQL.DatabaseContext
{
    public class Metadata
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;
        public required string Name { get; set; }
        public required MetadataType Type { get; set; }
        public required int OwnerId { get; set; }
        public required string AbsolutePath { get; set; }
        public int? ParentId { get; set; }
        public string? ContentType { get; set; }
        public bool IsDeleted { get; set; } = false;

        [ForeignKey(nameof(OwnerId))]
        public virtual User? Owner { get; set; }
        [ForeignKey(nameof(ParentId))]
        public virtual Metadata? ParentMetadata { get; set; }

        public virtual ICollection<Metadata> Children { get; set; } = null!;
        public virtual ICollection<Permission> Permissions { get; set; } = null!;
        public virtual ICollection<PermissionRequest> PermissionRequests { get; set; } = null!;
    }
}
