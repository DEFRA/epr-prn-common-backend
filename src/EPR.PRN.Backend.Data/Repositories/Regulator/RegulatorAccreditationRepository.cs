using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Common.Exceptions;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace EPR.PRN.Backend.Data.Repositories.Regulator;

public class RegulatorAccreditationRepository(EprContext eprContext) : IRegulatorAccreditationRepository
{
    public async Task<Accreditation> GetAccreditationById(Guid accreditationId)
    {
        return await eprContext.Accreditations
           .FirstOrDefaultAsync(a => a.ExternalId == accreditationId)
           ?? throw new KeyNotFoundException("Accreditation not found.");
    }

    public async Task<Accreditation> GetAccreditationPaymentFeesById(Guid accreditationId)
    {
        return await eprContext.Accreditations
            .Include(a => a.RegistrationMaterial)
                .ThenInclude(rm => rm.Material)
            .Include(a => a.RegistrationMaterial)
                .ThenInclude(rm => rm.Registration)
                    .ThenInclude(r => r.ReprocessingSiteAddress)
            .Include(a => a.AccreditationStatus)
            .FirstOrDefaultAsync(a => a.ExternalId == accreditationId)
            ?? throw new KeyNotFoundException("Accreditation not found.");

    }
    public async Task AccreditationMarkAsDulyMade(Guid accreditationId, int statusId, DateTime DulyMadeDate, DateTime DeterminationDate, Guid DulyMadeBy)
    {
        var accreditation = await eprContext.Accreditations.FirstOrDefaultAsync(rm => rm.ExternalId == accreditationId);
        
        if (accreditation is null) throw new KeyNotFoundException("Accreditation not found.");
            var material = await eprContext.RegistrationMaterials.FirstOrDefaultAsync(rm => rm.Id == accreditation.RegistrationMaterialId);
        
        if (material is null) throw new KeyNotFoundException("Material not found.");
            var dulyMade = await eprContext.AccreditationDulyMade
                .FirstOrDefaultAsync(adm => adm.AccreditationId == accreditation.Id)
                ?? new AccreditationDulyMade
                {
                    AccreditationId = accreditation.Id,
                    ExternalId = Guid.NewGuid()
                };

        var registration = await eprContext.Registrations
            .FirstOrDefaultAsync(x => x.Id == material.RegistrationId);

        if (registration == null)
            throw new KeyNotFoundException("Registration not found.");

        var applicationTypeId = registration.ApplicationTypeId;


        var taskid = await eprContext.LookupTasks
            .Where(t => t.Name == "DulyMade" && t.ApplicationTypeId == applicationTypeId)
            .Select(t => t.Id)
            .FirstOrDefaultAsync();
        var regulatorAccreditationTaskStatus = new RegulatorAccreditationTaskStatus
        {
            AccreditationId = accreditation.Id,
            TaskStatusId = statusId,
            ExternalId = Guid.NewGuid(),
            RegulatorTaskId = taskid,
            StatusCreatedDate = DateTime.UtcNow,
            StatusUpdatedBy = DulyMadeBy
        };

        // Set/update the fields
        dulyMade.TaskStatusId = statusId;
        dulyMade.DulyMadeDate = DulyMadeDate;
        dulyMade.DulyMadeBy = DulyMadeBy;

        var determination = new AccreditationDeterminationDate
        {
            AccreditationId = accreditation.Id,
            ExternalId = Guid.NewGuid(),
            DeterminationDate = DeterminationDate
        };

        // If this is a new entity, add it to the context
        if (dulyMade.Id == 0)
        {
            await eprContext.AccreditationDulyMade.AddAsync(dulyMade);
            await eprContext.RegulatorAccreditationTaskStatus.AddAsync(regulatorAccreditationTaskStatus);
        }

        await eprContext.AccreditationDeterminationDate.AddAsync(determination);

        await eprContext.SaveChangesAsync();
    }

}