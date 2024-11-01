using DigitalAssetManagement.Entities.Enums;
namespace DigitalAssetManagement.Entities.DomainEntities
{
    public class Permission
    {
        public int Id { get; set; }
        public required Role Role { get; set; }
        public required int UserId { get; set; }
        public required int MetadataId { get; set; }
    }
}
