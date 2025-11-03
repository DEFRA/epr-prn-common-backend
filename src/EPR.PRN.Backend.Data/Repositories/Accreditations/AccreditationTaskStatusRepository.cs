using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.DataModels.Accreditations;
using EPR.PRN.Backend.Data.Interfaces.Accreditation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EPR.PRN.Backend.Data.Repositories.Accreditations
{
    public class AccreditationTaskStatusRepository : IAccreditationTaskStatusRepository
    {
        private readonly EprContext _context;
        private readonly ILogger<AccreditationTaskStatusRepository> _logger;

        public AccreditationTaskStatusRepository(EprContext context, ILogger<AccreditationTaskStatusRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<AccreditationTaskStatus?> GetTaskStatusAsync(string taskName, Guid accreditationId)
        {
            return await GetTaskStatus(taskName, accreditationId);
        }

        public async Task UpdateStatusAsync(string taskName, Guid accreditationId, TaskStatuses status)
        {
            _logger.LogInformation("Updating status for task with TaskName {TaskName} And AccreditationId {AccreditationId} to {Status}", taskName, accreditationId, status);

            var taskStatus = await GetTaskStatus(taskName, accreditationId);

            var statusEntity = _context.LookupTaskStatuses.Single(lts => lts.Name == status.ToString());

            if (taskStatus == null)
            {
                var accreditation = await _context.Accreditations
                                         .Include(a => a.RegistrationMaterial)
                                             .ThenInclude(rm => rm.Registration)
                                         .SingleOrDefaultAsync(x => x.ExternalId == accreditationId);

                if (accreditation == null)
                    throw new KeyNotFoundException("Accreditation not found.");

                var registration = accreditation.RegistrationMaterial?.Registration;

                if (registration == null)
                    throw new KeyNotFoundException("Registration not found.");

                var journeyTypeId = _context.LookupJourneyTypes.SingleOrDefault(jt => jt.Name == "Accreditation")?.Id;
                if (journeyTypeId == null)
                    throw new InvalidOperationException("Journey type 'Accreditation' not found.");
                var task = await _context.LookupApplicantRegistrationTasks.SingleOrDefaultAsync(t => t.Name == taskName
                                                                                        && t.JourneyTypeId == journeyTypeId
                                                                                        && t.ApplicationTypeId == registration!.ApplicationTypeId
                                                                                        && t.IsMaterialSpecific);

                if (task == null)
                    throw new InvalidOperationException($"No Valid Task Exists: {taskName}");

                taskStatus = new AccreditationTaskStatus
                {
                    Accreditation = accreditation,
                    ExternalId = Guid.NewGuid(),
                    Task = task,
                    TaskStatus = statusEntity
                };

                await _context.AccreditationTaskStatus.AddAsync(taskStatus);


            }
            else
            {
                // Update the existing entity
                taskStatus.TaskStatus = statusEntity;
                _context.AccreditationTaskStatus.Update(taskStatus);
            }

            await _context.SaveChangesAsync();
            _logger.LogInformation("Successfully updated status for task with TaskName {TaskName} And AccreditationId {AccreditationId} to {Status}", taskName, accreditationId, status);
        }

        private async Task<AccreditationTaskStatus?> GetTaskStatus(string taskName, Guid accreditationId)
        {
            return await _context.AccreditationTaskStatus
                .Include(ts => ts.Task)
                .Include(ts => ts.TaskStatus)
                .Include(a => a.Accreditation)
                .FirstOrDefaultAsync(ts => ts.Accreditation.ExternalId == accreditationId && ts.Task.Name == taskName);
        }
    }
}
