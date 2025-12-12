using EPR.PRN.Backend.API.Common.Constants;
using EPR.PRN.Backend.API.Common.Dto;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.DataModels;

namespace EPR.PRN.Backend.API.Helpers;

// todo this should use AutoMapper
public static class Mappers
{
    public static Eprn ConvertToEprn(this SavePrnDetailsRequest prn)
    {
        return new Eprn
        {
            AccreditationNumber = prn.AccreditationNo!,
            AccreditationYear = prn.AccreditationYear.ToString()!,
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
            ObligationYear = prn.ObligationYear?.ToString() ?? PrnConstants.ObligationYearDefault.ToString(),
            PackagingProducer = prn.ProducerAgency!,
            PrnSignatory = prn.PrnSignatory,
            PrnSignatoryPosition = prn.PrnSignatoryPosition,
            ProducerAgency = prn.ProducerAgency!,
            ProcessToBeUsed = prn.RecoveryProcessCode,
            ReprocessingSite = string.Empty,
            StatusUpdatedOn = prn.EvidenceStatusCode == EprnStatus.CANCELLED ? prn.CancelledDate : prn.StatusDate,
            ExternalId = Guid.Empty, // set value in repo when inserting and set to new guid
            ReprocessorExporterAgency = prn.ReprocessorAgency!,
            Signature = null, 
            IsExport = IsExport(prn.EvidenceNo),
            CreatedBy = prn.CreatedByUser!,
            SourceSystemId = prn.SourceSystemId
        };
    }

    public static bool IsExport(string? evidenceNo)
    {
        if (string.IsNullOrEmpty(evidenceNo)) return false;

        // this will fail with an unhandled exception if the string is < 2 in length but not empty
        // this whole functon should be Trim then StartsWith
        var val = evidenceNo[..2].Trim();

        return string.Equals(val, PrnConstants.ExporterCodePrefixes.EaExport,
                   StringComparison.InvariantCultureIgnoreCase)
               || string.Equals(val, PrnConstants.ExporterCodePrefixes.SepaExport,
                   StringComparison.InvariantCultureIgnoreCase);
    }
}