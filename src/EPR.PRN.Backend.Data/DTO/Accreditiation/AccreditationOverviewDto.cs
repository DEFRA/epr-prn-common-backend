
namespace EPR.PRN.Backend.Data.DTO.Accreditiation
{
    public class AccreditationOverviewDto
    {
        public int Id { get; set; }
        public Guid ExternalId { get; set; }
        public Guid OrganisationId { get; set; }
        public int RegistrationMaterialId { get; set; }
        public int ApplicationTypeId { get; set; }
        public int AccreditationStatusId { get; set; }
        public string? DecFullName { get; set; }
        public string? DecJobTitle { get; set; }
        public string? AccreferenceNumber { get; set; }

        public int? AccreditationYear { get; set; }
        public int? PrnTonnage { get; set; }
        public bool PrnTonnageAndAuthoritiesConfirmed { get; set; }
        public decimal? InfrastructurePercentage { get; set; }
        public decimal? PackagingWastePercentage { get; set; }
        public decimal? BusinessCollectionsPercentage { get; set; }
        public decimal? NewUsesPercentage { get; set; }
        public decimal? NewMarketsPercentage { get; set; }
        public decimal? CommunicationsPercentage { get; set; }
        public decimal? OtherPercentage { get; set; }
        public string? InfrastructureNotes { get; set; }
        public string? PackagingWasteNotes { get; set; }
        public string? BusinessCollectionsNotes { get; set; }
        public string? NewUsesNotes { get; set; }
        public string? NewMarketsNotes { get; set; }
        public string? OtherNotes { get; set; }
        public bool BusinessPlanConfirmed { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }

        public ApplicationTypeDto? ApplicationType { get; set; }
        public AccreditationStatusDto? AccreditationStatus { get; set; }
        public RegistrationMaterialDto RegistrationMaterial { get; set; }
    }
}
