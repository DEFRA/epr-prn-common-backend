using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using Microsoft.EntityFrameworkCore;
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

        public async Task<RegulatorRegistrationTaskStatus?> GetTaskStatusAsync(string TaskName, int RegistrationId)
        {
            return await _context.RegulatorRegistrationTaskStatus.FirstOrDefaultAsync(x => x.Task.Name == TaskName && x.RegistrationId == RegistrationId);
        }

        public async Task UpdateStatusAsync(string TaskName, int RegistrationId, StatusTypes status, string? comments)
        {
            _logger.LogInformation("Updating status for task with TaskName {TaskName} And RegistrationId {RegistrationId} to {Status}", TaskName, RegistrationId, status);

            var taskStatus = await GetTaskStatus(TaskName, RegistrationId);

            var statusEntity = _context.LookupTaskStatuses.Single(lts => lts.Name == status.ToString());
            if (taskStatus == null)
            {
                
                // Create a new entity if it doesn't exist
                taskStatus = new RegulatorRegistrationTaskStatus
                {
                    RegistrationId = RegistrationId,
                    Task = _context.LookupTasks.Single(t => t.Name == TaskName && t.ApplicationTypeId == _context.Registrations.Find(RegistrationId).ApplicationTypeId),
                    TaskStatus = statusEntity,
                    Comments = comments
                };

                await _context.RegulatorRegistrationTaskStatus.AddAsync(taskStatus);
            }
            else
            {
                // Update the existing entity
                taskStatus.TaskStatus = statusEntity;
                taskStatus.Comments = comments;

                _context.RegulatorRegistrationTaskStatus.Update(taskStatus);
            }

            await _context.SaveChangesAsync();
            _logger.LogInformation("Successfully updated status for task with TaskName {TaskName} And RegistrationId {RegistrationId} to {Status}", TaskName, RegistrationId, status);
        }
        private async Task<RegulatorRegistrationTaskStatus?> GetTaskStatus(string TaskName, int RegistrationId)
        {
            return await _context.RegulatorRegistrationTaskStatus.FirstOrDefaultAsync(x => x.Task.Name == TaskName && x.RegistrationId == RegistrationId);
        }
    }
}