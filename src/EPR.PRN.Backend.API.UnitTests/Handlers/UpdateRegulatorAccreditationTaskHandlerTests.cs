using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Common.Exceptions;
using EPR.PRN.Backend.API.Handlers.Regulator;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using FluentAssertions;
using Moq;

namespace EPR.PRN.Backend.API.Tests.Handlers
{
    [TestClass]
    public class UpdateRegulatorAccreditationTaskHandlerTests
    {
        private Mock<IRegulatorAccreditationTaskStatusRepository> _repositoryMock = null!;
        private UpdateRegulatorAccreditationTaskHandler _handler = null!;

        [TestInitialize]
        public void Setup()
        {
            _repositoryMock = new Mock<IRegulatorAccreditationTaskStatusRepository>();
            _handler = new UpdateRegulatorAccreditationTaskHandler(_repositoryMock.Object);
        }

        [TestMethod]
        public async Task Handle_TaskStatusAlreadyComplete_ShouldThrowRegulatorInvalidOperationException()
        {
            // Arrange
            var command = new UpdateRegulatorAccreditationTaskCommand
            {
                TaskName = "Test Task",
                AccreditationId = Guid.NewGuid(),
                Status = RegulatorTaskStatus.Completed,
                UserName = "UserName"
            };

            var taskStatus = new RegulatorAccreditationTaskStatus
            {
                TaskStatus = new LookupTaskStatus { Name = RegulatorTaskStatus.Completed.ToString() }
            };

            _repositoryMock
                .Setup(r => r.GetTaskStatusAsync(command.TaskName, command.AccreditationId))
                .ReturnsAsync(taskStatus);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should()
                .ThrowAsync<RegulatorInvalidOperationException>()
                .WithMessage($"Cannot set task status to {RegulatorTaskStatus.Completed} as it is already {RegulatorTaskStatus.Completed}");
        }

        [TestMethod]
        public async Task Handle_TaskStatusUnrecognisedTransition_ShouldThrowRegulatorInvalidOperationException()
        {
            // Arrange
            var command = new UpdateRegulatorAccreditationTaskCommand
            {
                TaskName = "Test Task",
                AccreditationId = Guid.NewGuid(),
                Status = RegulatorTaskStatus.Queried,
                UserName = "UserName"
            };

            var taskStatus = new RegulatorAccreditationTaskStatus
            {
                TaskStatus = new LookupTaskStatus { Name = RegulatorTaskStatus.NotStarted.ToString() }
            };

            _repositoryMock
                .Setup(r => r.GetTaskStatusAsync(command.TaskName, command.AccreditationId))
                .ReturnsAsync(taskStatus);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should()
                .ThrowAsync<RegulatorInvalidOperationException>()
                .WithMessage($"Cannot set task status to {RegulatorTaskStatus.Queried} as it is {RegulatorTaskStatus.NotStarted}");
        }

        [TestMethod]
        public async Task Handle_TaskStatusAlreadyQueried_ShouldThrowRegulatorInvalidOperationException()
        {
            // Arrange
            var command = new UpdateRegulatorAccreditationTaskCommand
            {
                TaskName = "Test Task",
                AccreditationId = Guid.NewGuid(),
                Status = RegulatorTaskStatus.Queried,
                UserName = "UserName"
            };

            var taskStatus = new RegulatorAccreditationTaskStatus
            {
                TaskStatus = new LookupTaskStatus { Name = RegulatorTaskStatus.Queried.ToString() }
            };

            _repositoryMock
                .Setup(r => r.GetTaskStatusAsync(command.TaskName, command.AccreditationId))
                .ReturnsAsync(taskStatus);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should()
                .ThrowAsync<RegulatorInvalidOperationException>()
                .WithMessage($"Cannot set task status to {RegulatorTaskStatus.Queried} as it is already {RegulatorTaskStatus.Queried}");
        }

        [TestMethod]
        public async Task Handle_TaskStatusCompleteToQueried_ShouldThrowRegulatorInvalidOperationException()
        {
            // Arrange
            var command = new UpdateRegulatorAccreditationTaskCommand
            {
                TaskName = "Test Task",
                AccreditationId = Guid.NewGuid(),
                Status = RegulatorTaskStatus.Queried,
                UserName = "UserName"
            };

            var taskStatus = new RegulatorAccreditationTaskStatus
            {
                TaskStatus = new LookupTaskStatus { Name = RegulatorTaskStatus.Completed.ToString() }
            };

            _repositoryMock
                .Setup(r => r.GetTaskStatusAsync(command.TaskName, command.AccreditationId))
                .ReturnsAsync(taskStatus);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should()
                .ThrowAsync<RegulatorInvalidOperationException>()
                .WithMessage($"Cannot set task status to {RegulatorTaskStatus.Queried} as it is {RegulatorTaskStatus.Completed}");
        }

        [TestMethod]
        public async Task Handle_InvalidStatusType_ShouldThrowRegulatorInvalidOperationException()
        {
            // Arrange
            var command = new UpdateRegulatorAccreditationTaskCommand
            {
                TaskName = "Test Task",
                AccreditationId = Guid.NewGuid(),
                Status = (RegulatorTaskStatus)999,
                UserName = "UserName"
            };

            var existing = new RegulatorAccreditationTaskStatus
            {
                TaskStatus = new LookupTaskStatus { Name = RegulatorTaskStatus.Completed.ToString() }
            };

            _repositoryMock
                .Setup(r => r.GetTaskStatusAsync(command.TaskName, command.AccreditationId))
                .ReturnsAsync(existing);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should()
                .ThrowAsync<RegulatorInvalidOperationException>()
                .WithMessage($"Invalid status type: {command.Status}");
        }

        [TestMethod]
        public async Task Handle_ValidStatusUpdate_ShouldUpdateStatus()
        {
            // Arrange
            var command = new UpdateRegulatorAccreditationTaskCommand
            {
                TaskName = "Test Task",
                AccreditationId = Guid.NewGuid(),
                Status = RegulatorTaskStatus.Completed,
                Comments = "Completed",
                UserName = "UserName"
            };

            var taskStatus = new RegulatorAccreditationTaskStatus
            {
                TaskStatus = new LookupTaskStatus { Name = RegulatorTaskStatus.Queried.ToString() }
            };

            _repositoryMock
                .Setup(r => r.GetTaskStatusAsync(command.TaskName, command.AccreditationId))
                .ReturnsAsync(taskStatus);

            _repositoryMock
                .Setup(r => r.UpdateStatusAsync(
                    command.TaskName,
                    command.AccreditationId,
                    command.Status,
                    command.Comments,
                    It.IsAny<Guid>()))
                .Returns(Task.CompletedTask);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _repositoryMock.Verify(r => r.UpdateStatusAsync(
                command.TaskName,
                command.AccreditationId,
                command.Status,
                command.Comments,
                It.IsAny<Guid>()),
                Times.Once);
        }
    }
}
