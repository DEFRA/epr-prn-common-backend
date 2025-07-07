using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using MediatR;

namespace EPR.PRN.Backend.API.Handlers;

public class UpdateRegistrationMaterialTaskStatusHandler(IRegistrationMaterialRepository repository)
    : IRequestHandler<UpdateRegistrationMaterialTaskStatusCommand>
{
    public async Task Handle(UpdateRegistrationMaterialTaskStatusCommand command, CancellationToken cancellationToken)
    {
        await repository.UpdateRegistrationTaskStatusAsync(command.TaskName, command.RegistrationMaterialId, command.Status);
    }
}