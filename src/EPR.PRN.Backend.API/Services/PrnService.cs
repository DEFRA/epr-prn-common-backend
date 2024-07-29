﻿namespace EPR.PRN.Backend.API.Services
{
    using EPR.PRN.Backend.API.Common.DTO;
    using EPR.PRN.Backend.API.Helpers;
    using EPR.PRN.Backend.API.Repositories.Interfaces;
    using EPR.PRN.Backend.API.Services.Interfaces;
    using EPR.PRN.Backend.Data.DataModels;
    using System.Collections.Generic;

    public class PrnService(IRepository repository) : IPrnService
    {
        protected readonly IRepository _repository = repository;

        public async Task<PrnDto?> GetPrnForOrganisationById(Guid orgId, Guid prnId)
        {
            var prn = await _repository.GetPrnForOrganisationById(orgId, prnId);

            return prn == null ? null : (PrnDto)prn;
        }

        public async Task<List<PrnDto>> GetAllPrnByOrganisationId(Guid orgId)
        {
            return (await _repository.GetAllPrnByOrganisationId(orgId)).Select(x => (PrnDto)x).ToList();
        }

        public async Task UpdateStatus(Guid orgId, List<PrnUpdateStatusDto> prnUpdates)
        {
            using var transaction = _repository.BeginTransaction();
            
            var prns = await _repository.GetAllPrnByOrganisationId(orgId);
            
            if (prns.Count == 0)
                throw new NotFoundException($"No record present for organisation {orgId}");

            var nonExistingPrns = prnUpdates.Select(x => x.PrnId).Except(prns.Select(x => x.ExternalId));

            //makes sure all the prns exsits in system
            if (nonExistingPrns.Any())
                throw new NotFoundException($"{string.Join(",",nonExistingPrns)} Prns doesn't exists in system");

            foreach (var prnUpdate in prnUpdates)
            {
                var prn = prns.Find(x => x.ExternalId == prnUpdate.PrnId);

                if (prn!.PrnStatusId != (int)PrnStatusEnum.AWAITINGACCEPTANCE)
                    throw new ConflictException($"{prnUpdate.PrnId} cannot be accepted or rejected please refresh and try again");

                prn.PrnStatusId = (int)prnUpdate.Status;
            }
            await _repository.SaveTransaction(transaction);
        }
    }
}
