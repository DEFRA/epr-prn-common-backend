using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPR.PRN.Backend.Data.DataModels
{
    public class RegistrationMaterial
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Guid ExternalId { get; set; }

        [Required]
        public int RegistrationId { get; set; }

        [Required]
        public string MaterialId { get; set; }

        [Required]
        public int FeesId { get; set; }

        [Required]
        [MaxLength(12)]
        public required string ReferenceNumber { get; set; }

        [Required]
        public int PermitTypeId { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal MaximumReprocessingCapacityTonne { get; set; }

        public int MaximumReprocessingPeriodId { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal PPCReprocessingCapacityTonne { get; set; }

        public int? PPCPeriodId { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal WasteManagementReprocessingCapacityTonne { get; set; }

        public int? WasteManagementPeriodId { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal InstallationReprocessingTonne { get; set; }

        public int? InstallationPeriodId { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal EnvironmentalPermitWasteManagementTonne { get; set; }

        public int? EnvironmentalPermitWasteManagementPeriodId { get; set; }

        [Required]
        [MaxLength(50)]
        public required string WasteCarrierBrokerDealerRegistration { get; set; }

        [Required]
        public bool RegisteredWasteCarrierBrokerDealerFlag { get; set; }

        [Required]
        public bool IsMaterialRegistered { get; set; }

        [MaxLength(2000)]
        public required string ReasonForNotRegistration { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal MaxCapacityTonne { get; set; }

        public int? MaxPeriodId { get; set; }

        [ForeignKey("RegistrationId")]
        public virtual Registration Registration { get; set; } = null!;

        [ForeignKey("FeesId")]
        public virtual FeesAmount Fees { get; set; } = null!;

        [ForeignKey("PermitTypeId")]
        public virtual MaterialPermitType MaterialPermitType { get; set; } = null!;

        [ForeignKey("MaximumReprocessingPeriodId")]
        public virtual Period MaximumReprocessingPeriod { get; set; } = null!;

        [ForeignKey("PPCPeriodId")]
        public virtual Period PPCPeriod { get; set; } = null!;

        [ForeignKey("WasteManagementPeriodId")]
        public virtual Period WasteManagementPeriod { get; set; } = null!;

        [ForeignKey("InstallationPeriodId")]
        public virtual Period InstallationPeriod { get; set; } = null!;

        [ForeignKey("EnvironmentalPermitWasteManagementPeriodId")]
        public virtual Period EnvironmentalPermitWasteManagementPeriod { get; set; } = null!;

        [ForeignKey("MaxPeriodId")]
        public virtual Period MaxPeriod { get; set; } = null!;

        [ForeignKey("MaterialId")]
        public virtual Material Material { get; set; } = null!;
    }
}