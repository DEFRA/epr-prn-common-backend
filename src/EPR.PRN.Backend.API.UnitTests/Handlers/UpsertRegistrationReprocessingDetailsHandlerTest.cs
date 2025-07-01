using AutoFixture;
using AutoMapper;
using EPR.PRN.Backend.API.Handlers;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.DTO;
using EPR.PRN.Backend.Data.Interfaces;
using Moq;

namespace EPR.PRN.Backend.API.UnitTests.Handlers;

[TestClass]
public class UpsertRegistrationReprocessingDetailsHandlerTest
{
    private Mock<IMaterialRepository> _repositoryMock;
    private UpsertRegistrationReprocessingDetailsHandler _handler;
    private Mock<IMapper> _mapperMock;
    private static readonly Fixture Fixture = new();

    [TestInitialize]
    public void TestInitialize()
    {
        _repositoryMock = new Mock<IMaterialRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new UpsertRegistrationReprocessingDetailsHandler(_repositoryMock.Object, _mapperMock.Object);
    }

    [TestMethod]
    public async Task Handle_ShouldCall_RepositoryMethods_WithCorrectParameters()
    {
        // Arrange
        var command = Fixture.Create<RegistrationReprocessingIOCommand>();

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _repositoryMock.Verify(r => r.UpsertRegistrationReprocessingDetailsAsync(command.ExternalId, It.IsAny<RegistrationReprocessingIO>()), Times.Once);
    }
}