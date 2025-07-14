using AutoFixture;
using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Handlers;
using EPR.PRN.Backend.Data.Interfaces;
using Moq;

namespace EPR.PRN.Backend.API.UnitTests.Handlers;

[TestClass]
public class UpdateMaterialNotReprocessingReasonHandlerTest
{
    private Mock<IMaterialRepository> _repositoryMock;
    private UpdateMaterialNotReprocessingReasonHandler _handler;
    private static readonly Fixture Fixture = new();

    [TestInitialize]
    public void TestInitialize()
    {
        _repositoryMock = new Mock<IMaterialRepository>();
        _handler = new UpdateMaterialNotReprocessingReasonHandler(_repositoryMock.Object);
    }

    [TestMethod]
    public async Task Handle_ShouldCallRepository_WithCorrectParameters()
    {
        // Arrange
        var command = Fixture.Create<UpdateMaterialNotReprocessingReasonCommand>();

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _repositoryMock.Verify(r =>
            r.UpdateMaterialNotReprocessingReason(
                command.RegistrationMaterialId,
                command.MaterialNotReprocessingReason),
            Times.Once);
    }

    [TestMethod]
    [ExpectedException(typeof(NullReferenceException))]
    public async Task Handle_ShouldThrow_WhenCommandIsNull()
    {
        // Act
        await _handler.Handle(null, CancellationToken.None);
    }

    [TestMethod]
    public async Task Handle_ShouldThrow_WhenRepositoryThrows()
    {
        // Arrange
        var command = Fixture.Create<UpdateMaterialNotReprocessingReasonCommand>();
        _repositoryMock
            .Setup(r => r.UpdateMaterialNotReprocessingReason(It.IsAny<Guid>(), It.IsAny<string>()))
            .ThrowsAsync(new InvalidOperationException("Database error"));

        // Act & Assert
        await Assert.ThrowsExceptionAsync<InvalidOperationException>(() =>
            _handler.Handle(command, CancellationToken.None));
    }

    [TestMethod]
    public async Task Handle_ShouldPassCancellationToken()
    {
        // Arrange
        var command = Fixture.Create<UpdateMaterialNotReprocessingReasonCommand>();
        var cancellationToken = new CancellationTokenSource().Token;

        _repositoryMock
            .Setup(r => r.UpdateMaterialNotReprocessingReason(command.RegistrationMaterialId, command.MaterialNotReprocessingReason))
            .Returns(Task.CompletedTask)
            .Verifiable();

        // Act
        await _handler.Handle(command, cancellationToken);

        // Assert
        _repositoryMock.Verify();
    }
}