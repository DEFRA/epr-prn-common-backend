using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using Microsoft.Extensions.Logging;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace EPR.PRN.Backend.Data.Repositories.Regulator
{
    public class RegulatorApplicationTaskStatusRepository : IRegulatorApplicationTaskStatusRepository
    {
        private readonly EprRegistrationsContext _context;
        private readonly ILogger<RegulatorApplicationTaskStatusRepository> _logger;

        public RegulatorApplicationTaskStatusRepository(EprRegistrationsContext context, ILogger<RegulatorApplicationTaskStatusRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<RegulatorApplicationTaskStatus> GetTaskStatusByIdAsync(int id)
        {
            var taskStatus = await _context.RegulatorApplicationTaskStatus.FindAsync(id);

            return taskStatus != null ? taskStatus : throw new KeyNotFoundException($"Task status not found: {id}");
        }

        public async Task UpdateStatusAsync(int id, StatusTypes status, string? comments)
        {
            _logger.LogInformation("Updating status for task with ID {TaskId} to {Status}", id, status);

            var taskStatus = await _context.RegulatorApplicationTaskStatus.FindAsync(id);
            if (taskStatus == null)
            {
                _logger.LogWarning("Regulator application task status not found: {TaskId}", id);
                throw new KeyNotFoundException($"Regulator application task status not found: {id}");
            }

            taskStatus.TaskStatusId = (int)status;
            taskStatus.Comments = comments;

            await _context.SaveChangesAsync();
            _logger.LogInformation("Successfully updated status for task with ID {TaskId} to {Status}", id, status);
        }
    }
}