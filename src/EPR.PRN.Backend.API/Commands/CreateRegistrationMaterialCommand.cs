using System.Diagnostics.CodeAnalysis;
using EPR.PRN.Backend.API.Handlers;
using MediatR;

namespace EPR.PRN.Backend.API.Commands;

[ExcludeFromCodeCoverage]
public class CreateRegistrationMaterialCommand : IRequest<CreateRegistrationMaterialDto>
{
    public Guid RegistrationId { get; set; }

    public required string Material { get; set; }
}