using EPR.Accreditation.API.Controllers;
using EPR.Accreditation.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EPR.Accreditation.API.UnitTests.Controllers
{
    [TestClass]
    public class SiteMaterialControllerTests
    {
        private SiteMaterialController _siteMaterialController;
        private Mock<IAccreditationService> _mockAccreditationService;

        [TestInitialize]
        public void Init()
        {
            _mockAccreditationService = new Mock<IAccreditationService>();
            _siteMaterialController = new SiteMaterialController(_mockAccreditationService.Object);
        }

        [TestMethod]
        public async Task GetSiteMaterial_WithValidMaterialId_ReturnsOkWithMaterial()
        {
            // Arrange
            var externalId = Guid.NewGuid();
            var materialExternalId = Guid.NewGuid();
            var expectedMaterial = new Common.Dtos.AccreditationMaterial();

            _mockAccreditationService.Setup(s =>
                s.GetMaterial(
                    externalId,
                    null,
                    materialExternalId))
                .ReturnsAsync(expectedMaterial);

            // Act
            var result = await _siteMaterialController.GetSiteMaterial(externalId, materialExternalId) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreSame(expectedMaterial, result.Value);

            _mockAccreditationService.Verify(s => s.GetMaterial(externalId, null, materialExternalId), Times.Once());
        }

        [TestMethod]
        public async Task GetSiteMaterial_WithInvalidMaterialId_ReturnsNotFound()
        {
            // Arrange
            var externalId = Guid.NewGuid();
            var materialExternalId = Guid.NewGuid();

            _mockAccreditationService.Setup(s =>
                s.GetMaterial(
                    externalId,
                    null,
                    materialExternalId))
                .ReturnsAsync((Common.Dtos.AccreditationMaterial)null);

            // Act
            var result = await _siteMaterialController.GetSiteMaterial(externalId, materialExternalId) as NotFoundResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(404, result.StatusCode);

            _mockAccreditationService.Verify(s => s.GetMaterial(externalId, null, materialExternalId), Times.Once());
        }

        [TestMethod]
        public async Task CreateSiteMaterial_WithValidInput_ReturnsOkWithMaterialId()
        {
            // Arrange
            var externalId = Guid.NewGuid();
            var accreditationMaterial = new Common.Dtos.AccreditationMaterial();
            var expectedMaterialId = Guid.NewGuid();

            _mockAccreditationService.Setup(s =>
            s.CreateMaterial(
                externalId,
                null,
                accreditationMaterial))
            .ReturnsAsync(expectedMaterialId);

            // Act
            var result = await _siteMaterialController.CreateSiteMaterial(externalId, accreditationMaterial) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual(expectedMaterialId, result.Value);

            _mockAccreditationService.Verify(s => s.CreateMaterial(externalId, null, accreditationMaterial), Times.Once());
        }

        [TestMethod]
        public async Task CreateSiteMaterial_WithNullInput_ReturnsBadRequest()
        {
            // Arrange
            var externalId = Guid.NewGuid();

            // Act
            var result = await _siteMaterialController.CreateSiteMaterial(externalId, null) as BadRequestResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);

            _mockAccreditationService.Verify(s =>
                s.CreateMaterial(It.IsAny<Guid>(), null, null), Times.Never());
        }

        [TestMethod]
        public async Task UpdateSiteMaterial_CallsServiceWithCorrectParametersAndReturnsOk()
        {
            // Arrange
            var externalId = Guid.NewGuid();
            var materialExternalId = Guid.NewGuid();
            var accreditationMaterial = new Common.Dtos.AccreditationMaterial();

            // Act
            var result = await _siteMaterialController
                .UpdateSiteMaterial(
                externalId,
                materialExternalId,
                accreditationMaterial) as OkResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);

            _mockAccreditationService
                .Verify(s => s.UpdateMaterail(externalId, null, materialExternalId, accreditationMaterial), Times.Once);
        }
    }
}
