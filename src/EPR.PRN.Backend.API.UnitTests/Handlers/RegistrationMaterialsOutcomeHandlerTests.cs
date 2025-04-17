using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Handlers;
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
    public async Task Handle_Exporter_Granted_GeneratesReference()
    {
        // Arrange
        var material = CreateMaterial(RegistrationMaterialStatus.Refused, ApplicationOrganisationType.Exporter, "USA", "XYZ");

        var command = new RegistrationMaterialsOutcomeCommand
        {
            Id = material.Id,
            Status = RegistrationMaterialStatus.Granted,
            Comments = "Valid update"
        };

        _rmRepositoryMock.Setup(r => r.GetRegistrationMaterialById(command.Id)).ReturnsAsync(material);

        string capturedRef = null;
        _rmRepositoryMock.Setup(r => r.UpdateRegistrationOutCome(command.Id, (int)command.Status, command.Comments, It.IsAny<string>()))
                         .Callback<int, int, string, string>((_, _, _, reference) => capturedRef = reference)
                         .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        capturedRef.Should().NotBeNull();
        capturedRef.Should().Contain("USA").And.Contain("E").And.Contain("XYZ");
    }

    [TestMethod]
    public async Task Handle_Reprocessor_Granted_GeneratesReference()
    {
        // Arrange
        var material = CreateMaterial(RegistrationMaterialStatus.Refused, ApplicationOrganisationType.Reprocessor, "DEU", "ALU");

        var command = new RegistrationMaterialsOutcomeCommand
        {
            Id = material.Id,
            Status = RegistrationMaterialStatus.Granted,
            Comments = "Reprocess granted"
        };

        _rmRepositoryMock.Setup(r => r.GetRegistrationMaterialById(command.Id)).ReturnsAsync(material);

        string capturedRef = null;
        _rmRepositoryMock.Setup(r => r.UpdateRegistrationOutCome(command.Id, (int)command.Status, command.Comments, It.IsAny<string>()))
                         .Callback<int, int, string, string>((_, _, _, reference) => capturedRef = reference)
                         .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        capturedRef.Should().Contain("DEU").And.Contain("R").And.Contain("ALU");
    }

    [TestMethod]
    public async Task Handle_RefusedStatus_DoesNotGenerateReference()
    {
        // Arrange
        var material = CreateMaterial(null, ApplicationOrganisationType.Exporter, "GBR", "COP");

        var command = new RegistrationMaterialsOutcomeCommand
        {
            Id = material.Id,
            Status = RegistrationMaterialStatus.Refused,
            Comments = "Refused on check"
        };

        _rmRepositoryMock.Setup(r => r.GetRegistrationMaterialById(command.Id)).ReturnsAsync(material);

        string capturedRef = "INITIAL";
        _rmRepositoryMock.Setup(r => r.UpdateRegistrationOutCome(command.Id, (int)command.Status, command.Comments, It.IsAny<string>()))
                         .Callback<int, int, string, string>((_, _, _, reference) => capturedRef = reference)
                         .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        capturedRef.Should().BeNull();
    }

    [TestMethod]
    public async Task Handle_InvalidTransition_Throws()
    {
        // Arrange
        var material = CreateMaterial(RegistrationMaterialStatus.Granted, ApplicationOrganisationType.Exporter, "USA", "XYZ");

        var command = new RegistrationMaterialsOutcomeCommand
        {
            Id = material.Id,
            Status = RegistrationMaterialStatus.Granted,
            Comments = "No change"
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
        var material = CreateMaterial(RegistrationMaterialStatus.Granted, ApplicationOrganisationType.Exporter, "USA", "XYZ");

        var command = new RegistrationMaterialsOutcomeCommand
        {
            Id = material.Id,
            Status = RegistrationMaterialStatus.Refused,
            Comments = "Trying to revert"
        };

        _rmRepositoryMock.Setup(r => r.GetRegistrationMaterialById(command.Id)).ReturnsAsync(material);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Invalid outcome transition.");
    }

    [TestMethod]
    public async Task Handle_GenerateReference_WithNullCountry_GeneratesUNK()
    {
        // Arrange
        var material = CreateMaterial(null, ApplicationOrganisationType.Reprocessor, null, "GLS");

        var command = new RegistrationMaterialsOutcomeCommand
        {
            Id = material.Id,
            Status = RegistrationMaterialStatus.Granted,
            Comments = "Testing null country"
        };

        _rmRepositoryMock.Setup(r => r.GetRegistrationMaterialById(command.Id)).ReturnsAsync(material);

        string capturedRef = null;
        _rmRepositoryMock.Setup(r => r.UpdateRegistrationOutCome(command.Id, (int)command.Status, command.Comments, It.IsAny<string>()))
                         .Callback<int, int, string, string>((_, _, _, reference) => capturedRef = reference)
                         .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        capturedRef.Should().Contain("UNK").And.Contain("R").And.Contain("GLS");
    }

    private RegistrationMaterial CreateMaterial(RegistrationMaterialStatus? status, ApplicationOrganisationType orgType, string country, string materialCode)
    {
        return new RegistrationMaterial
        {
            Id = 1,
            StatusID = status.HasValue ? (int)status : null,
            MaterialId = 10,
            Material = new LookupMaterial { Id = 10, MaterialCode = materialCode },
            RegistrationId = 456,
            Registration = new Registration
            {
                Id = 456,
                ApplicationTypeId = (int)orgType,
                BusinessAddress = orgType == ApplicationOrganisationType.Exporter ? new LookupAddress { Country = country } : null,
                ReprocessingSiteAddress = orgType == ApplicationOrganisationType.Reprocessor ? new LookupAddress { Country = country } : null
            }
        };
    }
}
