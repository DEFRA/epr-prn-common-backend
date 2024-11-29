namespace EPR.PRN.Backend.API.Services
{
    using EPR.PRN.Backend.API.Common.DTO;
    using EPR.PRN.Backend.API.Helpers;
    using EPR.PRN.Backend.API.Repositories.Interfaces;
    using EPR.PRN.Backend.API.Services.Interfaces;
    using EPR.PRN.Backend.Data.DataModels;
    using System.Collections.Generic;

    public class PrnService : IPrnService
    {
        protected readonly IRepository _repository;
        private readonly ILogger<PrnService> _logger;

        public PrnService(IRepository repository, ILogger<PrnService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<PrnDto?> GetPrnForOrganisationById(Guid orgId, Guid prnId)
        {
            var prn = await _repository.GetPrnForOrganisationById(orgId, prnId);

            return prn == null ? null : (PrnDto)prn;
        }

        public async Task<List<PrnDto>> GetAllPrnByOrganisationId(Guid orgId)
        {
            return (await _repository.GetAllPrnByOrganisationId(orgId)).Select(x => (PrnDto)x).ToList();
        }

        public async Task<List<PrnUpdateStatus>?> GetModifiedPrnsbyDate(DateTime fromDate, DateTime toDate)
        {
            var modifiedPrns = await _repository.GetModifiedPrnsbyDate(fromDate, toDate);

            return modifiedPrns == null ? null : modifiedPrns;
        }

        public async Task<PaginatedResponseDto<PrnDto>> GetSearchPrnsForOrganisation(Guid orgId, PaginatedRequestDto request)
		{
			return await _repository.GetSearchPrnsForOrganisation( orgId,request);
		}

		public async Task UpdateStatus(Guid orgId, Guid userId, List<PrnUpdateStatusDto> prnUpdates)
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

                if (prn!.PrnStatusId == (int)EprnStatus.AWAITINGACCEPTANCE || prn!.PrnStatusId == (int)prnUpdate.Status)
                {
                    UpdatePrn(userId, prnUpdate, prn);
                }
                else
                {
                    throw new ConflictException($"{prnUpdate.PrnId} cannot be accepted or rejected please refresh and try again");
                }
            }
            await _repository.SaveTransaction(transaction);
        }

        private void UpdatePrn(Guid userId, PrnUpdateStatusDto prnUpdate, Eprn prn)
        {
            var updateDate = DateTime.UtcNow;
            var prnStatusHistory = new PrnStatusHistory()
            {
                PrnIdFk = prn.Id,
                PrnStatusIdFk = (int)prnUpdate.Status,
                CreatedOn = updateDate,
                CreatedByUser = userId,
            };

            _repository.AddPrnStatusHistory(prnStatusHistory);

            prn.PrnStatusId = (int)prnUpdate.Status;
            prn.LastUpdatedBy = userId;
            prn.LastUpdatedDate = updateDate;
            prn.StatusUpdatedOn = updateDate;
        }

        public async Task SavePrnDetails(SavePrnDetailsRequest prn)
        {
            try
            {
                Eprn prnEntity = new Eprn()
                {
                    AccreditationNumber = prn.AccreditationNo!,
                    AccreditationYear = prn.AccreditationYear!,
                    // CancelledDate = prn.CancelledDate, // This property /column does not exist on Eprn entity or DB table PRN
                    DecemberWaste = prn.DecemberWaste!.Value,
                    PrnNumber = prn.EvidenceNo!,
                    PrnStatusId = (int)prn.EvidenceStatusCode!.Value,
                    TonnageValue = prn.EvidenceTonnes!.Value,
                    IssueDate = prn.IssueDate!.Value,
                    IssuedByOrg = prn.IssuedByOrgName!,
                    MaterialName = prn.EvidenceMaterial!,
                    OrganisationName = prn.IssuedToOrgName!,
                    OrganisationId = prn.IssuedToEPRId!.Value,
                    IssuerNotes = prn.IssuerNotes,
                    IssuerReference = prn.IssuerRef!,
                    ObligationYear = prn.ObligationYear!,
                    PackagingProducer = string.Empty, // Not defined in NPWD to PRN mapping requirements
                    PrnSignatory = prn.PrnSignatory,
                    PrnSignatoryPosition = prn.PrnSignatoryPosition,
                    ProducerAgency = prn.ProducerAgency!,
                    ProcessToBeUsed = prn.RecoveryProcessCode,
                    ReprocessingSite = prn.ReprocessorAgency,
                    StatusUpdatedOn = prn.StatusDate,
                    LastUpdatedDate = prn.StatusDate!.Value,
                    ExternalId = prn.ExternalId!.Value,
                    ReprocessorExporterAgency = string.Empty,// Not defined in NPWD to PRN mapping requirements
                    Signature = null,  // Not defined in NPWD to PRN mapping requirements
                };

                await _repository.SavePrnDetails(prnEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError(message: ex.Message, exception: ex);
                throw;
            }
        }
    }
}
