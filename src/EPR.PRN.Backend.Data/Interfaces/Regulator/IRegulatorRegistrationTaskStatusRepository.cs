using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.DataModels.Registrations;

namespace EPR.PRN.Backend.Data.Interfaces.Regulator
{
    public interface IRegulatorRegistrationTaskStatusRepository
    {
        Task<RegulatorRegistrationTaskStatus?> GetTaskStatusAsync(string TaskName, Guid RegistrationId);
        Task UpdateStatusAsync(string TaskName, Guid RegistrationId, RegulatorTaskStatus status, string? comments, Guid user);
        Task AddRegistrationTaskQueryNoteAsync(Guid taskStatusId, Guid queryBy, string note);
    }
}