using DigitalAssetManagement.Application.Common.Attributes;
using System.ComponentModel.DataAnnotations;

namespace DigitalAssetManagement.Application.Dtos.Requests.Users
{
    public class EmailPasswordRegistrationRequest
    {
        [EmailAddress]
        public required string Email { get; set; }
        [Password]
        public required string Password { get; set; }
        public required string Name { get; set; }
        [PhoneNumber]
        public string? PhoneNumber { get; set; }
    }
}
