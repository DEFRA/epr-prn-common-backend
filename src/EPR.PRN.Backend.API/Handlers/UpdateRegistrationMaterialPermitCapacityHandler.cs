using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using MediatR;

namespace EPR.PRN.Backend.API.Handlers;

public class UpdateRegistrationMaterialPermitCapacityHandler(IRegistrationMaterialRepository repository)
    : IRequestHandler<UpdateRegistrationMaterialPermitCapacityCommand>
{
    public async Task Handle(UpdateRegistrationMaterialPermitCapacityCommand command, CancellationToken cancellationToken)
    {
        // Update Permit Capacity
        await repository
            .UpdateRegistrationMaterialPermitCapacity(command.RegistrationMaterialId, 
                                                      command.PermitTypeId, 
                                                      command.CapacityInTonnes, 
                                                      command.PeriodId);
    }
}