using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces;

namespace EPR.PRN.Backend.Data.Repositories
{
    public class RegistrationRepository(EprRegistrationsContext context) : IRegistrationRepository
    {
        public async Task<Registration?> GetRegistrationById(int registrationId)
        {
            //context.Database.EnsureCreated();
            return await context.Registrations.FindAsync(registrationId);
        }
    }
}
