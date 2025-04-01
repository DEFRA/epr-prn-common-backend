
using EPR.PRN.Backend.Data.DataModels.Registrations;

namespace EPR.PRN.Backend.Data.Interfaces
{
    public interface IRegistrationRepository
    {
        Task<Registration?> GetRegistrationById(int registrationId);
    }
}