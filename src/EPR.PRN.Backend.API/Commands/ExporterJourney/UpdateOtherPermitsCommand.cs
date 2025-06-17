using EPR.PRN.Backend.API.Dto.ExporterJourney;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.Commands.ExporterJourney;

[ExcludeFromCodeCoverage]
public class UpdateOtherPermitsCommand : IRequest
{
    public Guid UserId { get; set; }

    public Guid RegistrationId { get; set; }

    public UpdateOtherPermitsDto Dto { get; set; }
}
