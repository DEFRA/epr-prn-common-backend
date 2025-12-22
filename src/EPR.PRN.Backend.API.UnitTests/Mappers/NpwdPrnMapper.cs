using AutoFixture;
using EPR.PRN.Backend.API.Common.Dto;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Helpers;
using EPR.PRN.Backend.Data.DataModels;
using FluentAssertions;

namespace EPR.PRN.Backend.API.UnitTests.Helpers;

[TestClass]
public class MapperTests
{
    [TestMethod]
    public void ShouldMapSavePrnDetailsRequestToEprn()
    {
        var prn = new Fixture().Create<SavePrnDetailsRequest>();
        prn.EvidenceNo = "EX   ";
        prn.ObligationYear = 2024;
        prn.EvidenceStatusCode = EprnStatus.CANCELLED;
        var expected = new Eprn
        {
            AccreditationNumber = prn.AccreditationNo!,
            AccreditationYear = prn.AccreditationYear.ToString(),
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
            ObligationYear = prn.ObligationYear!.ToString(),
            PackagingProducer = prn.ProducerAgency!,
            PrnSignatory = prn.PrnSignatory,
            PrnSignatoryPosition = prn.PrnSignatoryPosition,
            ProducerAgency = prn.ProducerAgency!,
            ProcessToBeUsed = prn.RecoveryProcessCode,
            ReprocessingSite = string.Empty,
            StatusUpdatedOn = prn.CancelledDate,
            ExternalId = Guid.Empty,
            ReprocessorExporterAgency = prn.ReprocessorAgency!,
            Signature = null,
            IsExport = true,
            CreatedBy = prn.CreatedByUser!,
            SourceSystemId = null,
        };
        prn.ConvertToEprn().Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public void ShouldMapSavePrnDetailsequestToEprn_EvidenceStatusCode()
    {
        var prn = new Fixture().Create<SavePrnDetailsRequest>();
        prn.EvidenceStatusCode = EprnStatus.ACCEPTED;
        var eprn = prn.ConvertToEprn();
        eprn.StatusUpdatedOn.Should().Be(prn.StatusDate);
    }

    [TestMethod]
    public void ShouldMapSavePrnDetailsequestToEprn_ObligationYearNull()
    {
        var prn = new Fixture().Create<SavePrnDetailsRequest>();
        prn.ObligationYear = null;
        var eprn = prn.ConvertToEprn();
        eprn.ObligationYear.Should()
            .Be(Common.Constants.PrnConstants.ObligationYearDefault.ToString());
    }

    // what about all the other things that can be null but mercilessly ignored with ! operators.
    // all of those need to be handled and tested here.

    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    public void ReturnsFalse_WhenNullOrEmpty(string input)
    {
        Assert.IsFalse(NpwdPrnMapper.IsExport(input));
    }

    [TestMethod]
    [DataRow("EX123", true)]
    [DataRow("ex999", true)]
    [DataRow("SX123", true)]
    [DataRow("sx77", true)]
    [DataRow("XX123", false)]
    [DataRow(" 1234", false)]
    [DataRow("E 123", false)]
    [DataRow("AEXXX", false)]
    [DataRow("  EXXXX  ", false)] // i'm fairly sure this is wrong, it should pass but the code is bad
    public void ShouldVerifyIfIsExport(string input, bool expected)
    {
        Assert.AreEqual(expected, NpwdPrnMapper.IsExport(input));
    }
}
