using EPR.PRN.Backend.API.Common.Dto;
using EPR.PRN.Backend.API.Configs;
using EPR.PRN.Backend.API.Controllers;
using EPR.PRN.Backend.API.Dto;
using EPR.PRN.Backend.API.Helpers;
using EPR.PRN.Backend.API.Services.Interfaces;
using EPR.PRN.Backend.Obligation.Interfaces;
using FluentAssertions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EPR.PRN.Backend.API.UnitTests.Controllers
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class RegistrationControllerTests
    {
        private RegistrationController _systemUnderTest;
        private Mock<IRegistrationService> _mockRegistrationService;
        private Mock<ILogger<RegistrationController>> _mockLogger;


        [TestInitialize]
        public void TestInitialize()
        {
            _mockRegistrationService = new Mock<IRegistrationService>();
            _mockLogger = new Mock<ILogger<RegistrationController>>();

            _systemUnderTest = new RegistrationController(_mockRegistrationService.Object,
                _mockLogger.Object);
        }

        [TestMethod]
        public async Task GetRegistrationById_ReturnsOk_WhenValidId()
        {
            var registrationId = 1;
            var expectedRegistration = new RegistrationDto() { ApplicationTypeId = 1, OrganisatonId = Guid.NewGuid() };
            _mockRegistrationService.Setup(s => s.GetByIdAsync(registrationId)).ReturnsAsync(expectedRegistration);

            var result = await _systemUnderTest.GetByRegistrationId(registrationId) as OkObjectResult;

            result.Value.Should().BeEquivalentTo(expectedRegistration);
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task GetRegistrationById_NotFound_ReturnsNotFoundStatusCode()
        {
            var registrationId = 1;
            _mockRegistrationService.Setup(s => s.GetByIdAsync(registrationId)).Throws<NotFoundException>();

            var result = await _systemUnderTest.GetByRegistrationId(registrationId) as ObjectResult;

            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        [TestMethod]
        public async Task GetRegistrationById_InternalServerError_ReturnsInternalServerStatusCode()
        {
            var registrationId = 1;
            _mockRegistrationService.Setup(s => s.GetByIdAsync(registrationId)).Throws<ArgumentException>();

            var result = await _systemUnderTest.GetByRegistrationId(registrationId) as ObjectResult;

            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}
