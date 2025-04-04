using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Helpers;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EPR.PRN.Backend.API.Handlers;

public class UpdateRegulatorRegistrationTaskHandler : IRequestHandler<UpdateRegulatorRegistrationTaskCommand, Unit>
{
    private readonly IRegulatorRegistrationTaskStatusRepository _repository;
    private readonly ILogger<UpdateRegulatorRegistrationTaskHandler> _logger;

    public UpdateRegulatorRegistrationTaskHandler(IRegulatorRegistrationTaskStatusRepository repository, ILogger<UpdateRegulatorRegistrationTaskHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Unit> Handle(UpdateRegulatorRegistrationTaskCommand command, CancellationToken cancellationToken)
    {
        var taskStatus = await _repository.GetTaskStatusByIdAsync(command.Id);
        if (taskStatus == null)
        {
            _logger.LogWarning("Regulator Registration task status not found: {TaskId}", command.Id);
            throw new KeyNotFoundException($"Regulator Registration task status not found: {command.Id}");
        }

        if (command.Status == StatusTypes.Complete)
        {
            if (taskStatus.TaskStatusId == (int)StatusTypes.Complete)
            {
                _logger.LogError("Cannot set task status to {Status} as it is already complete: {TaskId}", StatusTypes.Complete, command.Id);
                throw new RegulatorInvalidOperationException($"Cannot set task status to {StatusTypes.Complete} as it is already {StatusTypes.Complete}: {command.Id}");
            }
        }
        else if (command.Status == StatusTypes.Queried)
        {
            if (taskStatus.TaskStatusId == (int)StatusTypes.Queried)
            {
                _logger.LogError("Cannot set task status to {Status} as it is already queried: {TaskId}", StatusTypes.Queried, command.Id);
                throw new RegulatorInvalidOperationException($"Cannot set task status to {StatusTypes.Queried} as it is already {StatusTypes.Queried}: {command.Id}");
            }
            else if (taskStatus.TaskStatusId == (int)StatusTypes.Complete)
            {
                _logger.LogError("Cannot set task status to {Status} as it is complete: {TaskId}", StatusTypes.Queried, command.Id);
                throw new RegulatorInvalidOperationException($"Cannot set task status to {StatusTypes.Queried} as it is {StatusTypes.Complete}: {command.Id}");
            }
        }
        else
        {
            _logger.LogError("Invalid status type: {Status}", command.Status);
            throw new RegulatorInvalidOperationException($"Invalid status type: {command.Status}");
        }

        await _repository.UpdateStatusAsync(command.Id, command.Status, command.Comment);

        return Unit.Value;
    }
}