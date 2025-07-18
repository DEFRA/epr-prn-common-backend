using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.DataModels.Accreditations;
using EPR.PRN.Backend.Data.Interfaces.Accreditations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EPR.PRN.Backend.Data.Repositories.Accreditations;

public class AccreditationFileUploadRepository(EprAccreditationContext eprContext, ILogger<AccreditationFileUploadRepository> logger) : IAccreditationFileUploadRepository
{
    public async Task<AccreditationFileUpload> GetByExternalId(Guid accreditationFileUploadId)
    {
        var entity = await eprContext.AccreditationFileUploads
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.ExternalId.Equals(accreditationFileUploadId));

        if (entity == null)
            throw new KeyNotFoundException("Accreditation file upload not found");

        return entity;
    }

    public async Task<List<AccreditationFileUpload>> GetByAccreditationId(Guid accreditationId, int fileUploadTypeId, int fileUploadStatusId)
    {
        int accreditationIdInt = await GetAccreditationIntegerId(accreditationId);

        return await eprContext.AccreditationFileUploads
            .AsNoTracking()
            .Where(x => x.AccreditationId == accreditationIdInt &&
                        x.FileUploadTypeId == fileUploadTypeId &&
                        x.FileUploadStatusId == fileUploadStatusId)
            .ToListAsync();
    }

    public async Task<Guid> Create(Guid accreditationId, AccreditationFileUpload fileUpload)
    {
        var newEntity = new AccreditationFileUpload
        {
            ExternalId = Guid.NewGuid(),
            AccreditationId = await GetAccreditationIntegerId(accreditationId),
            SubmissionId = fileUpload.SubmissionId,
            OverseasSiteId = fileUpload.OverseasSiteId,
            FileName = fileUpload.FileName,
            FileId = fileUpload.FileId,
            UploadedOn = fileUpload.UploadedOn,
            UploadedBy = fileUpload.UploadedBy,
            FileUploadTypeId = fileUpload.FileUploadTypeId,
            FileUploadStatusId = fileUpload.FileUploadStatusId
        };

        eprContext.AccreditationFileUploads.Add(newEntity);
        await eprContext.SaveChangesAsync();

        return newEntity.ExternalId;
    }

    public async Task Update(Guid accreditationId, AccreditationFileUpload fileUpload)
    {
        var existingEntity = await eprContext.AccreditationFileUploads
            .SingleOrDefaultAsync(x => x.ExternalId.Equals(fileUpload.ExternalId));

        if (existingEntity == null)
            throw new KeyNotFoundException("Accreditation file upload not found");

        existingEntity.AccreditationId = await GetAccreditationIntegerId(accreditationId);
        existingEntity.OverseasSiteId = fileUpload.OverseasSiteId;
        existingEntity.FileName = fileUpload.FileName;
        existingEntity.FileId = fileUpload.FileId;
        existingEntity.UploadedOn = fileUpload.UploadedOn;
        existingEntity.UploadedBy = fileUpload.UploadedBy;
        existingEntity.FileUploadTypeId = fileUpload.FileUploadTypeId;
        existingEntity.FileUploadStatusId = fileUpload.FileUploadStatusId;

        eprContext.Entry(existingEntity).State = EntityState.Modified;
        await eprContext.SaveChangesAsync();
    }

    public async Task Delete(Guid accreditationId, Guid fileId)
    {
        int accreditationIdInt = await GetAccreditationIntegerId(accreditationId);

        var existingEntity = await eprContext.AccreditationFileUploads
            .FirstOrDefaultAsync(x => x.AccreditationId == accreditationIdInt && x.FileId.Equals(fileId));

        if (existingEntity == null)
            throw new KeyNotFoundException("Accreditation file upload not found");

        existingEntity.FileUploadStatusId = (int)AccreditationFileUploadStatus.FileDeleted;
        eprContext.Entry(existingEntity).State = EntityState.Modified;
        await eprContext.SaveChangesAsync();
    }

    private async Task<int> GetAccreditationIntegerId(Guid accreditationId)
    {
        logger.LogInformation("Retrieving accreditation ID for external ID: {AccreditationId}.", accreditationId);
        var accreditationIdInt = await eprContext.Accreditations.Where(x => x.ExternalId == accreditationId).Select(x => x.Id).SingleOrDefaultAsync();

        if (accreditationIdInt == 0)
            throw new KeyNotFoundException("Accreditation not found");

        return accreditationIdInt;
    }
}