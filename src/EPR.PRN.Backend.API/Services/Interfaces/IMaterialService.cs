using EPR.PRN.Backend.API.Dto;

namespace EPR.PRN.Backend.API.Services.Interfaces;

/// <summary>
/// Defines an interface for a service that retrieves material data.
/// </summary>
public interface IMaterialService
{
    /// <summary>
    /// Retrieves all the applicable materials that can be applied for.
    /// </summary>
    /// <param name="cancellationToken">Propagates a notification that the operation should be cancelled.</param>
    /// <returns>Collection of materials, if any.</returns>
    Task<IList<MaterialDto>> GetAllMaterialsAsync(CancellationToken cancellationToken);
}