using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalAssetManagement.Domain.Entities
{
    public class BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? Id { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedDate { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;
    }
}
