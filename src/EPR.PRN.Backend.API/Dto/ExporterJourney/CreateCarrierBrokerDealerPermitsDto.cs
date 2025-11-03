using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.Dto.ExporterJourney;

[ExcludeFromCodeCoverage]
public class CreateCarrierBrokerDealerPermitsDto
{
    public required string WasteCarrierBrokerDealerRegistration { get; set; }
}
