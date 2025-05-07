using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.DataModels.Registrations;

namespace EPR.PRN.Backend.Data.Interfaces.Regulator
{
    public interface IRegulatorRegistrationTaskStatusRepository
    {
        Task<RegulatorRegistrationTaskStatus> GetTaskStatusAsync(string TaskName, int RegistrationId);
        Task UpdateStatusAsync(string TaskName, int RegistrationId, RegulatorTaskStatus status, string? comments, string userName);
    }
}