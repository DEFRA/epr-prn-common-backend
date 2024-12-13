using AutoFixture;
using BackendAccountService.Core.Models.Request;
using EPR.PRN.Backend.API.Common.DTO;
using EPR.PRN.Backend.API.Configs;
using EPR.PRN.Backend.API.Controllers;
using EPR.PRN.Backend.API.Helpers;
using EPR.PRN.Backend.API.Models;
using EPR.PRN.Backend.API.Services.Interfaces;
using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Obligation.DTO;
using EPR.PRN.Backend.Obligation.Interfaces;
using EPR.PRN.Backend.Obligation.Models;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Net;


namespace EPR.PRN.Backend.API.UnitTests.Services;

[TestClass]
public class PrnControllerTests
{
    private PrnController _systemUnderTest;
    private Mock<IPrnService> _mockPrnService;
    private Mock<ILogger<PrnController>> _mockLogger;
    private Mock<IObligationCalculatorService> _mockObligationCalculatorService;
    private Mock<IOptions<PrnObligationCalculationConfig>> _configMock;
    private Mock<IConfiguration> _configurationMock;
    private Mock<IValidator<SavePrnDetailsRequest>> _validatorSavePrnDetailsMock;

    private static readonly IFixture _fixture = new Fixture();

    [TestInitialize]
    public void TestInitialize()
    {
        _mockPrnService = new Mock<IPrnService>();
        _mockLogger = new Mock<ILogger<PrnController>>();
        _mockObligationCalculatorService = new Mock<IObligationCalculatorService>();

        _configMock = new Mock<IOptions<PrnObligationCalculationConfig>>();
        var config = new PrnObligationCalculationConfig { StartYear = 2024, EndYear = 2029 };
        _configMock.Setup(c => c.Value).Returns(config);

        _configurationMock = new Mock<IConfiguration>();
        _configurationMock.Setup(c => c["LogPrefix"]).Returns("[EPR.PRN.Backend]");

        _validatorSavePrnDetailsMock = new();

        _systemUnderTest = new PrnController(_mockPrnService.Object, 
            _mockLogger.Object, 
            _mockObligationCalculatorService.Object, 
            _configMock.Object, 
            _configurationMock.Object, 
            _validatorSavePrnDetailsMock.Object);
    }

    [TestMethod]
    public async Task GetPrn_ReturnsOkWithPrn_WhenValidPrnId()
    {
        var orgId = Guid.NewGuid();
        var prnId = Guid.NewGuid();
        var expectedPrn = _fixture.Create<PrnDto>();
        _mockPrnService.Setup(s => s.GetPrnForOrganisationById(orgId, prnId)).ReturnsAsync(expectedPrn);

        var result = await _systemUnderTest.GetPrn(orgId, prnId) as OkObjectResult;

        result.Value.Should().BeEquivalentTo(expectedPrn);
    }

    [TestMethod]
    public async Task GetPrn_ReturnsNotFound_WhenPrnIdDoesntExists()
    {
        var orgId = Guid.NewGuid();
        var prnId = Guid.NewGuid();

        _mockPrnService.Setup(s => s.GetPrnForOrganisationById(orgId, prnId)).ReturnsAsync((PrnDto)null);

        var result = await _systemUnderTest.GetPrn(orgId, prnId) as NotFoundResult;

        result.Should().NotBeNull();
        result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
    }

    [TestMethod]
    public async Task GetAllPrnByOrganisationId_ReturnsOkWithPrns_WhenValidOrgId()
    {
        var orgId = Guid.NewGuid();
        var expectedPrns = _fixture.CreateMany<PrnDto>().ToList();

        _mockPrnService.Setup(s => s.GetAllPrnByOrganisationId(orgId)).ReturnsAsync(expectedPrns);

        var result = await _systemUnderTest.GetAllPrnByOrganisationId(orgId) as OkObjectResult;

        result.Value.Should().BeEquivalentTo(expectedPrns);
    }

