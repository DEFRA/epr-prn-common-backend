namespace EPR.PRN.Backend.API.Controllers;

using System.Net;
using EPR.PRN.Backend.API.Common.Constants;
using EPR.PRN.Backend.API.Dto.Accreditation;
using EPR.PRN.Backend.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/AccreditationPRNIssueAuth")]
[FeatureGate(FeatureFlags.EnableAccreditation)]
public class AccreditationPrnIssueAuthController(IAccreditationPrnIssueAuthService accreditationPrnIssueAuthService, 
    ILogger<AccreditationPrnIssueAuthController> logger) : ControllerBase
{
    [HttpGet("{accreditationId}")]
    [ProducesResponseType(typeof(AccreditationPrnIssueAuthDto), 200)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByAccreditationId([FromRoute]Guid accreditationId)
    {
        List<AccreditationPrnIssueAuthDto> accreditationPrnIssueAuths = await accreditationPrnIssueAuthService.GetByAccreditationId(accreditationId);

        if (accreditationPrnIssueAuths == null)
        {
            return NotFound();
        }

        return Ok(accreditationPrnIssueAuths);
    }

    [HttpPost("{accreditationId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Post([FromRoute]Guid accreditationId, [FromBody]List<AccreditationPrnIssueAuthRequestDto> request)
    {
        await accreditationPrnIssueAuthService.ReplaceAllByAccreditationId(accreditationId, request);

        return NoContent();
    }
}