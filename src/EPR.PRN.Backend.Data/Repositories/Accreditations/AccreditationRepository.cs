using System.Diagnostics.CodeAnalysis;
using EPR.PRN.Backend.Data.DataModels.Accreditations;
using EPR.PRN.Backend.Data.Interfaces.Accreditations;
using Microsoft.EntityFrameworkCore;

namespace EPR.PRN.Backend.Data.Repositories.Accreditations;

public class AccreditationRepository(EprAccreditationContext eprContext) : IAccreditationRepository
{
    public async Task<Accreditation?> GetById(Guid accreditationId)
    {
        return await eprContext.Accreditations
            .AsNoTracking()
            .Include(x => x.ApplicationType)
            .Include(x => x.AccreditationStatus)
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
        accreditation.CreatedDate = currentTimestamp;
        accreditation.UpdatedDate = currentTimestamp;
        accreditation.ExternalId = Guid.NewGuid();

        eprContext.Accreditations.Add(accreditation);
        await eprContext.SaveChangesAsync();
    }

    public async Task Update(Accreditation accreditation)
    {
        var existingAccreditation = await eprContext.Accreditations.SingleAsync(x => x.ExternalId.Equals(accreditation.ExternalId));

        existingAccreditation.OrganisationId = accreditation.OrganisationId;
        existingAccreditation.RegistrationMaterialId = accreditation.RegistrationMaterialId;
        existingAccreditation.ApplicationTypeId = accreditation.ApplicationTypeId;
        existingAccreditation.AccreditationStatusId = accreditation.AccreditationStatusId;
        existingAccreditation.DecFullName = accreditation.DecFullName;
        existingAccreditation.DecJobTitle = accreditation.DecJobTitle;
        existingAccreditation.AccreferenceNumber = accreditation.AccreferenceNumber;
        existingAccreditation.AccreditationYear = accreditation.AccreditationYear;
        existingAccreditation.PrnTonnage = accreditation.PrnTonnage;
        existingAccreditation.PrnTonnageAndAuthoritiesConfirmed = accreditation.PrnTonnageAndAuthoritiesConfirmed;
        existingAccreditation.InfrastructurePercentage = accreditation.InfrastructurePercentage;
        existingAccreditation.PackagingWastePercentage = accreditation.PackagingWastePercentage;
        existingAccreditation.BusinessCollectionsPercentage = accreditation.BusinessCollectionsPercentage;
        existingAccreditation.NewUsesPercentage = accreditation.NewUsesPercentage;
        existingAccreditation.NewMarketsPercentage = accreditation.NewMarketsPercentage;
        existingAccreditation.CommunicationsPercentage = accreditation.CommunicationsPercentage;
        existingAccreditation.OtherPercentage = accreditation.OtherPercentage;
        existingAccreditation.InfrastructureNotes = accreditation.InfrastructureNotes;
        existingAccreditation.PackagingWasteNotes = accreditation.PackagingWasteNotes;
        existingAccreditation.BusinessCollectionsNotes = accreditation.BusinessCollectionsNotes;
        existingAccreditation.NewUsesNotes = accreditation.NewUsesNotes;
        existingAccreditation.NewMarketsNotes = accreditation.NewMarketsNotes;
        existingAccreditation.CommunicationsNotes = accreditation.CommunicationsNotes;
        existingAccreditation.OtherNotes = accreditation.OtherNotes;
        existingAccreditation.UpdatedBy = accreditation.UpdatedBy;
        existingAccreditation.UpdatedDate = DateTime.UtcNow;

        eprContext.Entry(existingAccreditation).State = EntityState.Modified;
        await eprContext.SaveChangesAsync();
    }

    [ExcludeFromCodeCoverage]
    public async Task ClearDownDatabase()
    {
        // Temporary: Aid to QA whilst Accreditation uses in-memory database.
        await eprContext.Database.EnsureDeletedAsync();
        await eprContext.Database.EnsureCreatedAsync();
    }
}