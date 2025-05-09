using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPR.PRN.Backend.Data.DataModels.Accreditations;

public class AccreditationPrnIssueAuth
{
    public int Id { get; set; }
    public Guid ExternalId { get; set; }
    public int AccreditationId { get; set; }
    public int PersonId { get; set; }

    public Accreditation Accreditation { get; set; }
}