using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EPR.PRN.Backend.Data.DataModels.Registrations;

namespace EPR.PRN.Backend.Data.DataModels.Accreditations;

public class AccreditationPrnIssueAuth
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int Id { get; set; }
    public Guid ExternalId { get; set; }
    public Guid AccreditationExternalId { get; set; }
    public int AccreditationId { get; set; }
    public Guid PersonExternalId { get; set; }
    public Accreditation? Accreditation { get; set; }
}