using AutoFixture.MSTest;
using EPR.PRN.Backend.API.Common.DTO;
using EPR.PRN.Backend.API.Controllers;
using EPR.PRN.Backend.API.Helpers;
using EPR.PRN.Backend.API.Services.Interfaces;
using EPR.PRN.Backend.Obligation.DTO;
using EPR.PRN.Backend.Obligation.Interfaces;
using EPR.PRN.Backend.Obligation.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace EPR.PRN.Backend.API.UnitTests.Services;

[ExcludeFromCodeCoverage]
[TestClass]
public class PrnControllerTests
{
    private PrnController _systemUnderTest;
    private Mock<IPrnService> _mockPrnService;
    private Mock<ILogger<PrnController>> _mockLogger;
    private Mock<IObligationCalculatorService> _mockObligationCalculatorService;

    [TestInitialize]
    public void TestInitialize()
    {
        _mockPrnService = new Mock<IPrnService>();
        _mockLogger = new Mock<ILogger<PrnController>>();
        _mockObligationCalculatorService = new Mock<IObligationCalculatorService>();
        _systemUnderTest = new PrnController(_mockPrnService.Object, _mockLogger.Object, _mockObligationCalculatorService.Object);
    }

    [TestMethod]
    [AutoData]
    public async Task GetPrn_ReturnsOkWithPrn_WhenValidPrnId(Guid orgId, Guid prnId, PrnDto expectedPrn)
    {
        _mockPrnService.Setup(s => s.GetPrnForOrganisationById(orgId, prnId)).ReturnsAsync(expectedPrn);

        var result = await _systemUnderTest.GetPrn(orgId, prnId) as OkObjectResult;

        result.Value.Should().BeEquivalentTo(expectedPrn);
    }

    [TestMethod]
    [AutoData]
    public async Task GetPrn_ReturnsNotFound_WhenPrnIdDoesntExists(Guid orgId, Guid prnId)
    {
        _mockPrnService.Setup(s => s.GetPrnForOrganisationById(orgId, prnId)).ReturnsAsync((PrnDto)null);

        var result = await _systemUnderTest.GetPrn(orgId, prnId) as NotFoundResult;

        result.Should().NotBeNull();
        result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
    }

    [TestMethod]
    [AutoData]
    public async Task GetAllPrnByOrganisationId_ReturnsOkWithPrns_WhenValidOrgId(Guid orgId, List<PrnDto> expectedPrns)
    {
        _mockPrnService.Setup(s => s.GetAllPrnByOrganisationId(orgId)).ReturnsAsync(expectedPrns);

        var result = await _systemUnderTest.GetAllPrnByOrganisationId(orgId) as OkObjectResult;

        result.Value.Should().BeEquivalentTo(expectedPrns);
    }

    [TestMethod]
    [AutoData]
    public async Task GetAllPrnByOrganisationId_ReturnsNotFound_WhenPrnIdDoesntExists(Guid orgId)
    {
        _mockPrnService.Setup(s => s.GetAllPrnByOrganisationId(orgId)).ReturnsAsync([]);

        var result = await _systemUnderTest.GetAllPrnByOrganisationId(orgId) as NotFoundResult;

        result.Should().NotBeNull();
        result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
    }

    [TestMethod]
    [AutoData]
    public async Task UpdatePrnStatus_ReturnsOk_WhenUpdateSuccessfully(Guid orgId, Guid userId, List<PrnUpdateStatusDto> prnUpdates)
    {
        _mockPrnService.Setup(s => s.UpdateStatus(orgId, userId, prnUpdates)).Returns(Task.CompletedTask);

        var result = await _systemUnderTest.UpdatePrnStatus(orgId, userId, prnUpdates) as OkResult;

        result.Should().NotBeNull();
        result.StatusCode.Should().Be((int)HttpStatusCode.OK);
    }

    [TestMethod]
    [AutoData]
    public async Task UpdatePrnStatus_ReturnsConflict_WhenServiceThrowsConflictException(Guid orgId, Guid userId, List<PrnUpdateStatusDto> prnUpdates)
    {
        _mockPrnService.Setup(s => s.UpdateStatus(orgId, userId, prnUpdates)).Throws<ConflictException>();

        var result = await _systemUnderTest.UpdatePrnStatus(orgId, userId, prnUpdates) as ObjectResult;

        result.Should().NotBeNull();
        result.StatusCode.Should().Be((int)HttpStatusCode.Conflict);
    }

