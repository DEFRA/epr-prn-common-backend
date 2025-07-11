using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Handlers;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using Moq;

namespace EPR.PRN.Backend.API.UnitTests.Handlers;

[TestClass]
public class UpdateMaximumWeightCommandHandlerTests
{
    private Mock<IRegistrationMaterialRepository> _repositoryMock;
    private UpdateMaximumWeightCommandHandler _handler;

    [TestInitialize]
    public void Setup()
    {
        _repositoryMock = new Mock<IRegistrationMaterialRepository>();
        _handler = new UpdateMaximumWeightCommandHandler(_repositoryMock.Object);
    }

    [TestMethod]
    public async Task Handle_ShouldCallRepositoryWithCorrectParameters()
    {
        // Arrange  
        var registrationMaterialId = Guid.NewGuid();
        var command = new UpdateMaximumWeightCommand
        {
            RegistrationMaterialId = registrationMaterialId,
            WeightInTonnes = 10,
            PeriodId = 1
        };

        _repositoryMock
            .Setup(r => r.UpdateMaximumWeightForSiteAsync(
                registrationMaterialId,
                10,
                1)).Returns(Task.CompletedTask).Verifiable();

        // Act  
        await _handler.Handle(command, CancellationToken.None);

        // Assert  
        _repositoryMock.Verify();
        _repositoryMock.Verify(r => r.UpdateMaximumWeightForSiteAsync(
                registrationMaterialId,
                10,
                1),
            Times.Once);
    }
}