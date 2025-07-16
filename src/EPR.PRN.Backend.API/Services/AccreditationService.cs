namespace EPR.PRN.Backend.API.Services;

using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Common.Helpers;
using EPR.PRN.Backend.API.Dto.Accreditation;
using EPR.PRN.Backend.API.Helpers;
using EPR.PRN.Backend.API.Services.Interfaces;
using EPR.PRN.Backend.Data.DataModels.Accreditations;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.DTO;
using EPR.PRN.Backend.Data.Interfaces;
using EPR.PRN.Backend.Data.Interfaces.Accreditations;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using EPR.PRN.Backend.Data.Repositories.Regulator;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

public class AccreditationService(
    IAccreditationRepository repository,
    IRegistrationRepository registrationRepository,
    IRegistrationMaterialRepository registrationMaterialRepository,
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

        AddressDto reprocessingSiteAddress = new AddressDto
        {
            AddressLine1 = "Reprocessing Site Address Line 1",
            AddressLine2 = "Reprocessing Site Address Line 2",
            TownCity = "Reprocessing Site Town/City",
            County = "County",
            PostCode = "AB12 3CD",
            GridReference = "TQ12345678",
            NationId = 1, // Assuming 1 is the ID for England           
            Country = "United Kingdom",
        };

        var registration = await registrationRepository.CreateRegistrationAsync(applicationTypeId, organisationId, reprocessingSiteAddress);


        if (registration == null)
        {
            logger.LogError("{Logprefix}: AccreditationService - GetOrCreateAccreditation: Failed to create registration for organisation {OrganisationId} with application type {ApplicationTypeId}", logPrefix, organisationId, applicationTypeId);
            throw new NotFoundException("Failed to create registration");
        }

        var materialName = materialId switch
        {
            1 => "Plastic",
            2 => "Steel",
            3 => "Aluminium",
            4 => "Glass",
            5 => "Paper/Board",
            6 => "Wood",
            _ => throw new ArgumentException("Invalid material ID")
        };

        var material = await registrationMaterialRepository.CreateAsync(registration.ExternalId, materialName);

        var entity = new Accreditation
        {
            RegistrationMaterialId = material.Id,
            AccreditationStatusId = 1,
            AccreditationYear = 2026,
            ApplicationReferenceNumber = string.Empty
        };

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


}