using EPR.PRN.Backend.API.Controllers;
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
        public async Task Save_ReturnsOk()
        {
            var model = new SaveAndContinueRequest() { RegistrationId = 1, Action = "Test", Controller = "Test", Parameters = null, Area = null };
            var result = await _systemUnderTest.Save(model) as OkResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }


        [TestMethod]
        public async Task Save_InternalServerError_ReturnsInternalServerStatusCode()
        {
            _mockSaveAndContinueService.Setup(s => s.AddAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Throws<ArgumentException>();

            var result = await _systemUnderTest.Save(null) as ObjectResult;

            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}
