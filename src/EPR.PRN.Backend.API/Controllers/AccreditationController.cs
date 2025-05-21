namespace EPR.PRN.Backend.API.Controllers;

using EPR.PRN.Backend.API.Common.Constants;
using EPR.PRN.Backend.API.Dto.Accreditation;
using EPR.PRN.Backend.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/Accreditation")]
[FeatureGate(FeatureFlags.EnableAccreditation)]
public class AccreditationController(IAccreditationService accreditationService) : ControllerBase
{
    [HttpGet("{organisationId}/{materialId}/{applicationTypeId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
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
    [ProducesResponseType(StatusCodes.Status200OK)]
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
}