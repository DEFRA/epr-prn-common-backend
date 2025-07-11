using EPR.PRN.Backend.API.Common.Constants;
using EPR.PRN.Backend.API.Dto.Accreditation;
using EPR.PRN.Backend.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;

namespace EPR.PRN.Backend.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/OverseasAccreditationSite")]
[FeatureGate(FeatureFlags.EnableAccreditation)]
public class OverseasAccreditationSiteController(IOverseasAccreditationSiteService overseasAccreditationSiteService) : ControllerBase
{
    [HttpGet("{accreditationId}")]
    [ProducesResponseType(typeof(OverseasAccreditationSiteDto), 200)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllByAccreditationId([FromRoute]Guid accreditationId)
    {
        var overseasAccreditationSites = await overseasAccreditationSiteService.GetAllByAccreditationId(accreditationId);

        if (overseasAccreditationSites == null)
        {
            return NotFound();
        }

        return Ok(overseasAccreditationSites);
    }

    [HttpPost("{accreditationId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Post([FromRoute]Guid accreditationId, [FromBody]OverseasAccreditationSiteDto request)
    {
        await overseasAccreditationSiteService.PostByAccreditationId(accreditationId, request);

        return NoContent();
    }
}
