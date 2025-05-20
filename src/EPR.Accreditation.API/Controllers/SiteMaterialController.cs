namespace EPR.Accreditation.API.Controllers
{
    using EPR.Accreditation.API.Services.Interfaces;
    using Microsoft.AspNetCore.Mvc;
    using DTO = EPR.Accreditation.API.Common.Dtos;

    [ApiController]
    [Route("api/Accreditation/{id}/Material")]
    public class SiteMaterialController : ControllerBase
    {
        protected readonly IAccreditationService _accreditationService;

        public SiteMaterialController(
            IAccreditationService accreditationService)
        {
            _accreditationService = accreditationService ?? throw new ArgumentNullException(nameof(accreditationService));
        }

        [HttpGet("{accreditationMaterialId}")]
        [ProducesResponseType(typeof(DTO.Response.AccreditationMaterial), 200)]
        public async Task<IActionResult> GetSiteMaterial(
            Guid id,
            Guid accreditationMaterialId)
        {
            var material = await _accreditationService.GetMaterial(
                id,
                accreditationMaterialId);

            if (material == null)
                return NotFound();

            return Ok(material);
        }

        [HttpPost("{materialid}")]
        public async Task<IActionResult> CreateSiteMaterial(
            Guid id,
            Guid materialId,
            [FromBody] DTO.Request.AccreditationMaterial accreditationMaterial)
        {
            if (accreditationMaterial == null)
                return BadRequest();

            var accreditationMaterialId = await _accreditationService.CreateMaterial(
                id,
                materialId,
                Common.Enums.OperatorType.Reprocessor,
                accreditationMaterial);

            return Ok(accreditationMaterialId);
        }

        [HttpPut("{accreditationMaterialId}")]
        public async Task<IActionResult> UpdateSiteMaterial(
            Guid id,
            Guid accreditationMaterialId,
            [FromBody] DTO.Request.AccreditationMaterial accreditationMaterial)
        {
            await _accreditationService.UpdateMaterail(
                id,
                accreditationMaterialId,
                accreditationMaterial);

            return Ok();
        }
    }
}
