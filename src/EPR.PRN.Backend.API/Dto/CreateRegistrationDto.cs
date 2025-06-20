namespace EPR.PRN.Backend.API.Dto;

/// <summary>
/// Represents the details of a created registration.
/// </summary>
public record CreateRegistrationDto
{
    /// <summary>
    /// The ID of the created registration.
    /// </summary>
    public Guid Id { get; set; }
}