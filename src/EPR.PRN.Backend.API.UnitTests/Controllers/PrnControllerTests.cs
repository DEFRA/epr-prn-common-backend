using AutoFixture;
using EPR.PRN.Backend.API.Common.DTO;
using EPR.PRN.Backend.API.Configs;
using EPR.PRN.Backend.API.Controllers;
using EPR.PRN.Backend.API.Helpers;
using EPR.PRN.Backend.API.Services.Interfaces;
using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Obligation.DTO;
using EPR.PRN.Backend.Obligation.Interfaces;
using EPR.PRN.Backend.Obligation.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        _systemUnderTest = new PrnController(_mockPrnService.Object, _mockLogger.Object, _mockObligationCalculatorService.Object, _configMock.Object);
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
    public async Task GetAllPrnByOrganisationId_ReturnsNotFound_WhenPrnIdDoesntExists()
    {
        var orgId = Guid.NewGuid();
        _mockPrnService.Setup(s => s.GetAllPrnByOrganisationId(orgId)).ReturnsAsync([]);

        var result = await _systemUnderTest.GetAllPrnByOrganisationId(orgId) as NotFoundResult;

        result.Should().NotBeNull();
        result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
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

        var result = await _systemUnderTest.CalculateAsync(organisationId, new List<SubmissionCalculationRequest>());

        result.Should().BeOfType<BadRequestObjectResult>();

        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult.Value.Should().BeEquivalentTo(new { message = "Submission calculation request cannot be null or empty." });
    }

    [TestMethod]
    public async Task CalculateAsync_WhenModelStateIsInvalid_ReturnsBadRequest()
    {
        var organisationId = Guid.NewGuid();

        _systemUnderTest.ModelState.AddModelError("Key", "Error message");

        var result = await _systemUnderTest.CalculateAsync(organisationId, new List<SubmissionCalculationRequest> { new SubmissionCalculationRequest() });

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

        var result = await _systemUnderTest.CalculateAsync(organisationId, new List<SubmissionCalculationRequest> { new SubmissionCalculationRequest() });

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

        var result = await _systemUnderTest.CalculateAsync(organisationId, new List<SubmissionCalculationRequest> { new SubmissionCalculationRequest() });

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

        var result = await _systemUnderTest.CalculateAsync(organisationId, new List<SubmissionCalculationRequest> { new SubmissionCalculationRequest() });

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

        var result = await _systemUnderTest.CalculateAsync(organisationId, new List<SubmissionCalculationRequest> { new SubmissionCalculationRequest() });

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
}
