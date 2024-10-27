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
        public required string Name { get; set; }
        public required MetadataType MetadataType { get; set; }
        public required string AbsolutePath { get; set; }

        public required int OwnerId { get; set; }
        [ForeignKey(nameof(OwnerId))]
        public virtual User? Owner { get; set; }
        public virtual ICollection<Permission> Permissions { get; set; } = null!;

        public int? ParentId { get; set; }
        [ForeignKey(nameof(ParentId))]
        public virtual Metadata? ParentMetadata { get; set; }
        public virtual ICollection<Metadata> Children { get; set; } = null!;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;
    }
}