    [TestMethod]
    public async Task UpdatePrnStatus_ReturnsOk_WhenUpdateSuccessfully()
    {
        var orgId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var prnUpdates = _fixture.CreateMany<PrnUpdateStatusDto>().ToList();

        _mockPrnService.Setup(s => s.UpdateStatus(orgId, userId, prnUpdates)).Returns(Task.CompletedTask);

        var result = await _systemUnderTest.UpdatePrnStatus(orgId, userId, prnUpdates) as OkResult;

        result.Should().NotBeNull();
        result.StatusCode.Should().Be((int)HttpStatusCode.OK);
    }

    [TestMethod]
    public async Task UpdatePrnStatus_ReturnsConflict_WhenServiceThrowsConflictException()
    {
        var orgId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var prnUpdates = _fixture.CreateMany<PrnUpdateStatusDto>().ToList();

        _mockPrnService.Setup(s => s.UpdateStatus(orgId, userId, prnUpdates)).Throws<ConflictException>();

        var result = await _systemUnderTest.UpdatePrnStatus(orgId, userId, prnUpdates) as ObjectResult;

        result.Should().NotBeNull();
        result.StatusCode.Should().Be((int)HttpStatusCode.Conflict);
    }

    [TestMethod]
    public async Task UpdatePrnStatus_ReturnsNotFound_WhenServiceThrowsNotFoundException()
    {
        var orgId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var prnUpdates = _fixture.CreateMany<PrnUpdateStatusDto>().ToList();

        _mockPrnService.Setup(s => s.UpdateStatus(orgId, userId, prnUpdates)).Throws<NotFoundException>();

        var result = await _systemUnderTest.UpdatePrnStatus(orgId, userId, prnUpdates) as ObjectResult;

        result.Should().NotBeNull();
        result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
    }

    [TestMethod]
    public async Task UpdatePrnStatus_ReturnsInternalServer_WhenServiceThrowsUnexpectedException()
    {
        var orgId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var prnUpdates = _fixture.CreateMany<PrnUpdateStatusDto>().ToList();

        _mockPrnService.Setup(s => s.UpdateStatus(orgId, userId, prnUpdates)).Throws<ArgumentNullException>();

        var result = await _systemUnderTest.UpdatePrnStatus(orgId, userId, prnUpdates) as ObjectResult;

        result.Should().NotBeNull();
        result.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }

    [TestMethod]
    public async Task CalculateAsync_WhenRequestIsNull_ReturnsBadRequest()
    {
        var organisationId = Guid.NewGuid();

        var result = await _systemUnderTest.CalculateAsync(organisationId, null);

        result.Should().BeOfType<BadRequestObjectResult>();

        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult.Value.Should().BeEquivalentTo(new { message = "Submission calculation request cannot be null or empty." });
    }

    [TestMethod]
    public async Task CalculateAsync_WhenRequestIsEmpty_ReturnsBadRequest()
    {
        var organisationId = Guid.NewGuid();

        var result = await _systemUnderTest.CalculateAsync(organisationId, []);

        result.Should().BeOfType<BadRequestObjectResult>();

        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult.Value.Should().BeEquivalentTo(new { message = "Submission calculation request cannot be null or empty." });
    }

