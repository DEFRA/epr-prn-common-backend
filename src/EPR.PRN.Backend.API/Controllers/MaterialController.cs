using EPR.PRN.Backend.API.Helpers;
using EPR.PRN.Backend.API.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Net;
using System.Security.Cryptography;
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/prn")]
public class MaterialController() : ControllerBase
{
    private readonly IMediator _mediator;

    public MaterialController(IMediator mediator)
    {
        _mediator = mediator;
    }

    #region Get methods

    [HttpGet("GetStatus")]
   
    public async Task<IActionResult> GetStatus()
    {
       
        return Ok(prn);
    }
    #endregion Get Methods

    #region Post Methods

    [HttpPost("UpdateStatus")]
    public async Task<IActionResult> UpdateStatus([FromBody] UpdateStatus statusData)
    {

    }

    

    #endregion Post Methods
}