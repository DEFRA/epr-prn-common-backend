using AutoFixture;
using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Handlers;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using Moq;

namespace EPR.PRN.Backend.API.UnitTests.Handlers;

[TestClass]
public class UpdateRegistrationMaterialPermitCapacityCommandHandlerTests
{
    private Mock<IRegistrationMaterialRepository> _repositoryMock;
    private UpdateRegistrationMaterialPermitCapacityHandler _handler;
    private static readonly Fixture _fixture = new();

    [TestInitialize]
    public void TestInitialize()
    {
        _repositoryMock = new Mock<IRegistrationMaterialRepository>();
        _handler = new UpdateRegistrationMaterialPermitCapacityHandler(_repositoryMock.Object);
    }

    [TestMethod]
    public async Task Handle_ShouldCall_RepositoryMethods_WithCorrectParameters()
    {
        // Arrange
        var registrationMaterialId = Guid.NewGuid();

        var command = _fixture.Build<UpdateRegistrationMaterialPermitCapacityCommand>()
            .With(x => x.PermitTypeId, (int)MaterialPermitType.PollutionPreventionAndControlPermit)
            .With(x => x.CapacityInTonnes, 1000)
            .With(x => x.PeriodId, 1)
            .With(x => x.RegistrationMaterialId, registrationMaterialId)
            .Create();

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _repositoryMock.Verify(r => r.UpdateRegistrationMaterialPermitCapacity(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<decimal>(), It.IsAny<int?>()), Times.Once);
    }
}
