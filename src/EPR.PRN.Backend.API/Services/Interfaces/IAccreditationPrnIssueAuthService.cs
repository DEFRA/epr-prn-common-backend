using EPR.PRN.Backend.API.Dto.Accreditation;

namespace EPR.PRN.Backend.API.Services.Interfaces;

public interface IAccreditationPrnIssueAuthService
{
    Task<List<AccreditationPrnIssueAuthDto>> GetByAccreditationId(Guid accreditationId);
    Task ReplaceAllByAccreditationId(Guid accreditationId, List<AccreditationPrnIssueAuthRequestDto> request);
}
