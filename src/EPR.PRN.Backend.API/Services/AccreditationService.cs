namespace EPR.PRN.Backend.API.Services;

using AutoMapper;
using EPR.PRN.Backend.API.Dto.Accreditation;
using EPR.PRN.Backend.API.Services.Interfaces;
using EPR.PRN.Backend.Data.DataModels.Accreditations;
using EPR.PRN.Backend.Data.Interfaces.Accreditations;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

public class AccreditationService(
    IAccreditationRepository repository,
    IMapper mapper,
    ILogger<AccreditationService> logger,
    IConfiguration configuration) : IAccreditationService
{
    private readonly string logPrefix = string.IsNullOrEmpty(configuration["LogPrefix"]) ? "[EPR.PRN.Backend]" : configuration["LogPrefix"]!;

    public async Task<AccreditationDto> GetAccreditationById(Guid accreditationId)
    {
        logger.LogInformation("{Logprefix}: AccreditationService - GetAccreditationById: request for accreditation {AccreditationId}", logPrefix, accreditationId);

        var entity = await repository.GetById(accreditationId);
        var accreditationDto = mapper.Map<AccreditationDto>(entity);

        return accreditationDto;
    }

    public async Task<Guid> CreateAccreditation(AccreditationRequestDto accreditationDto)
    {
        logger.LogInformation("{Logprefix}: AccreditationService - CreateAccreditation: request to create accreditation {Accreditation}", logPrefix, JsonConvert.SerializeObject(accreditationDto));

        var entity = mapper.Map<Accreditation>(accreditationDto);

        await repository.Create(entity);

        return entity.ExternalId;
    }

    public async Task UpdateAccreditation(AccreditationRequestDto accreditationDto)
    {
        logger.LogInformation("{Logprefix}: AccreditationService - UpdateAccreditation: request to update accreditation {Accreditation}", logPrefix, JsonConvert.SerializeObject(accreditationDto));

        var entity = mapper.Map<Accreditation>(accreditationDto);

        await repository.Update(entity);
    }
}