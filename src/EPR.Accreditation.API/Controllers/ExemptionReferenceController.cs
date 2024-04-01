using EPR.Accreditation.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using DTO = EPR.Accreditation.API.Common.Dtos;

namespace EPR.Accreditation.API.Controllers
{
    [ApiController]
    [Route("/api/[controller]/{id}")]
    public class ExemptionReferenceController : ControllerBase
    {
        protected readonly IAccreditationService _accreditationService;

        public ExemptionReferenceController(IAccreditationService accreditationService)
        {
            _accreditationService = accreditationService ?? throw new ArgumentNullException(nameof(accreditationService));
        }

        [HttpGet]
        [ProducesResponseType(typeof(DTO.ExemptionReference), 200)]
        public async Task<IActionResult> GetExemptionReference(Guid id)
        {
            var exemptionReference = await _accreditationService.GetExemptionReference(id);

            if (exemptionReference == null)
                return NotFound();

            return Ok(exemptionReference);
        }
    }
}
