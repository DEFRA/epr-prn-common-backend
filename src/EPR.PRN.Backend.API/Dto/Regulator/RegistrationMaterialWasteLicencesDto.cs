using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.Dto.Regulator
{
    [ExcludeFromCodeCoverage]
    public class RegistrationMaterialWasteLicencesDto:NoteBase
    {
        public required string PermitType { get; set; }
        public required string[] LicenceNumbers { get; set; }

        public decimal? CapacityTonne { get; set; }
        public string? CapacityPeriod { get; set; }

        public required decimal MaximumReprocessingCapacityTonne { get; set; }
        public required string MaximumReprocessingPeriod { get; set; }
        public required string MaterialName { get; set; }
        public required Guid RegistrationMaterialId { get; set; }
        public required Guid RegistrationId { get; set; }
        public string SiteAddress { get; set; } = string.Empty;
        public Guid RegulatorApplicationTaskStatusId { get; set; }

    }
}