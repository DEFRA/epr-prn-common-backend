using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Common.Dto;
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
    public void Setup()
    {
        _rmRepositoryMock = new Mock<IRegistrationMaterialRepository>();
        _handler = new RegistrationMaterialsOutcomeHandler(_rmRepositoryMock.Object);
    }

    [TestMethod]
    public async Task Handle_ShouldUpdateRegistrationOutcome_WhenValidTransition()
    {
        // Arrange
        var material = new RegistrationMaterial
        {
            Id = 1,
            StatusID = (int)RegistrationMaterialStatus.Granted,
            RegistrationId = 123
        };

        var command = new RegistrationMaterialsOutcomeCommand
        {
            Id = 1,
            Status = RegistrationMaterialStatus.Refused, 
            Comments = "Invalid outcome."
        };

        var referenceData = new RegistrationReferenceBackendDto
        {
            RegistrationType = "R",
            CountryCode = "US",
            OrganisationType = "Corp",
            MaterialCode = "XYZ"
        };

        _rmRepositoryMock.Setup(x => x.GetRegistrationMaterialById(command.Id)).ReturnsAsync(material);
        _rmRepositoryMock.Setup(x => x.GetRegistrationReferenceDataId(material.RegistrationId, command.Id)).ReturnsAsync(referenceData);
        _rmRepositoryMock.Setup(x => x.UpdateRegistrationOutCome(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
                         .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _rmRepositoryMock.Verify(x => x.UpdateRegistrationOutCome(command.Id, (int)command.Status, command.Comments, It.IsAny<string>()), Times.Once);
    }

    [TestMethod]
    public async Task Handle_ShouldThrowInvalidOperationException_WhenInvalidTransition()
    {
        // Arrange
        var material = new RegistrationMaterial
        {
            Id = 1,
            StatusID = (int)RegistrationMaterialStatus.Granted,
            RegistrationId = 123
        };

        var command = new RegistrationMaterialsOutcomeCommand
        {
            Id = 1,
            Status = RegistrationMaterialStatus.Granted,
            Comments = "Invalid outcome."
        };

        _rmRepositoryMock.Setup(x => x.GetRegistrationMaterialById(command.Id)).ReturnsAsync(material);

        // Act & Assert
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("Invalid outcome transition.");
    }

    [TestMethod]
    public async Task Handle_ShouldGenerateCorrectRegistrationReferenceNumber()
    {
        // Arrange
        var material = new RegistrationMaterial
        {
            Id = 1,
            StatusID = (int)RegistrationMaterialStatus.Granted,
            RegistrationId = 123
        };

        var command = new RegistrationMaterialsOutcomeCommand
        {
            Id = 1,
            Status = RegistrationMaterialStatus.Refused,
            Comments = "Invalid outcome."
        };

        var referenceData = new RegistrationReferenceBackendDto
        {
            RegistrationType = "R",
            CountryCode = "US",
            OrganisationType = "Corp",
            MaterialCode = "XYZ"
        };

        
        _rmRepositoryMock.Setup(x => x.GetRegistrationMaterialById(command.Id)).ReturnsAsync(material);
        _rmRepositoryMock.Setup(x => x.GetRegistrationReferenceDataId(material.RegistrationId, command.Id)).ReturnsAsync(referenceData);
        _rmRepositoryMock.Setup(x => x.UpdateRegistrationOutCome(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
                         .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        
        _rmRepositoryMock.Verify(x => x.UpdateRegistrationOutCome(command.Id, (int)command.Status, command.Comments, It.Is<string>(s =>
            s.Contains("23") &&
            s.Contains("123") && 
            s.Contains("XYZ") 
        )), Times.Once);
    }
}
