using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.Dto.Regulator;

[ExcludeFromCodeCoverage]

public class RegistrationAccreditationReferenceDto
{
    public string RegistrationType { get; } = "R";
    public string OrgCode { get; set; }
    public string MaterialCode { get; set; }
    public string ApplicationType { get; set; }
    public string RandomDigits { get; set; } = Random.Shared.Next(1000, 9999).ToString();
    public string RelevantYear { get; } = (DateTime.UtcNow.Year % 100).ToString("D2");
    public int NationId { get; set; } 
}