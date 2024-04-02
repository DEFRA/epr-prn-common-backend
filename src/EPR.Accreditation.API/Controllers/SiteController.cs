using EPR.Accreditation.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using DTO = EPR.Accreditation.API.Common.Dtos;

namespace EPR.Accreditation.API.Controllers
{
    [ApiController]
    public class SiteController : ControllerBase
    {
        protected readonly IAccreditationService _accreditationService;

        public SiteController(
            IAccreditationService accreditationService)
        {
            _accreditationService = accreditationService ?? throw new ArgumentNullException(nameof(accreditationService));
        }

        [HttpGet("/api/Accreditation/{externalId}/[controller]/{siteExternalId}")]
        [ProducesResponseType(typeof(DTO.Site), 200)]
        public async Task<IActionResult> GetSite(
            Guid externalId,
            Guid siteExternalId)
        {
            var site = await _accreditationService.GetSite(
                externalId,
                siteExternalId);

            if (site == null)
                return NotFound();

            return Ok(site);
        }

        [HttpPost("/api/Accreditation/{externalId}/[controller]")]
        [ProducesResponseType(typeof(Guid), 200)]
        public async Task<IActionResult> CreateSite(
            Guid externalId,
            DTO.Site site)
        {
            var siteExternalId = await _accreditationService.CreateSite(
                externalId,
                site);

            return Ok(siteExternalId);
        }

        [HttpPut("/api/Accreditation/{externalId}/[controller]")]
        public async Task<IActionResult> UpdateSite(DTO.Site site)
        {
            await _accreditationService.UpdateSite(site);

            return Ok();
        }

        [HttpGet("api/Site/{siteId}/ExemptionReference/{exemptionReferenceId}")]
        [ProducesResponseType(typeof(DTO.ExemptionReference), 200)]
        public async Task<IActionResult> GetExemptionReference(
            int exemptionReferenceId,
            int siteId)
        {
            var exemptionReference = await _accreditationService.GetExemptionReference(
                exemptionReferenceId,
                siteId);

            if (exemptionReference == null)
                return NotFound();

            return Ok(exemptionReference);
        }

        [HttpPost("api/Site/{siteId}/ExemptionReference")]
        public async Task<IActionResult> CreateExemptionReference(
            int siteId,
            [FromBody] DTO.ExemptionReference exemptionReference)
        {
            if (exemptionReference == null)
                return BadRequest();

            var exemptionReferenceId = await _accreditationService.CreateExemptionReference(
                siteId,
                exemptionReference);

            return Ok(exemptionReferenceId);
        }

        [HttpPut("api/Site/{siteId}/ExemptionReference/{exemptionReferenceId}")]
        public async Task<IActionResult> UpdateExemptionReference(
            int exemptionReferenceId,
            int siteId,
            [FromBody] DTO.ExemptionReference exemptionReference)
        {
            await _accreditationService.UpdateExemptionReference(
                exemptionReferenceId,
                siteId,
                exemptionReference);

            return Ok();
        }
    }
}