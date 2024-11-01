using DigitalAssetManagement.UseCases.Users;
using DigitalAssetManagement.Entities.Enums;

namespace DigitalAssetManagement.UseCases.Permissions
{
    public class PermissionResponse
    {
        public Role Role { get; set; }
        public UserResponse? User { get; set; }
    }
}
