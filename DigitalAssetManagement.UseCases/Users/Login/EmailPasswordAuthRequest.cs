using System.ComponentModel.DataAnnotations;

namespace DigitalAssetManagement.UseCases.Users.Login
{
    public class EmailPasswordAuthRequest
    {
        [EmailAddress]
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
