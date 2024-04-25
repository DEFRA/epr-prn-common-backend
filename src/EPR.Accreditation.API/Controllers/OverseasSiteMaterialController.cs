using EPR.Accreditation.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using DTO = EPR.Accreditation.API.Common.Dtos;

namespace EPR.Accreditation.API.Controllers
{
    [ApiController]
    [Route("api/Accreditation/{id}/OverseasSite/{overseasSiteId}/Material")]
    public class OverseasSiteMaterialController : ControllerBase
    {
        protected readonly IAccreditationService _accreditationService;

        public OverseasSiteMaterialController(IAccreditationService accreditationService)
        {
            _accreditationService = accreditationService ?? throw new ArgumentNullException(nameof(accreditationService));
        }

        //[HttpGet("{materialid}")]
        //[ProducesResponseType(typeof(DTO.AccreditationMaterial), 200)]
        //public async Task<IActionResult> GetOverseasSiteMaterial(
        //    Guid id,
        //    Guid overseasSiteId,
        //    Guid materialid)
        //{
        //    var material = await _accreditationService.GetMaterial(
        //        id,
        //        overseasSiteId,
        //        materialid);

        //    if (material == null)
        //        return NotFound();

        //    return Ok(material);
        //}

        //[HttpPost]
        //public async Task<IActionResult> CreateOverseasSiteMaterial(
        //    Guid id,
        //    Guid overseasSiteId,
        //    [FromBody] DTO.AccreditationMaterial accreditationMaterial)
        //{
        //    if (accreditationMaterial == null)
        //        return BadRequest();

        //    var materialId = await _accreditationService.CreateMaterial(
        //        id,
        //        overseasSiteId,
        //        accreditationMaterial);

        //    return Ok(materialId);
        //}

        //[HttpPut("{materialid}")]
        //public async Task<IActionResult> UpdateOverseasSiteMaterial(
        //    Guid id,
        //    Guid overseasSiteId,
        //    Guid materialid,
        //    [FromBody] DTO.AccreditationMaterial accreditationMaterial)
        //{
        //    await _accreditationService.UpdateMaterail(
        //        id,
        //        overseasSiteId,
        //        materialid,
        //        accreditationMaterial);

        //    return Ok();
        //}
    }
}
