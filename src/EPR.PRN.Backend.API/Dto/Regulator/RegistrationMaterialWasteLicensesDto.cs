namespace EPR.PRN.Backend.API.Dto.Regulator
{
    public class RegistrationMaterialWasteLicensesDto
    {
        public required string PermitType { get; set; }
        public string[] Number { get; set; }

        public decimal? CapacityTonne { get; set; }
        public string? Period { get; set; }

        public decimal MaximumReprocessingCapacityTonne { get; set; }
        public string MaximumReprocessingPeriod { get; set; }
        public string Material { get; set; }
    }
}