using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces;

namespace EPR.PRN.Backend.Data.Repositories
{
    public class RegulatorApplicationTaskStatusRepository(EprRegistrationsContext context) : IRegulatorApplicationTaskStatusRepository
    {
        public async Task UpdateStatusAsync(int id, StatusTypes status1, string? comments)
        {
            RegulatorApplicationTaskStatus? regulatorApplicationTaskStatus = context.RegulatorApplicationTaskStatus.Find(id);
            if(regulatorApplicationTaskStatus == null) {
                throw new Exception("Regulator application task status not found");
            }

            regulatorApplicationTaskStatus.TaskStatusId = (int)status1;
            regulatorApplicationTaskStatus.Comments = comments;
            await context.SaveChangesAsync();
        }
    }
}
