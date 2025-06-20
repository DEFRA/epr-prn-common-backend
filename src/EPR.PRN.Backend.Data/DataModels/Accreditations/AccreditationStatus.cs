using System.ComponentModel.DataAnnotations;

namespace EPR.PRN.Backend.Data.DataModels.Accreditations;

public class AccreditationStatus
{
    public int Id { get; set; }

    [MaxLength(100)]
    public string? Name { get; set; }
}