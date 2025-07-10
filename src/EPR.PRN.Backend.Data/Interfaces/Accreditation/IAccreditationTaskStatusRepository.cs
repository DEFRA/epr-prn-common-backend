using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.DataModels.Accreditations;

namespace EPR.PRN.Backend.Data.Interfaces.Accreditation
{
    public interface IAccreditationTaskStatusRepository
    {
        Task<AccreditationTaskStatus?> GetTaskStatusAsync(string taskName, Guid accreditationId);
        Task UpdateStatusAsync(string taskName, Guid accreditationId, TaskStatuses status); //, string? comments);
    }
}