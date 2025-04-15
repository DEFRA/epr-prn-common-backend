using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Helpers;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using MediatR;

namespace EPR.PRN.Backend.API.Handlers;

public abstract class UpdateRegulatorTaskHandlerBase<TCommand, TRepository, TTaskStatus> : IRequestHandler<TCommand, Unit>
    where TCommand : UpdateRegulatorTaskCommandBase
    where TRepository : IRegulatorTaskStatusRepository<TTaskStatus>
    where TTaskStatus : RegulatorTaskStatusBase
{
    protected readonly TRepository _repository;

    protected UpdateRegulatorTaskHandlerBase(TRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(TCommand command, CancellationToken cancellationToken)
    {
        var taskStatus = await _repository.GetTaskStatusAsync(command.TaskName, command.TypeId);

        if (taskStatus != null)
        {
            if (command.Status == StatusTypes.Completed)
            {
                if (taskStatus.TaskStatusId == (int)StatusTypes.Completed)
                {
                    throw new RegulatorInvalidOperationException($"Cannot set task status to {StatusTypes.Completed} as it is already {StatusTypes.Completed}: {command.TaskName}:{command.TypeId}");
                }
            }
            else if (command.Status == StatusTypes.Queried)
            {
                if (taskStatus.TaskStatusId == (int)StatusTypes.Queried)
                {
                    throw new RegulatorInvalidOperationException($"Cannot set task status to {StatusTypes.Queried} as it is already {StatusTypes.Queried}: {command.TaskName}:{command.TypeId}");
                }
                else if (taskStatus.TaskStatusId == (int)StatusTypes.Completed)
                {
                    throw new RegulatorInvalidOperationException($"Cannot set task status to {StatusTypes.Queried} as it is {StatusTypes.Completed}: {command.TaskName}:{command.TypeId}");
                }
                else
                {
                    throw new RegulatorInvalidOperationException($"Cannot set task status to {StatusTypes.Queried} as it is {(StatusTypes)taskStatus.TaskStatusId}: {command.TaskName}:{command.TypeId}");
                }
            }
            else
            {
                throw new RegulatorInvalidOperationException($"Invalid status type: {command.Status}");
            }
        }
        
        await _repository.UpdateStatusAsync(command.TaskName, command.TypeId, command.Status, command.Comment);

        return Unit.Value;
    }
}
