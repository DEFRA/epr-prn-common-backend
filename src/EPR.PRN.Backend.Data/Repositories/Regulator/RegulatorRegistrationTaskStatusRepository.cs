using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using Microsoft.Extensions.Logging;

namespace EPR.PRN.Backend.Data.Repositories.Regulator
{
    public class RegulatorRegistrationTaskStatusRepository : IRegulatorRegistrationTaskStatusRepository
    {
        private readonly EprRegistrationsContext _context;
        private readonly ILogger<RegulatorRegistrationTaskStatusRepository> _logger;

        public RegulatorRegistrationTaskStatusRepository(EprRegistrationsContext context, ILogger<RegulatorRegistrationTaskStatusRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task UpdateStatusAsync(int id, StatusTypes status, string? comments)
        {
            _logger.LogInformation("Updating status for task with ID {TaskId} to {Status}", id, status);

            var TaskStatus = await _context.RegulatorRegistrationTaskStatus.FindAsync(id);
            if (TaskStatus == null)
            {
                _logger.LogWarning("Regulator Registration task status not found: {TaskId}", id);
                throw new KeyNotFoundException($"Regulator Registration task status not found: {id}");
            }

            if (status == StatusTypes.Complete)
            {
                if (TaskStatus.TaskStatusId == (int)StatusTypes.Complete)
                {
                    _logger.LogError("Cannot set task status to {Status} as it is already complete: {TaskId}", StatusTypes.Complete, id);
                    throw new InvalidOperationException($"Cannot set task status to {StatusTypes.Complete} as it is already {StatusTypes.Complete}: {id}");
                }
            }
            else if (status == StatusTypes.Queried)
            {
                if (TaskStatus.TaskStatusId == (int)StatusTypes.Queried)
                {
                    _logger.LogError("Cannot set task status to {Status} as it is already queried: {TaskId}", StatusTypes.Queried, id);
                    throw new InvalidOperationException($"Cannot set task status to {StatusTypes.Queried} as it is already {StatusTypes.Queried}: {id}");
                }
                else if (TaskStatus.TaskStatusId == (int)StatusTypes.Complete)
                {
                    _logger.LogError("Cannot set task status to {Status} as it is complete: {TaskId}", StatusTypes.Queried, id);
                    throw new InvalidOperationException($"Cannot set task status to {StatusTypes.Queried} as it is {StatusTypes.Complete}: {id}");
                }
            }
            else
            {
                _logger.LogError("Invalid status type: {Status}", status);
                throw new InvalidOperationException($"Invalid status type: {status}");
            }

            TaskStatus.TaskStatusId = (int)status;
            TaskStatus.Comments = comments;

            await _context.SaveChangesAsync();
            _logger.LogInformation("Successfully updated status for task with ID {TaskId} to {Status}", id, status);
        }
    }
}