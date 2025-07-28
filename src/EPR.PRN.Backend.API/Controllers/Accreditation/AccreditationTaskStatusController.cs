﻿using EPR.PRN.Backend.API.Commands;
using System.ComponentModel.DataAnnotations;
using EPR.PRN.Backend.API.Common.Constants;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement.Mvc;
using EPR.PRN.Backend.API.Controllers.Regulator;
using FluentValidation;

namespace EPR.PRN.Backend.API.Controllers.Accreditation
{
    [FeatureGate(FeatureFlags.ReprocessorExporter)]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/AccreditationTaskStatus")]
    public class AccreditationTaskStatusController(
        IMediator mediator,
        IValidator<UpdateAccreditationTaskCommand> validator,
        ILogger<AccreditationTaskStatusController> logger)
        : ControllerBase
    {
        [HttpPost()]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateAccreditationTaskStatus([FromBody] UpdateAccreditationTaskCommand command)
        {
            logger.LogInformation(LogMessages.UpdateAccreditationTask);

            await validator.ValidateAndThrowAsync(command);

            await mediator.Send(command);

            return NoContent();
        }
    }
}
