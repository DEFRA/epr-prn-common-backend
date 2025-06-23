using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.Dto;

/// <summary>
/// Defines a lookup dto for the exemption references of a material.
/// </summary>
[ExcludeFromCodeCoverage]
public record ExemptionReferencesLookupDto
{
    /// <summary>
    /// The exemption reference number associated with the material.
    /// </summary>
    public string ReferenceNumber { get; set; } = null!;
}