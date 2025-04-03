using AutoFixture;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;

namespace EPR.PRN.Backend.API.UnitTests.Services;

[TestClass]
public class RegulatorRegistrationTaskStatusControllerTests
{
    private RegulatorRegistrationTaskStatusController _systemUnderTest;

    private Mock<IMediator> _mockMediator;
    private Mock<ILogger<RegulatorRegistrationTaskStatusController>> _mockLogger;
    private Mock<IValidator<UpdateRegulatorRegistrationTaskCommand>> _updateRegulatorRegistrationTaskCommandValidatorMock;
    private int TaskStatusId = 1;

    private static readonly IFixture _fixture = new Fixture();

    [TestInitialize]
    public void TestInitialize()
    {
        _mockMediator = new Mock<IMediator>();
        _mockLogger = new Mock<ILogger<RegulatorRegistrationTaskStatusController>>();

        _updateRegulatorRegistrationTaskCommandValidatorMock = new();

        _systemUnderTest = new RegulatorRegistrationTaskStatusController(_mockMediator.Object, _updateRegulatorRegistrationTaskCommandValidatorMock.Object);
    }

    [TestMethod]
    public async Task Patch_RegulatorApplicationTaskStatus_ReturnsOk_WhenValidUpdateTaskStatusRequestDto()
    {
        //Arrange
        var expectedTaskStatus = _fixture.Create<UpdateRegulatorRegistrationTaskCommand>();
        _mockMediator.Setup(m => m.Send(It.IsAny<UpdateRegulatorRegistrationTaskCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var validationResult = new ValidationResult();
        _updateRegulatorRegistrationTaskCommandValidatorMock.Setup(x => x.Validate(It.IsAny<UpdateRegulatorRegistrationTaskCommand>())).Returns(validationResult);

        //Act
        var result = await _systemUnderTest.UpdateRegistrationTaskStatus(TaskStatusId, expectedTaskStatus);

        //Assert
        result.Should().BeOfType<OkObjectResult>();
        (result as OkObjectResult).StatusCode.Should().Be((int)HttpStatusCode.OK);
    }
}
