using EPR.Accreditation.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using DTO = EPR.Accreditation.API.Common.Dtos;

namespace EPR.Accreditation.API.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class SaveAndContinueController : ControllerBase
    {
        protected readonly IAccreditationService _accreditationService;

        public SaveAndContinueController(IAccreditationService accreditationService)
        {
            _accreditationService = accreditationService ?? throw new ArgumentNullException(nameof(accreditationService));
        }

        [HttpGet("{id}/SaveAndContinue")]
        [ProducesResponseType(typeof(DTO.SaveAndContinue), 200)]
        public async Task<IActionResult> GetSaveAndContinue(Guid id)
        {
            var saveAndContinue = await _accreditationService.GetSaveAndContinue(id);

            if (saveAndContinue == null)
                return NotFound();

            return Ok(saveAndContinue);
        }

        [HttpPost("{id}/SaveAndContinue")]
        public async Task<IActionResult> AddSaveAndContinue(
            Guid id,
            [FromBody] DTO.SaveAndContinue saveAndContinue)
        {
            if (saveAndContinue == null)
                return BadRequest("No saveAndContinue record supplied");

            await _accreditationService.AddSaveAndContinue(
                id,
                saveAndContinue);

            return Ok();
        }

        [HttpDelete("{id}/SaveAndContinue")]
        public async Task<IActionResult> DeleteSaveAndContinue(Guid id)
        {
            await _accreditationService.DeleteSaveAndContinue(id);

            return Ok();
        }
    }
}
