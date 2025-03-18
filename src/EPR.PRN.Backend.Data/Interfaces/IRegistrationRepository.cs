using EPR.PRN.Backend.Data.DataModels;

namespace EPR.PRN.Backend.Data.Interfaces
{
    public  interface IRegistrationRepository
    {
        Task<Registration> GetByIdAsync(int id);
    }
}
