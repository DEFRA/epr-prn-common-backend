using EPR.Accreditation.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using DTO = EPR.Accreditation.API.Common.Dtos;

namespace EPR.Accreditation.API.Controllers
{
    [ApiController]
    [Route("api/Site/{siteId}/ExemptionReference")]
    public class ExemptionReferenceController : ControllerBase
    {
        protected readonly IAccreditationService _accreditationService;

        public ExemptionReferenceController(IAccreditationService accreditationService)
        {
            _accreditationService = accreditationService ?? throw new ArgumentNullException(nameof(accreditationService));
        }

        [HttpGet("{exemptionReferenceId}")]
        [ProducesResponseType(typeof(DTO.ExemptionReference), 200)]
        public async Task<IActionResult> GetExemptionReference(int exemptionReferenceId)
        {
            var exemptionReference = await _accreditationService.GetExemptionReference(exemptionReferenceId);

            if (exemptionReference == null)
                return NotFound();

            return Ok(exemptionReference);
        }

        [HttpPost("{exemptionReferenceId}")]
        public async Task<IActionResult> AddExemptionReference(
            int exemptionReferenceId,
            [FromBody] DTO.ExemptionReference exemptionReference)
        {
            if (exemptionReference == null)
                return BadRequest("No exemption reference supplied");

            await _accreditationService.AddExemptionReference(
                exemptionReferenceId,
                exemptionReference);

            return Ok();
        }

        [HttpPut("{exemptionReferenceId}")]
        public async Task<IActionResult> UpdateExemptionReference(
            int siteId,
            int exemptionReferenceId,
            [FromBody] DTO.ExemptionReference exemptionReference)
        {
            await _accreditationService.UpdateExemptionReference(
                siteId,
                exemptionReferenceId,
                exemptionReference);

            return Ok();
        }
    }
}
