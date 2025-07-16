namespace EPR.PRN.Backend.API.Controllers.Accreditation;

using System.Diagnostics.CodeAnalysis;
using EPR.PRN.Backend.API.Common.Constants;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Dto.Accreditation;
using EPR.PRN.Backend.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/Accreditation")]
[FeatureGate(FeatureFlags.EnableAccreditation)]
public class AccreditationController(
    IAccreditationService accreditationService,
    IAccreditationFileUploadService accreditationFileUploadService) : ControllerBase
{
    [HttpGet("{organisationId}/{materialId}/{applicationTypeId}")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(
        [FromRoute] Guid organisationId,
        [FromRoute] int materialId,
        [FromRoute] int applicationTypeId)
    {
        var accreditation = await accreditationService.GetOrCreateAccreditation(
            organisationId,
            materialId,
            applicationTypeId);

        return Ok(accreditation);
    }

    [HttpGet("{accreditationId}")]
    [ProducesResponseType(typeof(AccreditationDto), 200)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get([FromRoute]Guid accreditationId)
    {
        AccreditationDto accreditation = await accreditationService.GetAccreditationById(accreditationId);

        if (accreditation == null)
        {
            return NotFound();
        }

        return Ok(accreditation);
    }

    [HttpPost]
    [ProducesResponseType(typeof(AccreditationDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Post([FromBody]AccreditationRequestDto request)
    {
        Guid externalId;
        if (request.ExternalId == null)
        {
            externalId = await accreditationService.CreateAccreditation(request);
        }
        else
        {
            externalId = request.ExternalId.Value;
            await accreditationService.UpdateAccreditation(request);
        }

        var accreditation = await accreditationService.GetAccreditationById(externalId);

        return Ok(accreditation);
    }

    [HttpGet("Files/{externalId}")]
    [ProducesResponseType(typeof(AccreditationFileUploadDto), 200)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetFileUpload([FromRoute] Guid externalId)
    {
        var fileUpload = await accreditationFileUploadService.GetByExternalId(externalId);
        return fileUpload != null ? Ok(fileUpload) : NotFound();
    }

    [HttpGet("{accreditationId}/Files/{fileUploadTypeId}/{fileUploadStatusId?}")]
    [ProducesResponseType(typeof(List<AccreditationFileUploadDto>), 200)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetFileUploads(
        [FromRoute] Guid accreditationId,
        [FromRoute] int fileUploadTypeId,
        [FromRoute] int fileUploadStatusId = (int)AccreditationFileUploadStatus.UploadComplete)
    {
        if (!Enum.IsDefined(typeof(AccreditationFileUploadType), fileUploadTypeId))
            return BadRequest("FileUploadTypeId is invalid");

        if (!Enum.IsDefined(typeof(AccreditationFileUploadStatus), fileUploadStatusId))
            return BadRequest("FileUploadStatusId is invalid");

        List<AccreditationFileUploadDto> fileUploads = await accreditationFileUploadService.GetByAccreditationId(accreditationId, fileUploadTypeId, fileUploadStatusId);

        return Ok(fileUploads);
    }

    [HttpPost("{accreditationId}/Files")]
    [ProducesResponseType(typeof(AccreditationFileUploadDto), 200)]
    public async Task<IActionResult> UpsertFileUpload([FromRoute] Guid accreditationId, [FromBody] AccreditationFileUploadDto request)
    {
        Guid externalId;
        if (!request.ExternalId.HasValue || request.ExternalId == Guid.Empty)
        {
            externalId = await accreditationFileUploadService.CreateFileUpload(accreditationId, request);
        }
        else
        {
            externalId = request.ExternalId.Value;
            await accreditationFileUploadService.UpdateFileUpload(accreditationId, request);
        }

        var fileUpload = await accreditationFileUploadService.GetByExternalId(externalId);
        
        return Ok(fileUpload);
    }

    [HttpDelete("{accreditationId}/Files/{fileId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteFileUpload([FromRoute] Guid accreditationId, [FromRoute] Guid fileId)
    {
        await accreditationFileUploadService.DeleteFileUpload(accreditationId, fileId);

        return Ok();
    }
}