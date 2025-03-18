using EPR.PRN.Backend.API.Dto;
using EPR.PRN.Backend.API.Helpers;
using EPR.PRN.Backend.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EPR.PRN.Backend.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/registration")]
    public class RegistrationController(IRegistrationService registrationService, ILogger<RegistrationController> logger) : Controller
    {
        [HttpGet]
        [Route("{id:int}")]
        [ProducesResponseType(typeof(RegistrationDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetByRegistrationId(int id)
        {
            try
            {
                var registration = await registrationService.GetByIdAsync(id);

                return Ok(registration);
            }
            catch (NotFoundException ex)
            {
                logger.LogError(ex, "RegistrationController - GetByRegistrationId id: {id}: Recieved Unhandled exception", id);
                return Problem(ex.Message, null, (int)HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "RegistrationController - GetByRegistrationId id: {id}: Recieved Unhandled exception", id);
                return Problem("Internal Server Error", null, (int)HttpStatusCode.InternalServerError);
            }     
        }
    }
}
