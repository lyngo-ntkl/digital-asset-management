using DigitalAssetManagement.Domain.Enums;

namespace DigitalAssetManagement.Application.Dtos.Requests
{
    public class PermissionRequestDto
    {
        public required string Email { get; set; }
        public required Role Role { get; set; }
    }
}