    [TestMethod]
    public async Task CalculateAsync_WhenModelStateIsInvalid_ReturnsBadRequest()
    {
        var organisationId = Guid.NewGuid();

        _systemUnderTest.ModelState.AddModelError("Key", "Error message");

        var result = await _systemUnderTest.CalculateAsync(organisationId, [new()]);

        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [TestMethod]
    public async Task CalculateAsync_WhenCalculationFails_ReturnsInternalServerError()
    {
        var organisationId = Guid.NewGuid();
        var calculationResult = new CalculationResult { Success = false };
        _mockObligationCalculatorService
            .Setup(x => x.CalculateAsync(It.IsAny<Guid>(), It.IsAny<List<SubmissionCalculationRequest>>()))
            .ReturnsAsync(calculationResult);

        var result = await _systemUnderTest.CalculateAsync(organisationId, [new()]);

        result.Should().BeOfType<ObjectResult>().Which.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
    }

    [TestMethod]
    public async Task CalculateAsync_WhenCalculationSucceeds_ReturnsAccepted()
    {
        var organisationId = Guid.NewGuid();
        var Calculations = _fixture.CreateMany<ObligationCalculation>().ToList();
        var calculationResult = new CalculationResult
        {
            Success = true,
            Calculations = Calculations
        };

        _mockObligationCalculatorService
            .Setup(x => x.CalculateAsync(It.IsAny<Guid>(), It.IsAny<List<SubmissionCalculationRequest>>()))
            .ReturnsAsync(calculationResult);

        var result = await _systemUnderTest.CalculateAsync(organisationId, [new()]);

        result.Should().BeOfType<AcceptedResult>().Which.Value.Should().BeEquivalentTo(new
        {
            message = "Calculation successful.",
            data = calculationResult.Calculations
        });
    }

    [TestMethod]
    public async Task CalculateAsync_WhenTimeoutOccurs_ReturnsGatewayTimeout()
    {
        var organisationId = Guid.NewGuid();
        _mockObligationCalculatorService
            .Setup(x => x.CalculateAsync(It.IsAny<Guid>(), It.IsAny<List<SubmissionCalculationRequest>>()))
            .ThrowsAsync(new TimeoutException("Request timed out"));

        var result = await _systemUnderTest.CalculateAsync(organisationId, [new()]);

        result.Should().BeOfType<ObjectResult>().Which.StatusCode.Should().Be(StatusCodes.Status504GatewayTimeout);

        var objectResult = result as ObjectResult;
        objectResult.Value.Should().BeEquivalentTo(new { message = "Calculation timed out.", details = "Request timed out" });
    }


    [TestMethod]
    public async Task CalculateAsync_WhenUnexpectedErrorOccurs_ReturnsInternalServerError()
    {
        var organisationId = Guid.NewGuid();
        _mockObligationCalculatorService
            .Setup(x => x.CalculateAsync(It.IsAny<Guid>(), It.IsAny<List<SubmissionCalculationRequest>>()))
            .ThrowsAsync(new Exception("Unexpected error"));

        var result = await _systemUnderTest.CalculateAsync(organisationId, [new()]);

        result.Should().BeOfType<ObjectResult>().Which.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);

        var objectResult = result as ObjectResult;
        objectResult.Value.Should().BeEquivalentTo(new { message = "An error occurred during calculation.", details = "Unexpected error" });
    }

    [TestMethod]
    public async Task GetSearchPrns_ReturnsUnauthorizedWhenOrgIdNotPresent()
    {
        var request = _fixture.Create<PaginatedRequestDto>();

        var result = await _systemUnderTest.GetSearchPrns(Guid.Empty, request);
        result.Should().BeOfType<UnauthorizedResult>();
    }

    [TestMethod]
    public async Task GetSearchPrns_ReturnsResponse()
    {
        var orgId = Guid.NewGuid();
        var request = _fixture.Create<PaginatedRequestDto>();
        var response = _fixture.Create<PaginatedResponseDto<PrnDto>>();

        _mockPrnService.Setup(s => s.GetSearchPrnsForOrganisation(orgId, request)).ReturnsAsync(response);

        var result = await _systemUnderTest.GetSearchPrns(orgId, request);
        result.Should().BeOfType<OkObjectResult>().Which.Value.Should().Be(response);
    }

    [TestMethod]
    [DataRow(2023)] // Invalid year
    [DataRow(2030)] // Invalid year
    public async Task GetObligationCalculation_InvalidYear_ReturnsBadRequest(int year)
    {
        // Arrange
        var organisationId = Guid.NewGuid();

        // Act
        var result = await _systemUnderTest.GetObligationCalculation(organisationId, year);

        // Assert
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult.Should().NotBeNull();
        badRequestResult.StatusCode.Should().Be(400);
        badRequestResult.Value.Should().Be($"Invalid year provided: {year}.");
    }

