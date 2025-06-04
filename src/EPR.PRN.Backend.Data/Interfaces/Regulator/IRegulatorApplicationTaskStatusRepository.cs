using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.DataModels.Registrations;

namespace EPR.PRN.Backend.Data.Interfaces.Regulator
{
    public interface IRegulatorApplicationTaskStatusRepository
    {
        Task<RegulatorApplicationTaskStatus> GetTaskStatusAsync(string TaskName, int RegistrationMaterialId);
        Task UpdateStatusAsync(string TaskName, int RegistrationMaterialId, RegulatorTaskStatus status, string? comments, Guid user);
    }
}