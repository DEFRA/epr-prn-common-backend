using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Common.Exceptions;
using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EPR.PRN.Backend.Data.Repositories
{
    public class MaterialRepository(EprContext context, ILogger<MaterialRepository> logger) : IMaterialRepository
    {
		public async Task<IEnumerable<Material>> GetAllMaterials()
		{
			return await context.Material
								.AsNoTracking()
								.Include(m => m.PrnMaterialMappings)
								.ToListAsync();
		}

        public async Task<RegistrationMaterialContact> UpsertRegistrationMaterialContact(Guid registrationMaterialId, Guid userId)
        {
            var registrationMaterial = await context.RegistrationMaterials
                .Include(rm => rm.RegistrationMaterialContact)
                .SingleOrDefaultAsync(rm => rm.ExternalId == registrationMaterialId);

            if (registrationMaterial is null)
            {
                throw new KeyNotFoundException("Registration material not found.");
            }

            var registrationMaterialContact = registrationMaterial.RegistrationMaterialContact;

            if (registrationMaterialContact is null)
            {
                registrationMaterialContact = new RegistrationMaterialContact
                {
                    ExternalId = Guid.NewGuid(),
                    RegistrationMaterialId = registrationMaterial.Id,
                    UserId = userId
                };

                await context.RegistrationMaterialContacts.AddAsync(registrationMaterialContact);
            }
            else
            {
                registrationMaterialContact.UserId = userId;
            }

            await context.SaveChangesAsync();

            return registrationMaterialContact;
        }

        public async Task UpdateApplicationRegistrationTaskStatusAsync(string taskName, Guid registrationMaterialId, TaskStatuses status)
        {
            logger.LogInformation("Updating application status for task with TaskName {TaskName} And registrationMaterialId {registrationMaterialId} to {Status}", taskName, registrationMaterialId, status);

            var statusEntity = await context.LookupTaskStatuses.SingleAsync(lts => lts.Name == status.ToString());

            var taskStatus = await GetTaskStatusAsync(taskName, registrationMaterialId);
            if (taskStatus is null)
            {
                var registrationMaterial = await context.RegistrationMaterials.FirstOrDefaultAsync(o => o.ExternalId == registrationMaterialId);
                if (registrationMaterial is null)
                {
                    throw new KeyNotFoundException();
                }

                var registration = await context.Registrations.FirstOrDefaultAsync(o => o.Id == registrationMaterial.RegistrationId);
                if (registration is null)
                {
                    throw new KeyNotFoundException();
                }

                var task = await context
                    .LookupApplicantRegistrationTasks
                    .SingleOrDefaultAsync(t => t.Name == taskName && t.IsMaterialSpecific && t.ApplicationTypeId == registration.ApplicationTypeId);

                if (task is null)
                {
                    throw new RegulatorInvalidOperationException($"No Valid Task Exists: {taskName}");
                }

                // Create a new entity if it doesn't exist
                taskStatus = new ApplicantRegistrationTaskStatus
                {
                    ExternalId = Guid.NewGuid(),
                    RegistrationId = registration.Id,
                    Task = task,
                    TaskStatus = statusEntity,
                    RegistrationMaterialId = registrationMaterial.Id
                };

                await context.RegistrationTaskStatus.AddAsync(taskStatus);
            }
            else
            {
                // Update the existing entity
                taskStatus.TaskStatus = statusEntity;
                context.RegistrationTaskStatus.Update(taskStatus);
            }
            await context.SaveChangesAsync();

            logger.LogInformation("Successfully updated status for task with TaskName {TaskName} And RegistrationMaterialId {registrationMaterialId} to {Status}", taskName, registrationMaterialId, status);
        }

        public async Task<ApplicantRegistrationTaskStatus?> GetTaskStatusAsync(string taskName, Guid registrationMaterialId)
        {
            var taskStatus = await context
                .RegistrationTaskStatus
                .Include(ts => ts.TaskStatus)
                .Include(o => o.Registration)
                .FirstOrDefaultAsync(x => x.Task.Name == taskName && x.RegistrationMaterial.ExternalId == registrationMaterialId);

            return taskStatus;
        }
    }
}
