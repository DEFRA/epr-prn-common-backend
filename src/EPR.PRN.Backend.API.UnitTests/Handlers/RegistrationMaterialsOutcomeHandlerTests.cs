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
    public async Task Handle_InvalidTransition_Throws()
    {
        // Arrange
        var material = CreateMaterial(RegistrationMaterialStatus.Granted, ApplicationOrganisationType.Exporter, 1, "XYZ");

        var command = new RegistrationMaterialsOutcomeCommand
        {
            Id = material.Id,
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
            Id = material.Id,
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

  

    private RegistrationMaterial CreateMaterial(RegistrationMaterialStatus? status, ApplicationOrganisationType orgType, int? nationId, string materialCode)
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
