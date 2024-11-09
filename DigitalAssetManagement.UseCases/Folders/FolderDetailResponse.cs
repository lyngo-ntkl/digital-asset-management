using DigitalAssetManagement.UseCases.Permissions;

namespace DigitalAssetManagement.UseCases.Folders
{
    public class FolderDetailResponse
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required DateTime CreatedDate { get; set; }
        public required DateTime LastModifiedDate { get; set; }
        public ICollection<MetadataResponse> Children { get; set; } = null!;
        public ICollection<PermissionResponse>? Permissions { get; set; }
    }
}
