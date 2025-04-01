using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces;

namespace EPR.PRN.Backend.Data.Repositories
{
    public class RegulatorRegistrationTaskStatusRepository(EprRegistrationsContext context) : IRegulatorRegistrationTaskStatusRepository
    {
        public async Task UpdateStatusAsync(int id, StatusTypes status1, string? comments)
        {
            RegulatorRegistrationTaskStatus? regulatorRegistrationTaskStatus = context.RegulatorRegistrationTaskStatus.Find(id);
            if(regulatorRegistrationTaskStatus == null) {
                throw new Exception("Regulator registration task status not found");
            }

            regulatorRegistrationTaskStatus.TaskStatusId = (int)status1;
            regulatorRegistrationTaskStatus.Comments = comments;
            await context.SaveChangesAsync();
        }
    }
}
