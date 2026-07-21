using System.Diagnostics.CodeAnalysis;
using EPR.PRN.Backend.Data.DataModels;

namespace EPR.PRN.Backend.API.Dto;

[ExcludeFromCodeCoverage]
public class PrnRawDataDto : PrnBaseDto
{
    public List<PrnStatusHistoryRawDataDto> PrnStatusHistories { get; set; } = [];

    public static PrnRawDataDto FromEprn(Eprn prn)
    {
        return PopulateFromEprn(prn, new PrnRawDataDto());
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
