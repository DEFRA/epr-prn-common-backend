using EPR.PRN.Backend.API.Common.Enums;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.Dto.Regulator;

[ExcludeFromCodeCoverage]
public class MaterialPaymentFeeDto:NoteBase
{
    public Guid RegistrationId { get; set; }
    public int NationId { get; set; }
    public ApplicationOrganisationType ApplicationType { get; init; }
    public string MaterialName { get; set; }
    public string SiteAddress { get; set; } = string.Empty;
    public string ApplicationReferenceNumber { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public Guid RegulatorApplicationTaskStatusId { get; set; }
}
