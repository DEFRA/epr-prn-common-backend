using System.ComponentModel.DataAnnotations;
using AutoFixture;
using EPR.PRN.Backend.API.Common.Constants;
using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Data.Helpers;
using FluentAssertions;

namespace EPR.PRN.Backend.Data.UnitTests.Helpers;

[TestClass]
public class EprnTests
{
    private Fixture _fixture = new();

    [TestMethod]
    public void ShouldNotTruncateEprnStringsWhenNotTooLong()
    {
        var eprn = _fixture.Create<Eprn>();
        var (truncatedEprn, truncatedFields) = eprn.CreateCopyWithTruncatedStrings();
        truncatedEprn.Should().BeEquivalentTo(eprn);
        truncatedFields.Should().BeEmpty();
    }

    [TestMethod]
    [DataRow(nameof(Eprn.PrnSignatory), PrnConstants.MaxLengthPrnSignatory)]
    [DataRow(nameof(Eprn.IssuedByOrg), PrnConstants.MaxLengthIssuedByOrg)]
    [DataRow(nameof(Eprn.OrganisationName), PrnConstants.MaxLengthOrganisationName)]
    [DataRow(nameof(Eprn.AccreditationNumber), PrnConstants.MaxLengthAccreditationNumber)]
    [DataRow(nameof(Eprn.ReprocessingSite), PrnConstants.MaxLengthReprocessingSite)]
    [DataRow(nameof(Eprn.IssuerNotes), PrnConstants.MaxLengthIssuerNotes)]
    [DataRow(nameof(Eprn.ProducerAgency), PrnConstants.MaxLengthProducerAgency)]
    [DataRow(nameof(Eprn.IssuerReference), PrnConstants.MaxLengthIssuerReference)]
    [DataRow(nameof(Eprn.PrnSignatoryPosition), PrnConstants.MaxLengthPrnSignatoryPosition)]
    [DataRow(nameof(Eprn.Signature), PrnConstants.MaxLengthSignature)]
    [DataRow(nameof(Eprn.PackagingProducer), PrnConstants.MaxLengthPackagingProducer)]
    [DataRow(nameof(Eprn.CreatedBy), PrnConstants.MaxLengthCreatedBy)]
    public void ShouldTruncateSingleEprnStringProperty(string propertyName, int maxLength)
    {
        var eprn = _fixture.Create<Eprn>();
        var overlongValue = new string('A', maxLength + 10);

        typeof(Eprn).GetProperty(propertyName).SetValue(eprn, overlongValue);

        var (truncatedEprn, truncatedFields) = eprn.CreateCopyWithTruncatedStrings();
        var resultValue = (string)typeof(Eprn).GetProperty(propertyName).GetValue(truncatedEprn);

        resultValue.Should().NotBeNull();
        resultValue!.Length.Should().Be(maxLength);
        resultValue[..(maxLength - 3)].All(c => c == 'A').Should().BeTrue();
        resultValue.Should().EndWith("...");
        truncatedFields.Should().ContainSingle().Which.Should().Be(propertyName);
    }

    [TestMethod]
    [DataRow(nameof(Eprn.SourceSystemId), PrnConstants.MaxLengthSourceSystemId)]
    [DataRow(nameof(Eprn.PrnNumber), PrnConstants.MaxLengthPrnNumber)]
    [DataRow(nameof(Eprn.AccreditationYear), PrnConstants.MaxLengthAccreditationYear)]
    [DataRow(nameof(Eprn.MaterialName), PrnConstants.MaxLengthMaterialName)]
    [DataRow(
        nameof(Eprn.ReprocessorExporterAgency),
        PrnConstants.MaxLengthReprocessorExporterAgency
    )]
    [DataRow(nameof(Eprn.ProcessToBeUsed), PrnConstants.MaxLengthProcessToBeUsed)]
    [DataRow(nameof(Eprn.ObligationYear), PrnConstants.MaxLengthObligationYear)]
    public void ShouldNotTruncateExcludedProperties(string propertyName, int maxLength)
    {
        var eprn = _fixture.Create<Eprn>();
        var overlongValue = new string('A', maxLength + 10);

        typeof(Eprn).GetProperty(propertyName).SetValue(eprn, overlongValue);

        var (truncatedEprn, truncatedFields) = eprn.CreateCopyWithTruncatedStrings();
        var resultValue = (string)typeof(Eprn).GetProperty(propertyName).GetValue(truncatedEprn);

        resultValue.Should().Be(overlongValue);
        truncatedFields.Should().BeEmpty();
    }
}
