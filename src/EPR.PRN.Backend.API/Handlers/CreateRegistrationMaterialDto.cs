namespace EPR.PRN.Backend.API.Handlers;

/// <summary>
/// Represents details of a created material.
/// </summary>
public record CreateRegistrationMaterialDto
{
    /// <summary>
    /// The unique identifier for the material.
    /// </summary>
    public Guid Id { get; set; }
}