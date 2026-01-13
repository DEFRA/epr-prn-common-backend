using EPR.PRN.Backend.API.Common.Dto;
using EPR.PRN.Backend.API.Common.Enums;
using EprPrnIntegration.Common.Models.Rpd;

public class DataGenerator
{
    public static SaveNpwdPrnDetailsRequest CreateValidSaveNpwdPrnDetailsRequest()
    {
        return new SaveNpwdPrnDetailsRequest()
        {
            AccreditationNo = "ABC",
            AccreditationYear = 2018,
            CancelledDate = DateTime.UtcNow.AddDays(-1),
            DecemberWaste = true,
            EvidenceMaterial = "Aluminium",
            EvidenceNo = Guid.NewGuid().ToString(),
            EvidenceStatusCode = EprnStatus.AWAITINGACCEPTANCE,
            EvidenceTonnes = 5000,
            IssueDate = DateTime.UtcNow.AddDays(-5),
            IssuedByNPWDCode = "NPWD367742",
            IssuedByOrgName = "ANB",
            IssuedToEPRId = Guid.NewGuid(),
            IssuedToNPWDCode = "NPWD557742",
            IssuedToOrgName = "ZNZ",
            IssuerNotes = "no notes",
            IssuerRef = "ANB-1123",
            MaterialOperationCode = "R-PLA",
            ObligationYear = 2025,
            PrnSignatory = "Pat Anderson",
            PrnSignatoryPosition = "Director",
            ProducerAgency = "TTL",
            RecoveryProcessCode = "N11",
            ReprocessorAgency = "BEX",
            StatusDate = DateTime.UtcNow,
        };
    }

    public static SavePrnDetailsRequest CreateValidSavePrnDetailsRequest()
    {
        return new SavePrnDetailsRequest
        {
            PrnNumber = "PRN123",
            OrganisationId = Guid.NewGuid(),
            OrganisationName = "Org",
            PackagingProducer = AgencyName.EnvironmentAgency,
            ProducerAgency = AgencyName.EnvironmentAgency,
            ReprocessorExporterAgency = AgencyName.EnvironmentAgency,
            PrnStatusId = (int)EprnStatus.AWAITINGACCEPTANCE,
            TonnageValue = 0,
            MaterialName = RpdMaterialName.Aluminium,
            IssuerNotes = "Notes",
            PrnSignatory = "Sig",
            PrnSignatoryPosition = "Role",
            DecemberWaste = true,
            StatusUpdatedOn = DateTime.UtcNow.AddHours(1),
            IssueDate = DateTime.UtcNow.AddHours(2),
            IssuedByOrg = "Issuer",
            AccreditationNumber = "ACC123",
            ReprocessingSite = "Site",
            AccreditationYear = "2024",
            IsExport = true,
            SourceSystemId = "SYS",
            ProcessToBeUsed = RpdProcesses.R3,
            ObligationYear = "2025",
        };
    }
}
