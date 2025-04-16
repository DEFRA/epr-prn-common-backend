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
    public async Task Handle_ShouldUpdateRegistrationOutcome_WhenValidTransition()
    {
        // Arrange
        var material = new RegistrationMaterial
        {
            Id = 1,
            StatusID = (int)RegistrationMaterialStatus.Refused,
            MaterialId = 10,
            Material = new LookupMaterial
            {
                Id = 10,
                MaterialCode = "XYZ"
            },
            RegistrationId = 123,
            Registration = new Registration
            {
                Id = 123,
                ApplicationTypeId = (int)ApplicationOrganisationType.Exporter,
                BusinessAddressId = 99,
                BusinessAddress = new LookupAddress
                {
                    Id = 99,
                    Country = "USA"
                }
            }
        };
        
        var command = new RegistrationMaterialsOutcomeCommand
        {
            Id = 1,
            Status = RegistrationMaterialStatus.Granted,
            Comments = "Now granted."
        };

        _rmRepositoryMock.Setup(x => x.GetRegistrationMaterialById(command.Id)).ReturnsAsync(material);
        _rmRepositoryMock.Setup(x => x.UpdateRegistrationOutCome(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
                         .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _rmRepositoryMock.Verify(x => x.UpdateRegistrationOutCome(
            command.Id,
            (int)command.Status,
            command.Comments,
            It.Is<string>(s =>
                s.Contains("USA") && 
                s.Contains("XYZ")    
            )
        ), Times.Once);
    }

    [TestMethod]
    public async Task Handle_ShouldThrowInvalidOperationException_WhenInvalidTransition()
    {
        // Arrange
        var material = new RegistrationMaterial
        {
            Id = 1,
            StatusID = (int)RegistrationMaterialStatus.Granted,
            RegistrationId = 123,
            Registration = new Registration
            {
                Id = 123,
                ApplicationTypeId = (int)ApplicationOrganisationType.Exporter,
                BusinessAddressId = 99
            }
        };

        var command = new RegistrationMaterialsOutcomeCommand
        {
            Id = 1,
            Status = RegistrationMaterialStatus.Granted,
            Comments = "No change"
        };

        _rmRepositoryMock.Setup(x => x.GetRegistrationMaterialById(command.Id)).ReturnsAsync(material);
        
        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Invalid outcome transition.");
    }

    [TestMethod]
    public async Task Handle_ShouldGenerateValidReferenceNumber()
    {
        // Arrange
        var material = new RegistrationMaterial
        {
            Id = 1,
            RegistrationId = 456,
            Registration = new Registration
            {
                Id = 456,
                ApplicationTypeId = (int)ApplicationOrganisationType.Reprocessor,
                ReprocessingSiteAddressId = 88,
                ReprocessingSiteAddress = new LookupAddress
                {
                    Id = 88,
                    Country = "Germany"
                }
            },
            StatusID = null,
            MaterialId = 10,
            Material = new LookupMaterial
            {
                Id = 10,
                MaterialCode = "PLS"
            }
        };

        var command = new RegistrationMaterialsOutcomeCommand
        {
            Id = 1,
            Status = RegistrationMaterialStatus.Granted,
            Comments = "Incorrect details"
        };

        _rmRepositoryMock.Setup(x => x.GetRegistrationMaterialById(command.Id)).ReturnsAsync(material);
        
        string generatedReference = null;
        _rmRepositoryMock.Setup(x => x.UpdateRegistrationOutCome(command.Id, (int)command.Status, command.Comments, It.IsAny<string>()))
                         .Callback<int, int, string, string>((_, _, _, reference) => generatedReference = reference)
                         .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        generatedReference.Should().NotBeNull();
        generatedReference.Should().Contain("R").And.Contain("GER").And.Contain("P").And.Contain("PLS");
    }
}
