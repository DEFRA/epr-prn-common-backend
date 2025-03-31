using System.ComponentModel.DataAnnotations;

namespace EPR.PRN.Backend.Data
{
    public class RegistrationMaterial
    {
        [Key]
        public int Id { get; set; }
        public string ExternalId { get; set; }
        public int RegistrationId { get; set; }
        public int MaterialId { get; set; }
        public int FeesId { get; set; }
        public string ReferenceNumber { get; set; }
        public int PermitTypeId { get; set; }
        public decimal MaximumReprocessingCapacityTonne { get; set; }
        public int MaximumReprocessingPeriodID { get; set; }
        public int PPCPeriodId { get; set; }
        public decimal PPCReprocessingCapacityTonne { get; set; }
        public string PPCPermitNumber { get; set; }
        public int WasteManagementPeriodId { get; set; }
        public decimal WasteManagementReprocessingCapacityTonne { get; set; }
        public string WasteManagementLicenseNumber { get; set; }
        public int InstallationPeriodId { get; set; }
        public decimal EnvironmentalPermitWasteManagementTonne { get; set; }
        public string EnvironmentalPermitWasteManagementNumber { get; set; }
        public string WasteCarrierBrokerDealerRegistration { get; set; }
        public bool RegisteredWasteCarrierBrokerDealerFlag { get; set; }
        public bool IsMaterialRegistered { get; set; }
        public string ReasonforNotReg { get; set; }
        public int MaxPeriodId { get; set; }
        public decimal MaxCapacityTonne { get; set; }
        public int RegistrationMaterialStatusId { get; set; }
        public DateTime DulyMadeDate { get; set; }
        public DateTime DeterminationDate { get; set; }
        public string RegistrationReferenceNumber { get; set; }
        public Guid StatusUpdatedBy { get; set; }
        public DateTime StatusUpdatedDate { get; set; }
    }
}