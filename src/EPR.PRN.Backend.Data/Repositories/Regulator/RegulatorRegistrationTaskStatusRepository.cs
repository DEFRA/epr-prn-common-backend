using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Common.Exceptions;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace EPR.PRN.Backend.Data.Repositories.Regulator
{
    public class RegulatorRegistrationTaskStatusRepository : IRegulatorRegistrationTaskStatusRepository
    {
        private readonly EprContext _context;
        private readonly ILogger<RegulatorRegistrationTaskStatusRepository> _logger;

        public RegulatorRegistrationTaskStatusRepository(EprContext context, ILogger<RegulatorRegistrationTaskStatusRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<RegulatorRegistrationTaskStatus?> GetTaskStatusAsync(string TaskName, Guid RegistrationId)
        {
            return await GetTaskStatus(TaskName, RegistrationId);
        }

        public async Task UpdateStatusAsync(string TaskName, Guid RegistrationId, RegulatorTaskStatus status, string? comments, Guid user)
        {
            _logger.LogInformation("Updating status for task with TaskName {TaskName} And RegistrationId {RegistrationId} to {Status}", TaskName, RegistrationId, status);

            var taskStatus = await GetTaskStatus(TaskName, RegistrationId);

            var statusEntity = _context.LookupTaskStatuses.Single(lts => lts.Name == status.ToString());
            if (taskStatus == null)
            {
                var registration = _context.Registrations.SingleOrDefault(x => x.ExternalId == RegistrationId);
                if (registration == null)
                {
                    throw new KeyNotFoundException();
                }

                var task = _context.LookupTasks.SingleOrDefault(t => t.Name == TaskName && !t.IsMaterialSpecific && t.ApplicationTypeId == registration.ApplicationTypeId);
                if (task == null)
                {
                    throw new RegulatorInvalidOperationException($"No Valid Task Exists: {TaskName}");
                }

                // Create a new entity if it doesn't exist
                taskStatus = new RegulatorRegistrationTaskStatus
                {
                    RegistrationId = registration.Id,
                    ExternalId = Guid.NewGuid(),
                    Task = task,
                    TaskStatus = statusEntity,
                    StatusCreatedBy = user,
                    StatusCreatedDate = DateTime.UtcNow,
                    StatusUpdatedBy = user,
                    StatusUpdatedDate = DateTime.UtcNow,
                };

                await _context.RegulatorRegistrationTaskStatus.AddAsync(taskStatus);

                if (comments != null && status == RegulatorTaskStatus.Queried)
                {

                    var queryNote = new QueryNote
                    {
                        Notes = comments,
                        CreatedBy = user,
                        CreatedDate = DateTime.UtcNow
                    };
                    await _context.QueryNote.AddAsync(queryNote);

                    var registrationTaskStatusQueryNotes = new RegistrationTaskStatusQueryNotes
                    {
                        QueryNote = queryNote,
                        RegulatorRegistrationTaskStatus = taskStatus
                    };
                    await _context.RegistrationTaskStatusQueryNotes.AddAsync(registrationTaskStatusQueryNotes);

                }
            }
            else
            {
                // Update the existing entity
                taskStatus.TaskStatus = statusEntity;
                taskStatus.StatusUpdatedBy = user;
                taskStatus.StatusUpdatedDate = DateTime.UtcNow;

                _context.RegulatorRegistrationTaskStatus.Update(taskStatus);
            }

            await _context.SaveChangesAsync();
            _logger.LogInformation("Successfully updated status for task with TaskName {TaskName} And RegistrationId {RegistrationId} to {Status}", TaskName, RegistrationId, status);
        }
        public async Task AddRegistrationTaskQueryNoteAsync(Guid taskStatusId, Guid queryBy, string note)
        {
            var registrationTaskStatus = await _context.RegulatorRegistrationTaskStatus
                .FirstOrDefaultAsync(rm => rm.ExternalId == taskStatusId);

            if (registrationTaskStatus is null)
            {
                throw new KeyNotFoundException("Regulator Registration Task Status not found.");
            }
            else if ((RegulatorTaskStatus)registrationTaskStatus.TaskStatusId == RegulatorTaskStatus.Completed)
            {
                throw new KeyNotFoundException("Cannot insert query because the Regulator Registration Task Status is completed.");
            }
            var querynote = new QueryNote
            {
                Notes = note,
                CreatedBy = queryBy,
                CreatedDate = DateTime.UtcNow
            };
            await _context.QueryNote.AddAsync(querynote);

            var registrationTaskStatusQueryNotes = new RegistrationTaskStatusQueryNotes
            {
                QueryNote = querynote,
                RegulatorRegistrationTaskStatus = registrationTaskStatus
            };
            await _context.RegistrationTaskStatusQueryNotes.AddAsync(registrationTaskStatusQueryNotes);

          

            await _context.SaveChangesAsync();

            _logger.LogInformation("Successfully add query note for task");

        }
        private async Task<RegulatorRegistrationTaskStatus?> GetTaskStatus(string TaskName, Guid RegistrationId)
        {
            return await _context.RegulatorRegistrationTaskStatus.Include(ts => ts.TaskStatus).Include(y => y.Registration).FirstOrDefaultAsync(x => x.Task.Name == TaskName && x.Registration.ExternalId == RegistrationId);
        }
    }
}