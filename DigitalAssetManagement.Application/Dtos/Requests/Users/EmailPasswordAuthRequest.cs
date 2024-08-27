using System.ComponentModel.DataAnnotations;

namespace DigitalAssetManagement.Application.Dtos.Requests.Users
{
    public class EmailPasswordAuthRequest
    {
        [EmailAddress]
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
