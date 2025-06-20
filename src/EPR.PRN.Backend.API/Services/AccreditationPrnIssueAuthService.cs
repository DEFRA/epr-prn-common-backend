namespace EPR.PRN.Backend.API.Services;

using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using EPR.PRN.Backend.API.Dto.Accreditation;
using EPR.PRN.Backend.API.Services.Interfaces;
using EPR.PRN.Backend.Data.DataModels.Accreditations;
using EPR.PRN.Backend.Data.Interfaces.Accreditations;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

public class AccreditationPrnIssueAuthService(
    IAccreditationPrnIssueAuthRepository repository,
    IMapper mapper,
    ILogger<AccreditationPrnIssueAuthService> logger,
    IConfiguration configuration) : IAccreditationPrnIssueAuthService
{
    private readonly string logPrefix = string.IsNullOrEmpty(configuration["LogPrefix"]) ? "[EPR.PRN.Backend]" : configuration["LogPrefix"]!;

    public async Task<List<AccreditationPrnIssueAuthDto>> GetByAccreditationId(Guid accreditationId)
    {
        logger.LogInformation("{Logprefix}: AccreditationPrnIssueAuthService - GetByAccreditationId: request for accreditation {AccreditationId}", logPrefix, accreditationId);

        List<AccreditationPrnIssueAuth>? entities = await repository.GetByAccreditationId(accreditationId);
        var dtos = mapper.Map<List<AccreditationPrnIssueAuthDto>>(entities);

        return dtos;
    }

    public async Task ReplaceAllByAccreditationId(Guid accreditationId, List<AccreditationPrnIssueAuthRequestDto> request)
    {
        logger.LogInformation("{Logprefix}: AccreditationPrnIssueAuthService - ReplaceAllByAccreditationId: request for accreditation {AccreditationId}", logPrefix, accreditationId);

        var entities = mapper.Map<List<AccreditationPrnIssueAuth>>(request);
        await repository.ReplaceAllByAccreditationId(accreditationId, entities);
    }
}