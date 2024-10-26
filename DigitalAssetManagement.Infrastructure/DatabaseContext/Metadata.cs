using DigitalAssetManagement.Domain.Common;
using DigitalAssetManagement.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalAssetManagement.Infrastructure.PostgreSQL.DatabaseContext
{
    public class Metadata : BaseEntity
    {
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
        public virtual ICollection<Metadata> ChildrenMetadata { get; set; } = null!;
    }
}
