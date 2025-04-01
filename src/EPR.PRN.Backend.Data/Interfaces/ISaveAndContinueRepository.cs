using EPR.PRN.Backend.Data.DataModels;

namespace EPR.PRN.Backend.Data.Interfaces
{
    public interface ISaveAndContinueRepository
    {
        Task AddAsync(SaveAndContinue model);
    }
}
