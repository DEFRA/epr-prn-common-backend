using AutoFixture;
using AutoMapper;
using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Handlers;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using Moq;

namespace EPR.PRN.Backend.API.UnitTests.Handlers;

[TestClass]
public class UpsertRegistrationMaterialContactCommandHandlerTests
{
    private Mock<IRegistrationMaterialRepository> _repositoryMock;
    private UpsertRegistrationMaterialContactHandler _handler;
    private Mock<IMapper> _mapperMock;
    private static readonly Fixture Fixture = new();

    [TestInitialize]
    public void TestInitialize()
    {
        _repositoryMock = new Mock<IRegistrationMaterialRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new UpsertRegistrationMaterialContactHandler(_repositoryMock.Object, _mapperMock.Object);
    }

    [TestMethod]
    public async Task Handle_ShouldCall_RepositoryMethods_WithCorrectParameters()
    {
        // Arrange
        var command = Fixture.Create<UpsertRegistrationMaterialContactCommand>();

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _repositoryMock.Verify(r => r.UpsertRegistrationMaterialContact(command.RegistrationMaterialId, command.UserId), Times.Once);
    }
}