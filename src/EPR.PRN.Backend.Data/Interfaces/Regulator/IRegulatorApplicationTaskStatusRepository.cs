using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.DataModels.Registrations;

namespace EPR.PRN.Backend.Data.Interfaces.Regulator
{
    public interface IRegulatorApplicationTaskStatusRepository
    {
        Task<RegulatorApplicationTaskStatus> GetTaskStatusAsync(string TaskName, Guid RegistrationMaterialId);
        Task UpdateStatusAsync(string TaskName, Guid RegistrationMaterialId, RegulatorTaskStatus status, string? comments, Guid user);

        Task AddApplicationTaskQueryNoteAsync(Guid taskStatusId, Guid queryBy, string note);
    }
}