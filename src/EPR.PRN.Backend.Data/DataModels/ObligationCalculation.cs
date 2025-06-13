#nullable disable

using System.ComponentModel.DataAnnotations;

namespace EPR.PRN.Backend.Data.DataModels;

public class ObligationCalculation
{
    [Key]
    public int Id { get; set; }

    [Required]
    public Guid OrganisationId { get; set; }

    [Required]
	public int MaterialId { get; set; }

    [Required]
    public int MaterialObligationValue { get; set; }

    [Required]
    public int Year { get; set; }

    [Required]
    public DateTime CalculatedOn { get; set; }

    [Required]
    public int Tonnage { get; set; }

	[Required]
	public Guid SubmitterId { get; set; }

	[Required]
	public int SubmitterTypeId { get; set; }

	[Required]
	public bool IsDeleted { get; set; }

	public Material Material { get; set; } = null!;

	public ObligationCalculationOrganisationSubmitterType ObligationCalculationOrganisationSubmitterType { get; set; } = null!;
}