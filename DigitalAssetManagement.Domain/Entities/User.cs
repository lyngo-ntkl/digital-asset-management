using DigitalAssetManagement.Domain.Enums;

namespace DigitalAssetManagement.Domain.Entities
{
    public class User: BaseEntity
    {
        public required string Email {  get; set; }
        public required string PasswordHash { get; set; }
        public required string PasswordSalt { get; set; }
        public required string Name { get; set; }
        public string? PhoneNumber { get; set; }

        public virtual ICollection<Drive> Drives { get; set; } = null!;
        public virtual ICollection<Permission> Permissions { get; set; } = null!;
    }
}
