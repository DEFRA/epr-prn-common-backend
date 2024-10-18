namespace EPR.PRN.Backend.API.Services.Interfaces;

using EPR.PRN.Backend.API.Common.DTO;
using EPR.PRN.Backend.Data.DataModels;

public interface IPrnService
{
    Task<PrnDto?> GetPrnForOrganisationById(Guid orgId, Guid prnId);
    
    Task<List<PrnDto>> GetAllPrnByOrganisationId(Guid orgId);
    
    Task UpdateStatus(Guid orgId, Guid userId, List<PrnUpdateStatusDto> prnUpdates);
    
    Task<PaginatedResponseDto<PrnDto>> GetSearchPrnsForOrganisation(Guid orgId, PaginatedRequestDto request);

    Task<DateTime?> GetObligationCalculatorLastSuccessRun();

    Task AddObligationCalculatorLastSuccessRun(ObligationCalculatorLastSuccessRun lastSuccessfulRun);
}
