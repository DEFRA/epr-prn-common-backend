using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.Configs;

[ExcludeFromCodeCoverage]
public class PrnObligationCalculationConfig
{
    public const string SectionName = "PrnObligationCalculation";

    public int StartYear { get; set; }

    public int EndYear { get; set; }
}
