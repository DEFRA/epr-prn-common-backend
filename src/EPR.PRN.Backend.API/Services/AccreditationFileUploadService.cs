namespace EPR.PRN.Backend.API.Services;

using EPR.PRN.Backend.API.Dto.Accreditation;
using EPR.PRN.Backend.API.Services.Interfaces;
using EPR.PRN.Backend.Data.DataModels.Accreditations;
using EPR.PRN.Backend.Data.Interfaces.Accreditations;
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
        logger.LogInformation("{Logprefix}: AccreditationFileUploadService - Create. AccreditationId: {AccreditationId}, Request DTO: {Dto}", logPrefix, accreditationId, JsonConvert.SerializeObject(requestDto));

        var entity = MapDtoToEntity(requestDto);

        return await repository.Create(accreditationId, entity);
    }

    public async Task UpdateFileUpload(Guid accreditationId, AccreditationFileUploadDto requestDto)
    {
        logger.LogInformation("{Logprefix}: AccreditationFileUploadService - Update. AccreditationId: {AccreditationId}, Request DTO: {Dto}", logPrefix, accreditationId, JsonConvert.SerializeObject(requestDto));

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
        return new AccreditationFileUploadDto
        {
            ExternalId = entity.ExternalId,
            OverseasSiteId = entity.OverseasSiteId,
            Filename = entity.Filename,
            FileId = entity.FileId,
            UploadedOn = entity.UploadedOn,
            UploadedBy = entity.UploadedBy,
            FileUploadTypeId = entity.FileUploadTypeId,
            FileUploadStatusId = entity.FileUploadStatusId
        };
    }

    private AccreditationFileUpload MapDtoToEntity(AccreditationFileUploadDto dto)
    {
        return new AccreditationFileUpload
        {
            ExternalId = dto.ExternalId.HasValue ? dto.ExternalId.Value : Guid.Empty,
            OverseasSiteId = dto.OverseasSiteId,
            Filename = dto.Filename,
            FileId = dto.FileId,
            UploadedOn = dto.UploadedOn,
            UploadedBy = dto.UploadedBy,
            FileUploadTypeId = dto.FileUploadTypeId,
            FileUploadStatusId = dto.FileUploadStatusId
        };
    }
}