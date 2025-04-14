using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using Microsoft.EntityFrameworkCore;
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

        public async Task<RegulatorApplicationTaskStatus?> GetTaskStatusAsync(string TaskName, int RegistrationMaterialId)
        {
            return await GetTaskStatus(TaskName, RegistrationMaterialId);
        }

        public async Task UpdateStatusAsync(string TaskName, int RegistrationMaterialId, StatusTypes status, string? comments)
        {
            _logger.LogInformation("Updating status for task with TaskName {TaskName} And RegistrationMaterialId {RegistrationMaterialId} to {Status}", TaskName, RegistrationMaterialId, status);

            var taskStatus = await GetTaskStatus(TaskName, RegistrationMaterialId);

            var statusEntity = _context.LookupTaskStatuses.Single(lts => lts.Name == status.ToString());

            if (taskStatus == null)
            {
                var task = _context.LookupTasks.Single(t => t.Name == TaskName && t.ApplicationTypeId == _context.RegistrationMaterials.Include(r => r.Registration).Single(r => r.Id == RegistrationMaterialId).Registration.ApplicationTypeId);
                // Create a new entity if it doesn't exist
                taskStatus = new RegulatorApplicationTaskStatus
                {
                    Task = task,
                    RegistrationMaterialId = RegistrationMaterialId,
                    TaskStatus = statusEntity,
                    Comments = comments
                };

                await _context.RegulatorApplicationTaskStatus.AddAsync(taskStatus);
            }
            else
            {
                // Update the existing entity
                taskStatus.TaskStatus = statusEntity;
                taskStatus.Comments = comments;

                _context.RegulatorApplicationTaskStatus.Update(taskStatus);
            }

            await _context.SaveChangesAsync();
            _logger.LogInformation("Successfully updated status for task with TaskName {TaskName} And RegistrationMaterialId {RegistrationMaterialId} to {Status}", TaskName, RegistrationMaterialId, status);
        }
        private async Task<RegulatorApplicationTaskStatus?> GetTaskStatus(string TaskName, int RegistrationMaterialId)
        {
            return await _context.RegulatorApplicationTaskStatus.FirstOrDefaultAsync(x => x.Task.Name == TaskName && x.RegistrationMaterialId == RegistrationMaterialId);
        }
    }
}