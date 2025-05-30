using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Common.Exceptions;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace EPR.PRN.Backend.Data.Repositories.Regulator;

public class RegulatorRegistrationAccreditationRepository(EprContext eprContext) : IRegulatorRegistrationAccreditationRepository
{
    public async Task AccreditationMarkAsDulyMade(DateTime DulyMadeDate, DateTime DeterminationDate, Guid DulyMadeBy)
    {
        //ToDo: Update the status of the accreditation to duly made

        await eprContext.SaveChangesAsync();
    }

    public async Task<RegulatorAccreditationTaskStatus?> GetTaskStatusAsync(string TaskName, Guid RegistrationMaterialId)
    {
        return await GetTaskStatus(TaskName, RegistrationMaterialId);
    }

    public async Task UpdateStatusAsync(string TaskName, Guid RegistrationMaterialId, RegulatorTaskStatus status, string? comments, Guid user) => throw new NotImplementedException("This method is not implemented yet.");
    
    private async Task<RegulatorAccreditationTaskStatus?> GetTaskStatus(string TaskName, Guid RegistrationMaterialId) => throw new NotImplementedException("This method is not implemented yet.");

}