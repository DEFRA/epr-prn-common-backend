using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.Dto.Regulator;

[ExcludeFromCodeCoverage]
public class MaterialsAuthorisedOnSiteInfoDto
{
    public string MaterialName { get; set; } = string.Empty;
    public string? Reason { get; set; }
    public bool IsMaterialRegistered { get; set; }
}