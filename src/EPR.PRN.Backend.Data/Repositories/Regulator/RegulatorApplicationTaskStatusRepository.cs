using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces.Regulator;

namespace EPR.PRN.Backend.Data.Repositories.Regulator
{
    public class RegulatorApplicationTaskStatusRepository(EprRegistrationsContext context) : IRegulatorApplicationTaskStatusRepository
    {
        public async Task UpdateStatusAsync(int id, StatusTypes status1, string? comments)
        {
            RegulatorApplicationTaskStatus? regulatorApplicationTaskStatus = await context.RegulatorApplicationTaskStatus.FindAsync(id);
            if(regulatorApplicationTaskStatus == null) {
                throw new KeyNotFoundException($"Regulator application task status not found: {id}");
            }

            regulatorApplicationTaskStatus.TaskStatusId = (int)status1;
            regulatorApplicationTaskStatus.Comments = comments;
            await context.SaveChangesAsync();
        }
    }
}
