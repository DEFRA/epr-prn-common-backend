using System.Diagnostics.CodeAnalysis;
using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using MediatR;

namespace EPR.PRN.Backend.API.Handlers;

[ExcludeFromCodeCoverage(Justification = "TODO: To be done as part of create registration user story")]
public class CreateRegistrationMaterialHandler(IRegistrationMaterialRepository repository)
    : IRequestHandler<CreateRegistrationMaterialCommand, int>
{
    public async Task<int> Handle(CreateRegistrationMaterialCommand command, CancellationToken cancellationToken)
    {
        return await repository
            .CreateAsync(command.RegistrationId, command.Material);
    }
}