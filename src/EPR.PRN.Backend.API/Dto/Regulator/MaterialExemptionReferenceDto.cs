using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.Dto.Regulator;

[ExcludeFromCodeCoverage]
public class MaterialExemptionReferenceDto
{
    public required string ReferenceNumber { get; set; }
}