    [TestMethod]
    [AutoData]
    public async Task UpdatePrnStatus_ReturnsNotFound_WhenServiceThrowsNotFoundException(Guid orgId, Guid userId, List<PrnUpdateStatusDto> prnUpdates)
    {
        _mockPrnService.Setup(s => s.UpdateStatus(orgId, userId, prnUpdates)).Throws<NotFoundException>();

        var result = await _systemUnderTest.UpdatePrnStatus(orgId, userId, prnUpdates) as ObjectResult;

        result.Should().NotBeNull();
        result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
    }

    [TestMethod]
    [AutoData]
    public async Task UpdatePrnStatus_ReturnsInternalServer_WhenServiceThrowsUnexpectedException(Guid orgId, Guid userId, List<PrnUpdateStatusDto> prnUpdates)
    {
        _mockPrnService.Setup(s => s.UpdateStatus(orgId, userId, prnUpdates)).Throws<ArgumentNullException>();

        var result = await _systemUnderTest.UpdatePrnStatus(orgId, userId, prnUpdates) as ObjectResult;

        result.Should().NotBeNull();
        result.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }

    [TestMethod]
    public async Task GetObligationCalculation_ReturnsBadRequest_WhenIdIsInvalid()
    {
        int invalidId = -1;

        var result = await _systemUnderTest.GetObligationCalculation(invalidId) as BadRequestObjectResult;

        result.Should().NotBeNull();
        result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        result.Value.Should().Be($"Invalid Organisation Id : {invalidId}. Organisation Id must be a positive integer.");
    }

    [TestMethod]
    public async Task GetObligationCalculation_ReturnsNotFound_WhenObligationCalculationNotFound()
    {
        int validId = 1;
        _mockObligationCalculatorService.Setup(s => s.GetObligationCalculationByOrganisationId(validId)).ReturnsAsync((List<ObligationCalculationDto>)null);

        var result = await _systemUnderTest.GetObligationCalculation(validId) as NotFoundObjectResult;

        result.Should().NotBeNull();
        result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        result.Value.Should().Be($"Obligation calculation not found for Organisation Id : {validId}");
    }

    [TestMethod]
    [AutoData]
    public async Task GetObligationCalculation_ReturnsOk_WhenObligationCalculationIsFound(List<ObligationCalculationDto> obligationCalculation)
    {
        int validId = 1;
        _mockObligationCalculatorService.Setup(s => s.GetObligationCalculationByOrganisationId(validId)).ReturnsAsync(obligationCalculation);

        var result = await _systemUnderTest.GetObligationCalculation(validId) as OkObjectResult;

        result.Should().NotBeNull();
        result.StatusCode.Should().Be((int)HttpStatusCode.OK);
        result.Value.Should().BeEquivalentTo(obligationCalculation);
    }

    [TestMethod]
    [AutoData]
    public async Task CalculateAsync_ReturnsAccepted_WhenRequestIsValid(Guid id, SubmissionCalculationRequest request)
    {
        _mockObligationCalculatorService.Setup(s => s.ProcessApprovedPomData(id, request)).Returns(Task.CompletedTask);

        var result = await _systemUnderTest.CalculateAsync(id, request) as AcceptedResult;

        result.Should().NotBeNull();
        result.StatusCode.Should().Be((int)HttpStatusCode.Accepted);
        _mockObligationCalculatorService.Verify(s => s.ProcessApprovedPomData(id, request), Times.Once);
    }

    [TestMethod]
    [AutoData]
    public async Task CalculateAsync_ReturnsBadRequest_WhenModelStateIsInvalid(Guid id, SubmissionCalculationRequest request)
    {
        _systemUnderTest.ModelState.AddModelError("error", "Invalid request");

        var result = await _systemUnderTest.CalculateAsync(id, request) as BadRequestObjectResult;

        result.Should().NotBeNull();
        result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        result.Value.Should().BeOfType<SerializableError>(); // Check if ModelState is returned
        _mockObligationCalculatorService.Verify(s => s.ProcessApprovedPomData(It.IsAny<Guid>(), It.IsAny<SubmissionCalculationRequest>()), Times.Never);
    }
}