    [TestMethod]
    public async Task GetObligationCalculation_WhenIsSuccessFalse_Returns500()
    {
        // Arrange
        var organisationId = Guid.NewGuid();
        var year = 2025;
        var obligationResult = new ObligationCalculationResult { Errors = null, IsSuccess = false };
        _mockObligationCalculatorService.Setup(service => service.GetObligationCalculation(organisationId, year)).ReturnsAsync(obligationResult);

        // Act
        var result = await _systemUnderTest.GetObligationCalculation(organisationId, year);

        var statusCodeResult = result as ObjectResult;
        statusCodeResult.Should().NotBeNull();
        statusCodeResult.StatusCode.Should().Be(500);
    }

    [TestMethod]
    public async Task GetObligationCalculation_ValidYear_DataFound_ReturnsOk()
    {
        // Arrange
        var organisationId = Guid.NewGuid();
        var year = 2025;
        var fixture = new Fixture();
        var prns = fixture.CreateMany<ObligationData>(10).ToList();
        for (int i = 0; i < 2; i++)
        {
            prns[i].MaterialName = "Plastic";
            prns[i].OrganisationId = organisationId;

        }
        for (int i = 2; i < 5; i++)
        {
            prns[i].MaterialName = "Wood";
            prns[i].OrganisationId = organisationId;
        }

        var obligationResult = new ObligationCalculationResult { Errors = null, IsSuccess = true, ObligationModel = new ObligationModel { NumberOfPrnsAwaitingAcceptance = 8, ObligationData = prns } };

        // Mock the service to return obligation data
        _mockObligationCalculatorService
            .Setup(service => service.GetObligationCalculation(organisationId, year))
            .ReturnsAsync(obligationResult);

        // Act
        var result = await _systemUnderTest.GetObligationCalculation(organisationId, year);

        // Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult.StatusCode.Should().Be(200);
        okResult.Value.Should().BeEquivalentTo(new ObligationModel { ObligationData = prns, NumberOfPrnsAwaitingAcceptance = obligationResult.ObligationModel.NumberOfPrnsAwaitingAcceptance });
    }

    [TestMethod]
    public async Task GetModifiedPrnsbyDate_ReturnsOkWithPrns_WhenPrnsExist()
    {
        // Arrange
        var fromDate = DateTime.UtcNow.AddDays(-7);
        var toDate = DateTime.UtcNow;
        var mockPrns = new List<PrnUpdateStatus>
        {
            new() { EvidenceNo = "123", EvidenceStatusCode = "Modified", AccreditationYear= "2014" },
            new() { EvidenceNo = "456", EvidenceStatusCode = "Unchanged", AccreditationYear= "2014" }
        };

        _mockPrnService
            .Setup(service => service.GetModifiedPrnsbyDate(fromDate, toDate))
            .ReturnsAsync(mockPrns);

        var modifiedPrnsbyDateRequest = new ModifiedPrnsbyDateRequest
        {
            From = fromDate,
            To = toDate
        };

        // Act
        var result = await _systemUnderTest.GetModifiedPrnsbyDate(modifiedPrnsbyDateRequest);

        // Assert
        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        CollectionAssert.AreEqual(mockPrns, okResult.Value as List<PrnUpdateStatus>);
    }

    [TestMethod]
    public async Task GetModifiedPrnsbyDate_ReturnsNoContent_WhenNoPrnsExist()
    {
        // Arrange
        var fromDate = DateTime.UtcNow.AddDays(-7);
        var toDate = DateTime.UtcNow;

        _mockPrnService
            .Setup(service => service.GetModifiedPrnsbyDate(fromDate, toDate))
            .ReturnsAsync((List<PrnUpdateStatus>)null);
        
        var modifiedPrnsbyDateRequest = new ModifiedPrnsbyDateRequest
        {
            From = fromDate,
            To = toDate
        };
        // Act
        var result = await _systemUnderTest.GetModifiedPrnsbyDate(modifiedPrnsbyDateRequest);

        // Assert
        var statusCodeResult = result as StatusCodeResult;
        Assert.IsNotNull(statusCodeResult);
        Assert.AreEqual(204, statusCodeResult.StatusCode);
    }

