namespace EPR.Accreditation.API.UnitTests.Controllers
{
    using EPR.Accreditation.API.Common.Dtos;
    using EPR.Accreditation.API.Controllers;
    using EPR.Accreditation.API.Services.Interfaces;
    using Microsoft.AspNetCore.Mvc;
    using Moq;

    [TestClass]
    public class OverseasSiteControllerTests
    {
        private OverseasSiteController _overseasSiteController;
        private Mock<IAccreditationService> _mockAcreditationService;

        [TestInitialize]
        public void Init()
        {
            _mockAcreditationService = new Mock<IAccreditationService>();
            _overseasSiteController = new OverseasSiteController(_mockAcreditationService.Object);
        }

        [TestMethod]
        public async Task CreateSite_ValidData_ReturnsOkResultWithSiteExternalId()
        {
            // Arrange
            var externalId = Guid.NewGuid();
            var overseasSite = new OverseasReprocessingSite();
            var expectedSiteExternalId = Guid.NewGuid();

            _mockAcreditationService.Setup(s =>
                s.CreateOverseasSite(
                    externalId,
                    overseasSite))
                .ReturnsAsync(expectedSiteExternalId);

            // Act
            var result = await _overseasSiteController.CreateSite(externalId, overseasSite) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual(expectedSiteExternalId, result.Value);

            _mockAcreditationService.Verify(s => s.CreateOverseasSite(externalId, overseasSite), Times.Once());
        }

        [TestMethod]
        public async Task GetSite_ExistingSite_ReturnsOkResultWithSite()
        {
            // Arrange
            var externalId = Guid.NewGuid();
            var siteExternalId = Guid.NewGuid();
            var expectedSite = new OverseasReprocessingSite();

            _mockAcreditationService.Setup(s =>
                s.GetOverseasSite(
                    externalId,
                    siteExternalId))
                .ReturnsAsync(expectedSite);

            // Act
            var result = await _overseasSiteController.GetSite(externalId, siteExternalId) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual(expectedSite, result.Value);

            _mockAcreditationService.Verify(s => s.GetOverseasSite(externalId, siteExternalId), Times.Once());
        }

        [TestMethod]
        public async Task GetSite_NonExistingSite_ReturnsNotFoundResult()
        {
            // Arrange
            var externalId = Guid.NewGuid();
            var siteExternalId = Guid.NewGuid();

            _mockAcreditationService.Setup(s =>
                s.GetOverseasSite(
                    externalId,
                    siteExternalId))
                .ReturnsAsync((OverseasReprocessingSite)null);

            // Act
            var result = await _overseasSiteController.GetSite(externalId, siteExternalId) as NotFoundResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(404, result.StatusCode);

            _mockAcreditationService.Verify(s => s.GetOverseasSite(externalId, siteExternalId), Times.Once());
        }

        [TestMethod]
        public async Task UpdateSite_ValidData_ReturnsOkResult()
        {
            // Arrange
            var overseasSite = new OverseasReprocessingSite();

            // Act
            var result = await _overseasSiteController.UpdateSite(overseasSite) as OkResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);

            _mockAcreditationService.Verify(s => s.UpdateOverseasSite(overseasSite), Times.Once());
        }
    }
}
