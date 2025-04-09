using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.Common.Dto;

[ExcludeFromCodeCoverage]
public class RegistrationReferenceBackendDto
{   
    public string RegistrationType { get; set; } = "R";
    public string CountryCode { get; set; } = String.Empty;
    public string OrganisationType { get; set; } = String.Empty;
    public string MaterialCode { get; set; } = String.Empty;
}