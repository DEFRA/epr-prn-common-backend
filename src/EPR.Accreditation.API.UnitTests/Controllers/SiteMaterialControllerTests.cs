namespace EPR.Accreditation.API.UnitTests.Controllers
{
    using DTO = EPR.Accreditation.API.Common.Dtos;
    using EPR.Accreditation.API.Controllers;
    using EPR.Accreditation.API.Services.Interfaces;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using Enums = EPR.Accreditation.API.Common.Enums;

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
            var id = Guid.NewGuid();
            var materialid = Guid.NewGuid();
            var expectedMaterial = new Common.Dtos.Response.AccreditationMaterial();

            _mockAccreditationService.Setup(s =>
                s.GetMaterial(
                    id,
                    materialid))
                .ReturnsAsync(expectedMaterial);

            // Act
            var result = await _siteMaterialController.GetSiteMaterial(id, materialid) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreSame(expectedMaterial, result.Value);

            _mockAccreditationService.Verify(
                s =>
                    s.GetMaterial(
                        id,
                        materialid),
                Times.Once());
        }

        [TestMethod]
        public async Task GetSiteMaterial_WithInvalidMaterialId_ReturnsNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            var materialid = Guid.NewGuid();

            _mockAccreditationService.Setup(s =>
                s.GetMaterial(
                    id,
                    materialid))
                .ReturnsAsync((DTO.Response.AccreditationMaterial)null);

            // Act
            var result = await _siteMaterialController.GetSiteMaterial(id, materialid) as NotFoundResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(404, result.StatusCode);

            _mockAccreditationService.Verify(
                s =>
                    s.GetMaterial(
                        id,
                        materialid),
                Times.Once());
        }

        [TestMethod]
        public async Task CreateSiteMaterial_WithValidInput_ReturnsOkWithMaterialId()
        {
            // Arrange
            var id = Guid.NewGuid();
            var materialid = Guid.NewGuid();
            var accreditationMaterial = new Common.Dtos.Request.AccreditationMaterial();
            var expectedMaterialId = Guid.NewGuid();

            _mockAccreditationService.Setup(
                s =>
                    s.CreateMaterial(
                        id,
                        materialid,
                        Enums.OperatorType.Reprocessor,
                        accreditationMaterial))
            .ReturnsAsync(expectedMaterialId);

            // Act
            var result = await _siteMaterialController.CreateSiteMaterial(
                id,
                materialid,
                accreditationMaterial) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual(expectedMaterialId, result.Value);

            _mockAccreditationService.Verify(
                s =>
                    s.CreateMaterial(
                        id,
                        materialid,
                        Enums.OperatorType.Reprocessor,
                        accreditationMaterial),
                Times.Once());
        }

        [TestMethod]
        public async Task CreateSiteMaterial_WithNullInput_ReturnsBadRequest()
        {
            // Arrange
            var id = Guid.NewGuid();
            var materialid = Guid.NewGuid();

            // Act
            var result = await _siteMaterialController.CreateSiteMaterial(
                id,
                materialid,
                null) as BadRequestResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);

            _mockAccreditationService.Verify(s =>
                s.CreateMaterial(
                    It.IsAny<Guid>(),
                    It.IsAny<Guid>(),
                    It.IsAny<Enums.OperatorType>(),
                    null),
                Times.Never());
        }

        [TestMethod]
        public async Task UpdateSiteMaterial_CallsServiceWithCorrectParametersAndReturnsOk()
        {
            // Arrange
            var id = Guid.NewGuid();
            var materialid = Guid.NewGuid();
            var accreditationMaterial = new Common.Dtos.Request.AccreditationMaterial();

            // Act
            var result = await _siteMaterialController
                .UpdateSiteMaterial(
                id,
                materialid,
                accreditationMaterial) as OkResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);

            _mockAccreditationService.Verify(
                s =>
                    s.UpdateMaterail(
                        id,
                        materialid,
                        accreditationMaterial),
                Times.Once);
        }
    }
}
