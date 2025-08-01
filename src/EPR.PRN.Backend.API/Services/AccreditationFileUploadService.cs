﻿namespace EPR.PRN.Backend.API.Services;

using EPR.PRN.Backend.API.Common.Helpers;
using EPR.PRN.Backend.API.Dto.Accreditation;
using EPR.PRN.Backend.API.Services.Interfaces;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces.Accreditation;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

public class AccreditationFileUploadService(
    IAccreditationFileUploadRepository repository,
    ILogger<AccreditationFileUploadService> logger,
    IConfiguration configuration) : IAccreditationFileUploadService
{
    private readonly string logPrefix = string.IsNullOrEmpty(configuration["LogPrefix"]) ? "[EPR.PRN.Backend]" : configuration["LogPrefix"]!;

    public async Task<AccreditationFileUploadDto> GetByExternalId(Guid accreditationFileUploadId)
    {
        logger.LogInformation("{Logprefix}: AccreditationFileUploadService - GetByExternalId. ExternalId: {AccreditationFileUploadId}", logPrefix, accreditationFileUploadId);

        var entity = await repository.GetByExternalId(accreditationFileUploadId);
        var fileUploadDto = MapEntityToDto(entity);

        return fileUploadDto;
    }

    public async Task<List<AccreditationFileUploadDto>> GetByAccreditationId(Guid accreditationId, int fileUploadTypeId, int fileUploadStatusId)
    {
        logger.LogInformation("{Logprefix}: AccreditationFileUploadService - GetByAccreditationId. AccreditationId: {AccreditationId}", logPrefix, accreditationId);

        var entities = await repository.GetByAccreditationId(accreditationId, fileUploadTypeId, fileUploadStatusId);
        var dtos = entities.Select(MapEntityToDto).ToList();

        return dtos;
    }

    public async Task<Guid> CreateFileUpload(Guid accreditationId, AccreditationFileUploadDto requestDto)
    {
        logger.LogInformation("{Logprefix}: AccreditationFileUploadService - Create. AccreditationId: {AccreditationId}, Request DTO: {Dto}", logPrefix, accreditationId, LogParameterSanitizer.Sanitize(requestDto));

        var entity = MapDtoToEntity(requestDto);

        return await repository.Create(accreditationId, entity);
    }

    public async Task UpdateFileUpload(Guid accreditationId, AccreditationFileUploadDto requestDto)
    {
        logger.LogInformation("{Logprefix}: AccreditationFileUploadService - Update. AccreditationId: {AccreditationId}, Request DTO: {Dto}", logPrefix, accreditationId, LogParameterSanitizer.Sanitize(requestDto));

        var entity = MapDtoToEntity(requestDto);

        await repository.Update(accreditationId, entity);
    }

    public async Task DeleteFileUpload(Guid accreditationId, Guid fileId)
    {
        logger.LogInformation("{Logprefix}: AccreditationFileUploadService - Delete. AccreditationId: {AccreditationId}, FileId: {FileId}", logPrefix, accreditationId, fileId);

        await repository.Delete(accreditationId, fileId);
    }

    private AccreditationFileUploadDto MapEntityToDto(AccreditationFileUpload entity)
    {

        // all thie nullable types need to be fixed accross all teams for this model.
        return new AccreditationFileUploadDto
        {
            ExternalId = entity.ExternalId,
            SubmissionId = entity.SubmissionId,
            OverseasSiteId = entity.OverseasSiteId,
            Filename = entity.Filename!,
            FileId = entity.FileId,
            UploadedOn = entity.DateUploaded ?? DateTime.MinValue, // this should not be null.
            UploadedBy = entity.UpdatedBy,
            FileUploadTypeId = entity.FileUploadTypeId.GetValueOrDefault(),
            FileUploadStatusId = entity.FileUploadStatusId.GetValueOrDefault()
        };
    }

    private AccreditationFileUpload MapDtoToEntity(AccreditationFileUploadDto dto)
    {
        return new AccreditationFileUpload
        {
            ExternalId = dto.ExternalId.HasValue ? dto.ExternalId.Value : Guid.Empty,
            SubmissionId = dto.SubmissionId,
            OverseasSiteId = dto.OverseasSiteId,
            Filename = dto.Filename,
            FileId = dto.FileId.GetValueOrDefault(),
            DateUploaded = dto.UploadedOn,
            UpdatedBy = dto.UploadedBy,
            FileUploadTypeId = dto.FileUploadTypeId,
            FileUploadStatusId = dto.FileUploadStatusId
        };
    }
}