using EPR.PRN.Backend.API.Dto.Accreditation;

namespace EPR.PRN.Backend.API.Services.Interfaces;

public interface IOverseasAccreditationSiteService
{
    Task<List<OverseasAccreditationSiteDto>?> GetAllByAccreditationId(Guid accreditationId);

    Task PostByAccreditationId(Guid accreditationId, OverseasAccreditationSiteDto request);
}
