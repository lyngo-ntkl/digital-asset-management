using DigitalAssetManagement.Entities.Enums;
using DigitalAssetManagement.UseCases.Users.Read;

namespace DigitalAssetManagement.UseCases.Permissions
{
    public class PermissionResponse
    {
        public Role Role { get; set; }
        public UserResponse? User { get; set; }
    }
}
