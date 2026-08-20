namespace EPR.PRN.Backend.Data.Dto;

public sealed record PrnObligationSummaryDto(
    string MaterialName,
    int AcceptedTonnage,
    int AwaitingAcceptanceTonnage,
    int AwaitingAcceptanceCount
);
