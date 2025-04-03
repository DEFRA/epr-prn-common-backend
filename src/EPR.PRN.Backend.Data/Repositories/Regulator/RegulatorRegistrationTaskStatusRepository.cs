using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces.Regulator;

namespace EPR.PRN.Backend.Data.Repositories.Regulator
{
    public class RegulatorRegistrationTaskStatusRepository(EprRegistrationsContext context) : IRegulatorRegistrationTaskStatusRepository
    {
        public async Task UpdateStatusAsync(int id, StatusTypes status, string? comments)
        {
            RegulatorRegistrationTaskStatus? regulatorRegistrationTaskStatus = await context.RegulatorRegistrationTaskStatus.FindAsync(id);
            if(regulatorRegistrationTaskStatus == null) {
                throw new KeyNotFoundException($"Regulator registration task status not found :{id}");
            }

            regulatorRegistrationTaskStatus.TaskStatusId = (int)status;
            regulatorRegistrationTaskStatus.Comments = comments;
            await context.SaveChangesAsync();
        }
    }
}
