using System.Diagnostics.CodeAnalysis;
using EPR.PRN.Backend.Data.DataModels.Accreditations;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces.Accreditations;
using Microsoft.EntityFrameworkCore;

namespace EPR.PRN.Backend.Data.Repositories.Accreditations;

public class AccreditationRepository(EprContext eprContext) : IAccreditationRepository
{
    public async Task<Accreditation?> GetById(Guid accreditationId)
    {
        return await eprContext.Accreditations
            .AsNoTracking()
            .Include(x => x.ApplicationType)
            .Include(x => x.AccreditationStatus)
            
            .Include(x => x.RegistrationMaterial)
                .ThenInclude(x => x.Registration)
            .Include(x => x.RegistrationMaterial)
                .ThenInclude(x => x.Material)

            .SingleOrDefaultAsync(x => x.ExternalId.Equals(accreditationId));
    }

    public async Task<Accreditation?> GetAccreditationDetails(
        Guid organisationId,
        int materialId,
        int applicationTypeId)
    {
        return await eprContext.Accreditations
            .Include(x => x.RegistrationMaterial)
                .ThenInclude(x => x.Registration)
            .AsNoTracking()
            .Where(x =>
                x.RegistrationMaterial.Registration.OrganisationId == organisationId &&
                x.RegistrationMaterialId == materialId &&
                x.RegistrationMaterial.Registration.ApplicationTypeId == applicationTypeId)
            .SingleOrDefaultAsync();

        // use this if the organisation and application type ids are kept in accreditation table in the ADR
        return await eprContext.Accreditations
            .AsNoTracking()
            .Where(x =>
                x.OrganisationId == organisationId &&
                x.RegistrationMaterialId == materialId &&
                x.ApplicationTypeId == applicationTypeId)            
            .SingleOrDefaultAsync();
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
    }

    public async Task Update(Accreditation accreditation)
    {
        var existingAccreditation = await eprContext.Accreditations.SingleAsync(x => x.ExternalId.Equals(accreditation.ExternalId));

        /*
         * There should not be a secenario where the applcation type or organisation id is changed in eiter accreditation or organisation?
         * 
         */
        //existingAccreditation = await eprContext.Accreditations
        //    .Include(x => x.RegistrationMaterial)
        //        .ThenInclude(x => x.Registration)
        //    .SingleAsync(x => x.ExternalId.Equals(accreditation.ExternalId));

        //existingAccreditation.OrganisationId = accreditation.OrganisationId;
        existingAccreditation.RegistrationMaterialId = accreditation.RegistrationMaterialId;
        //existingAccreditation.ApplicationTypeId = accreditation.ApplicationTypeId;
        //existingAccreditation.RegistrationMaterial.Registration.ApplicationTypeId = accreditation.ApplicationTypeId.GetValueOrDefault();
        existingAccreditation.AccreditationStatusId = accreditation.AccreditationStatusId;
        existingAccreditation.DecFullName = accreditation.DecFullName;
        existingAccreditation.DecJobTitle = accreditation.DecJobTitle;
        existingAccreditation.ApplicationReferenceNumber = accreditation.ApplicationReferenceNumber;
        existingAccreditation.AccreditationYear = accreditation.AccreditationYear;
        existingAccreditation.PRNTonnage = accreditation.PRNTonnage;
        //existingAccreditation.PrnTonnageAndAuthoritiesConfirmed = accreditation.PrnTonnageAndAuthoritiesConfirmed;
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
        //existingAccreditation.BusinessPlanConfirmed = accreditation.BusinessPlanConfirmed;
        existingAccreditation.UpdatedBy = accreditation.UpdatedBy;
        existingAccreditation.UpdatedOn = DateTime.UtcNow;

        eprContext.Entry(existingAccreditation).State = EntityState.Modified;
        await eprContext.SaveChangesAsync();
    }

    [ExcludeFromCodeCoverage]
    public async Task ClearDownDatabase()
    {
        // Temporary: Aid to QA whilst Accreditation uses in-memory database.
        //await eprContext.Database.EnsureDeletedAsync();
        //await eprContext.Database.EnsureCreatedAsync();
    }
}