using EPR.Accreditation.API.Controllers;
using EPR.Accreditation.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EPR.Accreditation.API.UnitTests.Controllers
{
    [TestClass]
    public class AccreditationControllerTests
    {
        private AccreditationController _accredicationController;
        private Mock<IAccreditationService> _mockAccreditationService;

        [TestInitialize]
        public void Init()
        {
            _mockAccreditationService = new Mock<IAccreditationService>();
            _accredicationController = new AccreditationController(_mockAccreditationService.Object);
        }

        [TestMethod]
        public async Task GetAccreditation_ReturnsOkWithAccreditation_WithValidId_()
        {
            // Arrange
            var id = Guid.NewGuid();
            var expectedAccreditation = new API.Common.Dtos.Accreditation();

            _mockAccreditationService.Setup(s => s.GetAccreditation(id)).ReturnsAsync(expectedAccreditation);

            // Act
            var result = await _accredicationController.GetAccreditation(id) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreSame(expectedAccreditation, result.Value);

            _mockAccreditationService.Verify(s => s.GetAccreditation(id), Times.Once());
        }

        [TestMethod]
        public async Task UpdateAccreditation_WithValidInput_ReturnsOk()
        {
            // Arrange
            var id = Guid.NewGuid();
            var accreditation = new Common.Dtos.Accreditation();

            // Act
            var result = await _accredicationController.UpdateAccreditation(id, accreditation) as OkResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);

            _mockAccreditationService.Verify(s => s.UpdateAccreditation(id, accreditation), Times.Once);
        }

        [TestMethod]
        public async Task UpdateAccreditation_ReturnsBadRequest_WithNullInput()
        {
            // Arrange
            var id = Guid.NewGuid();

            // Act
            var result = await _accredicationController.UpdateAccreditation(id, null) as BadRequestResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);

            _mockAccreditationService.Verify(s =>
                s.UpdateAccreditation(
                    It.IsAny<Guid>(),
                    It.IsAny<Common.Dtos.Accreditation>()), Times.Never);
        }
    }
}
