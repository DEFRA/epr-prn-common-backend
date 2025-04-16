using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Common.Exceptions;
using EPR.PRN.Backend.API.Handlers;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using FluentAssertions;
using MediatR;
using Moq;

namespace EPR.PRN.Backend.API.Tests.Handlers
{
    [TestClass]
    public class UpdateRegulatorRegistrationTaskHandlerTests
    {
        private Mock<IRegulatorRegistrationTaskStatusRepository> _repositoryMock;
        private UpdateRegulatorRegistrationTaskHandler _handler;

        [TestInitialize]
        public void Setup()
        {
            _repositoryMock = new Mock<IRegulatorRegistrationTaskStatusRepository>();
            _handler = new UpdateRegulatorRegistrationTaskHandler(_repositoryMock.Object);
        }

        [TestMethod]
        public async Task Handle_TaskStatusAlreadyComplete_ShouldThrowRegulatorInvalidOperationException()
        {
            // Arrange
            var command = new UpdateRegulatorRegistrationTaskCommand { TaskName = "Test Task", RegistrationId = 1, Status = StatusTypes.Completed, UserName = "UserName" };
            var taskStatus = new RegulatorRegistrationTaskStatus { TaskStatus = new LookupTaskStatus { Name = StatusTypes.Completed.ToString() } };
            _repositoryMock.Setup(r => r.GetTaskStatusAsync(command.TaskName, command.TypeId)).ReturnsAsync(taskStatus);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<RegulatorInvalidOperationException>().WithMessage($"Cannot set task status to {StatusTypes.Completed} as it is already {StatusTypes.Completed}: {command.TaskName}:{command.TypeId}");
        }

        [TestMethod]
        public async Task Handle_TaskStatusAlreadyQueried_ShouldThrowRegulatorInvalidOperationException()
        {
            // Arrange
            var command = new UpdateRegulatorRegistrationTaskCommand { TaskName = "Test Task", RegistrationId = 1, Status = StatusTypes.Queried, UserName = "UserName" };
            var taskStatus = new RegulatorRegistrationTaskStatus { TaskStatus = new LookupTaskStatus { Name = StatusTypes.Queried.ToString() } };
            _repositoryMock.Setup(r => r.GetTaskStatusAsync(command.TaskName, command.TypeId)).ReturnsAsync(taskStatus);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<RegulatorInvalidOperationException>().WithMessage($"Cannot set task status to {StatusTypes.Queried} as it is already {StatusTypes.Queried}: {command.TaskName}:{command.TypeId}");
        }

        [TestMethod]
        public async Task Handle_TaskStatusCompleteToQueried_ShouldThrowRegulatorInvalidOperationException()
        {
            // Arrange
            var command = new UpdateRegulatorRegistrationTaskCommand { TaskName = "Test Task", RegistrationId = 1, Status = StatusTypes.Queried, UserName = "UserName" };
            var taskStatus = new RegulatorRegistrationTaskStatus { TaskStatus = new LookupTaskStatus { Name = StatusTypes.Completed.ToString() } };
            _repositoryMock.Setup(r => r.GetTaskStatusAsync(command.TaskName, command.TypeId)).ReturnsAsync(taskStatus);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<RegulatorInvalidOperationException>().WithMessage($"Cannot set task status to {StatusTypes.Queried} as it is {StatusTypes.Completed}: {command.TaskName}:{command.TypeId}");
        }

        [TestMethod]
        public async Task Handle_InvalidStatusType_ShouldThrowRegulatorInvalidOperationException()
        {
            // Arrange
            var command = new UpdateRegulatorRegistrationTaskCommand { TaskName = "Test Task", RegistrationId = 1, Status = (StatusTypes)999, UserName = "UserName" };
            var taskStatus = new RegulatorRegistrationTaskStatus { TaskStatusId = 1 };
            _repositoryMock.Setup(r => r.GetTaskStatusAsync(command.TaskName, command.TypeId)).ReturnsAsync(taskStatus);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<RegulatorInvalidOperationException>().WithMessage($"Invalid status type: {command.Status}");
        }

        [TestMethod]
        public async Task Handle_ValidStatusUpdate_ShouldUpdateStatus()
        {
            // Arrange
            var command = new UpdateRegulatorRegistrationTaskCommand { TaskName = "Test Task", RegistrationId = 1, Status = StatusTypes.Completed, Comment = "Completed", UserName = "UserName" };
            var taskStatus = new RegulatorRegistrationTaskStatus { TaskStatus = new LookupTaskStatus { Name = StatusTypes.Queried.ToString() } };
            _repositoryMock.Setup(r => r.GetTaskStatusAsync(command.TaskName, command.TypeId)).ReturnsAsync(taskStatus);
            _repositoryMock.Setup(r => r.UpdateStatusAsync(command.TaskName, command.TypeId, command.Status, command.Comment, command.UserName)).Returns(Task.CompletedTask);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _repositoryMock.Verify(r => r.UpdateStatusAsync(command.TaskName, command.TypeId, command.Status, command.Comment, command.UserName), Times.Once);
        }
    }
}
