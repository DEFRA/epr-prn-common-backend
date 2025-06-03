using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Common.Exceptions;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace EPR.PRN.Backend.Data.Repositories.Regulator
{
    public class RegulatorAccreditationTaskStatusRepository : IRegulatorAccreditationTaskStatusRepository
    {
        private readonly EprContext _context;
        private readonly ILogger<RegulatorAccreditationTaskStatusRepository> _logger;

        public RegulatorAccreditationTaskStatusRepository(EprContext context, ILogger<RegulatorAccreditationTaskStatusRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<RegulatorAccreditationTaskStatus?> GetTaskStatusAsync(string TaskName, Guid RegistrationId)
        {
            return await GetTaskStatus(TaskName, RegistrationId);
        }

        public async Task UpdateStatusAsync(string TaskName, Guid AccreditationId, RegulatorTaskStatus status, string? comments, Guid user)
        {
            _logger.LogInformation("Updating status for task with TaskName {TaskName} And AccreditationId {AccreditationId} to {Status}", TaskName, AccreditationId, status);

            var taskStatus = await GetTaskStatus(TaskName, AccreditationId);

            var statusEntity = _context.LookupTaskStatuses.Single(lts => lts.Name == status.ToString());
            if (taskStatus == null)
            {
                var accreditation = _context.Accreditations.SingleOrDefault(x => x.ExternalId == AccreditationId);
                
                if (accreditation == null)
                    throw new KeyNotFoundException("Accreditation not found.");

                var registrationMaterial = _context.RegistrationMaterials.SingleOrDefault(x => x.Id == accreditation.RegistrationMaterialId);

                var registration = _context.Registrations.SingleOrDefault(x => x.Id == registrationMaterial.RegistrationId);
                //var registration = _context.Registrations.SingleOrDefault(x => x.Materials.FirstOrDefault(m => m.MaterialId == registrationMaterial.Id));

                if (registration == null)
                    throw new KeyNotFoundException("Registration not found.");

                var task = _context.LookupTasks.SingleOrDefault(t => t.Name == TaskName && !t.IsMaterialSpecific && t.ApplicationTypeId == registration.ApplicationTypeId);
                
                if (task == null)
                    throw new RegulatorInvalidOperationException($"No Valid Task Exists: {TaskName}");

                // Create a new entity if it doesn't exist
                taskStatus = new RegulatorAccreditationTaskStatus
                {
                    AccreditationId = accreditation.Id,
                    ExternalId = Guid.NewGuid(),
                    Task = task,
                    TaskStatus = statusEntity,
                    StatusCreatedBy = user,
                    StatusCreatedDate = DateTime.UtcNow,
                    StatusUpdatedBy = user,
                    StatusUpdatedDate = DateTime.UtcNow,
                };

                await _context.RegulatorAccreditationTaskStatus.AddAsync(taskStatus);
            }
            else
            {
                // Update the existing entity
                taskStatus.TaskStatus = statusEntity;
                taskStatus.StatusUpdatedBy = user;
                taskStatus.StatusUpdatedDate = DateTime.UtcNow;

                _context.RegulatorAccreditationTaskStatus.Update(taskStatus);
            }

            await _context.SaveChangesAsync();
            _logger.LogInformation("Successfully updated status for task with TaskName {TaskName} And AccreditationId {AccreditationId} to {Status}", TaskName, AccreditationId, status);
        }
        private async Task<RegulatorAccreditationTaskStatus?> GetTaskStatus(string TaskName, Guid AccreditationId)
        {
            return await _context.RegulatorAccreditationTaskStatus.Include(ts => ts.TaskStatus)
                .Include(a => a.Accreditation)
                .FirstOrDefaultAsync(x => x.Task.Name == TaskName && x.ExternalId == AccreditationId);
        }
    }
}