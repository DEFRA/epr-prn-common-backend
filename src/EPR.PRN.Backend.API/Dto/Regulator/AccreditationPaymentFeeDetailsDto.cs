using System.Diagnostics.CodeAnalysis;
using EPR.PRN.Backend.API.Common.Enums;

namespace EPR.PRN.Backend.API.Dto.Regulator;

[ExcludeFromCodeCoverage]
public class AccreditationPaymentFeeDetailsDto
{
    public Guid AccreditationId { get; set; }
    public string OrganisationName { get; set; } = string.Empty;
    public string? SiteAddress { get; set; }
    public int NationId { get; set; }
    public string ApplicationReferenceNumber { get; set; } = string.Empty;
    public string MaterialName { get; set; } = string.Empty;
    public DateTime? SubmittedDate { get; set; }
    public decimal FeeAmount { get; set; }
    public ApplicationOrganisationType ApplicationType { get; init; }
    public string Regulator { get; set; } = string.Empty;
    public PrnTonnageType PRNTonnage { get; set; }
}

