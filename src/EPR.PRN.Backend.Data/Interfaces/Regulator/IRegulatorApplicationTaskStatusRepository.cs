using EPR.PRN.Backend.API.Common.Enums;

namespace EPR.PRN.Backend.Data.Interfaces.Regulator
{
    public interface IRegulatorApplicationTaskStatusRepository
    {
        Task UpdateStatusAsync(int id, StatusTypes status, string? comments);
    }
}