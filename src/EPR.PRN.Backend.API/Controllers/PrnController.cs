﻿using Microsoft.AspNetCore.Mvc;
using EPR.PRN.Backend.API.Services.Interfaces;
using DTO = EPR.PRN.Backend.API.Common.DTO;

namespace EPR.PRN.Backend.API.Controllers
{
    public class PrnController : Controller
    {
        protected readonly IPrnService _prnService;

        public PrnController(IPrnService prnService)
        {
            _prnService = prnService ?? throw new ArgumentNullException(nameof(prnService));
        }

        public IActionResult Index()
        {
            return View();
        }

        #region Get methods
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(DTO.PrnDTo), 200)]
        public async Task<IActionResult> GetPrn(int id)
        {
            var prn = await _prnService.GetPrnById(id);

            if (prn == null)
                return NotFound();

            return Ok(prn);
        }

        [HttpGet]
        public async Task<IActionResult> GetSK(int id)
        {
            var prn = await _prnService.GetPrnById(id);

            if (prn == null)
                return NotFound();

            return Ok(prn);
        }
        #endregion
    }
}
