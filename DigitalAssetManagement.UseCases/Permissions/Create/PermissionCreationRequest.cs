using DigitalAssetManagement.Entities.Enums;

namespace DigitalAssetManagement.UseCases.Permissions.Create
{
    public class PermissionCreationRequest
    {
        public required int MetadataId { get; set; }
        public required string Email { get; set; }
        public required Role Role { get; set; }
    }
}
