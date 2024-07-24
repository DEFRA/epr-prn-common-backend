using EPR.PRN.Backend.API.Common.DTO;
using EPR.PRN.Backend.API.Controllers;
using EPR.PRN.Backend.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EPR.PRN.Backend.API.UnitTests.Services
{
    [TestClass]
    public class PrnControllerTests
    {
        private PrnController _prnController;
        private Mock<IPrnService> _mockPrnService;

        [TestInitialize]
        public void Init()
        {
            _mockPrnService = new Mock<IPrnService>();
            _prnController = new PrnController(_mockPrnService.Object);
        }

        [TestMethod]
        public async Task GetAccreditation_ReturnsOkWithAccreditation_WithValidId_()
        {
            // Arrange
            var id = Guid.NewGuid();
            var expectedDTO = new PrnDTo();

            _mockPrnService.Setup(s => s.GetPrnById(id)).ReturnsAsync(expectedDTO);

            // Act
            var result = await _prnController.GetPrn(id) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreSame(expectedDTO, result.Value);

            _mockPrnService.Verify(s => s.GetPrnById(id), Times.Once());
        }
    }
}
