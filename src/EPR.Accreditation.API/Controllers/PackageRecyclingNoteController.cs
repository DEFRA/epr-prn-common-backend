using EPR.Accreditation.API.Helpers;
using EPR.Accreditation.API.Services;
using EPR.Accreditation.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using DTO = EPR.Accreditation.API.Common.Dtos;
using System;

namespace EPR.Accreditation.API.Controllers
{
    [ApiController]
    [Route("/api/prn")]
    public class PackageRecyclingNoteController : ControllerBase
    {
        protected IPackageRecyclingNoteService PrnService { get; }

        public PackageRecyclingNoteController(IPackageRecyclingNoteService prnService)
        {
            ArgumentNullException.ThrowIfNull(prnService);
            PrnService = prnService;
        }

        #region Get methods

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(DTO.PackageRecyclingNoteResponse), 200)]
        public async Task<IActionResult> GetPackageRecyclingNote(Guid id)
        {
            ArgumentNullException.ThrowIfNull(id);
            try
            {
                var prn = await PrnService.GetPackageRecyclingNote(id);
                if(prn == null)
                {
                    return NotFound();
                }
                return Ok(prn);
            }
            catch
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }  
        }

        [HttpGet("organisation/{id}")]
        [ProducesResponseType(typeof(IEnumerable<Guid>), 200)]
        public async Task<IActionResult> GetPrnsForOrganisation(Guid id)
        {
            ArgumentNullException.ThrowIfNull(id);
            try
            {
                var prns = await PrnService.GetPrnsForOrganisation(id);

                if ( prns == null || !prns.Any())
                {
                    return NotFound();
                }
                return Ok(prns);
            }
            catch
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        #endregion

        #region Post methods
        [HttpPost()]
        [ProducesResponseType(typeof(Guid), 200)]
        public async Task<IActionResult> CreatePackageRecyclingNote([FromBody] DTO.PackageRecyclingNoteRequest prn)
        {
            ArgumentNullException.ThrowIfNull(prn);
            try
            {
                var id = await PrnService.CreatePackageRecyclingNote(prn);
                return Ok(id);
            }
            catch
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost("status{id}")]
        public async Task<IActionResult> UpdatePrnStatus(Guid id, DTO.PrnStatusHistoryRequest status)
        {
            ArgumentNullException.ThrowIfNull(id);
            ArgumentNullException.ThrowIfNull(status);
            try
            {
                await PrnService.UpdatePrnStatus(id, status);
                return Ok();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        #endregion

        #region Put methods

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePrn(Guid id, DTO.PrnUpdateRequest prn)
        {
            ArgumentNullException.ThrowIfNull(prn);
            try
            {
                await PrnService.UpdatePrn(id, prn);
                return Ok();
            }
            catch(NotFoundException)
            {
                return NotFound();
            }
            catch
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        #endregion

        #region Delete methods

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePrn(Guid id)
        {
            ArgumentNullException.ThrowIfNull(id);
            try
            {
                await PrnService.DeletePrn(id);
                return Ok();
            }
            catch(NotFoundException)
            {
                return StatusCode((int)HttpStatusCode.PreconditionFailed);
            }
            catch
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        #endregion
    }
}
