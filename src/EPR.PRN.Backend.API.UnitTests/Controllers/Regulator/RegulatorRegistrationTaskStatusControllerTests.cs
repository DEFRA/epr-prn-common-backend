using AutoFixture;
using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Controllers.Regulator;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;

namespace EPR.PRN.Backend.API.UnitTests.Controllers.Regulator;

[TestClass]
public class RegulatorRegistrationTaskStatusControllerTests
{
    private RegulatorRegistrationTaskStatusController _systemUnderTest;

    private Mock<IMediator> _mockMediator;
    private Mock<ILogger<RegulatorRegistrationTaskStatusController>> _mockLogger;
    private Mock<IValidator<UpdateRegulatorRegistrationTaskCommand>> _updateRegulatorRegistrationTaskCommandValidatorMock;
    private static readonly IFixture _fixture = new Fixture();

    [TestInitialize]
    public void TestInitialize()
    {
        _mockMediator = new Mock<IMediator>();
        _mockLogger = new Mock<ILogger<RegulatorRegistrationTaskStatusController>>();

        _updateRegulatorRegistrationTaskCommandValidatorMock = new();

        _systemUnderTest = new RegulatorRegistrationTaskStatusController(_mockMediator.Object, _updateRegulatorRegistrationTaskCommandValidatorMock.Object, _mockLogger.Object);
    }

    [TestMethod]
    public async Task Patch_RegulatorApplicationTaskStatus_ReturnsOk_WhenValidUpdateTaskStatusRequestDto()
    {
        //Arrange
        var expectedTaskStatus = _fixture.Create<UpdateRegulatorRegistrationTaskCommand>();

        var validationResult = new ValidationResult();
        _updateRegulatorRegistrationTaskCommandValidatorMock.Setup(x => x.Validate(It.IsAny<UpdateRegulatorRegistrationTaskCommand>())).Returns(validationResult);

        //Act
        var result = await _systemUnderTest.UpdateRegistrationTaskStatus(expectedTaskStatus);

        //Assert
        result.Should().BeOfType<NoContentResult>();
        (result as NoContentResult).StatusCode.Should().Be((int)HttpStatusCode.NoContent);
    }

    [TestMethod]
    public async Task Patch_RegulatorRegistrationTaskStatus_ThrowsValidationException_WhenValidationFails()
    {
        // Arrange
        var validator = new InlineValidator<UpdateRegulatorRegistrationTaskCommand>();
        validator.RuleFor(x => x.Status).Must(_ => false).WithMessage("Validation failed");

        _systemUnderTest = new RegulatorRegistrationTaskStatusController(_mockMediator.Object, validator, _mockLogger.Object);

        var registrationMaterialId = Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a9");
        var requestDto = new UpdateRegulatorRegistrationTaskCommand
        {
            RegistrationId = registrationMaterialId,
            TaskName = "Test Task",
            Status = (RegulatorTaskStatus)999,
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
        var expectedTaskStatus = _fixture.Create<UpdateRegulatorRegistrationTaskCommand>();
        _mockMediator.Setup(m => m.Send(It.IsAny<UpdateRegulatorRegistrationTaskCommand>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception("Test exception"));

        // Act
        Func<Task> act = async () => await _systemUnderTest.UpdateRegistrationTaskStatus(expectedTaskStatus);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("Test exception");
    }
    [TestMethod]
    public async Task AddRegistrationTaskQueryNote_ReturnsNoContent_WhenValidInput()
    {
        // Arrange
        var validator = new InlineValidator<UpdateRegulatorRegistrationTaskCommand>();
        var taskId = Guid.NewGuid();
        var command = new AddRegistrationTaskQueryNoteCommand
        {
            Note = "This is a valid note",
            CreatedBy = Guid.NewGuid()
        };

        // Act
        _systemUnderTest = new RegulatorRegistrationTaskStatusController(_mockMediator.Object, validator, _mockLogger.Object);
        var result = await _systemUnderTest.RegistrationTaskQueryNote(taskId, command);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [TestMethod]
    public async Task AddRegistrationTaskQueryNote_ReturnsBadRequest_WhenValidationFails()
    {
        // Arrange
        var taskId = Guid.Empty;
        var command = new AddRegistrationTaskQueryNoteCommand()
        {
            Note = "", // Invalid note
            CreatedBy = Guid.Empty // Invalid QueryBy
        };

        // Assert
        
        Func<Task> act = () => _systemUnderTest.RegistrationTaskQueryNote(taskId, command);

        await act.Should().ThrowAsync<ValidationException>();
    }

    [TestMethod]
    public async Task AddRegistrationTaskQueryNote_ReturnsInternalServerError_WhenMediatorThrows()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        var command = new AddRegistrationTaskQueryNoteCommand
        {
            Note = "Some valid note",
            CreatedBy = Guid.NewGuid()
        };

        _mockMediator.Setup(x => x.Send(It.IsAny<AddRegistrationTaskQueryNoteCommand>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Unexpected failure"));

        // Act
        Func<Task> act = async () => await _systemUnderTest.RegistrationTaskQueryNote(taskId, command);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("Unexpected failure");
    }

}
