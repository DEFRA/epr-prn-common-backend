using EPR.PRN.Backend.Data.DTO;

namespace EPR.PRN.Backend.Data.Interfaces
{
    public interface IPrnRepository
    {
        Task<List<EprnResultsDto>> GetAcceptedAndAwaitingPrnsByYearAsync(Guid organisationId, int year);
        int GetPrnStatusCount(List<EprnResultsDto> prns, string status);
        List<EprnTonnageResultsDto> GetSumOfTonnageForMaterials(List<EprnResultsDto> prns, string status);
    }
}
