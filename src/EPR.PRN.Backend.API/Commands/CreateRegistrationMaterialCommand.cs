using System.Diagnostics.CodeAnalysis;
using MediatR;

namespace EPR.PRN.Backend.API.Commands;

[ExcludeFromCodeCoverage]
public class CreateRegistrationMaterialCommand : IRequest<int>
{
    public int RegistrationId { get; set; }

    public required string Material { get; set; }
}