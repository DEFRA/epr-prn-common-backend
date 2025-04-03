using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Handlers;
using Moq;
using FluentAssertions;
using EPR.PRN.Backend.Data.Interfaces.Regulator;

namespace EPR.PRN.Backend.Tests.Handlers
{
    [TestClass]
    public class UpdateRegulatorApplicationTaskHandlerTests
    {
        private Mock<IRegulatorApplicationTaskStatusRepository> _mockRepository;
        private UpdateRegulatorApplicationTaskHandler _handler;

        [TestInitialize]
        public void Setup()
        {
            _mockRepository = new Mock<IRegulatorApplicationTaskStatusRepository>();
            _handler = new UpdateRegulatorApplicationTaskHandler(_mockRepository.Object);
        }

        [TestMethod]
        public async Task Handle_ShouldCallUpdateStatusAsync_WhenCommandIsValid()
        {
            // Arrange
            var command = new UpdateRegulatorApplicationTaskCommand
            {
                Id = 1,
                Status = StatusTypes.Complete,
                Comment = null
            };

            _mockRepository
                .Setup(repo => repo.UpdateStatusAsync(command.Id, command.Status, command.Comment))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockRepository.Verify(repo => repo.UpdateStatusAsync(command.Id, command.Status, command.Comment), Times.Once);
        }

        [TestMethod]
        public async Task Handle_ShouldCallUpdateStatusAsync_WhenCommandStatusIsQueried()
        {
            // Arrange
            var command = new UpdateRegulatorApplicationTaskCommand
            {
                Id = 1,
                Status = StatusTypes.Queried,
                Comment = "Task Queried"
            };

            _mockRepository
                .Setup(repo => repo.UpdateStatusAsync(command.Id, command.Status, command.Comment))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockRepository.Verify(repo => repo.UpdateStatusAsync(command.Id, command.Status, command.Comment), Times.Once);
        }

        [TestMethod]
        public async Task Handle_ShouldThrowException_WhenUpdateFails()
        {
            // Arrange
            var command = new UpdateRegulatorApplicationTaskCommand
            {
                Id = 1,
                Status = StatusTypes.Complete,
                Comment = null
            };

            _mockRepository
                .Setup(repo => repo.UpdateStatusAsync(command.Id, command.Status, command.Comment))
                .ThrowsAsync(new Exception("Update failed"));

            // Act
            Func<Task> action = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await action.Should().ThrowAsync<Exception>();
            _mockRepository.Verify(repo => repo.UpdateStatusAsync(command.Id, command.Status, command.Comment), Times.Once);
        }
    }
}
