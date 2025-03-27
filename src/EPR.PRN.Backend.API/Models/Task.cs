#nullable disable
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.Models;

[ExcludeFromCodeCoverage]
public class Tasks
{
    public string TaskName { get; set; }
    public string Status { get; set; }
}