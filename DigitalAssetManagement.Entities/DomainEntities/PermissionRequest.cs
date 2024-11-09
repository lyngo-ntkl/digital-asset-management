using DigitalAssetManagement.Entities.Enums;

namespace DigitalAssetManagement.Entities.DomainEntities
{
    public class PermissionRequest
    {
        public int Id { get; set; }
        public required Role Role { get; set; }
        public required int UserId { get; set; }
        public required int MetadataId { get; set; }
    }
}
