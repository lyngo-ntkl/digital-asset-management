﻿using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalAssetManagement.Infrastructure.PostgreSQL.DatabaseContext
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        // TODO: send verification email
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }
        public required string PasswordSalt { get; set; }
        public required string Name { get; set; }
        // TODO: send verification phone number
        public string? PhoneNumber { get; set; }
        public byte[]? Avatar { get; set; }
        [JsonIgnore]
        public virtual ICollection<Metadata> Metadata { get; set; } = null!;
        [JsonIgnore]
        public virtual ICollection<Permission> Permissions { get; set; } = null!;
    }
}