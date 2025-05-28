using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.Data.DataModels.Registrations;

[Table("Public.RegulatorAccreditationRegistrationTaskStatus")]
[ExcludeFromCodeCoverage]
public class RegulatorAccreditationRegistrationTaskStatus : RegulatorTaskStatusBase
{
    public int RegistrationId { get; set; }
    public int AccreditationYear { get; set; }
}