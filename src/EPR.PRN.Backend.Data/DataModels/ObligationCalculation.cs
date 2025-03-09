#nullable disable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPR.PRN.Backend.Data.DataModels;

public class ObligationCalculation
{
    [Key]
    public int Id { get; set; }

    [Required]
    public Guid OrganisationId { get; set; }

    [Required]
	[ForeignKey("Material")]
	public int MaterialId { get; set; }

    [Required]
    public int MaterialObligationValue { get; set; }

    [Required]
    public int Year { get; set; }

    [Required]
    public DateTime CalculatedOn { get; set; }

    [Required]
    public int Tonnage { get; set; }
}