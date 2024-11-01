namespace DigitalAssetManagement.Entities.DomainEntities
{
    public class User
    {
        public int Id { get; set; }
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }
        public required string PasswordSalt { get; set; }
        public required string Name { get; set; }
        public string? PhoneNumber { get; set; }
        public byte[]? Avatar { get; set; }
    }
}
