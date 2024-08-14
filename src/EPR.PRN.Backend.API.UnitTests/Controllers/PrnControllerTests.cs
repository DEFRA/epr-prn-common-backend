using AutoFixture.MSTest;
using EPR.PRN.Backend.API.Common.DTO;
using EPR.PRN.Backend.API.Controllers;
using EPR.PRN.Backend.API.Helpers;
using EPR.PRN.Backend.API.Services.Interfaces;
using EPR.PRN.Backend.Obligation.DTO;
using EPR.PRN.Backend.Obligation.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;

namespace EPR.PRN.Backend.API.UnitTests.Services
{
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
        public async Task UpdatePrnStatus_ReturnsOk_WhenUpdateSuccessfully(Guid orgId, List<PrnUpdateStatusDto> prnUpdates)
        {
            _mockPrnService.Setup(s => s.UpdateStatus(orgId, prnUpdates)).Returns(Task.CompletedTask);

            var result = await _systemUnderTest.UpdatePrnStatus(orgId, prnUpdates) as OkResult;

            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }

        [TestMethod]
        [AutoData]
        public async Task UpdatePrnStatus_ReturnsConflict_WhenServiceThrowsConflictException(Guid orgId, List<PrnUpdateStatusDto> prnUpdates)
        {
            _mockPrnService.Setup(s => s.UpdateStatus(orgId, prnUpdates)).Throws<ConflictException>();

            var result = await _systemUnderTest.UpdatePrnStatus(orgId, prnUpdates) as ObjectResult;

            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.Conflict);
        }

        [TestMethod]
        [AutoData]
        public async Task UpdatePrnStatus_ReturnsNotFound_WhenServiceThrowsNotFoundException(Guid orgId, List<PrnUpdateStatusDto> prnUpdates)
        {
            _mockPrnService.Setup(s => s.UpdateStatus(orgId, prnUpdates)).Throws<NotFoundException>();

            var result = await _systemUnderTest.UpdatePrnStatus(orgId, prnUpdates) as ObjectResult;

            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        [TestMethod]
        [AutoData]
        public async Task UpdatePrnStatus_ReturnsInternalServer_WhenServiceThrowsUnexpectedException(Guid orgId, List<PrnUpdateStatusDto> prnUpdates)
        {
            _mockPrnService.Setup(s => s.UpdateStatus(orgId, prnUpdates)).Throws<ArgumentNullException>();

            var result = await _systemUnderTest.UpdatePrnStatus(orgId, prnUpdates) as ObjectResult;

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
            result.Value.Should().Be($"Invalid obligation ID : {invalidId}. ID must be a positive integer.");
        }

        [TestMethod]
        public async Task GetObligationCalculation_ReturnsNotFound_WhenObligationCalculationNotFound()
        {
            int validId = 1;
            _mockObligationCalculatorService.Setup(s => s.GetObligationCalculationById(validId)).ReturnsAsync((List<ObligationCalculationDto>)null);

            var result = await _systemUnderTest.GetObligationCalculation(validId) as NotFoundObjectResult;

            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
            result.Value.Should().Be($"Obligation calculation not found for Id : {validId}");
        }

        [TestMethod]
        [AutoData]
        public async Task GetObligationCalculation_ReturnsOk_WhenObligationCalculationIsFound(List<ObligationCalculationDto> obligationCalculation)
        {
            int validId = 1;
            _mockObligationCalculatorService.Setup(s => s.GetObligationCalculationById(validId)).ReturnsAsync(obligationCalculation);

            var result = await _systemUnderTest.GetObligationCalculation(validId) as OkObjectResult;

            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
            result.Value.Should().BeEquivalentTo(obligationCalculation);
        }
    }
}
