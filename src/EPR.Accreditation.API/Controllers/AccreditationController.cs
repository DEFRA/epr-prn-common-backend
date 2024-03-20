using EPR.Accreditation.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using DTO = EPR.Accreditation.API.Common.Dtos;

namespace EPR.Accreditation.API.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class AccreditationController : ControllerBase
    {
        protected readonly IAccreditationService _accreditationService;

        public AccreditationController(IAccreditationService accreditationService)
        {
            _accreditationService = accreditationService ?? throw new ArgumentNullException(nameof(accreditationService));
        }

        #region Get methods
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(DTO.Accreditation), 200)]
        public async Task<IActionResult> GetAccreditation(Guid id)
        {
            var accreditation = await _accreditationService.GetAccreditation(id);

            if (accreditation == null)
                return NotFound();

            return Ok(accreditation);
        }

        [HttpGet("{id}/TaskProgress")]
        [ProducesResponseType(typeof(List<DTO.AccreditationTaskProgress>), 200)]
        public async Task<IActionResult> GetTaskProgress(Guid id)
        {
            var taskProgress = await _accreditationService.GetTaskProgress(id);

            if (taskProgress == null)
                return NotFound();

            return Ok(taskProgress);
        }

        [HttpGet("{id}/Files")]
        [ProducesResponseType(typeof(List<DTO.FileUpload>), 200)]
        public async Task<IActionResult> GetFileRecords(Guid id)
        {
            var fileUploadRecords = await _accreditationService.GetFileRecords(id);

            if (fileUploadRecords == null ||
                !fileUploadRecords.Any())
                return NotFound();

            return Ok(fileUploadRecords);
        }

        [HttpGet("{id}/Site/{siteExternalId}/Material/{materialExternalId}")]
        [ProducesResponseType(typeof(DTO.AccreditationMaterial), 200)]
        public async Task<IActionResult> GetSiteMaterial(
            Guid id,
            Guid siteExternalId,
            Guid materialExternalId)
        {
            var material = await _accreditationService.GetMaterial(
                id,
                siteExternalId,
                null,
                materialExternalId);

            if (material == null)
                return NotFound();

            return Ok(material);
        }

        [HttpGet("{id}/OverseasSite/{siteExternalId}/Material/{materialExternalId}")]
        [ProducesResponseType(typeof(DTO.AccreditationMaterial), 200)]
        public async Task<IActionResult> GetOverseasSiteMaterial(
            Guid id,
            Guid overseasSiteExternalId,
            Guid materialExternalId)
        {
            var material = await _accreditationService.GetMaterial(
                id,
                null,
                overseasSiteExternalId,
                materialExternalId);

            if (material == null)
                return NotFound();

            return Ok(material);
        }
        #endregion

        #region Post methods
        [HttpPost]
        [ProducesResponseType(typeof(Guid), 200)]
        public async Task<IActionResult> CreateAccredition([FromBody] DTO.Accreditation accreditation)
        {
            if (accreditation == null)
                return BadRequest("Accredition data not suppleid");

            var externalId = await _accreditationService.CreateAccreditation(accreditation);

            return Ok(externalId);
        }

        [HttpPost("{id}/Files")]
        public async Task<IActionResult> AddFile(
            Guid id,
            [FromBody] DTO.FileUpload fileUpload)
        {
            if (fileUpload == null)
                return BadRequest("No file upload record supplied");

            await _accreditationService.AddFile(
                id,
                fileUpload);

            return Ok();
        }

        [HttpPost("{id}/Site/{siteExternalId}/Material")]
        public async Task<IActionResult> CreateSiteMaterial(
            Guid id,
            Guid siteExternalId,
            [FromBody] DTO.AccreditationMaterial accreditationMaterial)
        {
            if (accreditationMaterial == null)
                return BadRequest();

            var materialId = await _accreditationService.CreateMaterial(
                id,
                siteExternalId,
                null,
                accreditationMaterial);

            return Ok(materialId);
        }

        [HttpPost("{id}/OverseasSite/{overseasExternalSiteId}/Material")]
        public async Task<IActionResult> CreateOverseasSiteMaterial(
            Guid id,
            Guid overseasExternalSiteId,
            [FromBody] DTO.AccreditationMaterial accreditationMaterial)
        {
            if (accreditationMaterial == null)
                return BadRequest();

            var materialId = await _accreditationService.CreateMaterial(
                id,
                null,
                overseasExternalSiteId,
                accreditationMaterial);

            return Ok(materialId);
        }
        #endregion

        #region Put methods
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccreditation(
            Guid id,
            [FromBody] DTO.Accreditation accreditation)
        {
            if (accreditation == null)
                return BadRequest();

            if (id != accreditation.ExternalId)
                return BadRequest("External ID does not match");

            await _accreditationService.UpdateAccreditation(
                id,
                accreditation);

            return Ok();
        }

        [HttpPut("{id}/Site/{siteExternalId}/Material/{materialExternalId}")]
        public async Task<IActionResult> UpdateSiteMaterial(
            Guid id,
            Guid siteExternalId,
            Guid materialExternalId,
            [FromBody] DTO.AccreditationMaterial accreditationMaterial)
        {
            if (materialExternalId != accreditationMaterial.ExternalId)
                return BadRequest("External ID does not match");

            await _accreditationService.UpdateMaterail(
                id,
                siteExternalId,
                null,
                materialExternalId,
                accreditationMaterial);

            return Ok();
        }

        [HttpPut("{id}/OverseasSite/{siteExternalId}/Material/{materialExternalId}")]
        public async Task<IActionResult> UpdateOverseasSiteMaterial(
            Guid id,
            Guid siteExternalId,
            Guid materialExternalId,
            [FromBody] DTO.AccreditationMaterial accreditationMaterial)
        {
            if (materialExternalId != accreditationMaterial.ExternalId)
                return BadRequest("External ID does not match");

            await _accreditationService.UpdateMaterail(
                id,
                null,
                siteExternalId,
                materialExternalId,
                accreditationMaterial);

            return Ok();
        }
        #endregion

        #region Delete methods
        [HttpDelete("{id}/Files/{fileId}")]
        public async Task<IActionResult> DeleteFile(Guid id, Guid fileId)
        {
            await _accreditationService.DeleteFile(id, fileId);

            return Ok();
        }
        #endregion
    }
}
