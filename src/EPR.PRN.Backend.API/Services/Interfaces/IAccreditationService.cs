using EPR.PRN.Backend.API.Dto.Accreditation;

namespace EPR.PRN.Backend.API.Services.Interfaces;

public interface IAccreditationService
{
    Task<Guid> GetOrCreateAccreditation(
        Guid organisationId,
        int materialId,
        int applicationTypeId);

    Task<AccreditationDto> GetAccreditationById(Guid accreditationId);

    Task<Guid> CreateAccreditation(AccreditationRequestDto accreditationDto);

    Task UpdateAccreditation(AccreditationRequestDto accreditationDto);

    Task ClearDownDatabase();
}
