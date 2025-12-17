using System.Net;
using EPR.PRN.Backend.API.Common.Dto;
using EPR.PRN.Backend.API.Dto;
using EPR.PRN.Backend.Data.DataModels;
using FluentAssertions;
using Moq;
using Newtonsoft.Json.Linq;

namespace EPR.PRN.Backend.API.UnitTests.Controllers;

[TestClass]
public class PrnControllerV2Tests
{
    private readonly CustomWebApplicationFactory<Startup> _application = new();

    private static SavePrnDetailsRequestV2 CreateValidModel()
    {
        return new SavePrnDetailsRequestV2
        {
            PrnNumber = "PRN123",
            OrganisationId = Guid.NewGuid(),
            OrganisationName = "Org",
            ReprocessorExporterAgency = "Reprocessor",
            PrnStatusId = 1,
            TonnageValue = 0,
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
    public async Task ShouldAcceptValidModel()
    {
        var model = CreateValidModel();
        Eprn dbObj = null;
        _application.PrnService.Setup(s => s.SaveEprnDetails(It.IsAny<Eprn>())).Callback((Eprn e) => dbObj = e)
            .ReturnsAsync((Eprn e) => e);
        var returned = await _application.CallPostEndpoint<SavePrnDetailsRequestV2, PrnDto>("api/v2/prn", model);
        _application.PrnService.Verify(s => s.SaveEprnDetails(It.IsAny<Eprn>()));
        model.Should().BeEquivalentTo(dbObj, o => o
            .Excluding(e => e.Id)
            .Excluding(e => e.ObligationYear)
            .Excluding(e => e.CreatedOn)
            .Excluding(e => e.LastUpdatedBy)
            .Excluding(e => e.LastUpdatedDate)
            .Excluding(e => e.PrnStatusHistories)
            .Excluding(e => e.ExternalId)
            .Excluding(e => e.ProducerAgency)
            .Excluding(e => e.IssuerReference)
            .Excluding(e => e.Signature)
            .Excluding(e => e.IssueDate)
            .Excluding(e => e.ProcessToBeUsed)
            .Excluding(e => e.PackagingProducer)
            .Excluding(e => e.CreatedBy)
        );
        dbObj.Id.Should().Be(0);
        dbObj.ObligationYear.Should().Be(null);
        dbObj.CreatedOn.Should().Be(default);
        dbObj.LastUpdatedBy.Should().Be(Guid.Empty);
        dbObj.LastUpdatedDate.Should().Be(default);
        dbObj.PrnStatusHistories.Should().BeNull();
        dbObj.ExternalId.Should().Be(Guid.Empty);
        dbObj.ProducerAgency.Should().BeNull();
        dbObj.IssuerReference.Should().BeNull();
        dbObj.Signature.Should().BeNull();
        dbObj.IssueDate.Should().Be(default);
        dbObj.ProcessToBeUsed.Should().BeNull();
        dbObj.CreatedBy.Should().BeNull();

        returned.created.Should().BeEquivalentTo(dbObj, o => o
            .Excluding(p => p.PrnStatusHistories)
            .Excluding(p => p.SourceSystemId)
        );
        returned.location.Should().Be("api/v1/prn/0");
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
    [DataRow(nameof(SavePrnDetailsRequestV2.ReprocessingSite))]
    [DataRow(nameof(SavePrnDetailsRequestV2.DecemberWaste))]
    [DataRow(nameof(SavePrnDetailsRequestV2.IsExport))]
    [DataRow(nameof(SavePrnDetailsRequestV2.TonnageValue))]
    [DataRow(nameof(SavePrnDetailsRequestV2.PrnNumber))]
    [DataRow(nameof(SavePrnDetailsRequestV2.MaterialName))]
    public async Task ShouldValidateRequiredFields(string propertyName)
    {
        var model = CreateValidModel();
        var json = ToJsonWithoutField(model, propertyName);
        var response = await _application.CallPostEndpointWithJson("api/v2/prn/", json, HttpStatusCode.BadRequest);
        await response.ShouldHaveRequiredErrorMessage(propertyName);
        _application.PrnService.Verify(s => s.SaveEprnDetails(It.IsAny<Eprn>()), Times.Never);
    }
    
    [TestMethod]
    [DataRow(nameof(SavePrnDetailsRequestV2.PrnSignatoryPosition))]
    [DataRow(nameof(SavePrnDetailsRequestV2.StatusUpdatedOn))]
    [DataRow(nameof(SavePrnDetailsRequestV2.IssuerNotes))]
    public async Task NonRequiredFieldsCanBeOmmitted(string propertyName)
    {
        var model = CreateValidModel();
        var json = ToJsonWithoutField(model, propertyName);
        _application.PrnService.Setup(s => s.SaveEprnDetails(It.IsAny<Eprn>()))
            .ReturnsAsync((Eprn e) => e);
        await _application.CallPostEndpointWithJson("api/v2/prn/", json);
    }
    
    [TestMethod]
    [DataRow(nameof(SavePrnDetailsRequestV2.SourceSystemId))]
    [DataRow(nameof(SavePrnDetailsRequestV2.PrnSignatory))]
    [DataRow(nameof(SavePrnDetailsRequestV2.IssuedByOrg))]
    [DataRow(nameof(SavePrnDetailsRequestV2.OrganisationName))]
    [DataRow(nameof(SavePrnDetailsRequestV2.AccreditationNumber))]
    [DataRow(nameof(SavePrnDetailsRequestV2.AccreditationYear))]
    [DataRow(nameof(SavePrnDetailsRequestV2.ReprocessingSite))]
    public async Task ShouldValidateMinLengthFields(string propertyName)
    {
        var model = CreateValidModel();
        model.GetType().GetProperty(propertyName)!.SetValue(model, "");
        var response = await _application.CallPostEndpoint("api/v2/prn/", model, HttpStatusCode.BadRequest);
        await response.ShouldHaveValidationErrorMessage(propertyName);
        _application.PrnService.Verify(s => s.SaveEprnDetails(It.IsAny<Eprn>()), Times.Never);
    }
}