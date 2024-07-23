﻿namespace EPR.PRN.Backend.API.Repositories
{
    using EPR.PRN.Backend.API.Common.DTO;
    using EPR.PRN.Backend.API.Helpers;
    using EPR.PRN.Backend.API.Repositories.Interfaces;
    using EPR.PRN.Backend.Data;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using DTO = EPR.PRN.Backend.API.Common.DTO;

    public class Repository : IRepository
    {
        protected readonly EprContext _eprContext;

        public Repository(
            EprContext eprContext)
        {
            _eprContext = eprContext ?? throw new ArgumentNullException(nameof(EprContext));
        }

        public async Task<DTO.PrnDTo> GetPrnById(int id)
        {
            Data.DataModels.EPRN? prn = await _eprContext
                .Prn
                .Where(a => a.Id == id)
                .Select(a => (a))
                .FirstOrDefaultAsync();
            return prn;
        }

        public async Task<DTO.PrnDTo> GetPrnById(Guid id)
        {
            Data.DataModels.EPRN? prn = await _eprContext
                .Prn
                .Where(a => a.ExternalId == id)
                .Select(a => (a))
                .FirstOrDefaultAsync();
            return prn;
        }

        public async Task<List<DTO.PrnDTo>> GetAllPrnByOrganisationId(Guid id)
        {
            var prns = _eprContext.Prn.Where(x => x.OrganisationId == id).ToList();

            List<DTO.PrnDTo> listOfPrns = new List<DTO.PrnDTo>(prns.Select(x => (PrnDTo)x));

            return listOfPrns;
        }

        public async Task UpdatePrn(Guid id, DTO.PrnDTo prn)
        {
            var entity = await _eprContext
                                    .Prn
                                    .Where(a => a.ExternalId == id)
                                    .FirstOrDefaultAsync();

            if (entity == null)
            {
                throw new NotFoundException($"Prn record not found for ID: {id}");
            }

            entity.PrnStatusId = prn.PrnStatusId;
            await _eprContext.SaveChangesAsync();
        }
    }
}
