namespace EPR.PRN.Backend.Data.DataModels.Accreditations;

public class OverseasAccreditationSite
{
    public int Id { get; set; }

    public Guid ExternalId { get; set; }

    public int AccreditationId { get; set; }

    public int OverseasAddressId { get; set; }

    public required string OrganisationName { get; set; }

    public int MeetConditionsOfExportId { get; set; }

    public int StartDay { get; set; }

    public int StartMonth { get; set; }

    public int StartYear { get; set; }

    public int ExpiryDay { get; set; }

    public int ExpiryMonth { get; set; }

    public int ExpiryYear { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public int SiteCheckStatusId { get; set; }

    public AccreditationEntity? Accreditation { get; set; }
}
