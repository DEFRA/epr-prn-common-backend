using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Common.Exceptions;
using EPR.PRN.Backend.API.Handlers.Regulator;
using EPR.PRN.Backend.API.Helpers;
using EPR.PRN.Backend.API.Repositories;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces.Accreditation;
using MediatR;

namespace EPR.PRN.Backend.API.Handlers.Accreditation
{
    
    public class UpdateAccreditationTaskHandler(IAccreditationTaskStatusRepository repository)
    : IRequestHandler<UpdateAccreditationTaskCommand>
    {
        public async Task Handle(UpdateAccreditationTaskCommand command, CancellationToken cancellationToken)
        {

            var taskStatus = await repository.GetTaskStatusAsync(command.TaskName, command.AccreditationId);

            if (taskStatus == null)
            {
                throw new NotFoundException($"Task with name {command.TaskName} and accreditation ID {command.AccreditationId} not found.");
            }

            ValidateAndThrowIfInvalidStatus(command.Status, taskStatus!);

            await repository.UpdateStatusAsync(command.TaskName, command.AccreditationId, command.Status);
        }

        protected static void ValidateAndThrowIfInvalidStatus(TaskStatuses commandStatus, TaskStatusBase task)
        {
            // This logic will need to be updated to match the logic for accreditation tasks. It is currently the same as the regulator accreditation logic.
            if (task != null)
            {
                if (commandStatus == TaskStatuses.Completed)
                {

                    if (task.TaskStatus.Name == TaskStatuses.Completed.ToString())
                    {
                        throw new InvalidOperationException($"Cannot set task status to {TaskStatuses.Completed} as it is already {TaskStatuses.Completed}");
                    }
                }
                else if (commandStatus == TaskStatuses.Queried)
                {
                    if (task.TaskStatus.Name == TaskStatuses.Queried.ToString())
                    {
                        throw new InvalidOperationException($"Cannot set task status to {TaskStatuses.Queried} as it is already {TaskStatuses.Queried}");
                    }
                    else if (task.TaskStatus.Name == RegulatorTaskStatus.Completed.ToString())
                    {
                        throw new InvalidOperationException($"Cannot set task status to {TaskStatuses.Queried} as it is already {TaskStatuses.Completed}");
                    }
                    else
                    {
                        throw new InvalidOperationException($"Cannot set task status to {TaskStatuses.Queried} as it is already {task.TaskStatus.Name}");
                    }
                }
                else
                {
                    throw new InvalidOperationException($"Invalid status type: {commandStatus}");
                }
            }
        }
    }
    
    
}
