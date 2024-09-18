using AutoFixture;
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
    public async Task GetObligationCalculation_ReturnsOk_WhenObligationCalculationIsFound()
    {
        var obligationCalculation = _fixture.CreateMany<ObligationCalculationDto>().ToList();
        int validId = 1;
        _mockObligationCalculatorService.Setup(s => s.GetObligationCalculationByOrganisationId(validId)).ReturnsAsync(obligationCalculation);

        var result = await _systemUnderTest.GetObligationCalculation(validId) as OkObjectResult;

        result.Should().NotBeNull();
        result.StatusCode.Should().Be((int)HttpStatusCode.OK);
        result.Value.Should().BeEquivalentTo(obligationCalculation);
    }

    [TestMethod]
    public async Task CalculateAsync_ReturnsAccepted_WhenRequestIsValid()
    {
        var id = Guid.NewGuid();
        var request = _fixture.Create<SubmissionCalculationRequest>();
        _mockObligationCalculatorService.Setup(s => s.ProcessApprovedPomData(id, request)).Returns(Task.CompletedTask);

        var result = await _systemUnderTest.CalculateAsync(id, request) as AcceptedResult;

        result.Should().NotBeNull();
        result.StatusCode.Should().Be((int)HttpStatusCode.Accepted);
        _mockObligationCalculatorService.Verify(s => s.ProcessApprovedPomData(id, request), Times.Once);
    }

    [TestMethod]
    public async Task CalculateAsync_ReturnsBadRequest_WhenModelStateIsInvalid()
    {
        var id = Guid.NewGuid();
        var request = _fixture.Create<SubmissionCalculationRequest>();
        _systemUnderTest.ModelState.AddModelError("error", "Invalid request");

        var result = await _systemUnderTest.CalculateAsync(id, request) as BadRequestObjectResult;

        result.Should().NotBeNull();
        result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        result.Value.Should().BeOfType<SerializableError>(); // Check if ModelState is returned
        _mockObligationCalculatorService.Verify(s => s.ProcessApprovedPomData(It.IsAny<Guid>(), It.IsAny<SubmissionCalculationRequest>()), Times.Never);
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
