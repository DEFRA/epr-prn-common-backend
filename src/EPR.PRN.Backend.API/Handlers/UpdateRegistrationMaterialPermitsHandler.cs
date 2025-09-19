using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using MediatR;

namespace EPR.PRN.Backend.API.Handlers;

public class UpdateRegistrationMaterialPermitsHandler(IRegistrationMaterialRepository repository)
    : IRequestHandler<UpdateRegistrationMaterialPermitsCommand>
{
    public async Task Handle(UpdateRegistrationMaterialPermitsCommand command, CancellationToken cancellationToken)
    {
       _ = await repository.GetRegistrationMaterialById(command.RegistrationMaterialId)
         ?? throw new KeyNotFoundException($"Registration Material not found for external id: {command.RegistrationMaterialId}");
        // Permit Type
        await repository
            .UpdateRegistrationMaterialPermits(command.RegistrationMaterialId, command.PermitTypeId, command.PermitNumber);
    }
}