using EPR.PRN.Backend.API.Dto.ExporterJourney;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.Commands.ExporterJourney;

[ExcludeFromCodeCoverage]
public class UpdateCarrierBrokerDealerPermitsCommand : IRequest
{
    public Guid UserId { get; set; }

    public Guid RegistrationId { get; set; }

    public required UpdateCarrierBrokerDealerPermitsDto Dto { get; set; }
}
