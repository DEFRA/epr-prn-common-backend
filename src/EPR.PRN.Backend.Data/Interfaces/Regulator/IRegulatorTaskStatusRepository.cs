using EPR.PRN.Backend.API.Common.Enums;

namespace EPR.PRN.Backend.Data.Interfaces.Regulator
{
    public interface IRegulatorTaskStatusRepository<TTaskStatus>
    {
        Task<TTaskStatus?> GetTaskStatusAsync(string TaskName, int TypeId);
        Task UpdateStatusAsync(string TaskName, int RegistrationMaterialId, RegulatorTaskStatus status, string? comments, string userName);
    }
}