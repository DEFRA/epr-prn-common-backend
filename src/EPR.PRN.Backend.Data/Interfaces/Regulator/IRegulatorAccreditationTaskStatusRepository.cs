using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.DataModels.Registrations;

namespace EPR.PRN.Backend.Data.Interfaces.Regulator
{
    public interface IRegulatorAccreditationTaskStatusRepository
    {
        Task<RegulatorAccreditationTaskStatus?> GetTaskStatusAsync(string TaskName, Guid RegistrationId);
        Task UpdateStatusAsync(string TaskName, Guid RegistrationId, RegulatorTaskStatus status, string? comments, Guid user);
    }
}