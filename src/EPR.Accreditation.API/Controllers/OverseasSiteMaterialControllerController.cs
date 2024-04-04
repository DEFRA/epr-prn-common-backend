using EPR.Accreditation.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using DTO = EPR.Accreditation.API.Common.Dtos;

namespace EPR.Accreditation.API.Controllers
{
    [ApiController]
    [Route("api/Accreditation/{externalId}/OverseasSite/{overseasExternalSiteId}/Material")]
    public class OverseasSiteMaterialControllerController : ControllerBase
    {
        protected readonly IAccreditationService _accreditationService;

        public OverseasSiteMaterialControllerController(IAccreditationService accreditationService)
        {
            _accreditationService = accreditationService ?? throw new ArgumentNullException(nameof(accreditationService));
        }

        [HttpGet("{materialExternalId}")]
        [ProducesResponseType(typeof(DTO.AccreditationMaterial), 200)]
        public async Task<IActionResult> GetOverseasSiteMaterial(
            Guid externalId,
            Guid overseasSiteExternalId,
            Guid materialExternalId)
        {
            var material = await _accreditationService.GetMaterial(
                externalId,
                overseasSiteExternalId,
                materialExternalId);

            if (material == null)
                return NotFound();

            return Ok(material);
        }

        [HttpPost()]
        public async Task<IActionResult> CreateOverseasSiteMaterial(
            Guid externalId,
            Guid overseasExternalSiteId,
            [FromBody] DTO.AccreditationMaterial accreditationMaterial)
        {
            if (accreditationMaterial == null)
                return BadRequest();

            var materialId = await _accreditationService.CreateMaterial(
                externalId,
                overseasExternalSiteId,
                accreditationMaterial);

            return Ok(materialId);
        }

        [HttpPut("{materialExternalId}")]
        public async Task<IActionResult> UpdateOverseasSiteMaterial(
            Guid externalId,
            Guid siteExternalId,
            Guid materialExternalId,
            [FromBody] DTO.AccreditationMaterial accreditationMaterial)
        {
            if (materialExternalId != accreditationMaterial.ExternalId)
                return BadRequest("External ID does not match");

            await _accreditationService.UpdateMaterail(
                externalId,
                siteExternalId,
                materialExternalId,
                accreditationMaterial);

            return Ok();
        }
    }
}
