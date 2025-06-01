using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.Data.DataModels.Registrations;

[Table("Public.RegulatorAccreditationTaskStatus")]
[ExcludeFromCodeCoverage]
public class RegulatorAccreditationTaskStatus : RegulatorTaskStatusBase
{
    public Accreditation Accreditation { get; set; }
    [ForeignKey("Accreditation")]
    public int? AccreditationId { get; set; }
    [MaxLength(500)]
    public string? Comments { get; set; }
}