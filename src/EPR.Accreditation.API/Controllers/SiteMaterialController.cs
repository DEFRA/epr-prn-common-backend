using EPR.Accreditation.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using DTO = EPR.Accreditation.API.Common.Dtos;

namespace EPR.Accreditation.API.Controllers
{
    [ApiController]
    [Route("api/Accreditation/{externalId}/Site/Material")]
    public class SiteMaterialController : ControllerBase
    {
        protected readonly IAccreditationService _accreditationService;

        public SiteMaterialController(
            IAccreditationService accreditationService)
        {
            _accreditationService = accreditationService ?? throw new ArgumentNullException(nameof(accreditationService));
        }

        [HttpGet("{materialExternalId}")]
        [ProducesResponseType(typeof(DTO.AccreditationMaterial), 200)]
        public async Task<IActionResult> GetSiteMaterial(
            Guid externalId,
            Guid materialExternalId)
        {
            var material = await _accreditationService.GetMaterial(
                externalId,
                null,
                materialExternalId);

            if (material == null)
                return NotFound();

            return Ok(material);
        }

        [HttpPost()]
        public async Task<IActionResult> CreateSiteMaterial(
            Guid externalId,
            [FromBody] DTO.AccreditationMaterial accreditationMaterial)
        {
            if (accreditationMaterial == null)
                return BadRequest();

            var materialId = await _accreditationService.CreateMaterial(
                externalId,
                null,
                accreditationMaterial);

            return Ok(materialId);
        }

        [HttpPut("{materialExternalId}")]
        public async Task<IActionResult> UpdateSiteMaterial(
            Guid externalId,
            Guid materialExternalId,
            [FromBody] DTO.AccreditationMaterial accreditationMaterial)
        {
            await _accreditationService.UpdateMaterail(
                externalId,
                null,
                materialExternalId,
                accreditationMaterial);

            return Ok();
        }
    }
}
