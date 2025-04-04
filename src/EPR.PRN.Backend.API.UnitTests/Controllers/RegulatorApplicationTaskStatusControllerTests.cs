using AutoFixture;
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
        var result = await _systemUnderTest.UpdateRegistrationTaskStatus(TaskStatusId, expectedTaskStatus);

        //Assert
        result.Should().BeOfType<NoContentResult>();
        (result as NoContentResult).StatusCode.Should().Be((int)HttpStatusCode.NoContent);
    }

    //[TestMethod]
    //public async Task Patch_RegulatorApplicationTaskStatus_ThrowsValidationException_WhenValidationFails()
    //{
    //    // Arrange
    //    var expectedTaskStatus = _fixture.Create<UpdateRegulatorApplicationTaskCommand>();
    //    _updateRegulatorApplicationTaskCommandValidatorMock.Setup(v => v.ValidateAsync(It.IsAny<UpdateRegulatorApplicationTaskCommand>(), It.IsAny<CancellationToken>()))
    //        .ReturnsAsync(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Status", "Validation failed") }));

    //    // Act
    //    Func<Task> act = async () => await _systemUnderTest.UpdateRegistrationTaskStatus(TaskStatusId, expectedTaskStatus);

    //    // Assert
    //    await act.Should().ThrowAsync<ValidationException>().WithMessage("Validation failed");
    //}

    //[TestMethod]
    //public async Task Patch_RegulatorApplicationTaskStatus_ThrowsValidationException_WhenValidationFails()
    //{
    //    // Arrange
    //    var expectedTaskStatus = new UpdateRegulatorApplicationTaskCommand { Status = Common.Enums.StatusTypes.Complete }; // Invalid status

    //    ValidationResult result = new ValidationResult(new List<ValidationFailure>
    //                                {
    //                                    new ValidationFailure("Status", "Status cannot be empty.")
    //                                });

    //    _updateRegulatorApplicationTaskCommandValidatorMock
    //        .Setup(v => v.Validate(It.IsAny<UpdateRegulatorApplicationTaskCommand>()))
    //        .Returns(result);

    //    //var validatorMock = Substitute.For<IValidator<SaleRequestData>>();
    //    //ValidationResult result = new ValidationResult(new List<ValidationFailure>()
    //    //                                                       {
    //    //                                                           new ValidationFailure("TotalAmount",
    //    //                                                               "Total Amount was invalid.")
    //    //                                                       });

    //    //validatorMock.Validate(Arg.Any<ValidationContext>()).Returns(result);

    //    // Act
    //    Func<Task> act = async () => await _systemUnderTest.UpdateRegistrationTaskStatus(TaskStatusId, expectedTaskStatus);

    //    // Assert
    //    await act.Should().ThrowAsync<ValidationException>().WithMessage("Validation failed");
    //}

    //[TestMethod]
    //public async Task Patch_RegulatorApplicationTaskStatus_ThrowsValidationException_WhenValidationFails()
    //{
    //    // Arrange
    //    var registrationMaterialId = 1;
    //    var command = _fixture.Create<UpdateRegulatorApplicationTaskCommand>();
    //    var validationResult = new ValidationResult
    //    {
    //        Errors = { new ValidationFailure("Field", "Error") }
    //    };

    //    _updateRegulatorApplicationTaskCommandValidatorMock
    //        .Setup(v => v.ValidateAsync(command, default))
    //        .ReturnsAsync(validationResult);

    //    // Act & Assert
    //    await FluentActions.Invoking(async () =>
    //    {
    //        if (validationResult.Errors.Any())
    //        {
    //            throw new ValidationException(validationResult.Errors);
    //        }
    //        await _systemUnderTest.UpdateRegistrationTaskStatus(registrationMaterialId, command);
    //    })
    //        .Should().ThrowAsync<ValidationException>();
    //}

    [TestMethod]
    public async Task Patch_RegulatorApplicationTaskStatus_ThrowsException_WhenMediatorThrowsException()
    {
        // Arrange
        var expectedTaskStatus = _fixture.Create<UpdateRegulatorApplicationTaskCommand>();
        _mockMediator.Setup(m => m.Send(It.IsAny<UpdateRegulatorApplicationTaskCommand>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception("Test exception"));

        // Act
        Func<Task> act = async () => await _systemUnderTest.UpdateRegistrationTaskStatus(TaskStatusId, expectedTaskStatus);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("Test exception");
    }
}
