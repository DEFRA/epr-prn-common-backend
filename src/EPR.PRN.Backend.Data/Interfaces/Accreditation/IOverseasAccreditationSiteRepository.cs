using EPR.PRN.Backend.Data.DataModels.Registrations;

namespace EPR.PRN.Backend.Data.Interfaces.Accreditation;

public interface IOverseasAccreditationSiteRepository
{
    Task<List<OverseasAccreditationSite>?> GetAllByAccreditationId(Guid accreditationId);

    Task PostByAccreditationId(Guid accreditationId, OverseasAccreditationSite overseasAccreditationSite);
}
