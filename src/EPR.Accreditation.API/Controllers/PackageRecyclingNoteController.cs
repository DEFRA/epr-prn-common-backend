using System.Net;
using EPR.Accreditation.API.Helpers;
using EPR.Accreditation.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using DTO = EPR.Accreditation.API.Common.Dtos;

namespace EPR.Accreditation.API.Controllers
{
    [ApiController]
    [Route("/api/prn")]
    public class PackageRecyclingNoteController : ControllerBase
    {
        protected IPackageRecyclingNoteService PrnService { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PackageRecyclingNoteController"/> class.
        /// </summary>
        /// <param name="prnService"></param>
        public PackageRecyclingNoteController(IPackageRecyclingNoteService prnService)
        {
            ArgumentNullException.ThrowIfNull(prnService);
            this.PrnService = prnService;
        }

        #region Get methods

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(DTO.PackageRecyclingNoteResponse), 200)]
        public async Task<IActionResult> GetPackageRecyclingNote(Guid id)
        {
            ArgumentNullException.ThrowIfNull(id);
            try
            {
                var prn = await this.PrnService.GetPackageRecyclingNote(id);
                if (prn == null)
                {
                    return this.NotFound();
                }

                return this.Ok(prn);
            }
            catch
            {
                return this.StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("organisation/{id}")]
        [ProducesResponseType(typeof(IEnumerable<Guid>), 200)]
        public async Task<IActionResult> GetPrnsForOrganisation(Guid id)
        {
            ArgumentNullException.ThrowIfNull(id);
            try
            {
                var prns = await this.PrnService.GetPrnsForOrganisation(id);

                if (prns == null || !prns.Any())
                {
                    return this.NotFound();
                }

                return this.Ok(prns);
            }
            catch
            {
                return this.StatusCode((int)HttpStatusCode.InternalServerError);
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
                var id = await this.PrnService.CreatePackageRecyclingNote(prn);
                return this.Ok(id);
            }
            catch
            {
                return this.StatusCode((int)HttpStatusCode.InternalServerError);
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
                return this.NotFound();
            }
            catch
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        #endregion

        #region Put methods

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePrn(Guid id, DTO.PackageRecyclingNoteRequest prn)
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
            catch(Exception ex) when(
                ex is NotFoundException ||
                ex is InvalidOperationException)
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
