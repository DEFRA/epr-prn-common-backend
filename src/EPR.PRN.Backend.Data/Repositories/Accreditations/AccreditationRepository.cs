using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using EPR.PRN.Backend.Data.DataModels.Accreditations;
using EPR.PRN.Backend.Data.DTO.Accreditiation;
using EPR.PRN.Backend.Data.Interfaces.Accreditations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EPR.PRN.Backend.Data.Repositories.Accreditations;

public class AccreditationRepository(EprAccreditationContext eprContext, IMapper mapper, ILogger<AccreditationRepository> logger) : IAccreditationRepository
{
    public async Task<AccreditationEntity?> GetById(Guid accreditationId)
    {
        logger.LogInformation("Retrieving accreditation details for ExternalId: {AccreditationId}.", accreditationId);
        var accreditation =  await eprContext.Accreditations
            .AsNoTracking()
            .Include(x => x.ApplicationType)
            .Include(x => x.AccreditationStatus)
            .Include(x => x.RegistrationMaterial)
                .ThenInclude(x => x.Material)
            .SingleOrDefaultAsync(x => x.ExternalId.Equals(accreditationId));
        logger.LogInformation("Retrieved accreditation details for ExternalId: {AccreditationId} with Id {Id}.", accreditationId, accreditation?.Id);
        return accreditation;
    }

    public async Task<AccreditationEntity?> GetAccreditationDetails(
        Guid organisationId,
        int materialId,
        int applicationTypeId)
    {
        logger.LogInformation("Retrieving accreditation details for OrganisationId: {OrganisationId}, MaterialId: {MaterialId}, ApplicationTypeId: {ApplicationTypeId}.", organisationId, materialId, applicationTypeId);
        var accreditation =  await eprContext.Accreditations
            .AsNoTracking()
            .Where(x =>
                x.OrganisationId == organisationId &&
                x.RegistrationMaterialId == materialId &&
                x.ApplicationTypeId == applicationTypeId)
            .SingleOrDefaultAsync();
        logger.LogInformation("Retrieved accreditation details for OrganisationId: {OrganisationId}, MaterialId: {MaterialId}, ApplicationTypeId: {ApplicationTypeId} with Id {Id}.", organisationId, materialId, applicationTypeId, accreditation?.Id);
        return accreditation;
    }

    public async Task Create(AccreditationEntity accreditation)
    {
        var currentTimestamp = DateTime.UtcNow;
        accreditation.CreatedDate = currentTimestamp;
        accreditation.UpdatedDate = currentTimestamp;
        accreditation.ExternalId = Guid.NewGuid();

        eprContext.Accreditations.Add(accreditation);
        await eprContext.SaveChangesAsync();
        logger.LogInformation("Created new accreditation with ExternalId: {ExternalId} and Id: {Id}", accreditation.ExternalId, accreditation.Id);
    }

    public async Task Update(AccreditationEntity accreditation)
    {
        logger.LogInformation("Updating accreditation with ExternalId: {ExternalId} and Id {Id}.", accreditation.ExternalId, accreditation.Id);
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
        existingAccreditation.BusinessPlanConfirmed = accreditation.BusinessPlanConfirmed;
        existingAccreditation.UpdatedBy = accreditation.UpdatedBy;
        existingAccreditation.UpdatedDate = DateTime.UtcNow;

        eprContext.Entry(existingAccreditation).State = EntityState.Modified;
        await eprContext.SaveChangesAsync();
        logger.LogInformation("Updated accreditation with ExternalId: {ExternalId} and Id {Id}.", accreditation.ExternalId, accreditation.Id);  
    }

    [ExcludeFromCodeCoverage]
    public async Task ClearDownDatabase()
    {
        // Temporary: Aid to QA whilst Accreditation uses in-memory database.
        logger.LogInformation("Clearing down EPR Accreditation database for testing purposes.");
        await eprContext.Database.EnsureDeletedAsync();
        await eprContext.Database.EnsureCreatedAsync();
    }

    public async Task<IEnumerable<AccreditationOverviewDto>> GetAccreditationOverviewForOrgId(Guid organisationId)
    {
        var data= await eprContext.Accreditations
            .Include(x => x.ApplicationType)
            .Include(x => x.RegistrationMaterial)
            .Include(x => x.AccreditationStatus)
            .Where(a => a.OrganisationId == organisationId)
            
            .ToListAsync();

        return mapper.Map<List<AccreditationOverviewDto>>(data);
    }
}