using AutoFixture;
using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Handlers;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using Moq;

namespace EPR.PRN.Backend.API.UnitTests.Handlers;

[TestClass]
public class UpdateRegistrationMaterialPermitsCommandHandlerTests
{
    private Mock<IRegistrationMaterialRepository> _repositoryMock;
    private UpdateRegistrationMaterialPermitsHandler _handler;
    private static readonly Fixture _fixture = new();

    [TestInitialize]
    public void TestInitialize()
    {
        _repositoryMock = new Mock<IRegistrationMaterialRepository>();
        _handler = new UpdateRegistrationMaterialPermitsHandler(_repositoryMock.Object);
    }

    [TestMethod]
    public async Task Handle_ShouldCall_RepositoryMethods_WithCorrectParameters()
    {
        // Arrange
        var registrationMaterialId = Guid.NewGuid();

        var command = _fixture.Build<UpdateRegistrationMaterialPermitsCommand>()
            .With(x => x.PermitTypeId, (int)MaterialPermitType.PollutionPreventionAndControlPermit)
            .With(x => x.PermitNumber, "PPC-1234567890")
            .With(x => x.RegistrationMaterialId, registrationMaterialId)
            .Create();

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _repositoryMock.Verify(r => r.UpdateRegistrationMaterialPermits(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<string>()), Times.Once);
    }
}
