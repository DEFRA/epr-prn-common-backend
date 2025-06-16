using AutoFixture;
using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Handlers;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using FluentAssertions;
using Moq;

namespace EPR.PRN.Backend.API.UnitTests.Handlers;

[TestClass]
public class UpdateRegistrationMaterialPermitsCommandHandlerTests
{
    private Mock<IRegistrationMaterialRepository> _repositoryMock;
    private UpdateRegistrationMaterialPermitsHandler _handler;
    private static readonly IFixture _fixture = new Fixture();

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

        var registrationMaterial = new RegistrationMaterial
        {
            Id = 1,
            ExternalId = registrationMaterialId,
            PPCPermitNumber = string.Empty,
        };

        _repositoryMock.Setup(r => r.GetRegistrationMaterialById(It.IsAny<Guid>()))
            .ReturnsAsync(registrationMaterial);

        var command = _fixture.Build<UpdateRegistrationMaterialPermitsCommand>()
            .With(x => x.PermitTypeId, (int)MaterialPermitType.PollutionPreventionAndControlPermit)
            .With(x => x.PermitNumber, "PPC-1234567890")
            .With(x => x.RegistrationMaterialId, registrationMaterialId)
            .Create();

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _repositoryMock.Verify(r => r.GetRegistrationMaterialById(registrationMaterialId), Times.Once);
        _repositoryMock.Verify(r => r.UpdateRegistrationMaterialPermits(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<string?>()), Times.Once);
    }

    [TestMethod]
    public async Task Handle_ShouldThrowKeyNotFoundException_WhenRegistrationMaterialNotFound()
    {
        // Arrange
        var command = _fixture.Create<UpdateRegistrationMaterialPermitsCommand>();

        _repositoryMock.Setup(r => r.GetRegistrationMaterialById(command.RegistrationMaterialId))
            .ReturnsAsync((RegistrationMaterial)null);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Registration Material not found for external id: {command.RegistrationMaterialId}");
    }

}
