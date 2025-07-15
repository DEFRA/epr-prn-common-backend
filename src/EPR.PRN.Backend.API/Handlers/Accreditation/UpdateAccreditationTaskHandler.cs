using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Common.Exceptions;
using EPR.PRN.Backend.API.Handlers.Regulator;
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

            ValidateAndThrowIfInvalidStatus(command.Status, taskStatus);

            await repository.UpdateStatusAsync(command.TaskName, command.AccreditationId, command.Status);//, Guid.NewGuid());
        }

        protected static void ValidateAndThrowIfInvalidStatus(TaskStatuses commandStatus, TaskStatusBase task)
        {
            //if (task != null)
            //{
            //    if (commandStatus == RegulatorTaskStatus.Completed)
            //    {

            //        if (task.TaskStatus.Name == RegulatorTaskStatus.Completed.ToString())
            //        {
            //            throw new RegulatorInvalidOperationException($"Cannot set task status to {RegulatorTaskStatus.Completed} as it is already {RegulatorTaskStatus.Completed}");
            //        }
            //    }
            //    else if (commandStatus == RegulatorTaskStatus.Queried)
            //    {
            //        if (task.TaskStatus.Name == RegulatorTaskStatus.Queried.ToString())
            //        {
            //            throw new RegulatorInvalidOperationException($"Cannot set task status to {RegulatorTaskStatus.Queried} as it is already {RegulatorTaskStatus.Queried}");
            //        }
            //        else if (task.TaskStatus.Name == RegulatorTaskStatus.Completed.ToString())
            //        {
            //            throw new RegulatorInvalidOperationException($"Cannot set task status to {RegulatorTaskStatus.Queried} as it is {RegulatorTaskStatus.Completed}");
            //        }
            //        else
            //        {
            //            throw new RegulatorInvalidOperationException($"Cannot set task status to {RegulatorTaskStatus.Queried} as it is {task.TaskStatus.Name}");
            //        }
            //    }
            //    else
            //    {
            //        throw new RegulatorInvalidOperationException($"Invalid status type: {commandStatus}");
            //    }
            //}
        }
    }
    
    
}
