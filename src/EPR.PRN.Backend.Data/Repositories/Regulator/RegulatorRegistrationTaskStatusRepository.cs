using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.DataModels.Registrations;
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

        public async Task<RegulatorRegistrationTaskStatus?> GetTaskStatusByIdAsync(int id)
        {
            return await _context.RegulatorRegistrationTaskStatus.FindAsync(id);
        }

        public async Task UpdateStatusAsync(int id, StatusTypes status, string? comments)
        {
            _logger.LogInformation("Updating status for task with ID {TaskId} to {Status}", id, status);

            var taskStatus = await _context.RegulatorRegistrationTaskStatus.FindAsync(id);
            if (taskStatus == null)
            {
                _logger.LogWarning("Regulator Registration task status not found: {TaskId}", id);
                throw new KeyNotFoundException($"Regulator Registration task status not found: {id}");
            }

            taskStatus.TaskStatusId = (int)status;
            taskStatus.Comments = comments;

            await _context.SaveChangesAsync();
            _logger.LogInformation("Successfully updated status for task with ID {TaskId} to {Status}", id, status);
        }
    }
}