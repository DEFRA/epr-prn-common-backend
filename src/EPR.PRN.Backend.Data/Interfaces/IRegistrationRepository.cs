using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.DTO;

namespace EPR.PRN.Backend.Data.Interfaces
{
    public interface IRegistrationRepository
    {
        Task<int> CreateRegistrationAsync(int applicationTypeId, Guid organisationId);
        Task<RegistrationTaskStatus?> GetTaskStatusAsync(string taskName, int registrationId);
        Task UpdateRegistrationTaskStatusAsync(string taskName, int registrationId, TaskStatuses status, Guid userGuid);
        Task UpdateSiteAddressAsync(int registrationId, AddressDto reprocessingSiteAddress);
    }
}
