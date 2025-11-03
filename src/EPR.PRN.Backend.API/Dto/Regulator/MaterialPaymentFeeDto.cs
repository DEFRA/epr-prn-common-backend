using System.Diagnostics.CodeAnalysis;
using EPR.PRN.Backend.API.Common.Enums;

namespace EPR.PRN.Backend.API.Dto.Regulator;

[ExcludeFromCodeCoverage]
public class MaterialPaymentFeeDto : NoteBase
{
    public Guid RegistrationId { get; set; }
    public int NationId { get; set; }
    public ApplicationOrganisationType ApplicationType { get; init; }
    public string MaterialName { get; set; } = string.Empty;
    public string SiteAddress { get; set; } = string.Empty;
    public string ApplicationReferenceNumber { get; set; } = string.Empty;
    public DateTime? DulyMadeDate { get; set; }
    public DateTime? DeterminationDate { get; set; }
    public DateTime CreatedDate { get; set; }
    public Guid RegulatorApplicationTaskStatusId { get; set; }
}
