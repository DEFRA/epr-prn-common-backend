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
    public async Task<IActionResult> Post(
        [FromHeader(Name = "X-EPR-ORGANISATION")] Guid orgId,
        [FromHeader(Name = "X-EPR-USER")] Guid userId,
        [FromBody]AccreditationRequestDto request)
    {
        if (request.ExternalId == null)
        {
            await accreditationService.CreateAccreditation(request, orgId, userId);
        }
        else
        {
            await accreditationService.UpdateAccreditation(request, userId);
        }

        return NoContent();
    }
}