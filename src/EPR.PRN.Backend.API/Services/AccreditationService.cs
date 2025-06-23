namespace EPR.PRN.Backend.API.Services;

using System.Diagnostics.CodeAnalysis;
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

    public async Task<Guid> GetOrCreateAccreditation(
        Guid organisationId,
        int materialId,
        int applicationTypeId)
    {
        logger.LogInformation("{Logprefix}: AccreditationService - GetorCreateAccreditation: request for organisationId {organisationId} type of material {materialId} and applicationType {applicationTypeId}", logPrefix, organisationId, materialId, applicationTypeId);

        var entity = await repository.GetAccreditationDetails(
            organisationId,
            materialId,
            applicationTypeId);

        if (entity != null)
        {
            return entity.ExternalId;
        }

        entity = mapper.Map<AccreditationEntity>(
            new AccreditationRequestDto
            {
                OrganisationId = organisationId,
                RegistrationMaterialId = materialId,
                ApplicationTypeId = applicationTypeId,
                AccreditationStatusId = 1,
                AccreditationYear = 2026
            });

        await repository.Create(entity);

        return entity.ExternalId;
    }

    public async Task<AccreditationDto> GetAccreditationById(Guid accreditationId)
    {
        logger.LogInformation("{Logprefix}: AccreditationService - GetAccreditationById: request for accreditation {AccreditationId}", logPrefix, accreditationId);

        var entity = await repository.GetById(accreditationId);
        var accreditationDto = mapper.Map<AccreditationDto>(entity);

        return accreditationDto;
    }

    public async Task<Guid> CreateAccreditation(AccreditationRequestDto accreditationDto)
    {
        logger.LogInformation("{Logprefix}: AccreditationService - CreateAccreditation: request to create accreditation", logPrefix);

        var entity = mapper.Map<AccreditationEntity>(accreditationDto);

        await repository.Create(entity);

        return entity.ExternalId;
    }

    public async Task UpdateAccreditation(AccreditationRequestDto accreditationDto)
    {
        logger.LogInformation("{Logprefix}: AccreditationService - UpdateAccreditation: request to update accreditation", logPrefix);

        var entity = mapper.Map<AccreditationEntity>(accreditationDto);

        await repository.Update(entity);
    }

    [ExcludeFromCodeCoverage]
    public async Task ClearDownDatabase()
    {
        // Temporary: Aid to QA whilst Accreditation uses in-memory database.
        await repository.ClearDownDatabase();
    }
}