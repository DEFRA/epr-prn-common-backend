using System.ComponentModel.DataAnnotations;

namespace EPR.PRN.Backend.Data.DataModels.Accreditations;

public class SiteCheckStatus
{
    public int Id { get; set; }

    [MaxLength(100)]
    public string? Status { get; set; }
}