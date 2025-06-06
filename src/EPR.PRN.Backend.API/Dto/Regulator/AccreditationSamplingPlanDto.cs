using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.Dto.Regulator;
[ExcludeFromCodeCoverage]
public class AccreditationSamplingPlanDto
{
    public required string MaterialName { get; set; }
    public List<AccreditationSamplingPlanFileDto> Files { get; set; } = [];
}
