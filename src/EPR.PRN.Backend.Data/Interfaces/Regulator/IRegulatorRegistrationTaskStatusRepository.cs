using EPR.PRN.Backend.API.Common.Enums;

namespace EPR.PRN.Backend.Data.Interfaces.Regulator
{
    public interface IRegulatorRegistrationTaskStatusRepository
    {
        Task UpdateStatusAsync(int id, StatusTypes status, string? comments);
    }
}