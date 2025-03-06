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
        public int MaterialId { get; set; }

        [Required]
        [MaxLength(100)]
        public string FeesId { get; set; }

        [Required]
        [MaxLength(12)]
        public string ReferenceNumber { get; set; }

        [Required]
        public int PermitTypeId { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal MaximumReprocessingCapacityTonne { get; set; }

        [Required]
        public int MaximumReprocessingPeriodId { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal PPCReprocessingCapacityTonne { get; set; }

        [Required]
        public int PPCPeriodId { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal WasteManagementReprocessingCapacityTonne { get; set; }

        [Required]
        public int WasteManagementPeriodId { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal InstallationReprocessingTonne { get; set; }

        [Required]
        public int InstallationPeriodId { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal EnvironmentalPermitWasteManagementTonne { get; set; }

        [Required]
        public int EnvironmentalPermitWasteManagementPeriodId { get; set; }

        [Required]
        [MaxLength(50)]
        public string WasteCarrierBrokerDealerRegistration { get; set; }

        [Required]
        public bool RegisteredWasteCarrierBrokerDealerFlag { get; set; }

        [Required]
        public bool IsMaterialRegistered { get; set; }

        [MaxLength(2000)]
        public string ReasonForNotRegistration { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal MaxCapacityTonne { get; set; }

        [Required]
        public int MaxPeriodId { get; set; }
    }
}