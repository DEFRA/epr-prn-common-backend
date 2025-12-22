using EPR.PRN.Backend.API.Common.Constants;
using EPR.PRN.Backend.API.Common.Dto;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Validators;
using EprPrnIntegration.Common.Models.Rpd;
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

    [TestMethod]
    public void ShouldAcceptValidModel()
    {
        var model = DataGenerator.CreateValidSavePrnDetailsRequestV2();

        var results = Validate(model);

        results.Should().BeEmpty();
    }

    [TestMethod]
    [DataRow(1000, false)]
    [DataRow(null, false)]
    [DataRow((int)EprnStatus.CANCELLED, true)]
    [DataRow((int)EprnStatus.AWAITINGACCEPTANCE, true)]
    [DataRow((int)EprnStatus.ACCEPTED, false)]
    [DataRow((int)EprnStatus.REJECTED, false)]
    public void ShouldOnlyAcceptValidPrnStatusId(int? value, bool valid)
    {
        var model = DataGenerator.CreateValidSavePrnDetailsRequestV2();
        model.PrnStatusId = value;

        var results = Validate(model);
        if (valid)
            results.Should().BeEmpty();
        else
        {
            var res = results.FirstOrDefault(r =>
                r.PropertyName == nameof(SavePrnDetailsRequestV2.PrnStatusId)
            );
            res.Should().NotBeNull();
            res!.ToString().Should().Be("Prn Status Id must be one of 3, 4.");
        }
    }

    [TestMethod]
    [DataRow("1900")]
    [DataRow("10000")]
    [DataRow("abdf")]
    [DataRow(null)]
    public void ShouldNotAcceptAccreditationYearWhenInvalid(string value)
    {
        var model = DataGenerator.CreateValidSavePrnDetailsRequestV2();
        model.AccreditationYear = value;

        var results = Validate(model);

        var res = results.FirstOrDefault(r =>
            r.PropertyName == nameof(SavePrnDetailsRequestV2.AccreditationYear)
        );
        res.Should().NotBeNull();
        res!.ToString().Should().Be("Accreditation Year must be a valid year value.");
    }

    [TestMethod]
    [DataRow("1901")]
    [DataRow("9999")]
    [DataRow("2024")]
    public void ShouldAcceptAccreditationYearWhenValid(string year)
    {
        var model = DataGenerator.CreateValidSavePrnDetailsRequestV2();
        model.AccreditationYear = year;

        var results = Validate(model);

        results.Should().BeEmpty();
    }

    [TestMethod]
    [DataRow("1900")]
    [DataRow("10000")]
    [DataRow("abdf")]
    [DataRow(null)]
    public void ShouldNotAcceptObligationYearWhenInvalid(string value)
    {
        var model = DataGenerator.CreateValidSavePrnDetailsRequestV2();
        model.ObligationYear = value;

        var results = Validate(model);

        var res = results.FirstOrDefault(r =>
            r.PropertyName == nameof(SavePrnDetailsRequestV2.ObligationYear)
        );
        res.Should().NotBeNull();
        res!.ToString().Should().Be("Obligation Year must be a valid year value.");
    }

    [TestMethod]
    [DataRow("1901")]
    [DataRow("9999")]
    [DataRow("2024")]
    public void ShouldAcceptObligationYearWhenValid(string year)
    {
        var model = DataGenerator.CreateValidSavePrnDetailsRequestV2();
        model.ObligationYear = year;

        var results = Validate(model);

        results.Should().BeEmpty();
    }

    [TestMethod]
    public void ShouldNotAcceptRequiredGuidFieldsWhenEmpty()
    {
        var model = DataGenerator.CreateValidSavePrnDetailsRequestV2();
        model.OrganisationId = Guid.Empty;

        var results = Validate(model);

        var res = results.FirstOrDefault(r =>
            r.PropertyName == nameof(SavePrnDetailsRequestV2.OrganisationId)
        );
        res.Should().NotBeNull();
        res!.ToString().Should().EndWith(" must be a valid GUID");
    }

    [TestMethod]
    [DataRow(RpdMaterialName.Aluminium, true)]
    [DataRow(RpdMaterialName.Fibre, true)]
    [DataRow(RpdMaterialName.GlassRemelt, true)]
    [DataRow(RpdMaterialName.GlassOther, true)]
    [DataRow(RpdMaterialName.PaperBoard, true)]
    [DataRow(RpdMaterialName.Plastic, true)]
    [DataRow(RpdMaterialName.Steel, true)]
    [DataRow(RpdMaterialName.Wood, true)]
    [DataRow("InvalidMaterial", false)]
    [DataRow(null, false)]
    public void ShouldOnlyAcceptValidMaterialNames(string materialName, bool valid)
    {
        var model = DataGenerator.CreateValidSavePrnDetailsRequestV2();
        model.MaterialName = materialName;

        var results = Validate(model);
        if (valid)
            results.Should().BeEmpty();
        else
        {
            var res = results.FirstOrDefault(r =>
                r.PropertyName == nameof(SavePrnDetailsRequestV2.MaterialName)
            );
            res.Should().NotBeNull();
            res!
                .ToString()
                .Should()
                .Be(
                    "Material Name must be one of Aluminium, Fibre, Glass Re-melt, Glass Other, Paper/board, Plastic, Steel, Wood."
                );
        }
    }

    [TestMethod]
    [DataRow(RpdReprocessorExporterAgency.EnvironmentAgency, true)]
    [DataRow(RpdReprocessorExporterAgency.NaturalResourcesWales, true)]
    [DataRow(RpdReprocessorExporterAgency.NorthernIrelandEnvironmentAgency, true)]
    [DataRow(RpdReprocessorExporterAgency.ScottishEnvironmentProtectionAge, true)]
    [DataRow("Invalid", false)]
    [DataRow(null, false)]
    public void ShouldOnlyAcceptValidRpdReprocessorExporterAgency(string rea, bool valid)
    {
        var model = DataGenerator.CreateValidSavePrnDetailsRequestV2();
        model.ReprocessorExporterAgency = rea;

        var results = Validate(model);
        if (valid)
            results.Should().BeEmpty();
        else
        {
            var res = results.FirstOrDefault(r =>
                r.PropertyName == nameof(SavePrnDetailsRequestV2.ReprocessorExporterAgency)
            );
            res.Should().NotBeNull();
            res!
                .ToString()
                .Should()
                .Be(
                    "Reprocessor Exporter Agency must be one of Environment Agency, Natural Resources Wales, Northern Ireland Environment Agency, Scottish Environment Protection Age."
                );
        }
    }

    [TestMethod]
    [DataRow(RpdProcesses.R3, true)]
    [DataRow(RpdProcesses.R4, true)]
    [DataRow(RpdProcesses.R5, true)]
    [DataRow("Invalid", false)]
    [DataRow(null, false)]
    public void ShouldOnlyAcceptValidProcessesToBeUsed(string processToBeUsed, bool valid)
    {
        var model = DataGenerator.CreateValidSavePrnDetailsRequestV2();
        model.ProcessToBeUsed = processToBeUsed;

        var results = Validate(model);
        if (valid)
            results.Should().BeEmpty();
        else
        {
            var res = results.FirstOrDefault(r =>
                r.PropertyName == nameof(SavePrnDetailsRequestV2.ProcessToBeUsed)
            );
            res.Should().NotBeNull();
            res!.ToString().Should().Be("Process To Be Used must be one of R3, R4, R5.");
        }
    }

    [TestMethod]
    public void ShouldEnforceReprocessingSiteIfPRN()
    {
        var model = DataGenerator.CreateValidSavePrnDetailsRequestV2();
        // this means PRN
        model.IsExport = false;
        var strings = new List<string> { "", "  ", null };
        foreach (var s in strings)
        {
            model.ReprocessingSite = s;
            var results = Validate(model);
            var res = results.FirstOrDefault(r =>
                r.PropertyName == nameof(SavePrnDetailsRequestV2.ReprocessingSite)
            );
            res.Should().NotBeNull();
            res!.ToString().Should().Be("Reprocessing Site is required.");
        }
        model.ReprocessingSite = "Valid Site";
        Validate(model).Should().BeEmpty();
    }

    [TestMethod]
    public void ShouldNotEnforceReprocessingSiteIfNotPRN()
    {
        var model = DataGenerator.CreateValidSavePrnDetailsRequestV2();
        // this means not PRN
        model.IsExport = true;
        var strings = new List<string> { "", "  ", "Valid Site", null };
        foreach (var s in strings)
        {
            model.ReprocessingSite = s;
            Validate(model).Should().BeEmpty();
        }
    }

    [TestMethod]
    [DataRow(nameof(SavePrnDetailsRequestV2.PrnNumber), PrnConstants.MaxLengthPrnNumber)]
    [DataRow(nameof(SavePrnDetailsRequestV2.SourceSystemId), PrnConstants.MaxLengthSourceSystemId)]
    public void ShouldNotAcceptFieldsWithMaxLengthWhenLengthExceeded(
        string propertyName,
        int length
    )
    {
        var model = DataGenerator.CreateValidSavePrnDetailsRequestV2();
        typeof(SavePrnDetailsRequestV2)
            .GetProperty(propertyName)!
            .SetValue(model, new string('x', length + 1));

        var results = Validate(model);

        var res = results.FirstOrDefault(r => r.PropertyName == propertyName);
        res.Should().NotBeNull();
        res!.ToString().Should().EndWith($" cannot be longer than {length} characters.");

        model = DataGenerator.CreateValidSavePrnDetailsRequestV2();
        typeof(SavePrnDetailsRequestV2)
            .GetProperty(propertyName)!
            .SetValue(model, new string('2', length));

        results = Validate(model);

        results.Should().BeEmpty();
    }

    [TestMethod]
    [DataRow(nameof(SavePrnDetailsRequestV2.SourceSystemId))]
    [DataRow(nameof(SavePrnDetailsRequestV2.PrnNumber))]
    [DataRow(nameof(SavePrnDetailsRequestV2.PrnSignatory))]
    [DataRow(nameof(SavePrnDetailsRequestV2.StatusUpdatedOn))]
    [DataRow(nameof(SavePrnDetailsRequestV2.IssuedByOrg))]
    [DataRow(nameof(SavePrnDetailsRequestV2.OrganisationId))]
    [DataRow(nameof(SavePrnDetailsRequestV2.OrganisationName))]
    [DataRow(nameof(SavePrnDetailsRequestV2.AccreditationNumber))]
    [DataRow(nameof(SavePrnDetailsRequestV2.DecemberWaste))]
    [DataRow(nameof(SavePrnDetailsRequestV2.TonnageValue))]
    [DataRow(nameof(SavePrnDetailsRequestV2.IsExport))]
    public void ShouldNotAcceptMandatoryFieldsWhenNull(string propertyName)
    {
        var model = DataGenerator.CreateValidSavePrnDetailsRequestV2();
        typeof(SavePrnDetailsRequestV2).GetProperty(propertyName)!.SetValue(model, null);

        var results = Validate(model);

        var res = results.FirstOrDefault(r => r.PropertyName == propertyName);
        res.Should().NotBeNull();
        res!.ToString().Should().EndWith(" is required.");
    }

    [TestMethod]
    [DataRow(nameof(SavePrnDetailsRequestV2.PrnSignatoryPosition))]
    [DataRow(nameof(SavePrnDetailsRequestV2.ReprocessingSite))]
    [DataRow(nameof(SavePrnDetailsRequestV2.IssuerNotes))]
    public void ShouldAcceptOptionalFieldsWhenNull(string propertyName)
    {
        var model = DataGenerator.CreateValidSavePrnDetailsRequestV2();
        typeof(SavePrnDetailsRequestV2).GetProperty(propertyName)!.SetValue(model, null);

        Validate(model).Should().BeEmpty();
    }

    [TestMethod]
    [DataRow(nameof(SavePrnDetailsRequestV2.SourceSystemId))]
    [DataRow(nameof(SavePrnDetailsRequestV2.PrnSignatory))]
    [DataRow(nameof(SavePrnDetailsRequestV2.IssuedByOrg))]
    [DataRow(nameof(SavePrnDetailsRequestV2.OrganisationName))]
    [DataRow(nameof(SavePrnDetailsRequestV2.AccreditationNumber))]
    [DataRow(nameof(SavePrnDetailsRequestV2.PrnNumber))]
    public void ShouldNotAcceptMandatoryStringFieldsWhenEmpty(string propertyName)
    {
        var strings = new List<string> { "", "  " };
        var model = DataGenerator.CreateValidSavePrnDetailsRequestV2();
        foreach (var s in strings)
        {
            typeof(SavePrnDetailsRequestV2).GetProperty(propertyName)!.SetValue(model, s);

            var results = Validate(model);

            var res = results.FirstOrDefault(r => r.PropertyName == propertyName);
            res.Should().NotBeNull();
            res!.ToString().Should().EndWith(" is required.");
        }

        typeof(SavePrnDetailsRequestV2).GetProperty(propertyName)!.SetValue(model, "2024");

        var results2 = Validate(model);

        results2.Should().BeEmpty();
    }

    [TestMethod]
    [DataRow(nameof(SavePrnDetailsRequestV2.PrnSignatoryPosition))]
    [DataRow(nameof(SavePrnDetailsRequestV2.IssuerNotes))]
    [DataRow(nameof(SavePrnDetailsRequestV2.ReprocessingSite))]
    public void ShouldAcceptOptionalFieldsWhenEmpty(string propertyName)
    {
        var strings = new List<string> { "", null, "  " };
        var model = DataGenerator.CreateValidSavePrnDetailsRequestV2();
        foreach (var s in strings)
        {
            typeof(SavePrnDetailsRequestV2).GetProperty(propertyName)!.SetValue(model, s);

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
        var model = DataGenerator.CreateValidSavePrnDetailsRequestV2();
        model.TonnageValue = tonnageValue;

        var results = Validate(model);

        if (tonnageValue < 0)
        {
            var res = results.FirstOrDefault(r =>
                r.PropertyName == nameof(SavePrnDetailsRequestV2.TonnageValue)
            );
            res.Should().NotBeNull();
            res!.ToString().Should().Be("Tonnage Value must be valid positive value.");
        }
        else
        {
            results.Should().BeEmpty();
        }
    }
}
