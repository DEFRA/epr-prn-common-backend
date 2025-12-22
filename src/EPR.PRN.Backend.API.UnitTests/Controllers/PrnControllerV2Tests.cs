using System.Net;
using EPR.PRN.Backend.API.Common.Constants;
using EPR.PRN.Backend.API.Common.Dto;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Dto;
using EPR.PRN.Backend.Data.DataModels;
using EprPrnIntegration.Common.Models.Rpd;
using FluentAssertions;
using Moq;
using Newtonsoft.Json.Linq;

namespace EPR.PRN.Backend.API.UnitTests.Controllers;

[TestClass]
public class PrnControllerV2Tests
{
    private static CustomWebApplicationFactory<Startup> _application = null!;

    private static HttpClient _client = null!;

    [ClassInitialize]
    public static void Init(TestContext _)
    {
        _application = new();
        _client = _application.CreateClient();
    }

    [ClassCleanup]
    public static void Cleanup()
    {
        _client.Dispose();
        _application.Dispose();
    }

    [TestMethod]
    public async Task ShouldAcceptValidModel()
    {
        var model = DataGenerator.CreateValidSavePrnDetailsRequestV2();
        Eprn dbObj = null;
        _application
            .PrnService.Setup(s => s.SaveEprnDetails(It.IsAny<Eprn>()))
            .Callback((Eprn e) => dbObj = e)
            .ReturnsAsync((Eprn e) => e);
        var (created, location) = await _client.CallPostEndpoint<SavePrnDetailsRequestV2, PrnDto>(
            "api/v2/prn",
            model
        );
        _application.PrnService.Verify(s => s.SaveEprnDetails(It.IsAny<Eprn>()));
        model
            .Should()
            .BeEquivalentTo(
                dbObj,
                o =>
                    o.Excluding(e => e.Id)
                        .Excluding(e => e.CreatedOn)
                        .Excluding(e => e.LastUpdatedBy)
                        .Excluding(e => e.LastUpdatedDate)
                        .Excluding(e => e.PrnStatusHistories)
                        .Excluding(e => e.ExternalId)
                        .Excluding(e => e.ProducerAgency)
                        .Excluding(e => e.IssuerReference)
                        .Excluding(e => e.Signature)
                        .Excluding(e => e.IssueDate)
                        .Excluding(e => e.PackagingProducer)
                        .Excluding(e => e.CreatedBy)
                        .Excluding(e => e.IssuerReference)
            );
        dbObj.Id.Should().Be(0);
        dbObj.CreatedOn.Should().Be(default);
        dbObj.LastUpdatedBy.Should().Be(Guid.Empty);
        dbObj.LastUpdatedDate.Should().Be(default);
        dbObj.PrnStatusHistories.Should().BeNull();
        dbObj.ExternalId.Should().Be(Guid.Empty);
        dbObj.ProducerAgency.Should().BeNull();
        dbObj.Signature.Should().BeNull();
        dbObj.IssueDate.Should().Be(default);
        dbObj.CreatedBy.Should().BeNull();
        dbObj.IssuerReference.Should().Be("");

        created
            .Should()
            .BeEquivalentTo(
                dbObj,
                o =>
                    o.Excluding(p => p.PrnStatusHistories)
                        .Excluding(p => p.SourceSystemId)
                        .Excluding(p => p.IssuerReference)
            );
        location.Should().Be("api/v1/prn/0");
    }

    private static string ToJsonWithoutField(object obj, string propertyName)
    {
        var jObj = JObject.FromObject(obj);
        jObj.Remove(propertyName);
        return jObj.ToString();
    }

    [TestMethod]
    [DataRow(nameof(SavePrnDetailsRequestV2.SourceSystemId))]
    [DataRow(nameof(SavePrnDetailsRequestV2.PrnStatusId))]
    [DataRow(nameof(SavePrnDetailsRequestV2.PrnSignatory))]
    [DataRow(nameof(SavePrnDetailsRequestV2.IssuedByOrg))]
    [DataRow(nameof(SavePrnDetailsRequestV2.OrganisationId))]
    [DataRow(nameof(SavePrnDetailsRequestV2.OrganisationName))]
    [DataRow(nameof(SavePrnDetailsRequestV2.AccreditationNumber))]
    [DataRow(nameof(SavePrnDetailsRequestV2.AccreditationYear))]
    [DataRow(nameof(SavePrnDetailsRequestV2.ReprocessorExporterAgency))]
    [DataRow(nameof(SavePrnDetailsRequestV2.DecemberWaste))]
    [DataRow(nameof(SavePrnDetailsRequestV2.IsExport))]
    [DataRow(nameof(SavePrnDetailsRequestV2.TonnageValue))]
    [DataRow(nameof(SavePrnDetailsRequestV2.PrnNumber))]
    [DataRow(nameof(SavePrnDetailsRequestV2.MaterialName))]
    [DataRow(nameof(SavePrnDetailsRequestV2.ProcessToBeUsed))]
    [DataRow(nameof(SavePrnDetailsRequestV2.StatusUpdatedOn))]
    [DataRow(nameof(SavePrnDetailsRequestV2.ObligationYear))]
    public async Task ShouldValidateRequiredFields_Missing(string propertyName)
    {
        var model = DataGenerator.CreateValidSavePrnDetailsRequestV2();
        model.IssuerNotes = Guid.NewGuid().ToString();
        var json = ToJsonWithoutField(model, propertyName);
        var response = await _client.CallPostEndpointWithJson(
            "api/v2/prn/",
            json,
            HttpStatusCode.BadRequest
        );
        await response.ShouldHaveValidationErrorMessage(propertyName);
        _application.PrnService.Verify(
            s => s.SaveEprnDetails(It.Is<Eprn>(e => e.IssuerNotes == model.IssuerNotes)),
            Times.Never
        );
    }

