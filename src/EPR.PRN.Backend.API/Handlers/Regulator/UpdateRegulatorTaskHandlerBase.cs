using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Common.Exceptions;
using EPR.PRN.Backend.Data.DataModels.Registrations;

namespace EPR.PRN.Backend.API.Handlers.Regulator;

public abstract class UpdateRegulatorTaskHandlerBase
{
    protected static void ValidateAndThrowIfInvalidStatus(RegulatorTaskStatus commandStatus, RegulatorTaskStatusBase task)
    {
        if (task != null)
        {
            if (commandStatus == RegulatorTaskStatus.Completed)
            {

                if (task.TaskStatus.Name == RegulatorTaskStatus.Completed.ToString())
                {
                    throw new RegulatorInvalidOperationException($"Cannot set task status to {RegulatorTaskStatus.Completed} as it is already {RegulatorTaskStatus.Completed}");
                }
            }
            else if (commandStatus == RegulatorTaskStatus.Queried)
            {
                if (task.TaskStatus.Name == RegulatorTaskStatus.Queried.ToString())
                {
                    throw new RegulatorInvalidOperationException($"Cannot set task status to {RegulatorTaskStatus.Queried} as it is already {RegulatorTaskStatus.Queried}");
                }
                else if (task.TaskStatus.Name == RegulatorTaskStatus.Completed.ToString())
                {
                    throw new RegulatorInvalidOperationException($"Cannot set task status to {RegulatorTaskStatus.Queried} as it is {RegulatorTaskStatus.Completed}");
                }
                else
                {
                    throw new RegulatorInvalidOperationException($"Cannot set task status to {RegulatorTaskStatus.Queried} as it is {task.TaskStatus.Name}");
                }
            }
            else
            {
                throw new RegulatorInvalidOperationException($"Invalid status type: {commandStatus}");
            }
        }
    }
}
