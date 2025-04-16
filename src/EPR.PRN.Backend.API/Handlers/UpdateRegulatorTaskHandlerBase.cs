using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Common.Exceptions;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using MediatR;

namespace EPR.PRN.Backend.API.Handlers;

public abstract class UpdateRegulatorTaskHandlerBase<TCommand, TRepository, TTaskStatus> : IRequestHandler<TCommand>
    where TCommand : UpdateRegulatorTaskCommandBase
    where TRepository : IRegulatorTaskStatusRepository<TTaskStatus>
    where TTaskStatus : RegulatorTaskStatusBase
{
    protected readonly TRepository _repository;

    protected UpdateRegulatorTaskHandlerBase(TRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(TCommand command, CancellationToken cancellationToken)
    {
        var taskStatus = await _repository.GetTaskStatusAsync(command.TaskName, command.TypeId);

        if (taskStatus != null)
        {
            if (command.Status == StatusTypes.Completed)
            {
                if (taskStatus.TaskStatus.Name == StatusTypes.Completed.ToString())
                {
                    throw new RegulatorInvalidOperationException($"Cannot set task status to {StatusTypes.Completed} as it is already {StatusTypes.Completed}: {command.TaskName}:{command.TypeId}");
                }
            }
            else if (command.Status == StatusTypes.Queried)
            {
                if (taskStatus.TaskStatus.Name == StatusTypes.Queried.ToString())
                {
                    throw new RegulatorInvalidOperationException($"Cannot set task status to {StatusTypes.Queried} as it is already {StatusTypes.Queried}: {command.TaskName}:{command.TypeId}");
                }
                else if (taskStatus.TaskStatus.Name == StatusTypes.Completed.ToString())
                {
                    throw new RegulatorInvalidOperationException($"Cannot set task status to {StatusTypes.Queried} as it is {StatusTypes.Completed}: {command.TaskName}:{command.TypeId}");
                }
                else
                {
                    throw new RegulatorInvalidOperationException($"Cannot set task status to {StatusTypes.Queried} as it is {taskStatus.TaskStatus.Name}: {command.TaskName}:{command.TypeId}");
                }
            }
            else
            {
                throw new RegulatorInvalidOperationException($"Invalid status type: {command.Status}");
            }
        }
        
        await _repository.UpdateStatusAsync(command.TaskName, command.TypeId, command.Status, command.Comment, command.UserName);
    }
}
