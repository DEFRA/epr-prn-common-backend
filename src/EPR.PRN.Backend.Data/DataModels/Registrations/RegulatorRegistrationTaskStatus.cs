using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.Data.DataModels.Registrations;

[Table("Public.RegulatorRegistrationTaskStatus")]
[ExcludeFromCodeCoverage]
public class RegulatorRegistrationTaskStatus : RegulatorTaskStatusBase
{
    public Registration Registration { get; set; } = null!;
    [ForeignKey("Registration")]
    public int? RegistrationId { get; set; }
    public List<RegistrationTaskStatusQueryNote> RegistrationTaskStatusQueryNotes { get; set; } = new();
}