using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using EPR.PRN.Backend.Data.DataModels.Accreditations;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.DTO.Accreditiation;
using EPR.PRN.Backend.Data.Interfaces.Accreditation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EPR.PRN.Backend.Data.Repositories.Accreditations;

public class AccreditationRepository(EprContext eprContext, IMapper mapper, ILogger<AccreditationRepository> logger) : IAccreditationRepository
{
    public async Task<Accreditation?> GetById(Guid accreditationId)
    {
        logger.LogInformation("Retrieving accreditation details for ExternalId: {AccreditationId}.", accreditationId);
        var accreditation =  await eprContext.Accreditations
            .AsNoTracking()            
            .Include(x => x.AccreditationStatus)           
            .Include(x => x.RegistrationMaterial)
                .ThenInclude(x => x.Registration)                
            .Include(x => x.RegistrationMaterial)
                .ThenInclude(x => x.Material)
            .SingleOrDefaultAsync(x => x.ExternalId.Equals(accreditationId));
        logger.LogInformation("Retrieved accreditation details for ExternalId: {AccreditationId} with Id {Id}.", accreditationId, accreditation?.Id);
        return accreditation;
    }

    public async Task<Accreditation?> GetAccreditationDetails(
        Guid organisationId,
        int materialId,
        int applicationTypeId)
    {
        logger.LogInformation("Retrieving accreditation details for OrganisationId: {OrganisationId}, MaterialId: {MaterialId}, ApplicationTypeId: {ApplicationTypeId}.", organisationId, materialId, applicationTypeId);
        var accreditation =  await eprContext.Accreditations
            .Include(x => x.RegistrationMaterial)
                .ThenInclude(x => x.Registration)
            .AsNoTracking()
            .Where(x =>
                x.RegistrationMaterial.Registration.OrganisationId == organisationId &&
                x.RegistrationMaterialId == materialId &&
                x.RegistrationMaterial.Registration.ApplicationTypeId == applicationTypeId)
            .SingleOrDefaultAsync();
        logger.LogInformation("Retrieved accreditation details for OrganisationId: {OrganisationId}, MaterialId: {MaterialId}, ApplicationTypeId: {ApplicationTypeId} with Id {Id}.", organisationId, materialId, applicationTypeId, accreditation?.Id);
        return accreditation;
    }

    public async Task Create(Accreditation accreditation)
    {
   
        var currentTimestamp = DateTime.UtcNow;
        accreditation.CreatedOn = currentTimestamp;
        accreditation.UpdatedOn = currentTimestamp;
        accreditation.ExternalId = Guid.NewGuid();
        accreditation.CreatedBy =  accreditation.ExternalId; // These need to be replaced with the correct ids that must be passed through the apis from the front end.
        accreditation.UpdatedBy = accreditation.ExternalId;
        eprContext.Accreditations.Add(accreditation);
        await eprContext.SaveChangesAsync();
        logger.LogInformation("Created new accreditation with ExternalId: {ExternalId} and Id: {Id}", accreditation.ExternalId, accreditation.Id);
    }

    public async Task Update(Accreditation accreditation)
    {
        logger.LogInformation("Updating accreditation with ExternalId: {ExternalId} and Id {Id}.", accreditation.ExternalId, accreditation.Id);
        var existingAccreditation = await eprContext.Accreditations.SingleAsync(x => x.ExternalId.Equals(accreditation.ExternalId));

        /*
         * There should not be a secenario where the applcation type or organisation id is changed in eiter accreditation or organisation?
         * 
         */

        existingAccreditation.RegistrationMaterialId = accreditation.RegistrationMaterialId;
        existingAccreditation.AccreditationStatusId = accreditation.AccreditationStatusId;
        existingAccreditation.DecFullName = accreditation.DecFullName;
        existingAccreditation.DecJobTitle = accreditation.DecJobTitle;
        existingAccreditation.ApplicationReferenceNumber = accreditation.ApplicationReferenceNumber;
        existingAccreditation.AccreditationYear = accreditation.AccreditationYear;
        existingAccreditation.PRNTonnage = accreditation.PRNTonnage;
        existingAccreditation.InfrastructurePercentage = accreditation.InfrastructurePercentage;
        existingAccreditation.RecycledWastePercentage = accreditation.RecycledWastePercentage;
        existingAccreditation.BusinessCollectionsPercentage = accreditation.BusinessCollectionsPercentage;
        existingAccreditation.NewUsersRecycledPackagingWastePercentage = accreditation.NewUsersRecycledPackagingWastePercentage;
        existingAccreditation.NewMarketsPercentage = accreditation.NewMarketsPercentage;
        existingAccreditation.CommunicationsPercentage = accreditation.CommunicationsPercentage;
        existingAccreditation.NotCoveredOtherCategoriesPercentage = accreditation.NotCoveredOtherCategoriesPercentage;
        existingAccreditation.InfrastructureNotes = accreditation.InfrastructureNotes;
        existingAccreditation.RecycledWasteNotes = accreditation.RecycledWasteNotes;
        existingAccreditation.BusinessCollectionsNotes = accreditation.BusinessCollectionsNotes;
        existingAccreditation.NewUsersRecycledPackagingWasteNotes = accreditation.NewUsersRecycledPackagingWasteNotes;
        existingAccreditation.NewMarketsNotes = accreditation.NewMarketsNotes;
        existingAccreditation.CommunicationsNotes = accreditation.CommunicationsNotes;
        existingAccreditation.NotCoveredOtherCategoriesNotes = accreditation.NotCoveredOtherCategoriesNotes;
        existingAccreditation.UpdatedBy = accreditation.UpdatedBy;
        existingAccreditation.UpdatedOn = DateTime.UtcNow;

        eprContext.Entry(existingAccreditation).State = EntityState.Modified;
        await eprContext.SaveChangesAsync();
        logger.LogInformation("Updated accreditation with ExternalId: {ExternalId} and Id {Id}.", accreditation.ExternalId, accreditation.Id);  
    }



    public async Task<IEnumerable<AccreditationOverviewDto>> GetAccreditationOverviewForOrgId(Guid organisationId)
    {
        var data= await eprContext.Accreditations
             .Include(x => x.RegistrationMaterial)
                .ThenInclude(x => x.Registration)           
            .Where(a => a.RegistrationMaterial.Registration.OrganisationId == organisationId)            
            .ToListAsync();

        return mapper.Map<List<AccreditationOverviewDto>>(data);
    }
}