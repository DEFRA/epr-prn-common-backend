namespace EPR.PRN.Backend.Data.DTO;

public class RegistrationReprocessingIORawMaterialOrProductsDto
{
    public int Id { get; set; }

    public Guid ExternalId { get; set; }

    public int RegistrationReprocessingIOId { get; set; }

    public string RawMaterialOrProductName { get; set; } = string.Empty;

    public decimal TonneValue { get; set; }

    public bool IsInput { get; set; }
}