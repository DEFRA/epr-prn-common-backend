using EPR.PRN.Backend.API.Dto.ExporterJourney;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.Commands.ExporterJourney;

[ExcludeFromCodeCoverage]
public class CreateCarrierBrokerDealerPermitsCommand : IRequest<bool>
{
    public Guid UserId { get; set; }

    public Guid RegistrationId { get; set; }

	public required string WasteCarrierBrokerDealerRegistration { get; set; }

    public bool RegisteredWasteCarrierBrokerDealerFlag { get; set; }
}
