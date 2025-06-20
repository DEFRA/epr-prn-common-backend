using EPR.PRN.Backend.Data.DataModels.Accreditations;

namespace EPR.PRN.Backend.Data.Interfaces.Accreditations;

public interface IAccreditationFileUploadRepository
{
    Task<AccreditationFileUpload> GetByExternalId(Guid accreditationFileUploadId);
    Task<List<AccreditationFileUpload>> GetByAccreditationId(Guid accreditationId, int fileUploadTypeId, int fileUploadStatusId);
    Task<Guid> Create(Guid accreditationId, AccreditationFileUpload fileUpload);
    Task Update(Guid accreditationId, AccreditationFileUpload fileUpload);
    Task Delete(Guid accreditationId, Guid fileId);
}
