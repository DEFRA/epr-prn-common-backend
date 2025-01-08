using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.Configs;

[ExcludeFromCodeCoverage]
public class PrnApiOptions
{
    public const string SectionName = "PrnApiConfig";

    public string? BaseUrl { get; set; }
    public int? Timeout { get; set; }
    public string? ClientId { get; set; }
}
