using System.ComponentModel.DataAnnotations;

namespace DigitalAssetManagement.Application.Dtos.Requests.Users
{
    public class EmailPasswordRegistrationRequest
    {
        [EmailAddress]
        public required string Email { get; set; }
        // TODO: check password format
        public required string Password { get; set; }
        public required string Name { get; set; }
        // TODO: check phone number format
        public string? PhoneNumber { get; set; }
    }
}
