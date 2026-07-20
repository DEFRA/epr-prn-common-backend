using System.Diagnostics.CodeAnalysis;
using EPR.PRN.Backend.Data.DataModels;

namespace EPR.PRN.Backend.API.Dto;

[ExcludeFromCodeCoverage]
public abstract class PrnBaseDto
{
    public int Id { get; set; }

    public Guid ExternalId { get; set; }

    public string PrnNumber { get; set; } = null!;

    public Guid OrganisationId { get; set; }

    public string OrganisationName { get; set; } = null!;

    public string ProducerAgency { get; set; } = null!;

    public string ReprocessorExporterAgency { get; set; } = null!;

    public int PrnStatusId { get; set; }

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

    public DateTime? StatusUpdatedOn { get; set; }

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

    public string? SourceSystemId { get; set; }

    protected static T PopulateFromEprn<T>(Eprn prn, T dto)
        where T : PrnBaseDto
    {
        dto.Id = prn.Id;
        dto.ExternalId = prn.ExternalId;
        dto.PrnNumber = prn.PrnNumber;
        dto.OrganisationId = prn.OrganisationId;
        dto.OrganisationName = prn.OrganisationName;
        dto.ProducerAgency = prn.ProducerAgency;
        dto.ReprocessorExporterAgency = prn.ReprocessorExporterAgency;
        dto.PrnStatusId = prn.PrnStatusId;
        dto.TonnageValue = prn.TonnageValue;
        dto.MaterialName = prn.MaterialName;
        dto.IssuerNotes = prn.IssuerNotes;
        dto.IssuerReference = prn.IssuerReference;
        dto.PrnSignatory = prn.PrnSignatory;
        dto.PrnSignatoryPosition = prn.PrnSignatoryPosition;
        dto.Signature = prn.Signature;
        dto.IssueDate = prn.IssueDate;
        dto.ProcessToBeUsed = prn.ProcessToBeUsed;
        dto.DecemberWaste = prn.DecemberWaste;
        dto.StatusUpdatedOn = prn.StatusUpdatedOn;
        dto.IssuedByOrg = prn.IssuedByOrg;
        dto.AccreditationNumber = prn.AccreditationNumber;
        dto.ReprocessingSite = prn.ReprocessingSite;
        dto.AccreditationYear = prn.AccreditationYear;
        dto.ObligationYear = prn.ObligationYear;
        dto.PackagingProducer = prn.PackagingProducer;
        dto.CreatedBy = prn.CreatedBy;
        dto.CreatedOn = prn.CreatedOn;
        dto.LastUpdatedBy = prn.LastUpdatedBy;
        dto.LastUpdatedDate = prn.LastUpdatedDate;
        dto.IsExport = prn.IsExport;
        dto.SourceSystemId = prn.SourceSystemId;

        return dto;
    }
}
