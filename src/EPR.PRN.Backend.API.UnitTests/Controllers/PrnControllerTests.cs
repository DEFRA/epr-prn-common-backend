using AutoFixture.MSTest;
using EPR.PRN.Backend.API.Common.DTO;
using EPR.PRN.Backend.API.Controllers;
using EPR.PRN.Backend.API.Helpers;
using EPR.PRN.Backend.API.Services.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
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
        private Mock<IUrlHelper> _mockUrlHelper;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockUrlHelper = new Mock<IUrlHelper>();
            _mockPrnService = new Mock<IPrnService>();
            _mockLogger = new Mock<ILogger<PrnController>>();
            _mockUrlHelper = new Mock<IUrlHelper>();
            _systemUnderTest = new PrnController(_mockPrnService.Object, _mockLogger.Object, _mockUrlHelper.Object);
        }

        [TestMethod]
        [AutoData]
        public async Task GetPrn_ReturnsOkWithPrn_WhenValidPrnId(Guid orgId, Guid prnId, PrnDTo expectedPrn)
        {
            _mockPrnService.Setup(s => s.GetPrnForOrganisationById(orgId, prnId)).ReturnsAsync(expectedPrn);

            var result = await _systemUnderTest.GetPrn(orgId, prnId) as OkObjectResult;

            result.Value.Should().BeEquivalentTo(expectedPrn);
        }

        [TestMethod]
        [AutoData]
        public async Task GetPrn_ReturnsNotFound_WhenPrnIdDoesntExists(Guid orgId, Guid prnId)
        {
            _mockPrnService.Setup(s => s.GetPrnForOrganisationById(orgId, prnId)).ReturnsAsync((PrnDTo?)null);

            var result = await _systemUnderTest.GetPrn(orgId, prnId) as NotFoundResult;

            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        [TestMethod]
        [AutoData]
        public async Task GetAllPrnByOrganisationId_ReturnsOkWithPrns_WhenValidOrgId(Guid orgId, List<PrnDTo> expectedPrns)
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
            _mockUrlHelper.Setup(x => x.Content(It.IsAny<string>())).Returns<string>(x => "prn/status");
            _mockPrnService.Setup(s => s.UpdateStatus(orgId, prnUpdates)).Throws<ConflictException>();

            var result = await _systemUnderTest.UpdatePrnStatus(orgId, prnUpdates) as ObjectResult;

            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.Conflict);
        }

        [TestMethod]
        [AutoData]
        public async Task UpdatePrnStatus_ReturnsNotFound_WhenServiceThrowsNotFoundException(Guid orgId, List<PrnUpdateStatusDto> prnUpdates)
        {
            _mockUrlHelper.Setup(x => x.Content(It.IsAny<string>())).Returns<string>(x => "prn/status");
            _mockPrnService.Setup(s => s.UpdateStatus(orgId, prnUpdates)).Throws<NotFoundException>();

            var result = await _systemUnderTest.UpdatePrnStatus(orgId, prnUpdates) as ObjectResult;

            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        [TestMethod]
        [AutoData]
        public async Task UpdatePrnStatus_ReturnsInternalServer_WhenServiceThrowsUnexpectedException(Guid orgId, List<PrnUpdateStatusDto> prnUpdates)
        {
            _mockUrlHelper.Setup(x => x.Content(It.IsAny<string>())).Returns<string>(x => "prn/status");
            _mockPrnService.Setup(s => s.UpdateStatus(orgId, prnUpdates)).Throws<ArgumentNullException>();

            var result = await _systemUnderTest.UpdatePrnStatus(orgId, prnUpdates) as ObjectResult;

            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}