    [TestMethod]
    [DataRow(nameof(SavePrnDetailsRequestV2.SourceSystemId))]
    [DataRow(nameof(SavePrnDetailsRequestV2.PrnStatusId))]
    [DataRow(nameof(SavePrnDetailsRequestV2.PrnSignatory))]
    [DataRow(nameof(SavePrnDetailsRequestV2.IssuedByOrg))]
    [DataRow(nameof(SavePrnDetailsRequestV2.OrganisationId))]
    [DataRow(nameof(SavePrnDetailsRequestV2.OrganisationName))]
    [DataRow(nameof(SavePrnDetailsRequestV2.AccreditationNumber))]
    [DataRow(nameof(SavePrnDetailsRequestV2.AccreditationYear))]
    [DataRow(nameof(SavePrnDetailsRequestV2.ReprocessorExporterAgency))]
    [DataRow(nameof(SavePrnDetailsRequestV2.DecemberWaste))]
    [DataRow(nameof(SavePrnDetailsRequestV2.IsExport))]
    [DataRow(nameof(SavePrnDetailsRequestV2.TonnageValue))]
    [DataRow(nameof(SavePrnDetailsRequestV2.PrnNumber))]
    [DataRow(nameof(SavePrnDetailsRequestV2.MaterialName))]
    [DataRow(nameof(SavePrnDetailsRequestV2.ProcessToBeUsed))]
    [DataRow(nameof(SavePrnDetailsRequestV2.StatusUpdatedOn))]
    [DataRow(nameof(SavePrnDetailsRequestV2.ObligationYear))]
    public async Task ShouldValidateRequiredFields_Null(string propertyName)
    {
        var model = DataGenerator.CreateValidSavePrnDetailsRequestV2();
        model.GetType().GetProperty(propertyName)!.SetValue(model, null);
        model.IssuerNotes = Guid.NewGuid().ToString();
        var response = await _client.CallPostEndpoint(
            "api/v2/prn/",
            model,
            HttpStatusCode.BadRequest
        );
        await response.ShouldHaveValidationErrorMessage(propertyName);
        _application.PrnService.Verify(
            s => s.SaveEprnDetails(It.Is<Eprn>(e => e.IssuerNotes == model.IssuerNotes)),
            Times.Never
        );
    }

    [TestMethod]
    [DataRow(nameof(SavePrnDetailsRequestV2.PrnSignatoryPosition))]
    [DataRow(nameof(SavePrnDetailsRequestV2.IssuerNotes))]
    [DataRow(nameof(SavePrnDetailsRequestV2.ReprocessingSite))]
    public async Task NonRequiredFieldsCanBeOmmitted(string propertyName)
    {
        var model = DataGenerator.CreateValidSavePrnDetailsRequestV2();
        var json = ToJsonWithoutField(model, propertyName);
        _application
            .PrnService.Setup(s => s.SaveEprnDetails(It.IsAny<Eprn>()))
            .ReturnsAsync((Eprn e) => e);
        await _client.CallPostEndpointWithJson("api/v2/prn/", json);
    }

    [TestMethod]
    [DataRow(nameof(SavePrnDetailsRequestV2.SourceSystemId))]
    [DataRow(nameof(SavePrnDetailsRequestV2.PrnSignatory))]
    [DataRow(nameof(SavePrnDetailsRequestV2.IssuedByOrg))]
    [DataRow(nameof(SavePrnDetailsRequestV2.OrganisationName))]
    [DataRow(nameof(SavePrnDetailsRequestV2.AccreditationNumber))]
    [DataRow(nameof(SavePrnDetailsRequestV2.AccreditationYear))]
    [DataRow(nameof(SavePrnDetailsRequestV2.ProcessToBeUsed))]
    public async Task ShouldValidateMinLengthFields(string propertyName)
    {
        var model = DataGenerator.CreateValidSavePrnDetailsRequestV2();
        model.IssuerNotes = Guid.NewGuid().ToString();
        model.GetType().GetProperty(propertyName)!.SetValue(model, "");
        var response = await _client.CallPostEndpoint(
            "api/v2/prn/",
            model,
            HttpStatusCode.BadRequest
        );
        await response.ShouldHaveValidationErrorMessage(propertyName);
        _application.PrnService.Verify(
            s => s.SaveEprnDetails(It.Is<Eprn>(e => e.IssuerNotes == model.IssuerNotes)),
            Times.Never
        );
    }
}
