using EPR.PRN.Backend.API.Dto.Accreditation;
using EPR.PRN.Backend.Data.DataModels.Accreditations;

namespace EPR.PRN.Backend.API.Services.Interfaces;

public interface IOverseasAccreditationSiteService
{
    Task<List<OverseasAccreditationSiteDto>?> GetAllByAccreditationId(Guid accreditationId);

    Task PostByAccreditationId(Guid accreditationId, OverseasAccreditationSiteDto request);
}
