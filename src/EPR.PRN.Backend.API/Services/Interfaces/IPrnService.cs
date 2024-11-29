using EPR.PRN.Backend.API.Common.Dto;
using EPR.PRN.Backend.API.Dto;

namespace EPR.PRN.Backend.API.Services.Interfaces
{
    public interface IPrnService
    {
        Task<PrnDto?> GetPrnForOrganisationById(Guid orgId, Guid prnId);
        Task<List<PrnDto>> GetAllPrnByOrganisationId(Guid orgId);
        Task UpdateStatus(Guid orgId, Guid userId, List<PrnUpdateStatusDto> prnUpdates);
        Task<PaginatedResponseDto<PrnDto>> GetSearchPrnsForOrganisation(Guid orgId, PaginatedRequestDto request);
    }
}
