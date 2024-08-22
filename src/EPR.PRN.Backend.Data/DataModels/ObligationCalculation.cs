#nullable disable

using System.ComponentModel.DataAnnotations;

namespace EPR.PRN.Backend.Data.DataModels;

public class ObligationCalculation
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int OrganisationId { get; set; }

    [MaxLength(20)]
    [Required]
    public string MaterialName { get; set; }

    [Required]
    public int MaterialObligationValue { get; set; }

    [Required]
    public int Year { get; set; }

    [Required]
    public DateTime CalculatedOn { get; set; }
}