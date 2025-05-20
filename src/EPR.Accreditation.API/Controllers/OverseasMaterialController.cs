namespace EPR.Accreditation.API.Controllers
{
    using EPR.Accreditation.API.Services.Interfaces;
    using Microsoft.AspNetCore.Mvc;
    using DTO = EPR.Accreditation.API.Common.Dtos;

    [ApiController]
    [Route("/api/Accreditation/{id}/OverseasMaterial")]
    public class OverseasMaterialController : ControllerBase
    {
        protected readonly IAccreditationService _accreditationService;

        public OverseasMaterialController(
            IAccreditationService accreditationService)
        {
            _accreditationService = accreditationService ?? throw new ArgumentNullException(nameof(accreditationService));
        }

        [HttpGet("{accreditationMaterialid}")]
        [ProducesResponseType(typeof(DTO.Response.AccreditationMaterial), 200)]
        public async Task<IActionResult> GetOverseasSiteMaterial(
            Guid id,
            Guid overseasSiteId,
            Guid accreditationMaterialid)
        {
            var material = await _accreditationService.GetMaterial(
                id,
                accreditationMaterialid);

            if (material == null)
            {
                return NotFound();
            }

            return Ok(material);
        }

        [HttpPost("{materialId}")]
        [ProducesResponseType(typeof(Guid), 200)]
        public async Task<IActionResult> CreateMaterial(
            Guid id,
            Guid materialId,
            [FromBody] DTO.Request.AccreditationMaterial accreditationMaterial)
        {
            if (accreditationMaterial == null)
            {
                return BadRequest();
            }

            var accreditationMaterialId = await _accreditationService.CreateMaterial(
                id,
                materialId,
                Common.Enums.OperatorType.Exporter,
                accreditationMaterial);

            return Ok(accreditationMaterialId);
        }

        [HttpPut("{accreditationMaterialId}")]
        public async Task<IActionResult> UpdateOverseasSiteMaterial(
            Guid id,
            Guid accreditationMaterialid,
            [FromBody] DTO.Request.AccreditationMaterial accreditationMaterial)
        {
            if (accreditationMaterial == null)
            {
                return BadRequest();
            }

            await _accreditationService.UpdateMaterail(
                id,
                accreditationMaterialid,
                accreditationMaterial);

            return Ok();
        }
    }
}
