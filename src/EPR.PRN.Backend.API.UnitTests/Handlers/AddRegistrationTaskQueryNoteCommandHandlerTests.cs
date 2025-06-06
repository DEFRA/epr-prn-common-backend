using AutoFixture;
using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Handlers.Regulator;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using FluentAssertions;
using Moq;

namespace EPR.PRN.Backend.API.UnitTests.Handlers;

[TestClass]
public class AddRegistrationTaskQueryNoteCommandHandlerTests
{
   
    private Mock<IRegulatorRegistrationTaskStatusRepository> _mockRepository;
    private RegistrationTaskQueryNoteHandler _handler;
    private static readonly IFixture _fixture = new Fixture();
    [TestInitialize]
    public void Setup()
    {
        _mockRepository = new Mock<IRegulatorRegistrationTaskStatusRepository>();
        _handler = new RegistrationTaskQueryNoteHandler(_mockRepository.Object);
    }

    [TestMethod]
    public async Task Handle_CallsRepositoryWithCorrectParameters()
    {
        // Arrange
        var command = _fixture.Create<AddRegistrationTaskQueryNoteCommand>();

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mockRepository.Verify(r => r.AddRegistrationTaskQueryNoteAsync(
            command.RegulatorRegistrationTaskStatusId,
            command.CreatedBy,
            command.Note), Times.Once);
    }

    [TestMethod]
    public async Task Handle_ThrowsException_WhenRepositoryThrows()
    {
        // Arrange
        var command = _fixture.Create<AddRegistrationTaskQueryNoteCommand>();
        _mockRepository.Setup(r => r.AddRegistrationTaskQueryNoteAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>()))
                       .ThrowsAsync(new Exception("Repository failure"));

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("Repository failure");
    }

    [TestMethod]
    public async Task Handle_DoesNotThrow_WhenNoteIsEmpty()
    {
        // Arrange
        var command = new AddRegistrationTaskQueryNoteCommand
        {
            RegulatorRegistrationTaskStatusId = Guid.NewGuid(),
            CreatedBy = Guid.NewGuid(),
            Note = string.Empty // technically valid depending on validation
        };

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mockRepository.Verify(r => r.AddRegistrationTaskQueryNoteAsync(
            command.RegulatorRegistrationTaskStatusId,
            command.CreatedBy,
            command.Note), Times.Once);
    }

    [TestMethod]
    public async Task Handle_DoesNotThrow_WhenCalledWithValidInput()
    {
        // Arrange
        var command = _fixture.Create<AddRegistrationTaskQueryNoteCommand>();

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().NotThrowAsync();
    }

    [TestMethod]
    public async Task Handle_CanHandleGuidEmptyValues()
    {
        // Arrange
        var command = new AddRegistrationTaskQueryNoteCommand
        {
            RegulatorRegistrationTaskStatusId = Guid.Empty,
            CreatedBy = Guid.Empty,
            Note = "Some note"
        };

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mockRepository.Verify(r => r.AddRegistrationTaskQueryNoteAsync(
            command.RegulatorRegistrationTaskStatusId,
            command.CreatedBy,
            command.Note), Times.Once);
    }
}
