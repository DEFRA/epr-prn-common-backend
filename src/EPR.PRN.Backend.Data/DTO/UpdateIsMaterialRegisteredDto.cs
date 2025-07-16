using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.Data.DTO;

[ExcludeFromCodeCoverage]
public class UpdateIsMaterialRegisteredDto
{
	public Guid RegistrationMaterialId { get; set; }
	public bool? IsMaterialRegistered { get; set; }
}