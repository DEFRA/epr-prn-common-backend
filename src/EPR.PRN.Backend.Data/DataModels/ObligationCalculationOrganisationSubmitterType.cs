using System.ComponentModel.DataAnnotations;

namespace EPR.PRN.Backend.Data.DataModels;

public class ObligationCalculationOrganisationSubmitterType
{
	[Key]
	public int Id { get; set; }

	[Required]
	public string TypeName { get; set; } = null!;
}
