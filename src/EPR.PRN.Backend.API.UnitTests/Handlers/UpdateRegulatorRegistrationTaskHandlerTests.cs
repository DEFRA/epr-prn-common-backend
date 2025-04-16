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
            var command = new UpdateRegulatorRegistrationTaskCommand { TaskName = "Test Task", RegistrationId = 1, Status = RegulatorTaskStatus.Completed, UserName = "UserName" };
            var taskStatus = new RegulatorRegistrationTaskStatus { TaskStatus = new LookupTaskStatus { Name = RegulatorTaskStatus.Completed.ToString() } };
            _repositoryMock.Setup(r => r.GetTaskStatusAsync(command.TaskName, command.TypeId)).ReturnsAsync(taskStatus);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<RegulatorInvalidOperationException>().WithMessage($"Cannot set task status to {RegulatorTaskStatus.Completed} as it is already {RegulatorTaskStatus.Completed}");
        }

        [TestMethod]
        public async Task Handle_TaskStatusAlreadyQueried_ShouldThrowRegulatorInvalidOperationException()
        {
            // Arrange
            var command = new UpdateRegulatorRegistrationTaskCommand { TaskName = "Test Task", RegistrationId = 1, Status = RegulatorTaskStatus.Queried, UserName = "UserName" };
            var taskStatus = new RegulatorRegistrationTaskStatus { TaskStatus = new LookupTaskStatus { Name = RegulatorTaskStatus.Queried.ToString() } };
            _repositoryMock.Setup(r => r.GetTaskStatusAsync(command.TaskName, command.TypeId)).ReturnsAsync(taskStatus);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<RegulatorInvalidOperationException>().WithMessage($"Cannot set task status to {RegulatorTaskStatus.Queried} as it is already {RegulatorTaskStatus.Queried}");
        }

        [TestMethod]
        public async Task Handle_TaskStatusCompleteToQueried_ShouldThrowRegulatorInvalidOperationException()
        {
            // Arrange
            var command = new UpdateRegulatorRegistrationTaskCommand { TaskName = "Test Task", RegistrationId = 1, Status = RegulatorTaskStatus.Queried, UserName = "UserName" };
            var taskStatus = new RegulatorRegistrationTaskStatus { TaskStatus = new LookupTaskStatus { Name = RegulatorTaskStatus.Completed.ToString() } };
            _repositoryMock.Setup(r => r.GetTaskStatusAsync(command.TaskName, command.TypeId)).ReturnsAsync(taskStatus);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<RegulatorInvalidOperationException>().WithMessage($"Cannot set task status to {RegulatorTaskStatus.Queried} as it is {RegulatorTaskStatus.Completed}");
        }

        [TestMethod]
        public async Task Handle_InvalidStatusType_ShouldThrowRegulatorInvalidOperationException()
        {
            // Arrange
            var command = new UpdateRegulatorRegistrationTaskCommand { TaskName = "Test Task", RegistrationId = 1, Status = (RegulatorTaskStatus)999, UserName = "UserName" };
            var taskStatus = new RegulatorRegistrationTaskStatus { TaskStatus = new LookupTaskStatus { Name = RegulatorTaskStatus.Completed.ToString() } };
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
            var command = new UpdateRegulatorRegistrationTaskCommand { TaskName = "Test Task", RegistrationId = 1, Status = RegulatorTaskStatus.Completed, Comments = "Completed", UserName = "UserName" };
            var taskStatus = new RegulatorRegistrationTaskStatus { TaskStatus = new LookupTaskStatus { Name = RegulatorTaskStatus.Queried.ToString() } };
            _repositoryMock.Setup(r => r.GetTaskStatusAsync(command.TaskName, command.TypeId)).ReturnsAsync(taskStatus);
            _repositoryMock.Setup(r => r.UpdateStatusAsync(command.TaskName, command.TypeId, command.Status, command.Comments, command.UserName)).Returns(Task.CompletedTask);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _repositoryMock.Verify(r => r.UpdateStatusAsync(command.TaskName, command.TypeId, command.Status, command.Comments, command.UserName), Times.Once);
        }
    }
}
