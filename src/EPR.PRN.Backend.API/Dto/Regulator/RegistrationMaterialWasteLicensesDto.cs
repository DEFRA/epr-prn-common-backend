namespace EPR.PRN.Backend.API.Dto.Regulator
{
    public class RegistrationMaterialWasteLicensesDto
    {
        public required string PermitType { get; set; }
        public required string[] LicenseNumbers { get; set; }

        public decimal? CapacityTonne { get; set; }
        public string? CapacityPeriod { get; set; }

        public required decimal MaximumReprocessingCapacityTonne { get; set; }
        public required string MaximumReprocessingPeriod { get; set; }
        public required string MaterialName { get; set; }
    }
}