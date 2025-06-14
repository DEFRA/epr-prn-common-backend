using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.DTO;

namespace EPR.PRN.Backend.Data.Interfaces;

public interface IRegistrationRepository
{
    Task<Registration> CreateRegistrationAsync(int applicationTypeId, Guid organisationId, AddressDto address);
    Task<ApplicantRegistrationTaskStatus?> GetTaskStatusAsync(string taskName, int registrationId);
    Task<Registration?> GetByOrganisationAsync(int applicationTypeId, Guid organisationId);
    Task UpdateAsync(int registrationId, AddressDto businessAddress, AddressDto reprocessingSiteAddress, AddressDto legalDocumentsAddress);
    Task UpdateRegistrationTaskStatusAsync(string taskName, int registrationId, TaskStatuses status);
    Task UpdateSiteAddressAsync(int registrationId, AddressDto reprocessingSiteAddress);
}