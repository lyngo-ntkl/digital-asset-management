using DigitalAssetManagement.Entities.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalAssetManagement.Infrastructure.PostgreSQL.DatabaseContext
{
    public class PermissionRequest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required Role Role { get; set; }
        public required int UserId { get; set; }
        public required int MetadataId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User? User { get; set; }
        [ForeignKey(nameof(MetadataId))]
        public virtual Metadata? Metadata { get; set; }
    }
}
