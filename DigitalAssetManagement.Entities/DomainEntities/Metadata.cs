using DigitalAssetManagement.Entities.Enums;
namespace DigitalAssetManagement.Entities.DomainEntities
{
    public class Metadata
    {
        public int Id { get; set; }
        public required MetadataType Type { get; set; }
        public required string Name { get; set; }
        public required string AbsolutePath { get; set; }
        public required int OwnerId { get; set; }
        public int? ParentMetadataId { get; set; }
        public ICollection<Metadata> Children { get; set; } = null!;
        public bool IsDeleted { get; set; } = false;
    }
}