    [TestMethod]
    public async Task SavePrn_ReturnsStatusCode200_WhenValidInputSavedSuccessfully()
    {
        var dto = new SavePrnDetailsRequest()
        {
            AccreditationNo = "ABC",
            AccreditationYear = 2018,
            CancelledDate = DateTime.UtcNow.AddDays(-1),
            DecemberWaste = true,
            EvidenceMaterial = "Aluminium",
            EvidenceNo = Guid.NewGuid().ToString(),
            EvidenceStatusCode = EprnStatus.AWAITINGACCEPTANCE,
            EvidenceTonnes = 5000,
            IssueDate = DateTime.UtcNow.AddDays(-5),
            IssuedByNPWDCode = "NPWD367742",
            IssuedByOrgName = "ANB",
            IssuedToEPRId = Guid.NewGuid(),
            IssuedToNPWDCode = "NPWD557742",
            IssuedToOrgName = "ZNZ",
            IssuerNotes = "no notes",
            IssuerRef = "ANB-1123",
            MaterialOperationCode = "R-PLA",
            ObligationYear = 2025,
            PrnSignatory = "Pat Anderson",
            PrnSignatoryPosition = "Director",
            ProducerAgency = "TTL",
            RecoveryProcessCode = "N11",
            ReprocessorAgency = "BEX",
            StatusDate = DateTime.UtcNow,
        };

        var validationResult = new ValidationResult();

        _validatorSavePrnDetailsMock.Setup(x => x.Validate(It.IsAny<SavePrnDetailsRequest>()))
                                    .Returns(validationResult);

        _mockPrnService.Setup(s => s.SavePrnDetails(dto)).Returns(() => Task.CompletedTask);
        var result = await _systemUnderTest.SaveAsync(dto) as OkResult;

        result.Should().NotBeNull();
        result.StatusCode.Should().Be((int)HttpStatusCode.OK);
    }

    [TestMethod]
    [DataRow("AccreditationNo", null)]
    [DataRow("AccreditationYear", null)]
    [DataRow("CancelledDate", null)]
    [DataRow("EvidenceMaterial", null)]
    [DataRow("EvidenceNo", null)]
    [DataRow("EvidenceStatusCode", null)]
    [DataRow("EvidenceTonnes", null)]
    [DataRow("IssuedByOrgName", null)]
    [DataRow("IssuedToOrgName", null)]
    [DataRow("ProducerAgency", null)]
    [DataRow("RecoveryProcessCode", null)]
    [DataRow("StatusDate", null)]
    public async Task SavePrn_ReturnsStatusCode400_OnInvalidInput(string propertyName, object propertyValue)
    {
        var dto = new SavePrnDetailsRequest()
        {
            AccreditationNo = "ABC",
            AccreditationYear = 2018,
            CancelledDate = DateTime.UtcNow.AddDays(-1),
            DecemberWaste = true,
            EvidenceMaterial = "Aluminium",
            EvidenceNo = Guid.NewGuid().ToString(),
            EvidenceStatusCode = EprnStatus.AWAITINGACCEPTANCE,
            EvidenceTonnes = 5000,
            IssueDate = DateTime.UtcNow.AddDays(-5),
            IssuedByNPWDCode = "NPWD367742",
            IssuedByOrgName = "ANB",
            IssuedToEPRId = Guid.NewGuid(),
            IssuedToNPWDCode = "NPWD557742",
            IssuedToOrgName = "ZNZ",
            IssuerNotes = "no notes",
            IssuerRef = "ANB-1123",
            MaterialOperationCode = "R-PLA",
            ObligationYear = 2025,
            PrnSignatory = "Pat Anderson",
            PrnSignatoryPosition = "Director",
            ProducerAgency = "TTL",
            RecoveryProcessCode = "N11",
            ReprocessorAgency = "BEX",
            StatusDate = DateTime.UtcNow,
        };

        // Get all property names from DTO class
        var props = typeof(SavePrnDetailsRequest)
                        .GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)
                        .ToList();


        var matchingProp = props.FirstOrDefault(x => string.Equals(x.Name, propertyName, StringComparison.InvariantCulture));
        matchingProp.Should().NotBeNull();

        // Set the value of the property (overriding the default value set above) to the value passed in as the argument to this method
        matchingProp.SetValue(dto, propertyValue);

