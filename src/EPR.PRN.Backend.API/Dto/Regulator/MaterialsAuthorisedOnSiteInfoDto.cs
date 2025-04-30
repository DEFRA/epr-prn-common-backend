using EPR.PRN.Backend.API.Dto.Regulator;

public class MaterialsAuthorisedOnSiteInfoDto
{
    public string MaterialName { get; set; } = string.Empty;
    public string? Reason { get; set; }
    public bool IsMaterialRegistered { get; set; }
}