using EPR.PRN.Backend.API.Dto;
using EPR.PRN.Backend.API.Models;
using EPR.PRN.Backend.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EPR.PRN.Backend.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/saveandcontinue")]
    public class SaveAndContinueController(ISaveAndContinueService saveAndContinueService, ILogger<SaveAndContinueController> logger) : Controller
    {
        [HttpPost]
        [Route("Save")]
        [ProducesResponseType(typeof(RegistrationDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Save([FromBody] SaveAndContinueRequest request)
        {
            try
            {
                await saveAndContinueService.AddAsync(request.RegistrationId, request.Area, request.Action, request.Controller, request.Parameters);

                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "SaveAndContinueController - AddAsync: {request}: Recieved Unhandled exception", request);
                return Problem("Internal Server Error", null, (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
