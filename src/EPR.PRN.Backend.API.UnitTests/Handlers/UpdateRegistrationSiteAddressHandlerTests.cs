using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Handlers;
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
        var command = new UpdateRegistrationSiteAddressCommand
        {
            Id = 1,
            ReprocessingSiteAddress = new AddressDto
            {
                AddressLine1 = "123 Main St",
                TownCity = "Testville",
                Country = "UK",
                PostCode = "AB12 3CD"
            },
            LegalDocumentAddress = new AddressDto
            {
                AddressLine1 = "456 Legal Rd",
                TownCity = "Lawtown",
                Country = "UK",
                PostCode = "XY98 7ZT"
            }
        };

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _repositoryMock.Verify(r => r.UpdateSiteAddress(
            command.Id,
            command.ReprocessingSiteAddress,
            command.LegalDocumentAddress),
            Times.Once);
    }

    [TestMethod]
    public async Task Handle_ShouldCompleteSuccessfully_WhenRepositoryCallSucceeds()
    {
        // Arrange
        var command = new UpdateRegistrationSiteAddressCommand
        {
            Id = 1,
            ReprocessingSiteAddress = new AddressDto() { Id = 101 },
            LegalDocumentAddress = new AddressDto()
        };

        int? reprocessingSiteAddressId = 0;
        _repositoryMock.Setup(r => r.UpdateSiteAddress(It.IsAny<int>(), It.IsAny<AddressDto>(), It.IsAny<AddressDto>()))
            .Callback<int, AddressDto, AddressDto>((_, reprocessingSiteAddress, legalDocumentAddress) => reprocessingSiteAddressId = reprocessingSiteAddress.Id)
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
        var command = new UpdateRegistrationSiteAddressCommand
        {
            Id = 1,
            ReprocessingSiteAddress = new AddressDto(),
            LegalDocumentAddress = new AddressDto()
        };

        _repositoryMock.Setup(r => r.UpdateSiteAddress(It.IsAny<int>(), It.IsAny<AddressDto>(), It.IsAny<AddressDto>()))
                       .ThrowsAsync(new KeyNotFoundException("Registration not found."));

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Registration not found.");
    }
}
