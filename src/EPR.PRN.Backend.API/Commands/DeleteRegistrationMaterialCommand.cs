using System.Diagnostics.CodeAnalysis;
using MediatR;

namespace EPR.PRN.Backend.API.Commands;

[ExcludeFromCodeCoverage]
public class DeleteRegistrationMaterialCommand : IRequest
{
    public required Guid RegistrationMaterialId { get; set; }
}