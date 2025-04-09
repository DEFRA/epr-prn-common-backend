using EPR.PRN.Backend.API.Common.Enums;

namespace EPR.PRN.Backend.Data.Interfaces.Regulator
{
    public interface IRegulatorTaskStatusRepository<TTaskStatus>
    {
        Task<TTaskStatus> GetTaskStatusByIdAsync(int id);
        Task UpdateStatusAsync(int id, StatusTypes status, string? comments);
    }
}