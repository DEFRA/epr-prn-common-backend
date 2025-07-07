namespace EPR.PRN.Backend.API.Services;

using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using EPR.PRN.Backend.API.Common.Helpers;
using EPR.PRN.Backend.API.Dto.Accreditation;
using EPR.PRN.Backend.API.Services.Interfaces;
using EPR.PRN.Backend.Data.DataModels.Accreditations;
using EPR.PRN.Backend.Data.DataModels.Registrations;
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

        //THIS CODE NEEDS TO BE UPDATED TO WORK WITH THE EXISTING REGISTRATION MODELS AND STRUCTURE:

        // material id is actually registrationmaterial id. 
        // we may need to search registration materials on organisation id and material id. (organisationid is in the root registration so would have to search for the orgs registrations and find if it has a reg material for the material type)
        // if we do then we search for the accreditation assoiciated with that registration material.


        // if no accreditation exists with the registration maaterial:
        // 1 create a site (hard code to the details in all the figma)
        // 2 create registration for site and orgid and application type
        // 3 create registration material for material id linked ro reg
        // 4 create new accreditation for registrationmaterial.
        

        var entity = await repository.GetAccreditationDetails(
            organisationId,
            materialId,
            applicationTypeId);

        if (entity != null)
        {
            return entity.ExternalId;
        }

        entity = mapper.Map<Accreditation>(
            new AccreditationRequestDto
            {
                OrganisationId = organisationId,
                RegistrationMaterialId = materialId,
                ApplicationTypeId = applicationTypeId,
                AccreditationStatusId = 1,
                AccreditationYear = 2026,
                AccreferenceNumber = string.Empty
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
        logger.LogInformation("{Logprefix}: AccreditationService - CreateAccreditation: request to create accreditation {Accreditation}", logPrefix, LogParameterSanitizer.Sanitize(accreditationDto));

        var entity = mapper.Map<Accreditation>(accreditationDto);

        await repository.Create(entity);

        return entity.ExternalId;
    }

    public async Task UpdateAccreditation(AccreditationRequestDto accreditationDto)
    {
        logger.LogInformation("{Logprefix}: AccreditationService - UpdateAccreditation: request to update accreditation {Accreditation}", logPrefix, LogParameterSanitizer.Sanitize(accreditationDto));

        var entity = mapper.Map<Accreditation>(accreditationDto);

        await repository.Update(entity);
    }

    [ExcludeFromCodeCoverage]
    public async Task ClearDownDatabase()
    {
        // Temporary: Aid to QA whilst Accreditation uses in-memory database.
        await repository.ClearDownDatabase();
    }
}