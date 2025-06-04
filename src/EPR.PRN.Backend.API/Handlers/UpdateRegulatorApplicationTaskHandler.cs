using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using MediatR;

namespace EPR.PRN.Backend.API.Handlers;
public class UpdateRegulatorApplicationTaskHandler(IRegulatorApplicationTaskStatusRepository repository)
    : UpdateRegulatorTaskHandlerBase, IRequestHandler<UpdateRegulatorApplicationTaskCommand>
{
    public async Task Handle(UpdateRegulatorApplicationTaskCommand command, CancellationToken cancellationToken)
    {
        var taskStatus = await repository.GetTaskStatusAsync(command.TaskName, command.RegistrationMaterialId);

        ValidateAndThrowIfInvalidStatus(command.Status, taskStatus);

        await repository.UpdateStatusAsync(command.TaskName, command.RegistrationMaterialId, command.Status, command.Comments, Guid.NewGuid());
    }
}