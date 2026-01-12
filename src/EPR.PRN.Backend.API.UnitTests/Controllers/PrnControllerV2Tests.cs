using System.Net;
using Azure.Core;
using BackendAccountService.Core.Models.Request;
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
        var model = DataGenerator.CreateValidSavePrnDetailsRequest();
        Eprn dbObj = null;
        _application
            .PrnService.Setup(s => s.SaveEprnDetails(It.IsAny<Eprn>()))
            .Callback((Eprn e) => dbObj = e)
            .ReturnsAsync((Eprn e) => e);
        var (created, location) = await _client.CallPostEndpoint<SavePrnDetailsRequest, PrnDto>(
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
        dbObj.Signature.Should().BeNull();
        dbObj.CreatedBy.Should().BeNull();
        dbObj.IssuerReference.Should().Be("");
        dbObj.PackagingProducer.Should().Be("");
        dbObj.ProducerAgency.Should().Be("");

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
    [DataRow(nameof(SavePrnDetailsRequest.SourceSystemId))]
    [DataRow(nameof(SavePrnDetailsRequest.PrnStatusId))]
    [DataRow(nameof(SavePrnDetailsRequest.PrnSignatory))]
    [DataRow(nameof(SavePrnDetailsRequest.IssuedByOrg))]
    [DataRow(nameof(SavePrnDetailsRequest.OrganisationId))]
    [DataRow(nameof(SavePrnDetailsRequest.OrganisationName))]
    [DataRow(nameof(SavePrnDetailsRequest.AccreditationNumber))]
    [DataRow(nameof(SavePrnDetailsRequest.AccreditationYear))]
    [DataRow(nameof(SavePrnDetailsRequest.ReprocessorExporterAgency))]
    [DataRow(nameof(SavePrnDetailsRequest.DecemberWaste))]
    [DataRow(nameof(SavePrnDetailsRequest.IsExport))]
    [DataRow(nameof(SavePrnDetailsRequest.TonnageValue))]
    [DataRow(nameof(SavePrnDetailsRequest.PrnNumber))]
    [DataRow(nameof(SavePrnDetailsRequest.MaterialName))]
    [DataRow(nameof(SavePrnDetailsRequest.ProcessToBeUsed))]
    [DataRow(nameof(SavePrnDetailsRequest.StatusUpdatedOn))]
    [DataRow(nameof(SavePrnDetailsRequest.ObligationYear))]
    public async Task ShouldValidateRequiredFields_Missing(string propertyName)
    {
        var model = DataGenerator.CreateValidSavePrnDetailsRequest();
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
    [DataRow(nameof(SavePrnDetailsRequest.SourceSystemId))]
    [DataRow(nameof(SavePrnDetailsRequest.PrnStatusId))]
    [DataRow(nameof(SavePrnDetailsRequest.PrnSignatory))]
    [DataRow(nameof(SavePrnDetailsRequest.IssuedByOrg))]
    [DataRow(nameof(SavePrnDetailsRequest.OrganisationId))]
    [DataRow(nameof(SavePrnDetailsRequest.OrganisationName))]
    [DataRow(nameof(SavePrnDetailsRequest.AccreditationNumber))]
    [DataRow(nameof(SavePrnDetailsRequest.AccreditationYear))]
    [DataRow(nameof(SavePrnDetailsRequest.ReprocessorExporterAgency))]
    [DataRow(nameof(SavePrnDetailsRequest.DecemberWaste))]
    [DataRow(nameof(SavePrnDetailsRequest.IsExport))]
    [DataRow(nameof(SavePrnDetailsRequest.TonnageValue))]
    [DataRow(nameof(SavePrnDetailsRequest.PrnNumber))]
    [DataRow(nameof(SavePrnDetailsRequest.MaterialName))]
    [DataRow(nameof(SavePrnDetailsRequest.ProcessToBeUsed))]
    [DataRow(nameof(SavePrnDetailsRequest.StatusUpdatedOn))]
    [DataRow(nameof(SavePrnDetailsRequest.ObligationYear))]
    public async Task ShouldValidateRequiredFields_Null(string propertyName)
    {
        var model = DataGenerator.CreateValidSavePrnDetailsRequest();
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
    [DataRow(nameof(SavePrnDetailsRequest.PrnSignatoryPosition))]
    [DataRow(nameof(SavePrnDetailsRequest.IssuerNotes))]
    [DataRow(nameof(SavePrnDetailsRequest.ReprocessingSite))]
    [DataRow(nameof(SavePrnDetailsRequest.IssueDate))]
    public async Task NonRequiredFieldsCanBeOmmitted(string propertyName)
    {
        var model = DataGenerator.CreateValidSavePrnDetailsRequest();
        var json = ToJsonWithoutField(model, propertyName);
        _application
            .PrnService.Setup(s => s.SaveEprnDetails(It.IsAny<Eprn>()))
            .ReturnsAsync((Eprn e) => e);
        await _client.CallPostEndpointWithJson("api/v2/prn/", json);
    }

    [TestMethod]
    [DataRow(nameof(SavePrnDetailsRequest.SourceSystemId))]
    [DataRow(nameof(SavePrnDetailsRequest.PrnSignatory))]
    [DataRow(nameof(SavePrnDetailsRequest.IssuedByOrg))]
    [DataRow(nameof(SavePrnDetailsRequest.OrganisationName))]
    [DataRow(nameof(SavePrnDetailsRequest.AccreditationNumber))]
    [DataRow(nameof(SavePrnDetailsRequest.AccreditationYear))]
    [DataRow(nameof(SavePrnDetailsRequest.ProcessToBeUsed))]
    public async Task ShouldValidateMinLengthFields(string propertyName)
    {
        var model = DataGenerator.CreateValidSavePrnDetailsRequest();
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

    [TestMethod]
    public async Task GetModifiedPrnsbyDate_ReturnsOkWithPrns_WhenPrnsExist()
    {
        // Arrange
        var fromDate = DateTime.UtcNow.AddDays(-7);
        var toDate = DateTime.UtcNow;
        var mockPrns = new List<PrnUpdateStatus>
        {
            new()
            {
                PrnNumber = "123",
                PrnStatusId = 1,
                AccreditationYear = "2014",
                SourceSystemId = "1",
            },
            new()
            {
                PrnNumber = "456",
                PrnStatusId = 2,
                AccreditationYear = "2014",
                SourceSystemId = "2",
            },
        };

        _application
            .PrnService.Setup(service =>
                service.GetModifiedPrnsbyDate(
                    It.Is<DateTime>(d => (d - fromDate) < TimeSpan.FromSeconds(1)),
                    It.Is<DateTime>(d => (d - toDate) < TimeSpan.FromSeconds(1))
                )
            )
            .ReturnsAsync(mockPrns);

        // Act
        var result = await _client.CallGetEndpoint<List<PrnUpdateStatus>>(
            "api/v2/prn/modified-prns",
            HttpStatusCode.OK,
            new Dictionary<string, string>
            {
                { "From", fromDate.ToString("yyyy-MM-ddTHH:mm:ss") },
                { "To", toDate.ToString("yyyy-MM-ddTHH:mm:ss") },
            }
        );

        // Assert
        mockPrns.Should().BeEquivalentTo(result);
    }

    [TestMethod]
    public async Task GetModifiedPrnsbyDate_ReturnsNoContent_WhenNoPrnsExist()
    {
        // Arrange
        var fromDate = DateTime.UtcNow.AddDays(-7);
        var toDate = DateTime.UtcNow;

        _application
            .PrnService.Setup(service =>
                service.GetModifiedPrnsbyDate(
                    It.Is<DateTime>(d => (d - fromDate) < TimeSpan.FromSeconds(1)),
                    It.Is<DateTime>(d => (d - toDate) < TimeSpan.FromSeconds(1))
                )
            )
            .ReturnsAsync([]);

        // Act
        var result = await _client.CallGetEndpoint<List<PrnUpdateStatus>>(
            "api/v2/prn/modified-prns",
            HttpStatusCode.OK,
            new Dictionary<string, string>
            {
                { "From", fromDate.ToString("yyyy-MM-ddTHH:mm:ss") },
                { "To", toDate.ToString("yyyy-MM-ddTHH:mm:ss") },
            }
        );

        // Assert
        result.Should().BeEmpty();
    }

    [TestMethod]
    public async Task GetModifiedPrnsbyDate_ReturnsBadRequest_WhenNoQueryParams()
    {
        var result = await _client.CallGetEndpoint<List<PrnUpdateStatus>>(
            "api/v2/prn/modified-prns",
            HttpStatusCode.BadRequest
        );
        result.Should().BeNull();
    }

    [TestMethod]
    public async Task GetModifiedPrnsbyDate_ReturnsBadRequest_WithDefaultDates()
    {
        var fromDate = DateTime.UtcNow.AddDays(-7);
        var toDate = DateTime.UtcNow;
        var result = await _client.CallGetEndpoint<List<PrnUpdateStatus>>(
            "api/v2/prn/modified-prns",
            HttpStatusCode.BadRequest,
            new Dictionary<string, string> { { "To", toDate.ToString("yyyy-MM-ddTHH:mm:ss") } }
        );
        result.Should().BeNull();

        result = await _client.CallGetEndpoint<List<PrnUpdateStatus>>(
            "api/v2/prn/modified-prns",
            HttpStatusCode.BadRequest,
            new Dictionary<string, string> { { "From", fromDate.ToString("yyyy-MM-ddTHH:mm:ss") } }
        );
        result.Should().BeNull();
    }
}
