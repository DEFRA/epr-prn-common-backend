using EPR.PRN.Backend.API.Dto.Accreditation;

namespace EPR.PRN.Backend.API.Services.Interfaces;

public interface IAccreditationService
{
    Task<AccreditationDto> GetAccreditationById(Guid accreditationId);
    Task<Guid> CreateAccreditation(AccreditationRequestDto accreditationDto, Guid orgId, Guid userId);
    Task UpdateAccreditation(AccreditationRequestDto accreditationDto, Guid userId);
}
