using EPR.PRN.Backend.API.Common.Enums;

namespace EPR.PRN.Backend.API.Common.Dto.Regulator;
public class RegistrationMaterialDetailsDto
{
    public int Id { get; set; }
    public int RegistrationId { get; set; }
    public string MaterialName { get; set; } = string.Empty;
    public RegistrationMaterialStatus? Status { get; set; }
}
