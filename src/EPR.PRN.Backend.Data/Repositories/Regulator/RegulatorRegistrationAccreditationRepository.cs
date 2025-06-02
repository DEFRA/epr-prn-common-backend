using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Common.Exceptions;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace EPR.PRN.Backend.Data.Repositories.Regulator;

public class RegulatorRegistrationAccreditationRepository(EprContext eprContext) : IRegulatorRegistrationAccreditationRepository
{
    public async Task AccreditationMarkAsDulyMade(Guid accreditationId, int statusId, DateTime DulyMadeDate, DateTime DeterminationDate, Guid DulyMadeBy)
    {
     //   var accreditation = await eprContext.RegistrationMaterials.FirstOrDefaultAsync(rm => rm.ExternalId == registrationMaterialId);
     //   if (material is null) throw new KeyNotFoundException("Material not found.");
     //   var dulyMade = await eprContext.DulyMade
     //       .FirstOrDefaultAsync(rm => rm.RegistrationMaterial!.ExternalId == registrationMaterialId)
     //       ?? new DulyMade
     //       {
     //           RegistrationMaterialId = material.Id,
     //           RegistrationMaterial = material // Initialize the required member
     //       };

     //   var registration = await eprContext.Registrations
     //.FirstOrDefaultAsync(x => x.Id == material.RegistrationId);

     //   if (registration == null)
     //       throw new KeyNotFoundException("Registration not found.");

     //   var applicationTypeId = registration.ApplicationTypeId;


     //   var taskid = await eprContext.LookupTasks
     //       .Where(t => t.Name == "CheckAccreditationStatus" && t.ApplicationTypeId == applicationTypeId)
     //       .Select(t => t.Id)
     //       .FirstOrDefaultAsync();
     //   var regulatorApplicationTaskStatus = new RegulatorAccreditationTaskStatus
     //   {
     //       TaskStatusId = statusId,
     //       ExternalId = material.ExternalId,
     //       RegulatorTaskId = taskid,
     //       StatusCreatedDate = DateTime.UtcNow,
     //       StatusUpdatedBy = DulyMadeBy

     //   };

     //   // Set/update the fields
     //   dulyMade.TaskStatusId = statusId;
     //   dulyMade.DeterminationDate = DeterminationDate;
     //   dulyMade.DulyMadeDate = DulyMadeDate;
     //   dulyMade.DulyMadeBy = DulyMadeBy;
     //   dulyMade.ExternalId = material.ExternalId;

     //   // If this is a new entity, add it to the context
     //   if (dulyMade.Id == 0)
     //   {
     //       await eprContext.DulyMade.AddAsync(dulyMade);
     //       await eprContext.RegulatorApplicationTaskStatus.AddAsync(regulatorApplicationTaskStatus);
     //   }

        await eprContext.SaveChangesAsync();
    }

}