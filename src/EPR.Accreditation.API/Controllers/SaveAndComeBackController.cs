using EPR.Accreditation.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using DTO = EPR.Accreditation.API.Common.Dtos;

namespace EPR.Accreditation.API.Controllers
{
    [ApiController]
    [Route("/api/[controller]/{id}")]
    public class SaveAndComeBackController : ControllerBase
    {
        protected readonly IAccreditationService _accreditationService;

        public SaveAndComeBackController(IAccreditationService accreditationService)
        {
            _accreditationService = accreditationService ?? throw new ArgumentNullException(nameof(accreditationService));
        }

        [HttpGet]
        [ProducesResponseType(typeof(DTO.SaveAndComeBack), 200)]
        public async Task<IActionResult> GetSaveAndComeBack(Guid id)
        {
            var saveAndContinue = await _accreditationService.GetSaveAndComeBack(id);

            if (saveAndContinue == null)
                return NotFound();

            return Ok(saveAndContinue);
        }

        [HttpPost]
        public async Task<IActionResult> AddSaveAndComeBack(
            Guid id,
            [FromBody] DTO.SaveAndComeBack saveAndComeBack)
        {
            if (saveAndComeBack == null)
                return BadRequest("No saveAndContinue record supplied");

            await _accreditationService.AddSaveAndComeBack(
                id,
                saveAndComeBack);

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteSaveAndComeBack(Guid id)
        {
            await _accreditationService.DeleteSaveAndComeBack(id);

            return Ok();
        }
    }
}
