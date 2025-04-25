using EPR.PRN.Backend.Data.DataModels.Registrations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPR.PRN.Backend.API.Dto.Regulator
{
    public class MaterialWasteLicensesDto
    {
        public required string PermitType { get; set; }
        public string[]? WasteExemption { get; set; }
        public string? PPCPermitNumber { get; set; }
        public string? WasteManagementLicenseNumber { get; set; }
        public string? InstallationPermitNumber { get; set; }
        public string? EnvironmentalPermitWasteManagementNumber { get; set; }
        public decimal? PPCReprocessingCapacityTonne { get; set; }
        public decimal? WasteManagementReprocessingCapacityTonne { get; set; }
        public decimal? InstallationReprocessingTonne { get; set; }
        public decimal? EnvironmentalPermitWasteManagementTonne { get; set; }
        public string? PPCPeriod { get; set; }
        public string? WasteManagementPeriod { get; set; }
        public string? InstallationPeriod { get; set; }
        public string? EnvironmentalPermitWasteManagementPeriod { get; set; }
        public decimal MaximumReprocessingCapacityTonne { get; set; }
        public string? MaximumReprocessingPeriod { get; set; }
    }
}