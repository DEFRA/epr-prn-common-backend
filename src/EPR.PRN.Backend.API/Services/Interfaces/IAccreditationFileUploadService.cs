using EPR.PRN.Backend.API.Dto.Accreditation;

namespace EPR.PRN.Backend.API.Services.Interfaces;

public interface IAccreditationFileUploadService
{
    Task<AccreditationFileUploadDto> GetByExternalId(Guid accreditationFileUploadId);
    Task<List<AccreditationFileUploadDto>> GetByAccreditationId(Guid accreditationId, int fileUploadTypeId, int fileUploadStatusId);
    Task<Guid> CreateFileUpload(Guid accreditationId, AccreditationFileUploadDto accreditationDto);
    Task UpdateFileUpload(Guid accreditationId, AccreditationFileUploadDto accreditationDto);
    Task DeleteFileUpload(Guid accreditationId, Guid fileId);
}
