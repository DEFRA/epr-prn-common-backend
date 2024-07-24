using EPR.PRN.Backend.API.Common.DTO;
using EPR.PRN.Backend.API.Repositories.Interfaces;
using EPR.PRN.Backend.API.Services;
using Moq;
using static Azure.Core.HttpHeader;

namespace EPR.PRN.Backend.API.UnitTests.Services
{
    [TestClass]
    public class PrnServiceTests
    {
        private PrnService _prnService;
        private Mock<IRepository> _mockRepository;

        [TestInitialize]
        public void Init()
        {
            _mockRepository = new Mock<IRepository>();
            _prnService = new PrnService(_mockRepository.Object);
        }

        [TestMethod]
        public async Task GetPrn_WithValidId_ReturnsExpectedDTO()
        {
            // Arrange
            var id = Guid.NewGuid();
            PrnDTo expectedDTO = new PrnDTo();

            _mockRepository.Setup(r => r.GetPrnById(id)).ReturnsAsync(expectedDTO);

            //// Act
            var result = await _prnService.GetPrnById(id);

            //// Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedDTO, result);

            _mockRepository.Verify(r => r.GetPrnById(id), Times.Once());
        }

        [TestMethod]
        public async Task GetPrn_WithInvalidId_ReturnsNull()
        {
            // Arrange
            var id = Guid.NewGuid();

            _mockRepository.Setup(r => r.GetPrnById(id)).ReturnsAsync((PrnDTo)null);

            // Act
            var result = await _prnService.GetPrnById(id);

            // Assert
            Assert.IsNull(result);

            _mockRepository.Verify(r => r.GetPrnById(id), Times.Once);
        }

        [TestMethod]
        public async Task AcceptPrn_CallsRepositoryWithCorrectParameters()
        {
            // Arrange
            PrnDTo expectedDTO = new PrnDTo();
            var id = Guid.NewGuid();
            expectedDTO.ExternalId = id;

            _mockRepository.Setup(r => r.GetPrnById(id)).ReturnsAsync(expectedDTO);

            // Act
            await _prnService.AcceptPrn(id);

            // Assert
            _mockRepository.Verify(r => r.GetPrnById(id), Times.Once());
            _mockRepository.Verify(r => r.UpdatePrn(id, expectedDTO), Times.Once);
        }
    }
}
