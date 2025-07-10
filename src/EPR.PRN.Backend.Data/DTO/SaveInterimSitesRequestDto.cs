namespace EPR.PRN.Backend.Data.DTO;
public class SaveInterimSitesRequestDto
{
    public Guid RegistrationMaterialId { get; set; }
    public required List<OverseasMaterialReprocessingSiteDto> OverseasMaterialReprocessingSites { get; set; }
}
