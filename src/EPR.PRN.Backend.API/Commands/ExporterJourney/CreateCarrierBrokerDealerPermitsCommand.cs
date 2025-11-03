using System.Diagnostics.CodeAnalysis;
using MediatR;

namespace EPR.PRN.Backend.API.Commands.ExporterJourney;

[ExcludeFromCodeCoverage]
public class CreateCarrierBrokerDealerPermitsCommand : IRequest<bool>
{
    public Guid UserId { get; set; }

    public Guid RegistrationId { get; set; }

    public required string WasteCarrierBrokerDealerRegistration { get; set; }
}
