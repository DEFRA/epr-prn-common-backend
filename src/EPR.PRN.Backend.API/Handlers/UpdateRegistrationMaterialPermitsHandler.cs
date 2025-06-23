using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using MediatR;

namespace EPR.PRN.Backend.API.Handlers;

public class UpdateRegistrationMaterialPermitsHandler(IRegistrationMaterialRepository repository)
    : IRequestHandler<UpdateRegistrationMaterialPermitsCommand>
{
    public async Task Handle(UpdateRegistrationMaterialPermitsCommand command, CancellationToken cancellationToken)
    {
        // Permit Type
        await repository
            .UpdateRegistrationMaterialPermits(command.RegistrationMaterialId, command.PermitTypeId, command.PermitNumber);
    }
}