using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.Data.DataModels.Registrations;
[ExcludeFromCodeCoverage]
[Table("Public.OverseasAccreditationSite")]
public class OverseasAccreditationSite
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int Id { get; set; }
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid ExternalId { get; set; }
    [ForeignKey("AccreditationId")]
    public int AccreditationId { get; set; }
    public Accreditation? Accreditation { get; set; }


    public int OverseasAddressId { get; set; }
    public OverseasAddress? overseasAddress { get; set; }

    public required string OrganisationName { get; set; }
    [ForeignKey("MeetConditionsOfExportId")]
    public int MeetConditionsOfExportId { get; set; }
    public LookupMeetConditionsOfExport? MeetConditionsOfExport { get; set; }
    public int StartDay { get; set; }

    public int StartMonth { get; set; }

    public int StartYear { get; set; }

    public int ExpiryDay { get; set; }

    public int ExpiryMonth { get; set; }

    public int ExpiryYear { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    [ForeignKey("SiteCheckStatusId")]
    public int SiteCheckStatusId { get; set; }
    public LookupSiteCheckStatus? SiteCheckStatus { get; set; }

    
}
