using EPR.PRN.Backend.API.Dto;
using EPR.PRN.Backend.API.Services.Interfaces;
using EPR.PRN.Backend.Data.Interfaces;

namespace EPR.PRN.Backend.API.Services;

/// <summary>
/// Implementation for <see cref="IMaterialService"/>.
/// </summary>
/// <param name="logger">The logger instance to log to.</param>
/// <param name="materialRepository">Provides access to an Api to manage materials.</param>
public class MaterialService(ILogger<MaterialService> logger, IMaterialRepository materialRepository)
    : IMaterialService
{
    private readonly ILogger<MaterialService> _logger = logger;
    private readonly IMaterialRepository _materialRepository = materialRepository;

    /// <inheritdoc />>.
    public async Task<IList<MaterialDto>> GetAllMaterialsAsync(CancellationToken cancellationToken)
    {
        var materials = await _materialRepository.GetAllMaterials();
        materials = materials.ToList();

        if (!materials.Any())
        {
            _logger.LogInformation("Found no materials");

            return Enumerable.Empty<MaterialDto>().ToList();
        }

        _logger.LogInformation("Found {Count} materials.", materials.Count());
            
        return materials.Select(m => new MaterialDto
        {
            Name = m.MaterialName,
            Code = m.MaterialCode
        }).ToList();
    }
}