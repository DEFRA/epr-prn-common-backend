using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.DataModels.Registrations;

namespace EPR.PRN.Backend.Data.Interfaces.Regulator;

public interface IRegulatorRegistrationAccreditationRepository
{
    Task AccreditationMarkAsDulyMade(DateTime DulyMadeDate, DateTime DeterminationDate, Guid DulyMadeBy);

    Task UpdateStatusAsync(string TaskName, Guid RegistrationMaterialId, RegulatorTaskStatus status, string? comments, Guid user);
    Task<RegulatorAccreditationTaskStatus?> GetTaskStatusAsync(string TaskName, Guid RegistrationMaterialId);
}
