using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.Dto.Accreditation;

[ExcludeFromCodeCoverage]
public class OverseasAccreditationSiteDto
{
    public Guid ExternalId { get; set; }

    public required string OrganisationName { get; set; }

    public int MeetConditionsOfExportId { get; set; }

    public int SiteCheckStatusId { get; set; }
}
