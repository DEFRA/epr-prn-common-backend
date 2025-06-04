using EPR.PRN.Backend.Obligation.Models;

namespace EPR.PRN.Backend.API.Services.Interfaces
{
    /// <summary>
    /// Interface for Material Exemption Reference Service.
    /// </summary>
    public interface IMaterialExemptionReferenceService
    {
        /// <summary>
        /// Creates a material exemption reference asynchronously.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> CreateMaterialExemptionReferenceAsync(List<MaterialExemptionReferenceRequest> materialExemptionReferences, CancellationToken cancellationToken);
    }
}
