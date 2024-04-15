using EPR.Accreditation.API.Controllers;
using EPR.Accreditation.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EPR.Accreditation.API.UnitTests.Controllers
{
    [TestClass]
    public class SaveAndComeBackControllerTests
    {
        private SaveAndComeBackController _saveAndComeBackController;
        private Mock<IAccreditationService> _mockAccreditationService;

        [TestInitialize]
        public void Init()
        {
            _mockAccreditationService = new Mock<IAccreditationService>();
            _saveAndComeBackController = new SaveAndComeBackController(_mockAccreditationService.Object);
        }

        [TestMethod]
        public async Task GetSaveAndComeBack_WithValidId_ReturnsOkWithSaveAndComeBack()
        {
            // Arrange
            var id = Guid.NewGuid();
            var expectedSaveAndComeBack = new Common.Dtos.SaveAndComeBack();

            _mockAccreditationService.Setup(s => s.GetSaveAndComeBack(id)).ReturnsAsync(expectedSaveAndComeBack);

            // Act
            var result = await _saveAndComeBackController.GetSaveAndComeBack(id) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreSame(expectedSaveAndComeBack, result.Value);

            _mockAccreditationService.Verify(s => s.GetSaveAndComeBack(id), Times.Once());
        }

        [TestMethod]
        public async Task GetSaveAndComeBack_ReturnsNotFound_WithInvalidId()
        {
            // Arrange
            var id = Guid.NewGuid();

            _mockAccreditationService.Setup(s =>
                s.GetSaveAndComeBack(id))
                .ReturnsAsync((Common.Dtos.SaveAndComeBack)null);

            // Act
            var result = await _saveAndComeBackController.GetSaveAndComeBack(id) as NotFoundResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(404, result.StatusCode);

            _mockAccreditationService.Verify(s => s.GetSaveAndComeBack(id), Times.Once);
        }

        [TestMethod]
        public async Task AddSaveAndComeBack_WithValidInput_ReturnsOk()
        {
            // Arrange
            var id = Guid.NewGuid();
            var saveAndComeBack = new Common.Dtos.SaveAndComeBack();

            // Act
            var result = await _saveAndComeBackController.AddSaveAndComeBack(id, saveAndComeBack) as OkResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);

            _mockAccreditationService.Verify(s => s.AddSaveAndComeBack(id, saveAndComeBack), Times.Once);
        }

        [TestMethod]
        public async Task AddSaveAndComeBack_WithNullInput_ReturnsBadRequest()
        {
            // Arrange
            var id = Guid.NewGuid();

            // Act
            var result = await _saveAndComeBackController.AddSaveAndComeBack(id, null) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
            Assert.AreEqual("No saveAndContinue record supplied", result.Value);

            _mockAccreditationService.Verify(s =>
                s.AddSaveAndComeBack(
                    It.IsAny<Guid>(),
                    It.IsAny<Common.Dtos.SaveAndComeBack>())
                , Times.Never);
        }

        [TestMethod]
        public async Task DeleteSaveAndComeBack_CallsServiceWithCorrectParameterAndReturnsOk()
        {
            // Arrange
            var id = Guid.NewGuid();

            // Act
            var result = await _saveAndComeBackController.DeleteSaveAndComeBack(id) as OkResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);

            _mockAccreditationService.Verify(s => s.DeleteSaveAndComeBack(id), Times.Once);
        }
    }
}
