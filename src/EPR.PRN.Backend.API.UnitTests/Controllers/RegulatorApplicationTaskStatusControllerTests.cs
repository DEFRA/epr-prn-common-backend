using AutoFixture;
using AutoMapper;
using BackendAccountService.Core.Models.Request;
using EPR.PRN.Backend.API.Common.Dto;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Configs;
using EPR.PRN.Backend.API.Controllers;
using EPR.PRN.Backend.API.Dto;
using EPR.PRN.Backend.API.Helpers;
using EPR.PRN.Backend.API.Models;
using EPR.PRN.Backend.API.Services.Interfaces;
using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Obligation.Dto;
using EPR.PRN.Backend.Obligation.Interfaces;
using EPR.PRN.Backend.Obligation.Models;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Net;

namespace EPR.PRN.Backend.API.UnitTests.Services;

[TestClass]
public class RegulatorApplicationTaskStatusControllerTests
{
    private RegulatorApplicationTaskStatusController _systemUnderTest;

    private Mock<IMediator> _mockMediator;
    private Mock<ILogger<RegulatorApplicationTaskStatusController>> _mockLogger;
    private Mock<IValidator<UpdateRegulatorApplicationTaskCommand>> _updateRegulatorApplicationTaskCommandValidatorMock;
    private int TaskStatusId = 1;

    private static readonly IFixture _fixture = new Fixture();

    [TestInitialize]
    public void TestInitialize()
    {
        _mockMediator = new Mock<IMediator>();
        _mockLogger = new Mock<ILogger<RegulatorApplicationTaskStatusController>>();

        _updateRegulatorApplicationTaskCommandValidatorMock = new();

        _systemUnderTest = new RegulatorApplicationTaskStatusController(_mockMediator.Object, _updateRegulatorApplicationTaskCommandValidatorMock.Object);
    }

    [TestMethod]
    public async Task Patch_RegulatorApplicationTaskStatus_ReturnsOk_WhenValidUpdateTaskStatusRequestDto()
    {
        //Arrange
        var expectedTaskStatus = _fixture.Create<UpdateRegulatorApplicationTaskCommand>();
        _mockMediator.Setup(m => m.Send(It.IsAny<UpdateRegulatorApplicationTaskCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var validationResult = new ValidationResult();
        _updateRegulatorApplicationTaskCommandValidatorMock.Setup(x => x.Validate(It.IsAny<UpdateRegulatorApplicationTaskCommand>())).Returns(validationResult);

        //Act
        var result = await _systemUnderTest.UpdateRegistrationTaskStatus(TaskStatusId, expectedTaskStatus);

        //Assert
        result.Should().BeOfType<OkObjectResult>();
        (result as OkObjectResult).StatusCode.Should().Be((int)HttpStatusCode.OK);
    }
}
