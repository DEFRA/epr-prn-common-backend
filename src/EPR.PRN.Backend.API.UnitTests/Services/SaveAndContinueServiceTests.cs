using AutoFixture;
using EPR.PRN.Backend.API.Services;
using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Data.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.UnitTests.Services
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class SaveAndContinueServiceTests
    {
        private SaveAndContinueService _systemUnderTest;
        private Mock<ISaveAndContinueRepository> _mockRepository;
        private Mock<ILogger<SaveAndContinueService>> _mockLogger;
        private static readonly IFixture _fixture = new Fixture();

        [TestInitialize]
        public void Init()
        {
            _mockRepository = new Mock<ISaveAndContinueRepository>();

            _mockLogger = new Mock<ILogger<SaveAndContinueService>>();

            _systemUnderTest = new SaveAndContinueService(_mockRepository.Object, _mockLogger.Object);
        }

        [TestMethod]
        public async Task AddAsync_WithValidParams_ShouldBeSuccessful()
        {
            int registrationId = 1;
            string action = "Action";
            string controller = "Controller";
            string area = "Area";
            string parameters = JsonConvert.SerializeObject(new { Test = "test" });

            await _systemUnderTest.AddAsync(registrationId, area, action, controller, parameters);

            _mockRepository.Verify(r => r.AddAsync(It.IsAny<SaveAndContinue>()), Times.Once);
            _mockRepository.VerifyNoOtherCalls();
        }
    }
}
