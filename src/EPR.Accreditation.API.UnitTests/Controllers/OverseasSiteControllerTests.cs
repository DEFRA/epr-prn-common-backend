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
        public async Task CreateSite_ValidData_ReturnsOkResultWithSiteid()
        {
            // Arrange
            var id = Guid.NewGuid();
            var materialId = Guid.NewGuid();
            var overseasSite = new OverseasReprocessingSite();
            var expectedSiteid = Guid.NewGuid();

            _mockAcreditationService.Setup(s =>
                s.CreateOverseasSite(
                    id,
                    materialId,
                    overseasSite))
                .ReturnsAsync(expectedSiteid);

            // Act
            var result = await _overseasSiteController.CreateSite(id, materialId, overseasSite) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual(expectedSiteid, result.Value);

            _mockAcreditationService.Verify(s => s.CreateOverseasSite(id, materialId, overseasSite), Times.Once());
        }

        [TestMethod]
        public async Task GetSite_ExistingSite_ReturnsOkResultWithSite()
        {
            // Arrange
            var id = Guid.NewGuid();
            var siteid = Guid.NewGuid();
            var expectedSite = new OverseasReprocessingSite();

            _mockAcreditationService.Setup(s =>
                s.GetOverseasSite(
                    id,
                    siteid))
                .ReturnsAsync(expectedSite);

            // Act
            var result = await _overseasSiteController.GetSite(id, siteid) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual(expectedSite, result.Value);

            _mockAcreditationService.Verify(s => s.GetOverseasSite(id, siteid), Times.Once());
        }

        [TestMethod]
        public async Task GetSite_NonExistingSite_ReturnsNotFoundResult()
        {
            // Arrange
            var id = Guid.NewGuid();
            var siteid = Guid.NewGuid();

            _mockAcreditationService.Setup(s =>
                s.GetOverseasSite(
                    id,
                    siteid))
                .ReturnsAsync((OverseasReprocessingSite)null);

            // Act
            var result = await _overseasSiteController.GetSite(id, siteid) as NotFoundResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(404, result.StatusCode);

            _mockAcreditationService.Verify(s => s.GetOverseasSite(id, siteid), Times.Once());
        }

        [TestMethod]
        public async Task UpdateSite_ValidData_ReturnsOkResult()
        {
            // Arrange
            var id = Guid.NewGuid();
            var siteId = Guid.NewGuid();
            var overseasSite = new OverseasReprocessingSite();

            // Act
            var result = await _overseasSiteController.UpdateSite(
                id,
                siteId,
                overseasSite) as OkResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);

            _mockAcreditationService.Verify(s =>
                s.UpdateOverseasSite(
                    id,
                    siteId,
                    overseasSite),
                Times.Once());
        }
    }
}
