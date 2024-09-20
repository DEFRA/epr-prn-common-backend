using AutoFixture;
using EPR.PRN.Backend.API.Common.DTO;
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
    private static readonly IFixture _fixture = new Fixture();

    [TestInitialize]
    public void TestInitialize()
    {
        _mockPrnService = new Mock<IPrnService>();
        _mockLogger = new Mock<ILogger<PrnController>>();
        _mockObligationCalculatorService = new Mock<IObligationCalculatorService>();
        _systemUnderTest = new PrnController(_mockPrnService.Object, _mockLogger.Object, _mockObligationCalculatorService.Object);
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
    public async Task CalculateAsync_WhenOrganisationIdIsInvalid_ReturnsBadRequest()
    {
        var result = await _systemUnderTest.CalculateAsync(0, new List<SubmissionCalculationRequest>());

        result.Should().BeOfType<BadRequestObjectResult>();

        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult.Value.Should().BeEquivalentTo(new { message = "Invalid Organisation ID." });
    }

    [TestMethod]
    public async Task CalculateAsync_WhenRequestIsNull_ReturnsBadRequest()
    {
        var result = await _systemUnderTest.CalculateAsync(1, null);

        result.Should().BeOfType<BadRequestObjectResult>();

        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult.Value.Should().BeEquivalentTo(new { message = "Submission calculation request cannot be null or empty." });
    }

    [TestMethod]
    public async Task CalculateAsync_WhenRequestIsEmpty_ReturnsBadRequest()
    {
        var result = await _systemUnderTest.CalculateAsync(1, new List<SubmissionCalculationRequest>());

        result.Should().BeOfType<BadRequestObjectResult>();

        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult.Value.Should().BeEquivalentTo(new { message = "Submission calculation request cannot be null or empty." });
    }

    [TestMethod]
    public async Task CalculateAsync_WhenModelStateIsInvalid_ReturnsBadRequest()
    {
        _systemUnderTest.ModelState.AddModelError("Key", "Error message");

        var result = await _systemUnderTest.CalculateAsync(1, new List<SubmissionCalculationRequest> { new SubmissionCalculationRequest() });

        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [TestMethod]
    public async Task CalculateAsync_WhenCalculationFails_ReturnsInternalServerError()
    {
        var calculationResult = new CalculationResult { Success = false };
        _mockObligationCalculatorService
            .Setup(x => x.CalculateAsync(It.IsAny<int>(), It.IsAny<List<SubmissionCalculationRequest>>()))
            .ReturnsAsync(calculationResult);

        var result = await _systemUnderTest.CalculateAsync(1, new List<SubmissionCalculationRequest> { new SubmissionCalculationRequest() });

        result.Should().BeOfType<ObjectResult>().Which.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
    }

    [TestMethod]
    public async Task CalculateAsync_WhenCalculationSucceeds_ReturnsAccepted()
    {
        var Calculations = _fixture.CreateMany<ObligationCalculation>().ToList();
        var calculationResult = new CalculationResult
        {
            Success = true,
            Calculations = Calculations
        };

        _mockObligationCalculatorService
            .Setup(x => x.CalculateAsync(It.IsAny<int>(), It.IsAny<List<SubmissionCalculationRequest>>()))
            .ReturnsAsync(calculationResult);

        var result = await _systemUnderTest.CalculateAsync(1, new List<SubmissionCalculationRequest> { new SubmissionCalculationRequest() });

        result.Should().BeOfType<AcceptedResult>().Which.Value.Should().BeEquivalentTo(new
        {
            message = "Calculation successful.",
            data = calculationResult.Calculations
        });
    }

    [TestMethod]
    public async Task CalculateAsync_WhenTimeoutOccurs_ReturnsGatewayTimeout()
    {
        _mockObligationCalculatorService
            .Setup(x => x.CalculateAsync(It.IsAny<int>(), It.IsAny<List<SubmissionCalculationRequest>>()))
            .ThrowsAsync(new TimeoutException("Request timed out"));

        var result = await _systemUnderTest.CalculateAsync(1, new List<SubmissionCalculationRequest> { new SubmissionCalculationRequest() });

        result.Should().BeOfType<ObjectResult>().Which.StatusCode.Should().Be(StatusCodes.Status504GatewayTimeout);

        var objectResult = result as ObjectResult;
        objectResult.Value.Should().BeEquivalentTo(new { message = "Calculation timed out.", details = "Request timed out" });
    }


    [TestMethod]
    public async Task CalculateAsync_WhenUnexpectedErrorOccurs_ReturnsInternalServerError()
    {
        _mockObligationCalculatorService
            .Setup(x => x.CalculateAsync(It.IsAny<int>(), It.IsAny<List<SubmissionCalculationRequest>>()))
            .ThrowsAsync(new Exception("Unexpected error"));

        var result = await _systemUnderTest.CalculateAsync(1, new List<SubmissionCalculationRequest> { new SubmissionCalculationRequest() });

        result.Should().BeOfType<ObjectResult>().Which.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);

        var objectResult = result as ObjectResult;
        objectResult.Value.Should().BeEquivalentTo(new { message = "An error occurred during calculation.", details = "Unexpected error" });
    }

    [TestMethod]
    public async Task GetObligationCalculation_Should_ReturnBadRequest_WhenOrganisationIdIsInvalid()
    {
        // Arrange
        int invalidOrganisationId = -1;

        // Act
        var result = await _systemUnderTest.GetObligationCalculation(invalidOrganisationId);

        // Assert
        var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        badRequestResult.StatusCode.Should().Be(400);
        badRequestResult.Value.Should().Be($"Invalid Organisation Id : {invalidOrganisationId}. Organisation Id must be a positive integer.");
    }

    [TestMethod]
    public async Task GetObligationCalculation_Should_ReturnNotFound_WhenObligationCalculationDoesNotExist()
    {
        // Arrange
        int organisationId = 1;
        _mockObligationCalculatorService.Setup(x => x.GetObligationCalculationByOrganisationId(organisationId))
                    .ReturnsAsync((List<ObligationCalculationDto>)null); // Simulating null response

        // Act
        var result = await _systemUnderTest.GetObligationCalculation(organisationId);

        // Assert
        var notFoundResult = result.Should().BeOfType<NotFoundObjectResult>().Subject;
        notFoundResult.StatusCode.Should().Be(404);
        notFoundResult.Value.Should().Be($"Obligation calculation not found for Organisation Id : {organisationId}");
    }

    [TestMethod]
    public async Task GetObligationCalculation_Should_ReturnOk_WhenObligationCalculationExists()
    {
        // Arrange
        int organisationId = 1;
        var obligationCalculationDto = new List<ObligationCalculationDto>
            {
                new ObligationCalculationDto { /* Initialize properties if needed */ }
            };
        _mockObligationCalculatorService.Setup(x => x.GetObligationCalculationByOrganisationId(organisationId))
                    .ReturnsAsync(obligationCalculationDto);

        // Act
        var result = await _systemUnderTest.GetObligationCalculation(organisationId);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
        okResult.Value.Should().BeEquivalentTo(obligationCalculationDto);
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
}
