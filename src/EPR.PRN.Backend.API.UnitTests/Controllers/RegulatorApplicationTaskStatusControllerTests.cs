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
    private Mock<IMapper> _mockMapper;
    private Mock<ILogger<RegulatorApplicationTaskStatusController>> _mockLogger;
    private Mock<IOptions<PrnObligationCalculationConfig>> _configMock;
    private Mock<IConfiguration> _configurationMock;
    private Mock<IValidator<UpdateTaskStatusRequestDto>> _UpdateTastStatusRequestDtoValidator;
    private int TaskStatusId = 1;

    private static readonly IFixture _fixture = new Fixture();

    [TestInitialize]
    public void TestInitialize()
    {
        _mockMediator = new Mock<IMediator>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<RegulatorApplicationTaskStatusController>>();

        _UpdateTastStatusRequestDtoValidator = new();

        _systemUnderTest = new RegulatorApplicationTaskStatusController(_mockMediator.Object, _mockMapper.Object, _UpdateTastStatusRequestDtoValidator.Object);


    }

    [TestMethod]
    public async Task Patch_RegulatorApplicationTaskStatus_ReturnsOk_WhenValidUpdateTaskStatusRequestDto()
    {
        var expectedTaskStatus = _fixture.Create<UpdateTaskStatusRequestDto>();
        _mockMediator.Setup(m => m.Send(It.IsAny<UpdateRegulatorApplicationTaskCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var result = await _systemUnderTest.UpdateRegistrationTaskStatus(expectedTaskStatus., expectedTaskStatus) as OkObjectResult;

        
    }

}
