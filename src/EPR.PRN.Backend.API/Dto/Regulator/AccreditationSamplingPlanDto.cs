using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.Dto.Regulator
{
    [ExcludeFromCodeCoverage]
    public class AccreditationSamplingPlanDto
    {        
        public Guid AccreditationId { get; set; }
        public List<AccreditationSamplingPlanFileDto> Files { get; set; } = [];
    }
}
