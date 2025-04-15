using AutoFixture;
using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Controllers;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;

namespace EPR.PRN.Backend.API.UnitTests.Controllers;

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

        _systemUnderTest = new RegulatorApplicationTaskStatusController(_mockMediator.Object, _updateRegulatorApplicationTaskCommandValidatorMock.Object, _mockLogger.Object);
    }

    [TestMethod]
    public async Task Patch_RegulatorApplicationTaskStatus_ReturnsOk_WhenValidUpdateTaskStatusRequestDto()
    {
        //Arrange
        var expectedTaskStatus = _fixture.Create<UpdateRegulatorApplicationTaskCommand>();
        _mockMediator.Setup(m => m.Send(It.IsAny<UpdateRegulatorApplicationTaskCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(Unit.Value);

        var validationResult = new ValidationResult();
        _updateRegulatorApplicationTaskCommandValidatorMock.Setup(x => x.Validate(It.IsAny<UpdateRegulatorApplicationTaskCommand>())).Returns(validationResult);

        //Act
        var result = await _systemUnderTest.UpdateRegistrationTaskStatus(expectedTaskStatus);

        //Assert
        result.Should().BeOfType<NoContentResult>();
        (result as NoContentResult).StatusCode.Should().Be((int)HttpStatusCode.NoContent);
    }

    [TestMethod]
    public async Task Patch_RegulatorApplicationTaskStatus_ThrowsValidationException_WhenValidationFails()
    {
        // Arrange
        var validator = new InlineValidator<UpdateRegulatorApplicationTaskCommand>();
        validator.RuleFor(x => x.Status).Must(_ => false).WithMessage("Validation failed");

        _systemUnderTest = new RegulatorApplicationTaskStatusController(_mockMediator.Object, validator, _mockLogger.Object);

        var registrationMaterialId = 1;
        var requestDto = new UpdateRegulatorApplicationTaskCommand
        {
            TaskName = "Test Task",
            Status = (StatusTypes)999,
            UserName = "UserName"
        };

        // Act & Assert
        await FluentActions.Invoking(() =>
            _systemUnderTest.UpdateRegistrationTaskStatus(requestDto)
        ).Should().ThrowAsync<ValidationException>();
    }

    [TestMethod]
    public async Task Patch_RegulatorApplicationTaskStatus_ThrowsException_WhenMediatorThrowsException()
    {
        // Arrange
        var expectedTaskStatus = _fixture.Create<UpdateRegulatorApplicationTaskCommand>();
        _mockMediator.Setup(m => m.Send(It.IsAny<UpdateRegulatorApplicationTaskCommand>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception("Test exception"));

        // Act
        Func<Task> act = async () => await _systemUnderTest.UpdateRegistrationTaskStatus(expectedTaskStatus);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("Test exception");
    }
}
