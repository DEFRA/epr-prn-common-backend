using System.ComponentModel.DataAnnotations;

namespace EPR.PRN.Backend.Data.DataModels.Accreditations;

public class ApplicationType
{
    public int Id { get; set; }

    [MaxLength(200)]
    public string? Name { get; set; }
}