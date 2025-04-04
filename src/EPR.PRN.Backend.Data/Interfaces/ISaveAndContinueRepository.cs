using EPR.PRN.Backend.Data.DataModels;

namespace EPR.PRN.Backend.Data.Interfaces
{
    public interface ISaveAndContinueRepository
    {
        Task AddAsync(SaveAndContinue model);
        Task<SaveAndContinue> GetAsync(int registrationId, string controller, string area);
        Task<List<SaveAndContinue>> GetAllAsync(int registrationId, string controller, string area);
    }
}
