using System.Net;
using EPR.PRN.Backend.API.Common.Dto;
using EPR.PRN.Backend.Data.DataModels;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
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
        await _application.CallPostEndpoint("api/v2/prn/prn-details", model, HttpStatusCode.OK);
        _application.PrnService.Verify(s => s.SaveEprnDetails(It.IsAny<Eprn>()));
    }
    
    private static string ToJsonWithoutField(object obj, string propertyName)
    {
        var jObj = JObject.FromObject(obj);
        jObj.Remove(propertyName);
        Console.WriteLine(jObj);
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
        var response = await _application.CallPostEndpointWithJson("api/v2/prn/prn-details", json, HttpStatusCode.BadRequest);
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
        var response = await _application.CallPostEndpoint("api/v2/prn/prn-details", model, HttpStatusCode.BadRequest);
        await response.ShouldHaveValidationErrorMessage(propertyName);
        _application.PrnService.Verify(s => s.SaveEprnDetails(It.IsAny<Eprn>()), Times.Never);
    }
}