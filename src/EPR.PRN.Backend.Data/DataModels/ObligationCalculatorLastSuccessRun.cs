using System.ComponentModel.DataAnnotations;

namespace EPR.PRN.Backend.Data.DataModels;

public class ObligationCalculatorLastSuccessRun
{
    [Key]
    public int Id { get; set; }

    public DateTime? LastSuccessfulRunDate { get; set; }
}
