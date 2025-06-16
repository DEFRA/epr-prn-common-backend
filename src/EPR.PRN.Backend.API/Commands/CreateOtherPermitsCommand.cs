using EPR.PRN.Backend.API.Dto.Regulator;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.Commands;

[ExcludeFromCodeCoverage]
public class CreateOtherPermitsCommand : IRequest<bool>
{
    public Guid UserId { get; set; }

    public Guid RegistrationId { get; set; }

    public CreateOtherPermitsDto Dto { get; set; }
}
