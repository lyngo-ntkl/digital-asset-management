using DigitalAssetManagement.UseCases.Common.Attributes;
using System.ComponentModel.DataAnnotations;

namespace DigitalAssetManagement.UseCases.Users.Create
{
    public class RegistrationRequest
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
