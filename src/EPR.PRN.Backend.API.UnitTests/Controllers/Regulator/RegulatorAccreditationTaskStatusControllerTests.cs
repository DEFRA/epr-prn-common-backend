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
public class RegulatorAccreditationTaskStatusControllerTests
{
    private RegulatorAccreditationTaskStatusController _systemUnderTest;

    private Mock<IMediator> _mockMediator;
    private Mock<ILogger<RegulatorAccreditationTaskStatusController>> _mockLogger;
    private Mock<IValidator<UpdateRegulatorAccreditationTaskCommand>> _validatorMock;
    private static readonly IFixture _fixture = new Fixture();

    [TestInitialize]
    public void TestInitialize()
    {
        _mockMediator = new Mock<IMediator>();
        _mockLogger = new Mock<ILogger<RegulatorAccreditationTaskStatusController>>();

        _validatorMock = new();

        _systemUnderTest = new RegulatorAccreditationTaskStatusController(_mockMediator.Object, _validatorMock.Object, _mockLogger.Object);
    }

    [TestMethod]
    public async Task Patch_RegulatorAccreditationTaskStatus_ReturnsOk_WhenValidUpdateTaskStatusRequestDto()
    {
        //Arrange
        var expectedTaskStatus = _fixture.Create<UpdateRegulatorAccreditationTaskCommand>();

        var validationResult = new ValidationResult();
        _validatorMock.Setup(x => x.Validate(It.IsAny<UpdateRegulatorAccreditationTaskCommand>())).Returns(validationResult);

        //Act
        var result = await _systemUnderTest.UpdateAccreditationTaskStatus(expectedTaskStatus);

        //Assert
        result.Should().BeOfType<NoContentResult>();
        (result as NoContentResult).StatusCode.Should().Be((int)HttpStatusCode.NoContent);
    }

    [TestMethod]
    public async Task Patch_RegulatorAccreditationTaskStatus_ThrowsValidationException_WhenValidationFails()
    {
        // Arrange
        var validator = new InlineValidator<UpdateRegulatorAccreditationTaskCommand>();
        validator.RuleFor(x => x.Status).Must(_ => false).WithMessage("Validation failed");

        _systemUnderTest = new RegulatorAccreditationTaskStatusController(_mockMediator.Object, validator, _mockLogger.Object);

        var accreditationId = Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a9");
        var requestDto = new UpdateRegulatorAccreditationTaskCommand
        {
            AccreditationId = accreditationId,
            TaskName = "Test Task",
            Status = (RegulatorTaskStatus)999,
            UserName = "UserName"
        };

        // Act & Assert
        await FluentActions.Invoking(() =>
            _systemUnderTest.UpdateAccreditationTaskStatus(requestDto)
        ).Should().ThrowAsync<ValidationException>();
    }

    [TestMethod]
    public async Task Patch_RegulatorAccreditationTaskStatus_ThrowsException_WhenMediatorThrowsException()
    {
        // Arrange
        var expectedTaskStatus = _fixture.Create<UpdateRegulatorAccreditationTaskCommand>();
        _mockMediator.Setup(m => m.Send(It.IsAny<UpdateRegulatorAccreditationTaskCommand>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception("Test exception"));

        // Act
        Func<Task> act = async () => await _systemUnderTest.UpdateAccreditationTaskStatus(expectedTaskStatus);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("Test exception");
    }
}
