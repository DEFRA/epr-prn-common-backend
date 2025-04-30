using EPR.PRN.Backend.API.Dto.Regulator;

public class MaterialsAuthorisedOnSiteDto
{
    public int RegistrationId { get; set; }
    public  string OrganisationName { get; init; }
    public  string SiteAddress { get; init; }
    public List<MaterialsAuthorisedOnSiteInfoDto> MaterialsAuthorisation { get; set; } = [];
}