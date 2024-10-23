using DigitalAssetManagement.Application.Dtos.Responses.Users;
using DigitalAssetManagement.Domain.Enums;

namespace DigitalAssetManagement.Application.Dtos.Responses
{
    public class PermissionResponseDto
    {
        public Role Role { get; set; }
        public UserResponseDto? User { get; set; }
    }
}