        // Set validation error on the validator for the target input property
        var validationErrors = new[]
        {
            new ValidationFailure(propertyName, $"{propertyName} is not valid")
        };

        var validationResult = new ValidationResult(validationErrors);

        // Setup validator mock to return custom validation result
        _validatorSavePrnDetailsMock.Setup(x => x.Validate(It.IsAny<SavePrnDetailsRequest>()))
                                    .Returns(validationResult);



        _mockPrnService.Setup(s => s.SavePrnDetails(dto)).Returns(() => Task.CompletedTask);
        var result = await _systemUnderTest.SaveAsync(dto) as BadRequestObjectResult;

        result.Should().NotBeNull();
        result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        var errors = result.Value as IEnumerable<ValidationFailure>;
        errors.Should().NotBeNull();

        errors.Select(x => x.PropertyName)
            .Should()
            .Contain(propertyName);
    }

    [TestMethod]
    public async Task SavePrn_ReturnsInternalServerError_WhenServiceThrowsException()
    {
        var dto = new SavePrnDetailsRequest()
        {
            AccreditationNo = "ABC",
            AccreditationYear = 2018,
            CancelledDate = DateTime.UtcNow.AddDays(-1),
            DecemberWaste = true,
            EvidenceMaterial = "Aluminium",
            EvidenceNo = Guid.NewGuid().ToString(),
            EvidenceStatusCode = EprnStatus.AWAITINGACCEPTANCE,
            EvidenceTonnes = 5000,
            IssueDate = DateTime.UtcNow.AddDays(-5),
            IssuedByNPWDCode = "NPWD367742",
            IssuedByOrgName = "ANB",
            IssuedToEPRId = Guid.NewGuid(),
            IssuedToNPWDCode = "NPWD557742",
            IssuedToOrgName = "ZNZ",
            IssuerNotes = "no notes",
            IssuerRef = "ANB-1123",
            MaterialOperationCode = "R-PLA",
            ObligationYear = 2025,
            PrnSignatory = "Pat Anderson",
            PrnSignatoryPosition = "Director",
            ProducerAgency = "TTL",
            RecoveryProcessCode = "N11",
            ReprocessorAgency = "BEX",
            StatusDate = DateTime.UtcNow,
        };

        // setup mock validator
        var validationResult = new ValidationResult();

        _validatorSavePrnDetailsMock.Setup(x => x.Validate(It.IsAny<SavePrnDetailsRequest>()))
                                    .Returns(validationResult);

        // Setup mock PrnService
        _mockPrnService.Setup(s => s.SavePrnDetails(dto)).Throws<ApplicationException>();

        // Act
        var result = await _systemUnderTest.SaveAsync(dto) as ObjectResult;

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }

    [TestMethod]
    [DataRow("AccreditationNo", "ABC122378123123712381273123123123")]
    [DataRow("AccreditationYear", 25678)]
    [DataRow("EvidenceMaterial", "Material201223234234234234234")]
    [DataRow("EvidenceNo", "EV1231293812931231231231231")]
    [DataRow("IssuedByOrgName", "OrgName12313123123123123123123123123123213123123123123OrgName12313123123123123123123123123123213123123123123OrgName12313123123123123123123123123123213123123123123")]
    [DataRow("IssuedToOrgName", "OrgName12313123123123123123123123123123213123123123123OrgName12313123123123123123123123123123213123123123123OrgName12313123123123123123123123123123213123123123123")]
    [DataRow("ProducerAgency", "AgName12313123123123123123123123123123213123123123123OrgName12313123123123123123123123123123213123123123123OrgName12313123123123123123123123123123213123123123123")]
    [DataRow("RecoveryProcessCode", "Code123234342342342342342342342")]
    public async Task SavePrn_ReturnsStatusCode400_OnDataValidationFailure(string propertyName, object propertyValue)
    {
        var dto = new SavePrnDetailsRequest()
        {
            AccreditationNo = "ABC",
            AccreditationYear = 2018,
            CancelledDate = DateTime.UtcNow.AddDays(-1),
            DecemberWaste = true,
            EvidenceMaterial = "Aluminium",
            EvidenceNo = Guid.NewGuid().ToString(),
            EvidenceStatusCode = EprnStatus.AWAITINGACCEPTANCE,
            EvidenceTonnes = 5000,
            IssueDate = DateTime.UtcNow.AddDays(-5),
            IssuedByNPWDCode = "NPWD367742",
            IssuedByOrgName = "ANB",
            IssuedToEPRId = Guid.NewGuid(),
            IssuedToNPWDCode = "NPWD557742",
            IssuedToOrgName = "ZNZ",
            IssuerNotes = "no notes",
            IssuerRef = "ANB-1123",
            MaterialOperationCode = "R-PLA",
            ObligationYear = 2025,
            PrnSignatory = "Pat Anderson",
            PrnSignatoryPosition = "Director",
            ProducerAgency = "TTL",
            RecoveryProcessCode = "N11",
            ReprocessorAgency = "BEX",
            StatusDate = DateTime.UtcNow,
        };

        // Get all property names from DTO class
        var props = typeof(SavePrnDetailsRequest)
                        .GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)
                        .ToList();


        var matchingProp = props.FirstOrDefault(x => string.Equals(x.Name, propertyName, StringComparison.InvariantCulture));
        matchingProp.Should().NotBeNull();

        // Set the value of the property (overriding the default value set above) to the value passed in as the argument to this method
        matchingProp.SetValue(dto, propertyValue);

        // Set validation error on the validator for the target input property
        var validationErrors = new[]
        {
            new ValidationFailure(propertyName, $"{propertyName} is not valid")
        };

        var validationResult = new ValidationResult(validationErrors);

        // Setup validator mock to return custom validation result
        _validatorSavePrnDetailsMock.Setup(x => x.Validate(It.IsAny<SavePrnDetailsRequest>()))
                                    .Returns(validationResult);



        _mockPrnService.Setup(s => s.SavePrnDetails(dto)).Returns(() => Task.CompletedTask);
        var result = await _systemUnderTest.SaveAsync(dto) as BadRequestObjectResult;

        result.Should().NotBeNull();
        result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        var errors = result.Value as IEnumerable<ValidationFailure>;
        errors.Should().NotBeNull();

        errors.Select(x => x.PropertyName)
            .Should()
            .Contain(propertyName);
    }
    [TestMethod]
    public async Task PeprToNpwdSyncedPrns_ReturnsNotFound_WhenServiceThrowsNotFoundException()
    {
        var syncPrns = _fixture.CreateMany<InsertSyncedPrn>().ToList();

        _mockPrnService.Setup(s => s.InsertPeprNpwdSyncPrns(syncPrns)).Throws<NotFoundException>();

        var result = await _systemUnderTest.PeprToNpwdSyncedPrns(syncPrns) as ObjectResult;

        result.Should().NotBeNull();
        result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
    }

    [TestMethod]
    public async Task PeprToNpwdSyncedPrns_ReturnsInternalServer_WhenServiceThrowsUnexpectedException()
    {
        var syncPrns = _fixture.CreateMany<InsertSyncedPrn>().ToList();

        _mockPrnService.Setup(s => s.InsertPeprNpwdSyncPrns(syncPrns)).Throws<ArgumentNullException>();

        var result = await _systemUnderTest.PeprToNpwdSyncedPrns(syncPrns) as ObjectResult;

        result.Should().NotBeNull();
        result.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }

    [TestMethod]
    public async Task PeprToNpwdSyncedPrns_CallsService_ReturnOk()
    {
        var syncPrns = _fixture.CreateMany<InsertSyncedPrn>().ToList();

        _mockPrnService.Setup(s => s.InsertPeprNpwdSyncPrns(syncPrns)).Returns(Task.CompletedTask);

        var result = await _systemUnderTest.PeprToNpwdSyncedPrns(syncPrns);
        result.Should().BeOfType<OkResult>().Which.StatusCode.Should().Be(200);
        _mockPrnService.Verify(x => x.InsertPeprNpwdSyncPrns(syncPrns), Times.Once());
    }
}
