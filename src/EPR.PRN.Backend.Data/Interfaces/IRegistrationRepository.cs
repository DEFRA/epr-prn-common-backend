using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.DTO;

namespace EPR.PRN.Backend.Data.Interfaces
{
    public interface IRegistrationRepository
    {
        Task<RegistrationTaskStatus?> GetTaskStatusAsync(string taskName, int registrationId);
        Task UpdateRegistrationTaskStatusAsync(string taskName, int registrationId, TaskStatuses status);
        Task UpdateSiteAddressAsync(int registrationId, AddressDto reprocessingSiteAddress, AddressDto legalDocumentAddress);
    }
}
