using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Common.Constants;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Handlers;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using FluentAssertions;
using Moq;

namespace EPR.PRN.Backend.API.UnitTests.Handlers;

[TestClass]
public class UpdateRegistrationMaterialTaskStatusHandlerTests : HandlerTestsBase<IRegistrationMaterialRepository>
{
    private UpdateRegistrationMaterialTaskStatusHandler _handler;

    [TestInitialize]
    public void TestInitialize()
    {
        _handler = new UpdateRegistrationMaterialTaskStatusHandler(MockRepository.Object);
    }

    [TestMethod]
    public async Task Handle_ShouldCallRepository_WithCorrectParameters()
    {
        // Arrange
        var registrationMaterialId = Guid.NewGuid();
        var command = new UpdateRegistrationMaterialTaskStatusCommand
        {
            RegistrationMaterialId = registrationMaterialId,
            Status = TaskStatuses.Completed,
            TaskName = ApplicantRegistrationTaskNames.SiteAddressAndContactDetails
        };

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        MockRepository.Verify(r => r.UpdateRegistrationTaskStatusAsync(
                ApplicantRegistrationTaskNames.SiteAddressAndContactDetails,
                command.RegistrationMaterialId,
                TaskStatuses.Completed),
            Times.Once);
    }

    [TestMethod]
    public async Task Handle_ShouldThrowException_WhenRepositoryThrows()
    {
        // Arrange
        var registrationMaterialId = Guid.NewGuid();
        var command = new UpdateRegistrationMaterialTaskStatusCommand
        {
            RegistrationMaterialId = registrationMaterialId,
            Status = TaskStatuses.Completed,
            TaskName = ApplicantRegistrationTaskNames.SiteAddressAndContactDetails
        };

        MockRepository.Setup(r => r.UpdateRegistrationTaskStatusAsync(ApplicantRegistrationTaskNames.SiteAddressAndContactDetails, registrationMaterialId, TaskStatuses.Completed)).ThrowsAsync(new KeyNotFoundException("Registration not found."));

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Registration not found.");
    }
}