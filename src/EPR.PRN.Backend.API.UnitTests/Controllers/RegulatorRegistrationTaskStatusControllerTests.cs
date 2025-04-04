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

        _systemUnderTest = new RegulatorRegistrationTaskStatusController(_mockMediator.Object, _updateRegulatorRegistrationTaskCommandValidatorMock.Object, _mockLogger.Object);
    }

    [TestMethod]
    public async Task Patch_RegulatorApplicationTaskStatus_ReturnsOk_WhenValidUpdateTaskStatusRequestDto()
    {
        //Arrange
        var expectedTaskStatus = _fixture.Create<UpdateRegulatorRegistrationTaskCommand>();
        _mockMediator.Setup(m => m.Send(It.IsAny<UpdateRegulatorRegistrationTaskCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(Unit.Value);

        var validationResult = new ValidationResult();
        _updateRegulatorRegistrationTaskCommandValidatorMock.Setup(x => x.Validate(It.IsAny<UpdateRegulatorRegistrationTaskCommand>())).Returns(validationResult);

        //Act
        var result = await _systemUnderTest.UpdateRegistrationTaskStatus(TaskStatusId, expectedTaskStatus);

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

        var registrationMaterialId = 1;
        var requestDto = new UpdateRegulatorRegistrationTaskCommand
        {
            Status = (StatusTypes)999
        };

        // Act & Assert
        await FluentActions.Invoking(() =>
            _systemUnderTest.UpdateRegistrationTaskStatus(registrationMaterialId, requestDto)
        ).Should().ThrowAsync<ValidationException>();
    }

    [TestMethod]
    public async Task Patch_RegulatorApplicationTaskStatus_ThrowsException_WhenMediatorThrowsException()
    {
        // Arrange
        var expectedTaskStatus = _fixture.Create<UpdateRegulatorRegistrationTaskCommand>();
        _mockMediator.Setup(m => m.Send(It.IsAny<UpdateRegulatorRegistrationTaskCommand>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception("Test exception"));

        // Act
        Func<Task> act = async () => await _systemUnderTest.UpdateRegistrationTaskStatus(TaskStatusId, expectedTaskStatus);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("Test exception");
    }
}
