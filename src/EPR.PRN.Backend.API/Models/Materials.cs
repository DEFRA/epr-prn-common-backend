#nullable disable
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.Models;

[ExcludeFromCodeCoverage]
public class Materials
{
    public string MaterialId { get; set; }
    public string MaterialName { get; set; }
    public string Status { get; set; }
    public DateTime DeterminationDate { get; set; }
    public string ReferenceNumber { get; set; }
    public List<Tasks> Tasks { get; set; }
}