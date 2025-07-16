using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.Dto.Regulator;

[ExcludeFromCodeCoverage]
public class RegistrationWasteCarrierDto : NoteBase
{
    public Guid RegistrationId { get; init; }
    public string SiteAddress { get; set; } = string.Empty;
    public string? WasteCarrierBrokerDealerNumber { get; set; } = string.Empty;
    public Guid RegulatorRegistrationTaskStatusId { get; set; }
}
