using System;
using EPR.PRN.Backend.Data.DataModels;

namespace EPR.PRN.Backend.API.Common.DTO
{
    public class PrnDTo
    {
        public int Id { get; set; }

        public Guid ExternalId { get; set; }

        public required string PrnNumber { get; set; }

        public Guid OrganisationId { get; set; }

        public string OrganisationName { get; set; }

        public string ProducerAgency { get; set; }

        public string ReprocessorExporterAgency { get; set; }

        public int PrnStatusId { get; set; }

        public int TonnageValue { get; set; }

        public required string MaterialName { get; set; }

        public string? IssuerNotes { get; set; }

        public string IssuerReference { get; set; }

        public string? PrnSignatory { get; set; }

        public string? PrnSignatoryPosition { get; set; }

        public string? Signature { get; set; }

        public DateTime IssueDate { get; set; }

        public string? ProcessToBeUsed { get; set; }

        public bool DecemberWaste { get; set; }

        public DateTime? CancelledDate { get; set; }

        public string IssuedByOrg { get; set; }

        public string AccreditationNumber { get; set; }

        public string? ReprocessingSite { get; set; }

        public string AccreditationYear { get; set; }

        public string ObligationYear { get; set; }

        public string PackagingProducer { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public Guid LastUpdatedBy { get; set; }

        public DateTime LastUpdatedDate { get; set; }

        public bool IsExport { get; set; }

        public static implicit operator PrnDTo(EPRN prn)
        {
            return new PrnDTo
            {
                Id = prn.Id,
                AccreditationNumber = prn.AccreditationNumber,
                CancelledDate = prn.CancelledDate,
                CreatedOn = prn.CreatedOn,
                AccreditationYear = prn.AccreditationYear,
                MaterialName = prn.MaterialName,
                PrnNumber = prn.PrnNumber,
                DecemberWaste = prn.DecemberWaste,
                CreatedBy = prn.CreatedBy,
                ExternalId = prn.ExternalId,
                IsExport = prn.IsExport,
                IssueDate = prn.IssueDate,
                IssuedByOrg = prn.IssuedByOrg,
                IssuerNotes = prn.IssuerNotes,
                IssuerReference = prn.IssuerReference,
                LastUpdatedBy = prn.LastUpdatedBy,
                LastUpdatedDate = prn.LastUpdatedDate,
                ObligationYear = prn.ObligationYear,
                OrganisationId = prn.OrganisationId,
                OrganisationName = prn.OrganisationName,
                PackagingProducer = prn.PackagingProducer,
                PrnSignatory = prn.PrnSignatory,
                PrnSignatoryPosition = prn.PrnSignatoryPosition,
                PrnStatusId = prn.PrnStatusId,
                ProcessToBeUsed = prn.ProcessToBeUsed,
                ProducerAgency = prn.ProducerAgency,
                ReprocessingSite = prn.ReprocessingSite,
                ReprocessorExporterAgency = prn.ReprocessorExporterAgency,
                Signature = prn.Signature,
                TonnageValue = prn.TonnageValue
            };
        }
    }
}
