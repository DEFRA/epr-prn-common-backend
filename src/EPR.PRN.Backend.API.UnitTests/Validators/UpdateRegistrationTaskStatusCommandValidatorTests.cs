using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Validators;
using FluentAssertions;
using FluentValidation.TestHelper;
namespace EPR.PRN.Backend.API.UnitTests.Validators;

[TestClass]
public class UpdateRegistrationTaskStatusCommandValidatorTests
{
    private UpdateRegistrationTaskStatusCommandValidator _validator;

    [TestInitialize]
    public void Setup()
    {
        _validator = new UpdateRegistrationTaskStatusCommandValidator();
    }

    [TestMethod]
    [DataRow(TaskStatuses.Started)]
    [DataRow(TaskStatuses.Queried)]
    [DataRow(TaskStatuses.Completed)]
    public async Task Validate_ShouldPass_ForAllowedStatuses(TaskStatuses validStatus)
    {
        // Arrange
        var registrationId = Guid.NewGuid();
        var command = new UpdateRegistrationTaskStatusCommand 
        { 
            Status = validStatus, 
            RegistrationId = registrationId, 
            TaskName = "Test Task" };

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [TestMethod]
    [DataRow(TaskStatuses.CannotStartYet)]
    [DataRow(TaskStatuses.NotStarted)]
    public async Task Validate_ShouldFail_ForDisallowedStatuses(TaskStatuses invalidStatus)
    {
        // Arrange
        var registrationId = Guid.NewGuid();
        var command = new UpdateRegistrationTaskStatusCommand 
        { 
            Status = invalidStatus,
            RegistrationId = registrationId,
            TaskName = "Test Task"
        };
    
        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(x => x.Status)
              .WithErrorMessage("Invalid Status value");
    }
}
