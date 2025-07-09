using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.DTO;
using EPR.PRN.Backend.Data.DTO.Registration;
using EPR.PRN.Backend.Data.Interfaces.Regulator;

namespace EPR.PRN.Backend.Data.Interfaces;

public interface IRegistrationRepository : IRepositoryMarker
{
    Task<Registration?> GetRegistrationByExternalId(Guid externalId, CancellationToken cancellationToken);
    Task<Registration> CreateRegistrationAsync(int applicationTypeId, Guid organisationId, AddressDto address);
    Task<ApplicantRegistrationTaskStatus?> GetTaskStatusAsync(string taskName, Guid registrationId);
    Task<Registration?> GetByOrganisationAsync(int applicationTypeId, Guid organisationId);
    Task UpdateAsync(Guid registrationId, AddressDto businessAddress, AddressDto reprocessingSiteAddress, AddressDto legalDocumentsAddress);
    Task UpdateRegistrationTaskStatusAsync(string taskName, Guid registrationId, TaskStatuses status);
    Task UpdateSiteAddressAsync(Guid registrationId, AddressDto reprocessingSiteAddress);
    Task<IEnumerable<RegistrationOverviewDto>> GetRegistrationsOverviewForOrgIdAsync(Guid organisationId);
    Task UpdateApplicantRegistrationTaskStatusAsync(string taskName, Guid registrationId, TaskStatuses status);
    Task<Registration?> GetAsync(Guid registrationId);
    Task<List<LookupApplicantRegistrationTask>> GetRequiredTasks(int applicationTypeId, bool isMaterialSpecific, int journeyTypeId);
    Task<Registration> GetTasksForRegistrationAndMaterialsAsync(Guid registrationId);
}