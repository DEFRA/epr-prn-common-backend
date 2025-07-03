namespace EPR.PRN.Backend.Data.DTO;

public class UpdateOverseasAddressDto
{
    public List<OverseasAddressDto> OverseasAddresses { get; set; } = [];
    public Guid RegistrationMaterialId { get; set; }
}
