using EPR.PRN.Backend.API.Common.Constants;
using EPR.PRN.Backend.API.Common.Dto;
using EPR.PRN.Backend.API.Validators;
using FluentAssertions;
using FluentValidation.Results;

namespace EPR.PRN.Backend.API.UnitTests.Validators;

[TestClass]
public class SavePrnDetailsRequestValidatorV2Tests
{
    private static List<ValidationFailure> Validate(SavePrnDetailsRequestV2 model)
    {
        var validator = new SavePrnDetailsRequestV2Validator();
        return validator.Validate(model).Errors;
    }

    private static SavePrnDetailsRequestV2 CreateValidModel()
    {
        return new SavePrnDetailsRequestV2
        {
            PrnNumber = "PRN123",
            OrganisationId = Guid.NewGuid(),
            OrganisationName = "Org",
            ReprocessorExporterAgency = "Reprocessor",
            PrnStatusId = 1,
            TonnageValue = 2,
            MaterialName = "Plastic",
            IssuerNotes = "Notes",
            PrnSignatory = "Sig",
            PrnSignatoryPosition = "Role",
            DecemberWaste = true,
            StatusUpdatedOn = DateTime.UtcNow,
            IssuedByOrg = "Issuer",
            AccreditationNumber = "ACC123",
            ReprocessingSite = "Site",
            AccreditationYear = "2024",
            IsExport = false,
            SourceSystemId = "SYS"
        };
    }

    [TestMethod]
    public void ShouldAcceptValidModel()
    {
        var model = CreateValidModel();

        var results = Validate(model);

        results.Should().BeEmpty();
    }

    [TestMethod]
    [DataRow("1899")]
    [DataRow("10000")]
    [DataRow("abdf")]
    public void ShouldNotAcceptAccreditationYearWhenInvalid(string value)
    {
        var model = CreateValidModel();
        model.AccreditationYear = value;

        var results = Validate(model);

        var res = results.FirstOrDefault(r => r.PropertyName == nameof(SavePrnDetailsRequestV2.AccreditationYear));
        res.Should().NotBeNull();
        res!.ToString().Should().Be("Accreditation Year must be a valid year value.");
    }

    [TestMethod]
    [DataRow("1899")]
    [DataRow("10000")]
    [DataRow("abdf")]
    public void ShouldAcceptAccreditationYearWhenValid(string year)
    {
        var model = CreateValidModel();
        model.AccreditationYear = year;

        var results = Validate(model);

        results.Should().ContainSingle(r =>
            r.PropertyName == nameof(SavePrnDetailsRequestV2.AccreditationYear));
    }

    [TestMethod]
    public void ShouldNotAcceptRequiredGuidFieldsWhenEmpty()
    {
        var model = CreateValidModel();
        model.OrganisationId = Guid.Empty;

        var results = Validate(model);

        var res = results.FirstOrDefault(r => r.PropertyName == nameof(SavePrnDetailsRequestV2.OrganisationId));
        res.Should().NotBeNull();
        res!.ToString().Should().EndWith(" must be a valid GUID");
    }

