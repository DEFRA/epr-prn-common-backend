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
            ExternalId = Guid.NewGuid(),
            PrnNumber = "PRN123",
            OrganisationId = Guid.NewGuid(),
            OrganisationName = "Org",
            ProducerAgency = "Producer",
            ReprocessorExporterAgency = "Reprocessor",
            PrnStatusId = 1,
            TonnageValue = 0,
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
    public async Task ShouldAcceptValidModel()
    {
        var model = CreateValidModel();
        Eprn dbObj = null; 
        _application.PrnService.Setup(s => s.SaveEprnDetails(It.IsAny<Eprn>())).Callback((Eprn e) => dbObj = e).ReturnsAsync((Eprn e) => e);
        var returned = await _application.CallPostEndpoint<SavePrnDetailsRequestV2,PrnDto>("api/v2/prn", model);
        _application.PrnService.Verify(s => s.SaveEprnDetails(It.IsAny<Eprn>()));
        model.Should().BeEquivalentTo(dbObj, o => o
            .Excluding(e => e.Id)
            .Excluding(e => e.ObligationYear)
            .Excluding(e => e.CreatedOn)
            .Excluding(e => e.LastUpdatedBy)
            .Excluding(e => e.LastUpdatedDate)
            .Excluding(e => e.PrnStatusHistories));
        dbObj.Id.Should().Be(0);
        dbObj.ObligationYear.Should().Be(null);
        dbObj.CreatedOn.Should().Be(default);
        dbObj.LastUpdatedBy.Should().Be(Guid.Empty);
        dbObj.LastUpdatedDate.Should().Be(default);
        dbObj.PrnStatusHistories.Should().BeNull();

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
    [DataRow(nameof(SavePrnDetailsRequestV2.AccreditationNumber))]
    [DataRow(nameof(SavePrnDetailsRequestV2.AccreditationYear))]
    [DataRow(nameof(SavePrnDetailsRequestV2.MaterialName))]
    [DataRow(nameof(SavePrnDetailsRequestV2.PrnNumber))]
    [DataRow(nameof(SavePrnDetailsRequestV2.IssuedByOrg))]
    [DataRow(nameof(SavePrnDetailsRequestV2.OrganisationName))]
    [DataRow(nameof(SavePrnDetailsRequestV2.OrganisationId))]
    [DataRow(nameof(SavePrnDetailsRequestV2.PackagingProducer))]
    [DataRow(nameof(SavePrnDetailsRequestV2.ReprocessorExporterAgency))]
    [DataRow(nameof(SavePrnDetailsRequestV2.IssuerReference))]
    [DataRow(nameof(SavePrnDetailsRequestV2.CreatedBy))]
    [DataRow(nameof(SavePrnDetailsRequestV2.SourceSystemId))]
    [DataRow(nameof(SavePrnDetailsRequestV2.ProducerAgency))]
    [DataRow(nameof(SavePrnDetailsRequestV2.ExternalId))]
    [DataRow(nameof(SavePrnDetailsRequestV2.ReprocessingSite))]
    public async Task ShouldValidateRequiredFields(string propertyName)
    {
        var model = CreateValidModel();
        var json = ToJsonWithoutField(model, propertyName);
        var response = await _application.CallPostEndpointWithJson("api/v2/prn/", json, HttpStatusCode.BadRequest);
        await response.ShouldHaveRequiredErrorMessage(propertyName);
        _application.PrnService.Verify(s => s.SaveEprnDetails(It.IsAny<Eprn>()), Times.Never);
    }
    
    
    [TestMethod]
    [DataRow(nameof(SavePrnDetailsRequestV2.AccreditationNumber))]
    [DataRow(nameof(SavePrnDetailsRequestV2.AccreditationYear))]
    [DataRow(nameof(SavePrnDetailsRequestV2.MaterialName))]
    [DataRow(nameof(SavePrnDetailsRequestV2.PrnNumber))]
    [DataRow(nameof(SavePrnDetailsRequestV2.IssuedByOrg))]
    [DataRow(nameof(SavePrnDetailsRequestV2.OrganisationName))]
    [DataRow(nameof(SavePrnDetailsRequestV2.PackagingProducer))]
    [DataRow(nameof(SavePrnDetailsRequestV2.ReprocessorExporterAgency))]
    [DataRow(nameof(SavePrnDetailsRequestV2.CreatedBy))]
    [DataRow(nameof(SavePrnDetailsRequestV2.SourceSystemId))]
    [DataRow(nameof(SavePrnDetailsRequestV2.ProducerAgency))]
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