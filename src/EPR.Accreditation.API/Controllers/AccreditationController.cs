﻿namespace EPR.Accreditation.API.Controllers
{
    using EPR.Accreditation.API.Common.Enums;
    using EPR.Accreditation.API.Services.Interfaces;
    using Microsoft.AspNetCore.Mvc;
    using DTO = EPR.Accreditation.API.Common.Dtos;

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

        /// <summary>
        /// Endpoint to get the data for the relevant CYA page
        /// </summary>
        /// <param name="id">Accreditation ID</param>
        /// <param name="materialId">Site material ID</param>
        /// <param name="siteId">Reprocessor site ID</param>
        /// <param name="overseasSiteId">Exporter site ID</param>
        /// <param name="section">Required CYA section</param>
        /// <returns>DTO for relevant CYA page</returns>
        [HttpGet("{id}/Material/{materialId}/CheckAnswers/{section}")]
        [ProducesResponseType(typeof(DTO.CheckAnswers), 200)]
        public async Task<IActionResult> GetCheckAnswers(
            Guid id,
            Guid materialId,
            Guid? siteId,
            Guid? overseasSiteId,
            CheckAnswersSection section)
        {
            var checkYourAnswers = await _accreditationService.GetCheckAnswers(
                id,
                materialId,
                siteId,
                overseasSiteId,
                section);

            if (checkYourAnswers == null)
                return NotFound();

            return Ok(checkYourAnswers);
        }
        #endregion

        #region Post methods
        [HttpPost]
        [ProducesResponseType(typeof(Guid), 200)]
        public async Task<IActionResult> CreateAccreditation([FromBody] DTO.Accreditation accreditation)
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
        #endregion

        #region Put methods
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccreditation(
            Guid id,
            [FromBody] DTO.Accreditation accreditation)
        {
            if (accreditation == null)
                return BadRequest();

            await _accreditationService.UpdateAccreditation(
                id,
                accreditation);

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
