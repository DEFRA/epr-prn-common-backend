using EPR.Accreditation.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using DTO = EPR.Accreditation.API.Common.Dtos;

namespace EPR.Accreditation.API.Controllers
{
    [ApiController]
    [Route("/api/[controller]/{id}")]
    public class SaveAndContinueController : ControllerBase
    {
        protected readonly IAccreditationService _accreditationService;

        public SaveAndContinueController(IAccreditationService accreditationService)
        {
            _accreditationService = accreditationService ?? throw new ArgumentNullException(nameof(accreditationService));
        }

        [HttpGet]
        [ProducesResponseType(typeof(DTO.SaveAndContinue), 200)]
        public async Task<IActionResult> GetSaveAndContinue(Guid id)
        {
            var saveAndContinue = await _accreditationService.GetSaveAndContinue(id);

            if (saveAndContinue == null)
                return NotFound();

            return Ok(saveAndContinue);
        }

        [HttpGet("/api/[controller]/HasApplicationSaved/{id}")]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> GetHasApplicationSaved(Guid id)
        {
            var hasApplicationSaved = await _accreditationService.GetHasApplicationSaved(id);

            return Ok(hasApplicationSaved);
        }

        [HttpPost]
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

        [HttpDelete]
        public async Task<IActionResult> DeleteSaveAndContinue(Guid id)
        {
            await _accreditationService.DeleteSaveAndContinue(id);

            return Ok();
        }
    }
}
