namespace EPR.PRN.Backend.API.Common.DTO
{
    using EPR.PRN.Backend.Data.DataModels;
    using System;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class PrnDto
    {
        public int Id { get; set; }

        public Guid ExternalId { get; set; }

        public string PrnNumber { get; set; } = null!;

        public Guid OrganisationId { get; set; }

        public string OrganisationName { get; set; } = null!;

        public string ProducerAgency { get; set; } = null!;

        public string ReprocessorExporterAgency { get; set; } = null!;

        private int PrnStatusId { get; set; }

        public EprnStatus PrnStatus => (EprnStatus)PrnStatusId;
        public int TonnageValue { get; set; }

        public string MaterialName { get; set; } = null!;

        public string? IssuerNotes { get; set; }

        public string IssuerReference { get; set; } = null!;

        public string? PrnSignatory { get; set; }

        public string? PrnSignatoryPosition { get; set; }

        public string? Signature { get; set; }

        public DateTime IssueDate { get; set; }

        public string? ProcessToBeUsed { get; set; }

        public bool DecemberWaste { get; set; }

        public DateTime? CancelledDate { get; set; }

        public string IssuedByOrg { get; set; } = null!;

        public string AccreditationNumber { get; set; } = null!;

        public string? ReprocessingSite { get; set; }

        public string AccreditationYear { get; set; } = null!;

        public string ObligationYear { get; set; } = null!;

        public string PackagingProducer { get; set; } = null!;

        public string? CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public Guid LastUpdatedBy { get; set; }

        public DateTime LastUpdatedDate { get; set; }

        public bool IsExport { get; set; }

        public static implicit operator PrnDto(EPRN prn)
        {
            return new PrnDto()
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
