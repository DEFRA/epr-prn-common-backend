using EPR.Accreditation.API.Controllers;
using EPR.Accreditation.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EPR.Accreditation.API.UnitTests.Controllers
{
    [TestClass]
    public class SiteControllerTests
    {
        private SiteController _siteController;
        private Mock<IAccreditationService> _mockAccreditationService;

        [TestInitialize]
        public void Init()
        {
            _mockAccreditationService = new Mock<IAccreditationService>();
            _siteController = new SiteController(_mockAccreditationService.Object);
        }

        [TestMethod]
        public async Task CreateSite_WithValidInput_ReturnsOkWithSiteExternalId()
        {
            // Arrange
            var externalId = Guid.NewGuid();
            var site = new Common.Dtos.Site();
            var expectedSiteExternalId = Guid.NewGuid();

            _mockAccreditationService.Setup(s => s.CreateSite(externalId, site)).ReturnsAsync(expectedSiteExternalId);

            // Act
            var result = await _siteController.CreateSite(externalId, site) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual(expectedSiteExternalId, result.Value);

            _mockAccreditationService.Verify(s => s.CreateSite(externalId, site), Times.Once());
        }

        [TestMethod]
        public async Task GetSite_WithValidId_ReturnsOkWithSite()
        {
            // Arrange
            var externalId = Guid.NewGuid();
            var expectedSite = new Common.Dtos.Site();

            _mockAccreditationService.Setup(s => s.GetSite(externalId)).ReturnsAsync(expectedSite);

            // Act
            var result = await _siteController.GetSite(externalId) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreSame(expectedSite, result.Value);

            _mockAccreditationService.Verify(s => s.GetSite(externalId), Times.Once());
        }

        [TestMethod]
        public async Task GetSite_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var externalId = Guid.NewGuid();

            _mockAccreditationService.Setup(s => s.GetSite(externalId)).ReturnsAsync((Common.Dtos.Site)null);

            // Act
            var result = await _siteController.GetSite(externalId) as NotFoundResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(404, result.StatusCode);

            _mockAccreditationService.Verify(s => s.GetSite(externalId), Times.Once());
        }

        [TestMethod]
        public async Task UpdateSite_CallsServiceWithCorrectParametersAndReturnsOk()
        {
            // Arrange
            var externalId = Guid.NewGuid();
            var site = new Common.Dtos.Site();

            // Act
            var result = await _siteController.UpdateSite(externalId, site) as OkResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);

            _mockAccreditationService.Verify(s => s.UpdateSite(externalId, site), Times.Once);
        }
    }
}
