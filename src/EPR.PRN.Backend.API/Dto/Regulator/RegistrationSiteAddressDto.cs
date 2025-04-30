using EPR.PRN.Backend.API.Dto.Regulator;
public class RegistrationSiteAddressDto
{
    public int RegistrationId { get; set; }
    public int NationId { get; set; }
    public string SiteAddress { get; set; } = string.Empty;
    public string GridReference { get; set; } = string.Empty;
    public string LegalCorrespondenceAddress { get; set; } = string.Empty;
}