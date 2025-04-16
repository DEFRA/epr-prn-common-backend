using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Handlers;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using FluentAssertions;
using MediatR;
using Moq;
using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Common.Exceptions;

namespace EPR.PRN.Backend.API.Tests.Handlers
{
    [TestClass]
    public class UpdateRegulatorApplicationTaskHandlerTests
    {
        private Mock<IRegulatorApplicationTaskStatusRepository> _repositoryMock;
        private UpdateRegulatorApplicationTaskHandler _handler;

        [TestInitialize]
        public void Setup()
        {
            _repositoryMock = new Mock<IRegulatorApplicationTaskStatusRepository>();
            _handler = new UpdateRegulatorApplicationTaskHandler(_repositoryMock.Object);
        }

        [TestMethod]
        public async Task Handle_TaskStatusAlreadyComplete_ShouldThrowRegulatorInvalidOperationException()
        {
            // Arrange
            var command = new UpdateRegulatorApplicationTaskCommand { TaskName = "Test Task", RegistrationMaterialId = 1, Status = RegulatorTaskStatus.Completed, UserName = "UserName" };
            var taskStatus = new RegulatorApplicationTaskStatus { TaskStatus = new LookupTaskStatus { Name = RegulatorTaskStatus.Completed.ToString() } };
            _repositoryMock.Setup(r => r.GetTaskStatusAsync(command.TaskName, command.TypeId)).ReturnsAsync(taskStatus);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<RegulatorInvalidOperationException>().WithMessage($"Cannot set task status to {RegulatorTaskStatus.Completed} as it is already {RegulatorTaskStatus.Completed}: {command.TaskName}:{command.TypeId}");
        }
        [TestMethod]
        public async Task Handle_TaskStatusUnrecognised_ShouldThrowRegulatorInvalidOperationException()
        {
            // Arrange
            var command = new UpdateRegulatorApplicationTaskCommand { TaskName = "Test Task", RegistrationMaterialId = 1, Status = RegulatorTaskStatus.Queried, UserName = "UserName" };
            var taskStatus = new RegulatorApplicationTaskStatus { TaskStatus = new LookupTaskStatus { Name = RegulatorTaskStatus.NotStarted.ToString() } };
            _repositoryMock.Setup(r => r.GetTaskStatusAsync(command.TaskName, command.TypeId)).ReturnsAsync(taskStatus);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<RegulatorInvalidOperationException>().WithMessage($"Cannot set task status to {RegulatorTaskStatus.Queried} as it is NotStarted: {command.TaskName}:{command.TypeId}");
        }

        [TestMethod]
        public async Task Handle_TaskStatusAlreadyQueried_ShouldThrowRegulatorInvalidOperationException()
        {
            // Arrange
            var command = new UpdateRegulatorApplicationTaskCommand { TaskName = "Test Task", RegistrationMaterialId = 1, Status = RegulatorTaskStatus.Queried, UserName = "UserName" };
            var taskStatus = new RegulatorApplicationTaskStatus { TaskStatus = new LookupTaskStatus { Name = RegulatorTaskStatus.Queried.ToString() } };
            _repositoryMock.Setup(r => r.GetTaskStatusAsync(command.TaskName, command.TypeId)).ReturnsAsync(taskStatus);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<RegulatorInvalidOperationException>().WithMessage($"Cannot set task status to {RegulatorTaskStatus.Queried} as it is already {RegulatorTaskStatus.Queried}: {command.TaskName}:{command.TypeId}");
        }

        [TestMethod]
        public async Task Handle_TaskStatusCompleteToQueried_ShouldThrowRegulatorInvalidOperationException()
        {
            // Arrange
            var command = new UpdateRegulatorApplicationTaskCommand { TaskName = "Test Task", RegistrationMaterialId = 1, Status = RegulatorTaskStatus.Queried, UserName = "UserName" };
            var taskStatus = new RegulatorApplicationTaskStatus { TaskStatus = new LookupTaskStatus { Name = RegulatorTaskStatus.Completed.ToString() } };
            _repositoryMock.Setup(r => r.GetTaskStatusAsync(command.TaskName, command.TypeId)).ReturnsAsync(taskStatus);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<RegulatorInvalidOperationException>().WithMessage($"Cannot set task status to {RegulatorTaskStatus.Queried} as it is {RegulatorTaskStatus.Completed}: {command.TaskName}:{command.TypeId}");
        }

        [TestMethod]
        public async Task Handle_InvalidStatusType_ShouldThrowRegulatorInvalidOperationException()
        {
            // Arrange
            var command = new UpdateRegulatorApplicationTaskCommand { TaskName = "Test Task", RegistrationMaterialId = 1, Status = (RegulatorTaskStatus)999, UserName = "UserName" };
            var taskStatus = new RegulatorApplicationTaskStatus { TaskStatus = new LookupTaskStatus { Name = RegulatorTaskStatus.Completed.ToString() } };
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
            var command = new UpdateRegulatorApplicationTaskCommand { TaskName = "Test Task", RegistrationMaterialId = 1, Status = RegulatorTaskStatus.Completed, Comment = "Completed", UserName = "UserName" };
            var taskStatus = new RegulatorApplicationTaskStatus { TaskStatus = new LookupTaskStatus { Name = RegulatorTaskStatus.Queried.ToString() } };
            _repositoryMock.Setup(r => r.GetTaskStatusAsync(command.TaskName, command.TypeId)).ReturnsAsync(taskStatus);
            _repositoryMock.Setup(r => r.UpdateStatusAsync(command.TaskName, command.TypeId, command.Status, command.Comment, command.UserName)).Returns(Task.CompletedTask);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _repositoryMock.Verify(r => r.UpdateStatusAsync(command.TaskName, command.TypeId, command.Status, command.Comment, command.UserName), Times.Once);
        }
    }
}
