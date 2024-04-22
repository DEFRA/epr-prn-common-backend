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
        public async Task CreateSite_WithValidInput_ReturnsOkWithSiteid()
        {
            // Arrange
            var id = Guid.NewGuid();
            var site = new Common.Dtos.Site();
            var expectedSiteid = Guid.NewGuid();

            _mockAccreditationService.Setup(s => s.CreateSite(id, site)).ReturnsAsync(expectedSiteid);

            // Act
            var result = await _siteController.CreateSite(id, site) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual(expectedSiteid, result.Value);

            _mockAccreditationService.Verify(s => s.CreateSite(id, site), Times.Once());
        }

        [TestMethod]
        public async Task GetSite_WithValidId_ReturnsOkWithSite()
        {
            // Arrange
            var id = Guid.NewGuid();
            var expectedSite = new Common.Dtos.Site();

            _mockAccreditationService.Setup(s => s.GetSite(id)).ReturnsAsync(expectedSite);

            // Act
            var result = await _siteController.GetSite(id) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreSame(expectedSite, result.Value);

            _mockAccreditationService.Verify(s => s.GetSite(id), Times.Once());
        }

        [TestMethod]
        public async Task GetSite_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();

            _mockAccreditationService.Setup(s => s.GetSite(id)).ReturnsAsync((Common.Dtos.Site)null);

            // Act
            var result = await _siteController.GetSite(id) as NotFoundResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(404, result.StatusCode);

            _mockAccreditationService.Verify(s => s.GetSite(id), Times.Once());
        }

        [TestMethod]
        public async Task UpdateSite_CallsServiceWithCorrectParametersAndReturnsOk()
        {
            // Arrange
            var id = Guid.NewGuid();
            var site = new Common.Dtos.Site();

            // Act
            var result = await _siteController.UpdateSite(id, site) as OkResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);

            _mockAccreditationService.Verify(s => s.UpdateSite(id, site), Times.Once);
        }
    }
}
