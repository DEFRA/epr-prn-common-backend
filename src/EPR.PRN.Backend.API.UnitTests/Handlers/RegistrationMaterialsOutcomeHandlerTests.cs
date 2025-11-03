using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Handlers.Regulator;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using FluentAssertions;
using Moq;

namespace EPR.PRN.Backend.API.UnitTests.Handlers;

[TestClass]
public class RegistrationMaterialsOutcomeHandlerTests
{
    private Mock<IRegistrationMaterialRepository> _rmRepositoryMock;
    private RegistrationMaterialsOutcomeHandler _handler;

    [TestInitialize]
    public void TestInitialize()
    {
        _rmRepositoryMock = new Mock<IRegistrationMaterialRepository>();
        _handler = new RegistrationMaterialsOutcomeHandler(_rmRepositoryMock.Object);
    }

    [TestMethod]
    public async Task Handle_InvalidTransition_Throws()
    {
        // Arrange
        var material = CreateMaterial(RegistrationMaterialStatus.Granted, ApplicationOrganisationType.Exporter, 1, "XYZ");

        var command = new RegistrationMaterialsOutcomeCommand
        {
            Id = material.ExternalId,
            Status = RegistrationMaterialStatus.Granted,
            Comments = "No change",
            RegistrationReferenceNumber = "REF0005-03"
        };

        _rmRepositoryMock.Setup(r => r.GetRegistrationMaterialById(command.Id)).ReturnsAsync(material);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Invalid outcome transition.");
    }

    [TestMethod]
    public async Task Handle_InvalidTransition_RevertFromGranted_Throws()
    {
        // Arrange
        var material = CreateMaterial(RegistrationMaterialStatus.Granted, ApplicationOrganisationType.Exporter, 1, "XYZ");

        var command = new RegistrationMaterialsOutcomeCommand
        {
            Id = material.ExternalId,
            Status = RegistrationMaterialStatus.Refused,
            Comments = "Trying to revert",
            RegistrationReferenceNumber = "REF0005-03"
        };

        _rmRepositoryMock.Setup(r => r.GetRegistrationMaterialById(command.Id)).ReturnsAsync(material);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Invalid outcome transition.");
    }

    [TestMethod]
    public async Task Handle_TransitionFromNullStatus_AllowsAnyValidChange()
    {
        // Arrange
        var material = CreateMaterial(null, ApplicationOrganisationType.Exporter, 1, "PLST");

        var command = new RegistrationMaterialsOutcomeCommand
        {
            Id = material.ExternalId,
            Status = RegistrationMaterialStatus.Refused,
            Comments = "Initial setting",
            RegistrationReferenceNumber = "REF-NULL",
            User = Guid.NewGuid()

        };

        _rmRepositoryMock.Setup(r => r.GetRegistrationMaterialById(command.Id)).ReturnsAsync(material);
        _rmRepositoryMock.Setup(r => r.UpdateRegistrationOutCome(command.Id, (int)command.Status, command.Comments, command.RegistrationReferenceNumber, command.User)).Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _rmRepositoryMock.Verify(r => r.UpdateRegistrationOutCome(command.Id, (int)command.Status, command.Comments, "REF-NULL", command.User), Times.Once);
    }
    [TestMethod]
    public async Task Handle_TransitionToGranted_SetsRegistrationReferenceNumber()
    {
        // Arrange
        var material = CreateMaterial(null, ApplicationOrganisationType.Reprocessor, 1, "ALUM");

        var command = new RegistrationMaterialsOutcomeCommand
        {
            Id = material.ExternalId,
            Status = RegistrationMaterialStatus.Granted,
            Comments = "All good",
            RegistrationReferenceNumber = "REF-GRANTED",
            User = Guid.NewGuid()
        };

        _rmRepositoryMock.Setup(r => r.GetRegistrationMaterialById(command.Id)).ReturnsAsync(material);
        _rmRepositoryMock.Setup(r => r.UpdateRegistrationOutCome(command.Id, (int)command.Status, command.Comments, command.RegistrationReferenceNumber, command.User)).Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _rmRepositoryMock.Verify(r => r.UpdateRegistrationOutCome(command.Id, (int)command.Status, command.Comments, "REF-GRANTED", command.User), Times.Once);
    }
    [TestMethod]
    public async Task Handle_ValidTransition_CallsUpdateRegistrationOutCome()
    {
        // Arrange
        var material = CreateMaterial(null, ApplicationOrganisationType.Exporter, 1, "PLST");

        var command = new RegistrationMaterialsOutcomeCommand
        {
            Id = material.ExternalId,
            Status = RegistrationMaterialStatus.Refused,
            Comments = "Valid update",
            RegistrationReferenceNumber = "REF0001",
            User = Guid.NewGuid()
        };

        _rmRepositoryMock.Setup(r => r.GetRegistrationMaterialById(command.Id)).ReturnsAsync(material);
        _rmRepositoryMock.Setup(r => r.UpdateRegistrationOutCome(command.Id, (int)command.Status, command.Comments, command.RegistrationReferenceNumber, command.User)).Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _rmRepositoryMock.Verify(r => r.UpdateRegistrationOutCome(command.Id, (int)command.Status, command.Comments, command.RegistrationReferenceNumber, command.User), Times.Once);
    }



    private static RegistrationMaterial CreateMaterial(RegistrationMaterialStatus? status, ApplicationOrganisationType orgType, int? nationId, string materialCode)
    {
        return new RegistrationMaterial
        {
            Id = 1,
            StatusId = status.HasValue ? (int)status : null,
            MaterialId = 10,
            Material = new LookupMaterial { Id = 10, MaterialCode = materialCode },
            RegistrationId = 456,
            Registration = new Registration
            {
                Id = 456,
                ApplicationTypeId = (int)orgType,
                BusinessAddress = orgType == ApplicationOrganisationType.Exporter ? new Address { NationId = nationId } : null,
                ReprocessingSiteAddress = orgType == ApplicationOrganisationType.Reprocessor ? new Address { NationId = nationId } : null
            }
        };
    }
}
