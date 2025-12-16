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
            ExternalId = Guid.NewGuid(),
            PrnNumber = "PRN123",
            OrganisationId = Guid.NewGuid(),
            OrganisationName = "Org",
            ProducerAgency = "Producer",
            ReprocessorExporterAgency = "Reprocessor",
            PrnStatusId = 1,
            TonnageValue = 2,
            MaterialName = "Plastic",
            IssuerNotes = "Notes",
            IssuerReference = "Ref",
            PrnSignatory = "Sig",
            PrnSignatoryPosition = "Role",
            Signature = "Signature",
            IssueDate = DateTime.UtcNow,
            ProcessToBeUsed = "PROC",
            DecemberWaste = true,
            StatusUpdatedOn = DateTime.UtcNow,
            IssuedByOrg = "Issuer",
            AccreditationNumber = "ACC123",
            ReprocessingSite = "Site",
            AccreditationYear = "2024",
            PackagingProducer = "Packager",
            CreatedBy = "user",
            IsExport = false,
            SourceSystemId = "SYS"
        };
    }

    [TestMethod]
    public void Valid_model_has_no_validation_errors()
    {
        var model = CreateValidModel();

        var results = Validate(model);

        results.Should().BeEmpty();
    }

    [TestMethod]
    [DataRow("")]
    public void AccreditationNumber_ShouldFailWhenInvalid(string value)
    {
        var model = CreateValidModel();
        model.AccreditationNumber = value;

        var results = Validate(model);

        results.Should().ContainSingle(r =>
            r.PropertyName ==nameof(SavePrnDetailsRequestV2.AccreditationNumber));
    }

    [TestMethod]
    [DataRow("1899")]
    [DataRow("10000")]
    [DataRow("")]
    [DataRow("abdf")]
    public void AccredidationYear_ShouldFailWhenInvalid(string value)
    {
        var model = CreateValidModel();
        model.AccreditationYear = value;

        var results = Validate(model);

        results.Should().Contain(r =>
            r.PropertyName == nameof(SavePrnDetailsRequestV2.AccreditationYear));
    }

    [TestMethod]
    [DataRow("1899")]
    [DataRow("10000")]
    [DataRow("abdf")]
    public void AccredidationYear_ShouldPassWhenValid(string year)
    {
        var model = CreateValidModel();
        model.AccreditationYear = year;

        var results = Validate(model);

        results.Should().ContainSingle(r =>
            r.PropertyName == nameof(SavePrnDetailsRequestV2.AccreditationYear));
    }

    [TestMethod]
    public void Required_guid_fields_fail_when_empty()
    {
        var model = CreateValidModel();
        model.OrganisationId = Guid.Empty;

        var results = Validate(model);

        results.Should().ContainSingle(r =>
                r.PropertyName == nameof(SavePrnDetailsRequestV2.OrganisationId));
    }

    [TestMethod]
    [DataRow(nameof(SavePrnDetailsRequestV2.AccreditationNumber), PrnConstants.MaxLengthAccreditationNumber)]
    [DataRow(nameof(SavePrnDetailsRequestV2.MaterialName), PrnConstants.MaxLengthMaterialName)]
    [DataRow(nameof(SavePrnDetailsRequestV2.PrnNumber), PrnConstants.MaxLengthPrnNumber)]
    [DataRow(nameof(SavePrnDetailsRequestV2.IssuedByOrg), PrnConstants.MaxLengthIssuedByOrg)]
    [DataRow(nameof(SavePrnDetailsRequestV2.OrganisationName), PrnConstants.MaxLengthOrganisationName)]
    [DataRow(nameof(SavePrnDetailsRequestV2.PackagingProducer), PrnConstants.MaxLengthPackagingProducer)]
    [DataRow(nameof(SavePrnDetailsRequestV2.ProcessToBeUsed), PrnConstants.MaxLengthProcessToBeUsed)]
    [DataRow(nameof(SavePrnDetailsRequestV2.ReprocessorExporterAgency), PrnConstants.MaxLengthReprocessorExporterAgency)]
    [DataRow(nameof(SavePrnDetailsRequestV2.IssuerNotes), PrnConstants.MaxLengthIssuerNotes)]
    [DataRow(nameof(SavePrnDetailsRequestV2.IssuerReference), PrnConstants.MaxLengthIssuerReference)]
    [DataRow(nameof(SavePrnDetailsRequestV2.PrnSignatory), PrnConstants.MaxLengthPrnSignatory)]
    [DataRow(nameof(SavePrnDetailsRequestV2.PrnSignatoryPosition), PrnConstants.MaxLengthPrnSignatoryPosition)]
    [DataRow(nameof(SavePrnDetailsRequestV2.CreatedBy), PrnConstants.MaxLengthCreatedBy)]
    [DataRow(nameof(SavePrnDetailsRequestV2.SourceSystemId), PrnConstants.MaxLengthSourceSystemId)]
    [DataRow(nameof(SavePrnDetailsRequestV2.ProducerAgency), PrnConstants.MaxLengthProducerAgency)]
    [DataRow(nameof(SavePrnDetailsRequestV2.Signature), PrnConstants.MaxLengthSignature)]
    [DataRow(nameof(SavePrnDetailsRequestV2.ReprocessingSite), PrnConstants.MaxLengthReprocessingSite)]
    public void MaxLength_fields_fail_when_exceeded(string propertyName, int length)
    {
        var model = CreateValidModel();
        typeof(SavePrnDetailsRequestV2)
            .GetProperty(propertyName)!
            .SetValue(model, new string('x', length + 1));

        var results = Validate(model);

        results.Should().Contain(r =>
            r.PropertyName == propertyName);


        model = CreateValidModel();
        typeof(SavePrnDetailsRequestV2)
            .GetProperty(propertyName)!
            .SetValue(model, new string('x', length));

        results = Validate(model);

        results.Should().BeEmpty();
    }

    [TestMethod]
    [DataRow(nameof(SavePrnDetailsRequestV2.AccreditationNumber), 1)]
    [DataRow(nameof(SavePrnDetailsRequestV2.MaterialName), 1)]
    [DataRow(nameof(SavePrnDetailsRequestV2.PrnNumber), 1)]
    [DataRow(nameof(SavePrnDetailsRequestV2.IssuedByOrg), 1)]
    [DataRow(nameof(SavePrnDetailsRequestV2.OrganisationName), 1)]
    [DataRow(nameof(SavePrnDetailsRequestV2.PackagingProducer), 1)]
    [DataRow(nameof(SavePrnDetailsRequestV2.ProcessToBeUsed), 1)]
    [DataRow(nameof(SavePrnDetailsRequestV2.ReprocessorExporterAgency), 1)]
    [DataRow(nameof(SavePrnDetailsRequestV2.CreatedBy), 1)]
    [DataRow(nameof(SavePrnDetailsRequestV2.SourceSystemId), 1)]
    [DataRow(nameof(SavePrnDetailsRequestV2.ReprocessingSite), 1)]
    public void MinLength_fields_fail_when_too_small(string propertyName, int length)
    {
        var model = CreateValidModel();
        typeof(SavePrnDetailsRequestV2)
            .GetProperty(propertyName)!
            .SetValue(model, new string('x', length - 1));

        var results = Validate(model);

        results.Should().Contain(r =>
            r.PropertyName == propertyName);


        model = CreateValidModel();
        typeof(SavePrnDetailsRequestV2)
            .GetProperty(propertyName)!
            .SetValue(model, new string('x', length));

        results = Validate(model);

        results.Should().BeEmpty();
    }

    [TestMethod]
    public void TonnageValue_fails_when_negative()
    {
        var model = CreateValidModel();
        model.TonnageValue = -1;

        var results = Validate(model);

        results.Should().Contain(r =>
            r.PropertyName == nameof(SavePrnDetailsRequestV2.TonnageValue));
    }
}