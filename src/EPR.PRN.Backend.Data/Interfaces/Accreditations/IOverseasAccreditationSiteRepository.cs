using EPR.PRN.Backend.Data.DataModels.Accreditations;

namespace EPR.PRN.Backend.Data.Interfaces.Accreditations;

public interface IOverseasAccreditationSiteRepository
{
    Task<List<OverseasAccreditationSite>?> GetAllByAccreditationId(Guid accreditationId);

    Task PostByAccreditationId(Guid accreditationId, OverseasAccreditationSite overseasAccreditationSite);
}
