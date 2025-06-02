using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.Data.DataModels.Registrations;

[Table("Public.RegulatorRegistrationTaskStatus")]
[ExcludeFromCodeCoverage]
public class RegulatorRegistrationTaskStatus: RegulatorTaskStatusBase
{
    public Registration Registration { get; set; }
    [ForeignKey("Registration")]
    public int? RegistrationId { get; set; }
    [MaxLength(500)]
    public string? Comments { get; set; }
}