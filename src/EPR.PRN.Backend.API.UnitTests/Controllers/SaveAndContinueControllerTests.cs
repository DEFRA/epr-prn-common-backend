using EPR.PRN.Backend.API.Controllers;
using EPR.PRN.Backend.API.Dto;
using EPR.PRN.Backend.API.Models;
using EPR.PRN.Backend.API.Services.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace EPR.PRN.Backend.API.UnitTests.Controllers
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class SaveAndContinueControllerTests
    {
        private SaveAndContinueController _systemUnderTest;
        private Mock<ISaveAndContinueService> _mockSaveAndContinueService;
        private Mock<ILogger<SaveAndContinueController>> _mockLogger;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockSaveAndContinueService = new Mock<ISaveAndContinueService>();
            _mockLogger = new Mock<ILogger<SaveAndContinueController>>();

            _systemUnderTest = new SaveAndContinueController(_mockSaveAndContinueService.Object,
                _mockLogger.Object);
        }

        [TestMethod]
        public async Task Create_ReturnsOk()
        {
            var model = new SaveAndContinueRequest() { RegistrationId = 1, Action = "Test", Controller = "Test", Parameters = null, Area = null };
            var result = await _systemUnderTest.Create(model) as OkResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task Create_RequestObjectIsNull_ReturnsBadRequest()
        {
            var result = await _systemUnderTest.Create(null) as BadRequestResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }


        [TestMethod]
        public async Task Create_InternalServerError_ReturnsInternalServerStatusCode()
        {
            _mockSaveAndContinueService.Setup(s => s.AddAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Throws<ArgumentException>();

            var result = await _systemUnderTest.Create(null) as ObjectResult;

            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }

        [TestMethod]
        public async Task Get_ReturnsOk()
        {
            var saveAndContinueData = new SaveAndContinueDto{ Action = "Action1", Controller = "Controller", Id = 1, Area = "Registration", RegistrationId = 1 } ;
            _mockSaveAndContinueService.Setup(x => x.GetAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(saveAndContinueData);

            var result = await _systemUnderTest.Get(1, "Controller", "Registration") as OkObjectResult;
            var modelResult = result.Value as SaveAndContinueDto;

            modelResult.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task Get_InternalServerError_ReturnsInternalServerStatusCode()
        {
            _mockSaveAndContinueService.Setup(x => x.GetAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).Throws<ArgumentException>();

            var result = await _systemUnderTest.Get(1, null, null) as ObjectResult;

            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }

        [TestMethod]
        public async Task GetAll_ReturnsOk()
        {
            var saveAndContinueData = new List<SaveAndContinueDto> { new() { Action = "Action1", Controller = "Controller", Id = 1, Area = "Registration", RegistrationId = 1 } };
            _mockSaveAndContinueService.Setup(x => x.GetAllAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(saveAndContinueData);

            var result = await _systemUnderTest.GetAll(1, "Controller", "Registration") as OkObjectResult;
            var modelResult = result.Value as List<SaveAndContinueDto>;

            modelResult.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task GetAll_InternalServerError_ReturnsInternalServerStatusCode()
        {
            _mockSaveAndContinueService.Setup(x => x.GetAllAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).Throws<ArgumentException>();

            var result = await _systemUnderTest.GetAll(1,null, null) as ObjectResult;

            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}
