using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using MediatR;

namespace EPR.PRN.Backend.API.Handlers.Regulator;

public class UpdateRegulatorAccreditationTaskHandler(IRegulatorAccreditationTaskStatusRepository repository)
    : UpdateRegulatorTaskHandlerBase, IRequestHandler<UpdateRegulatorAccreditationTaskCommand>
{
    public async Task Handle(UpdateRegulatorAccreditationTaskCommand command, CancellationToken cancellationToken)
    {
        var taskStatus = await repository.GetTaskStatusAsync(command.TaskName, command.AccreditationId);

         ValidateAndThrowIfInvalidStatus(command.Status, taskStatus);

        await repository.UpdateStatusAsync(command.TaskName, command.AccreditationId, command.Status, command.Comments, Guid.NewGuid());
    }
}
