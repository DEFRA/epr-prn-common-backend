using System.ComponentModel.DataAnnotations;
using EPR.PRN.Backend.API.Common.Constants;

namespace EPR.PRN.Backend.Data.DataModels
{
	public class Eprn
{


    [Key]
    public int Id { get; set; }

    [Required]
    public Guid ExternalId { get; set; }

    [MaxLength(PrnConstants.MaxLengthPrnNumber)]
    public required string PrnNumber { get; set; }

    [Required]
    public Guid OrganisationId { get; set; }

    [Required, MaxLength(PrnConstants.MaxLengthOrganisationName)]
    public required string OrganisationName { get; set; }

    [MaxLength(PrnConstants.MaxLengthProducerAgency)]
    public required string ProducerAgency { get; set; }

    [MaxLength(PrnConstants.MaxLengthReprocessorExporterAgency)]
    public required string ReprocessorExporterAgency { get; set; }

    public int PrnStatusId { get; set; }

    public int TonnageValue { get; set; }

    [MaxLength(PrnConstants.MaxLengthMaterialName)]
    public required string MaterialName { get; set; }

    [MaxLength(PrnConstants.MaxLengthIssuerNotes)]
    public string? IssuerNotes { get; set; }

    [MaxLength(PrnConstants.MaxLengthIssuerReference)]
    public required string IssuerReference { get; set; }

    [MaxLength(PrnConstants.MaxLengthPrnSignatory)]
    public string? PrnSignatory { get; set; }

    [MaxLength(PrnConstants.MaxLengthPrnSignatoryPosition)]
    public string? PrnSignatoryPosition { get; set; }

    [MaxLength(PrnConstants.MaxLengthSignature)]
    public string? Signature { get; set; }

    public DateTime IssueDate { get; set; }

    [MaxLength(PrnConstants.MaxLengthProcessToBeUsed)]
    public string? ProcessToBeUsed { get; set; }

    public bool DecemberWaste { get; set; }

    public DateTime? StatusUpdatedOn { get; set; }

    [MaxLength(PrnConstants.MaxLengthIssuedByOrg)]
    public required string IssuedByOrg { get; set; }

    [MaxLength(PrnConstants.MaxLengthAccreditationNumber)]
    public required string AccreditationNumber { get; set; }

    [MaxLength(PrnConstants.MaxLengthReprocessingSite)]
    public required string? ReprocessingSite { get; set; }

    [MaxLength(PrnConstants.MaxLengthAccreditationYear)]
    public required string AccreditationYear { get; set; }

    [MaxLength(PrnConstants.MaxLengthObligationYear)]
    public required string ObligationYear { get; set; }

    [MaxLength(PrnConstants.MaxLengthPackagingProducer)]
    public required string PackagingProducer { get; set; }

    [MaxLength(PrnConstants.MaxLengthCreatedBy)]
    public string? CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    [Required]
    public Guid LastUpdatedBy { get; set; }

    public DateTime LastUpdatedDate { get; set; }

    public bool IsExport { get; set; }

    public virtual ICollection<PrnStatusHistory> PrnStatusHistories { get; set; } = null!;

    [MaxLength(PrnConstants.MaxLengthSourceSystemId)]
    public string? SourceSystemId { get; set; } = null;
}

}