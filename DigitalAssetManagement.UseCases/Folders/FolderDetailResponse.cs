using DigitalAssetManagement.UseCases.Permissions;

namespace DigitalAssetManagement.UseCases.Folders
{
    public class FolderDetailResponse
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public ICollection<MetadataResponse>? Children { get; set; }
        public ICollection<PermissionResponse>? Permissions { get; set; }
    }
}
