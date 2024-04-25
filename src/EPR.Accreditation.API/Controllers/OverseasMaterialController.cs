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

        [HttpGet("{materialid}")]
        [ProducesResponseType(typeof(DTO.AccreditationMaterial), 200)]
        public async Task<IActionResult> GetOverseasSiteMaterial(
            Guid id,
            Guid overseasSiteId,
            Guid materialid)
        {
            var material = await _accreditationService.GetMaterial(
                id,
                materialid);

            if (material == null)
                return NotFound();

            return Ok(material);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Guid), 200)]
        public async Task<IActionResult> CreateMaterial(
            Guid id,
            [FromBody] DTO.AccreditationMaterial accreditationMaterial)
        {
            if (accreditationMaterial == null)
            {
                return BadRequest();
            }

            var materialId = await _accreditationService.CreateMaterial(
                id,
                Common.Enums.OperatorType.Exporter,
                accreditationMaterial);

            return Ok(materialId);
        }

        [HttpPut("{materialid}")]
        public async Task<IActionResult> UpdateOverseasSiteMaterial(
            Guid id,
            Guid materialid,
            [FromBody] DTO.AccreditationMaterial accreditationMaterial)
        {
            await _accreditationService.UpdateMaterail(
                id,
                materialid,
                accreditationMaterial);

            return Ok();
        }

        [HttpPut("{materialid}/Site/{overseasSiteId}")]
        public async Task<IActionResult> UpdateOverseasSiteMaterial(
            Guid id,
            Guid materialid,
            Guid overseasSiteId,
            [FromBody] DTO.AccreditationMaterial accreditationMaterial)
        {
            await _accreditationService.UpdateMaterail(
                id,
                materialid,
                accreditationMaterial,
                overseasSiteId);

            return Ok();
        }
    }
}
