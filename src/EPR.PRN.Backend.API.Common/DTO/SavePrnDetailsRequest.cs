namespace EPR.PRN.Backend.API.Common.DTO
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using EPR.PRN.Backend.API.Common.Enums;

    [ExcludeFromCodeCoverage]
    public class SavePrnDetailsRequest
    {
        public string? AccreditationNo { get; set; }
        public int? AccreditationYear { get; set; }
        public DateTime? CancelledDate { get; set; }
        public bool? DecemberWaste { get; set; }
        public string? EvidenceMaterial { get; set; }
        public string? EvidenceNo { get; set; }
        public PrnStatus? EvidenceStatusCode { get; set; }
        public int? EvidenceTonnes { get; set; }
        public DateTime? IssueDate { get; set; }
        public string? IssuedByNPWDCode { get; set; }
        public string? IssuedByOrgName { get; set; }
        public string? IssuedToNPWDCode { get; set; }
        public string? IssuedToOrgName { get; set; }
        public Guid? IssuedToEPRId { get; set; }
        public string? IssuerNotes { get; set; }
        public string? IssuerRef { get; set; }
        public string? MaterialOperationCode { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ObligationYear { get; set; }
        public string? PrnSignatory { get; set; }
        public string? PrnSignatoryPosition { get; set; }
        public string? ProducerAgency { get; set; }
        public string? RecoveryProcessCode { get; set; }
        public string? ReprocessorAgency { get; set; }
        public DateTime? StatusDate { get; set; }
        public Guid? ExternalId { get; set; }
    }
}
