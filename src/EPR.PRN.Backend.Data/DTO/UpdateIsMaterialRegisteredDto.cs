namespace EPR.PRN.Backend.Data.DTO;

public class UpdateIsMaterialRegisteredDto
{
	public Guid RegistrationMaterialId { get; set; }
	public bool? IsMaterialRegistered { get; set; }
}