using EPR.PRN.Backend.Data.DTO;

namespace EPR.PRN.Backend.Data.Interfaces
{
    public interface IPrnRepository
    {
        Task<List<EprnResultsDto>> GetSumOfTonnageForMaterials(Guid organisationId, string status);
    }
}
