using System.Diagnostics.CodeAnalysis;
using EPR.PRN.Backend.Data.DataModels;

namespace EPR.PRN.Backend.API.Dto;

[ExcludeFromCodeCoverage]
public class PrnRawDataDto
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

    public List<PrnStatusHistoryRawDataDto> PrnStatusHistories { get; set; } = [];

    public static PrnRawDataDto FromEprn(Eprn prn)
    {
        return new PrnRawDataDto
        {
            Id = prn.Id,
            ExternalId = prn.ExternalId,
            PrnNumber = prn.PrnNumber,
            OrganisationId = prn.OrganisationId,
            OrganisationName = prn.OrganisationName,
            ProducerAgency = prn.ProducerAgency,
            ReprocessorExporterAgency = prn.ReprocessorExporterAgency,
            PrnStatusId = prn.PrnStatusId,
            TonnageValue = prn.TonnageValue,
            MaterialName = prn.MaterialName,
            IssuerNotes = prn.IssuerNotes,
            IssuerReference = prn.IssuerReference,
            PrnSignatory = prn.PrnSignatory,
            PrnSignatoryPosition = prn.PrnSignatoryPosition,
            Signature = prn.Signature,
            IssueDate = prn.IssueDate,
            ProcessToBeUsed = prn.ProcessToBeUsed,
            DecemberWaste = prn.DecemberWaste,
            StatusUpdatedOn = prn.StatusUpdatedOn,
            IssuedByOrg = prn.IssuedByOrg,
            AccreditationNumber = prn.AccreditationNumber,
            ReprocessingSite = prn.ReprocessingSite,
            AccreditationYear = prn.AccreditationYear,
            ObligationYear = prn.ObligationYear,
            PackagingProducer = prn.PackagingProducer,
            CreatedBy = prn.CreatedBy,
            CreatedOn = prn.CreatedOn,
            LastUpdatedBy = prn.LastUpdatedBy,
            LastUpdatedDate = prn.LastUpdatedDate,
            IsExport = prn.IsExport,
            SourceSystemId = prn.SourceSystemId,
        };
    }
}

[ExcludeFromCodeCoverage]
public class PrnStatusHistoryRawDataDto
{
    public int Id { get; set; }

    public DateTime CreatedOn { get; set; }

    public Guid CreatedByUser { get; set; }

    public Guid CreatedByOrganisationId { get; set; }

    public int PrnStatusIdFk { get; set; }

    public int PrnIdFk { get; set; }

    public string? Comment { get; set; }

    public string? ObligationYear { get; set; }

    public static PrnStatusHistoryRawDataDto FromPrnStatusHistory(PrnStatusHistory history)
    {
        return new PrnStatusHistoryRawDataDto
        {
            Id = history.Id,
            CreatedOn = history.CreatedOn,
            CreatedByUser = history.CreatedByUser,
            CreatedByOrganisationId = history.CreatedByOrganisationId,
            PrnStatusIdFk = history.PrnStatusIdFk,
            PrnIdFk = history.PrnIdFk,
            Comment = history.Comment,
            ObligationYear = history.ObligationYear,
        };
    }
}
