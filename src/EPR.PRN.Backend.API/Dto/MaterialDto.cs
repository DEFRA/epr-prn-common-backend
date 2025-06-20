using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.Dto;

/// <summary>
/// Represents a model for a material.
/// </summary>
[ExcludeFromCodeCoverage]
public record MaterialDto
{
    /// <summary>
    /// The name of the material.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// The code for the material.
    /// </summary>
    public string Code { get; set; } = null!;
}