    [TestMethod]
    [DataRow(nameof(SavePrnDetailsRequestV2.AccreditationNumber), PrnConstants.MaxLengthAccreditationNumber)]
    [DataRow(nameof(SavePrnDetailsRequestV2.MaterialName), PrnConstants.MaxLengthMaterialName)]
    [DataRow(nameof(SavePrnDetailsRequestV2.PrnNumber), PrnConstants.MaxLengthPrnNumber)]
    [DataRow(nameof(SavePrnDetailsRequestV2.IssuedByOrg), PrnConstants.MaxLengthIssuedByOrg)]
    [DataRow(nameof(SavePrnDetailsRequestV2.OrganisationName), PrnConstants.MaxLengthOrganisationName)]
    [DataRow(nameof(SavePrnDetailsRequestV2.ReprocessorExporterAgency),
        PrnConstants.MaxLengthReprocessorExporterAgency)]
    [DataRow(nameof(SavePrnDetailsRequestV2.IssuerNotes), PrnConstants.MaxLengthIssuerNotes)]
    [DataRow(nameof(SavePrnDetailsRequestV2.PrnSignatory), PrnConstants.MaxLengthPrnSignatory)]
    [DataRow(nameof(SavePrnDetailsRequestV2.PrnSignatoryPosition), PrnConstants.MaxLengthPrnSignatoryPosition)]
    [DataRow(nameof(SavePrnDetailsRequestV2.SourceSystemId), PrnConstants.MaxLengthSourceSystemId)]
    [DataRow(nameof(SavePrnDetailsRequestV2.ReprocessingSite), PrnConstants.MaxLengthReprocessingSite)]
    public void ShouldNotAcceptFieldsWithMaxLengthWhenLengthExceeded(string propertyName, int length)
    {
        var model = CreateValidModel();
        typeof(SavePrnDetailsRequestV2)
            .GetProperty(propertyName)!
            .SetValue(model, new string('x', length + 1));

        var results = Validate(model);

        var res = results.FirstOrDefault(r => r.PropertyName == propertyName);
        res.Should().NotBeNull();
        res!.ToString().Should().EndWith($" cannot be longer than {length} characters.");

        model = CreateValidModel();
        typeof(SavePrnDetailsRequestV2)
            .GetProperty(propertyName)!
            .SetValue(model, new string('2', length));

        results = Validate(model);

        results.Should().BeEmpty();
    }

    [TestMethod]
    [DataRow(nameof(SavePrnDetailsRequestV2.SourceSystemId))]
    [DataRow(nameof(SavePrnDetailsRequestV2.PrnSignatory))]
    [DataRow(nameof(SavePrnDetailsRequestV2.IssuedByOrg))]
    [DataRow(nameof(SavePrnDetailsRequestV2.OrganisationName))]
    [DataRow(nameof(SavePrnDetailsRequestV2.AccreditationNumber))]
    [DataRow(nameof(SavePrnDetailsRequestV2.AccreditationYear))]
    [DataRow(nameof(SavePrnDetailsRequestV2.ReprocessingSite))]
    [DataRow(nameof(SavePrnDetailsRequestV2.MaterialName))]
    public void ShouldNotAcceptMandatoryFieldsWhenEmpty(string propertyName)
    {
        var strings = new List<string> { "", null, "  " };
        var model = CreateValidModel();
        foreach (var s in strings)
        {
            typeof(SavePrnDetailsRequestV2)
                .GetProperty(propertyName)!
                .SetValue(model, s);

            var results = Validate(model);

            var res = results.FirstOrDefault(r => r.PropertyName == propertyName);
            res.Should().NotBeNull();
            res!.ToString().Should().EndWith(" is required.");
        }

        typeof(SavePrnDetailsRequestV2)
            .GetProperty(propertyName)!
            .SetValue(model, "2024");

        var results2 = Validate(model);

        results2.Should().BeEmpty();
    }

    [TestMethod]
    [DataRow(nameof(SavePrnDetailsRequestV2.PrnNumber))]
    [DataRow(nameof(SavePrnDetailsRequestV2.PrnSignatoryPosition))]
    [DataRow(nameof(SavePrnDetailsRequestV2.IssuerNotes))]
    [DataRow(nameof(SavePrnDetailsRequestV2.ReprocessorExporterAgency))]
    public void ShouldAcceptOptionalFieldsWhenEmpty(string propertyName)
    {
        var strings = new List<string> { "", null, "  " };
        var model = CreateValidModel();
        foreach (var s in strings)
        {
            typeof(SavePrnDetailsRequestV2)
                .GetProperty(propertyName)!
                .SetValue(model, s);

            var results = Validate(model);

            results.Should().BeEmpty();
        }
    }

    [TestMethod]
    [DataRow(-1)]
    [DataRow(0)]
    [DataRow(1)]
    public void ShouldOnlyAcceptValidTonnages(int tonnageValue)
    {
        var model = CreateValidModel();
        model.TonnageValue = tonnageValue;

        var results = Validate(model);

        if (tonnageValue < 0)
        {
            var res = results.FirstOrDefault(r => r.PropertyName == nameof(SavePrnDetailsRequestV2.TonnageValue));
            res.Should().NotBeNull();
            res!.ToString().Should().Be("Tonnage Value must be valid positive value.");
        }
        else
        {
            results.Should().BeEmpty();
        }
    }
}