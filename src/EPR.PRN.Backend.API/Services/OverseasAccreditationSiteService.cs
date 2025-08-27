using AutoMapper;
using EPR.PRN.Backend.API.Common.Helpers;
using EPR.PRN.Backend.API.Dto.Accreditation;
using EPR.PRN.Backend.API.Services.Interfaces;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces.Accreditation;
using Microsoft.Extensions.Logging;

namespace EPR.PRN.Backend.API.Services;

public class OverseasAccreditationSiteService(
    IOverseasAccreditationSiteRepository repository,
    IMapper mapper,
    ILogger<OverseasAccreditationSiteService> logger) : IOverseasAccreditationSiteService
{
    public async Task<List<OverseasAccreditationSiteDto>?> GetAllByAccreditationId(Guid accreditationId)
    {
        logger.LogInformation("OverseasAccreditationSiteService.GetAllByAccreditationId: request for accreditation {AccreditationId}", accreditationId);

        List<OverseasAccreditationSite>? entities = await repository.GetAllByAccreditationId(accreditationId);
        var dtos = mapper.Map<List<OverseasAccreditationSiteDto>>(entities);

        return dtos;
    }

    public async Task PostByAccreditationId(Guid accreditationId, OverseasAccreditationSiteDto request)
    {
        logger.LogInformation("OverseasAccreditationSiteService.PostByAccreditationId: request for accreditation {AccreditationId}, new request: {Request}", accreditationId, LogParameterSanitizer.Sanitize(request));

        var entity = mapper.Map<OverseasAccreditationSite>(request);
        await repository.PostByAccreditationId(accreditationId, entity);
    }
}
