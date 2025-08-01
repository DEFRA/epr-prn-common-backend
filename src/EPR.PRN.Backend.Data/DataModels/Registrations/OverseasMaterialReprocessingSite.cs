﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPR.PRN.Backend.Data.DataModels.Registrations
{
    [ExcludeFromCodeCoverage]
    [Table("Public.OverseasMaterialReprocessingSite")]
    public class OverseasMaterialReprocessingSite
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ExternalId { get; set; }
        [ForeignKey("OverseasAddressId")]
        public int OverseasAddressId { get; set; }
        public OverseasAddress? OverseasAddress { get; set; }
        [ForeignKey("RegistrationMaterialId")]
        public int RegistrationMaterialId { get; set; }
        public RegistrationMaterial? RegistrationMaterial { get; set; }
    }
}
