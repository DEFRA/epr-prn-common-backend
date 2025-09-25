using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.DTO;
using EPR.PRN.Backend.Data.DTO.Registration;

namespace EPR.PRN.Backend.Data.Interfaces;

public interface IRegistrationRepository
{
    Task<Registration?> GetRegistrationByExternalId(Guid externalId, CancellationToken cancellationToken);
    Task<Registration> CreateRegistrationAsync(int applicationTypeId, Guid organisationId, AddressDto reprocessingSiteAddress);
    Task<ApplicantRegistrationTaskStatus?> GetTaskStatusAsync(string taskName, Guid registrationId);
    Task<Registration?> GetByOrganisationAsync(int applicationTypeId, Guid organisationId);
    Task UpdateAsync(Guid registrationId, AddressDto businessAddress, AddressDto reprocessingSiteAddress, AddressDto legalDocumentsAddress);
    Task UpdateRegistrationTaskStatusAsync(string taskName, Guid registrationId, TaskStatuses status);
    Task UpdateSiteAddressAsync(Guid registrationId, AddressDto reprocessingSiteAddress);
    Task<IEnumerable<RegistrationOverviewDto>> GetRegistrationsOverviewForOrgIdAsync(Guid organisationId);
}