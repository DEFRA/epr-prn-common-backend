using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Handlers;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.DTO;
using EPR.PRN.Backend.Data.Interfaces;
using FluentAssertions;
using Moq;

namespace EPR.PRN.Backend.API.UnitTests.Handlers;

[TestClass]
public class UpdateRegistrationSiteAddressHandlerTests
{
    private Mock<IRegistrationRepository> _repositoryMock;
    private UpdateRegistrationSiteAddressHandler _handler;

    [TestInitialize]
    public void TestInitialize()
    {
        _repositoryMock = new Mock<IRegistrationRepository>();
        _handler = new UpdateRegistrationSiteAddressHandler(_repositoryMock.Object);
    }

    [TestMethod]
    public async Task Handle_ShouldCallRepositoryUpdateSiteAddress_WithCorrectParameters()
    {
        // Arrange
        var registrationId = Guid.NewGuid();
        var command = new UpdateRegistrationSiteAddressCommand
        {
            RegistrationId = registrationId,
            ReprocessingSiteAddress = new AddressDto
            {
                AddressLine1 = "123 Main St",
                TownCity = "Testville",
                Country = "UK",
                PostCode = "AB12 3CD"
            }
        };

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _repositoryMock.Verify(r => r.UpdateSiteAddressAsync(
            command.RegistrationId,
            command.ReprocessingSiteAddress),
            Times.Once);
    }

    [TestMethod]
    public async Task Handle_ShouldCompleteSuccessfully_WhenRepositoryCallSucceeds()
    {
        // Arrange
        var registrationId = Guid.NewGuid();
        var command = new UpdateRegistrationSiteAddressCommand
        {
            RegistrationId = registrationId,
            ReprocessingSiteAddress = new AddressDto() { Id = 101 },
        };

        int? reprocessingSiteAddressId = 0;
        _repositoryMock.Setup(r => r.UpdateSiteAddressAsync(It.IsAny<Guid>(), It.IsAny<AddressDto>()))
            .Callback<Guid, AddressDto>((_, reprocessingSiteAddress) => reprocessingSiteAddressId = reprocessingSiteAddress.Id)
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        reprocessingSiteAddressId.Should().BeGreaterThan(0);
    }

    [TestMethod]
    public async Task Handle_ShouldThrowException_WhenRepositoryThrows()
    {
        // Arrange
        var registrationId = Guid.NewGuid();
        var command = new UpdateRegistrationSiteAddressCommand
        {
            RegistrationId = registrationId,
            ReprocessingSiteAddress = new AddressDto(),
        };

        _repositoryMock
            .Setup(r => r.UpdateSiteAddressAsync(It.IsAny<Guid>(), It.IsAny<AddressDto>()))
            .ThrowsAsync(new KeyNotFoundException("Registration not found."));

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Registration not found.");
    }
}