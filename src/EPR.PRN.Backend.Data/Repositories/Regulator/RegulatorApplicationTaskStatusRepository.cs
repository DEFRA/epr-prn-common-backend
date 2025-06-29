﻿using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Common.Exceptions;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EPR.PRN.Backend.Data.Repositories.Regulator
{
    public class RegulatorApplicationTaskStatusRepository : IRegulatorApplicationTaskStatusRepository
    {
        private readonly EprContext _context;
        private readonly ILogger<RegulatorApplicationTaskStatusRepository> _logger;

        public RegulatorApplicationTaskStatusRepository(EprContext context, ILogger<RegulatorApplicationTaskStatusRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<RegulatorApplicationTaskStatus?> GetTaskStatusAsync(string TaskName, Guid RegistrationMaterialId)
        {
            return await GetTaskStatus(TaskName, RegistrationMaterialId);
        }

        public async Task UpdateStatusAsync(string TaskName, Guid RegistrationMaterialId, RegulatorTaskStatus status, string? comments, Guid user)
        {
            _logger.LogInformation("Updating status for task with TaskName {TaskName} And RegistrationMaterialId {RegistrationMaterialId} to {Status}", TaskName, RegistrationMaterialId, status);

            var taskStatus = await GetTaskStatus(TaskName, RegistrationMaterialId);

            var statusEntity = _context.LookupTaskStatuses.Single(lts => lts.Name == status.ToString());

            if (taskStatus == null)
            {
                var registrationMaterial = _context.RegistrationMaterials.Include(r => r.Registration).SingleOrDefault(r => r.ExternalId == RegistrationMaterialId);
                if (registrationMaterial == null)
                {
                    throw new KeyNotFoundException();
                }

                var task = _context.LookupTasks.SingleOrDefault(t => t.Name == TaskName && t.IsMaterialSpecific && t.ApplicationTypeId == registrationMaterial.Registration.ApplicationTypeId);
                if (task == null)
                {
                    throw new RegulatorInvalidOperationException($"No Valid Task Exists: {TaskName}");
                }

                // Create a new entity if it doesn't exist
                taskStatus = new RegulatorApplicationTaskStatus
                {
                    Task = task,
                    RegistrationMaterialId = registrationMaterial.Id,
                    ExternalId = Guid.NewGuid(),
                    TaskStatus = statusEntity,
                    StatusCreatedBy = user,
                    StatusCreatedDate = DateTime.UtcNow,
                    //StatusUpdatedBy = user,
                    //StatusUpdatedDate = DateTime.UtcNow,
                };


                await _context.RegulatorApplicationTaskStatus.AddAsync(taskStatus);

                if (comments != null && status == RegulatorTaskStatus.Queried)
                {

                    var queryNote = new Note
                    {
                        Notes = comments,
                        CreatedBy = user,
                        CreatedDate = DateTime.UtcNow
                    };
                    await _context.QueryNote.AddAsync(queryNote);

                    var applicationTaskStatusQueryNotes = new ApplicationTaskStatusQueryNote
                    {
                        Note = queryNote,
                        RegulatorApplicationTaskStatus = taskStatus
                    };
                    await _context.ApplicationTaskStatusQueryNotes.AddAsync(applicationTaskStatusQueryNotes);

                }

            }
            else
            {
                // Update the existing entity
                taskStatus.TaskStatus = statusEntity;
                taskStatus.StatusUpdatedBy = user;
                taskStatus.StatusUpdatedDate = DateTime.UtcNow;

                _context.RegulatorApplicationTaskStatus.Update(taskStatus);
                
            }

            await _context.SaveChangesAsync();
            _logger.LogInformation("Successfully updated status for task with TaskName {TaskName} And RegistrationMaterialId {RegistrationMaterialId} to {Status}", TaskName, RegistrationMaterialId, status);
        }
        public async Task AddApplicationTaskQueryNoteAsync(Guid taskStatusId, Guid queryBy, string note)
        {
            var applicationTaskStatus = await _context.RegulatorApplicationTaskStatus
                .FirstOrDefaultAsync(rm => rm.ExternalId == taskStatusId);

            if (applicationTaskStatus is null)
            {
                throw new KeyNotFoundException("Regulator Application Task Status not found.");
            }
            else if ((RegulatorTaskStatus)applicationTaskStatus.TaskStatusId == RegulatorTaskStatus.Completed)
            {
                throw new KeyNotFoundException("Cannot insert query because the Regulator Application Task Status is completed.");
            }
            var querynote = new Note
                {
                    Notes = note,
                    CreatedBy = queryBy,
                    CreatedDate = DateTime.UtcNow
                };
            await _context.QueryNote.AddAsync(querynote);

            var applicationTaskStatusQueryNotes =  new ApplicationTaskStatusQueryNote
                {
                    Note = querynote,
                    RegulatorApplicationTaskStatusId = applicationTaskStatus.Id
                };
            await _context.ApplicationTaskStatusQueryNotes.AddAsync(applicationTaskStatusQueryNotes);

            await _context.SaveChangesAsync();

            _logger.LogInformation("Successfully add query note for task");

        }

        private async Task<RegulatorApplicationTaskStatus?> GetTaskStatus(string TaskName, Guid RegistrationMaterialId)
        {
            return await _context.RegulatorApplicationTaskStatus.Include(ts => ts.Task).Include(ts => ts.TaskStatus).Include(y => y.RegistrationMaterial).FirstOrDefaultAsync(x => x.Task.Name == TaskName && x.RegistrationMaterial.ExternalId == RegistrationMaterialId);
        }

       
    }
}