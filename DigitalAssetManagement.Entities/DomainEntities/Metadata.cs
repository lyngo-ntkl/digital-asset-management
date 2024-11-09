using DigitalAssetManagement.Entities.Enums;
namespace DigitalAssetManagement.Entities.DomainEntities
{
    public class Metadata
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime LastModifiedDate { get; set; } = DateTime.UtcNow;
        public required string Name { get; set; }
        public required MetadataType Type { get; set; }
        public required int OwnerId { get; set; }
        public required string AbsolutePath { get; set; }
        public int? ParentId { get; set; }
        public string? ContentType { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
