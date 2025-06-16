using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.Dto;

[ExcludeFromCodeCoverage]
public class IdNamePairDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
}
