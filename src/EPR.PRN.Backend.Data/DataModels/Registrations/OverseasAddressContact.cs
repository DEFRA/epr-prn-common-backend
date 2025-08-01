﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPR.PRN.Backend.Data.DataModels.Registrations
{
    [ExcludeFromCodeCoverage]
    [Table("Public.OverseasAddressContact")]
    public class OverseasAddressContact
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public OverseasAddress? OverseasAddress { get; set; }
        [ForeignKey("OverseasAddressId")]
        public int OverseasAddressId { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ExternalId { get; set; }
        [MaxLength(100)]
        public required string FullName { get; set; }
        [MaxLength(100)]
        public required string Email { get; set; }
        [MaxLength(25)]
        public required string PhoneNumber { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid CreatedBy { get; set; }
    }
}
