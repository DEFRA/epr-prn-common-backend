using EPR.PRN.Backend.API.Helpers;
using EPR.PRN.Backend.API.Services;
using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Data.Interfaces;
using FluentAssertions;
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

        [TestMethod]
        public async Task GetAsync_WithValidParams_ShouldBeSuccessful()
        {
            var saveAndContinueData = new SaveAndContinue(){ Action = "Action1", Controller = "Controller", Id = 1, Area = "Registration", RegistrationId = 1 } ;

            _mockRepository.Setup(x=>x.GetAsync(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(saveAndContinueData);

            var result = await _systemUnderTest.GetAsync(1, "Registration");

            _mockRepository.Verify(r => r.GetAsync(It.IsAny<int>(), It.IsAny<string>()), Times.Once);
            _mockRepository.VerifyNoOtherCalls();
        }

        [TestMethod]
        public async Task GetAsync_NoRecords_ThrowNotFoundException()
        {
            var registrationId = 1;
            var area = "Registration";

            _mockRepository.Setup(r => r.GetAsync(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync((SaveAndContinue)null);

            await _systemUnderTest
            .Invoking(x => x.GetAsync(registrationId, area))
            .Should()
            .ThrowAsync<NotFoundException>();
        }

        [TestMethod]
        public async Task GetAllAsync_WithValidParams_ShouldBeSuccessful()
        {
            var saveAndContinueData = new List<SaveAndContinue>() { new(){ Action = "Action1", Controller = "Controller", Id = 1, Area = "Registration", RegistrationId = 1 } };

            _mockRepository.Setup(x => x.GetAllAsync(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(saveAndContinueData);

            var result = await _systemUnderTest.GetAllAsync(1, "Registration");

            result.Should().NotBeNull();
            result.Count.Should().Be(1);

            _mockRepository.Verify(r => r.GetAllAsync(It.IsAny<int>(), It.IsAny<string>()), Times.Once);
            _mockRepository.VerifyNoOtherCalls();
        }

        [TestMethod]
        public async Task GetAllAsync_NoRecords_ThrowNotFoundException()
        {
            var registrationId = 1;
            var area = "Registration";

            _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync((List<SaveAndContinue>)null);

            await _systemUnderTest
            .Invoking(x => x.GetAsync(registrationId, area))
            .Should()
            .ThrowAsync<NotFoundException>();
        }
    }
}
