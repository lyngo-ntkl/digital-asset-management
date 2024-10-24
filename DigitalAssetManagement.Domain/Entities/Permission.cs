﻿using DigitalAssetManagement.Domain.Common;
using DigitalAssetManagement.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalAssetManagement.Domain.Entities
{
    public class Permission: BaseEntity
    {
        public required Role Role { get; set; }
        public required int UserId { get; set; }
        public required int MetadataId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual User? User { get; set; }
        [ForeignKey(nameof(MetadataId))]
        public virtual Metadata? Metadata { get; set; }
    }
}
