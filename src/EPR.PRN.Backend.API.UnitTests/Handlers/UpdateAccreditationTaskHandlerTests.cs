using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Handlers;
using EPR.PRN.Backend.API.Handlers.Accreditation;
using EPR.PRN.Backend.API.Helpers;
using EPR.PRN.Backend.Data.DataModels.Accreditations;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces;
using EPR.PRN.Backend.Data.Interfaces.Accreditation;
using FluentAssertions;
using Moq;

namespace EPR.PRN.Backend.API.UnitTests.Handlers
{
    [TestClass]
    public class UpdateAccreditationTaskHandlerTests
    {
        [TestMethod]
        [DataRow("InProgress","Completed")]
        [DataRow("NotStarted","InProgress")]
        [DataRow("InProgress", "Queried")]
        [DataRow("Queried", "Completed")]
        [DataRow("Queried", "Queried")]
        [DataRow("Completed", "Queried")]
        public Task Handle_CallsUpdateAccreditationTaskAsync(string status, string statusReturned)
        {
            // Arrange
            var mockRepo = new Mock<IAccreditationTaskStatusRepository>();
            mockRepo.Setup(r => r.GetTaskStatusAsync(It.IsAny<string>(), It.IsAny<Guid>()))
                .ReturnsAsync(new Data.DataModels.Accreditations.AccreditationTaskStatus
                {
                    TaskStatus = new LookupTaskStatus { Name = statusReturned },

                    Accreditation = new()

                });
            mockRepo.Setup(r => r.UpdateStatusAsync(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<TaskStatuses>()))
                .Returns(Task.CompletedTask);
            var handler = new UpdateAccreditationTaskHandler(mockRepo.Object);
            var command = new UpdateAccreditationTaskCommand
            {
                TaskName = statusReturned,
                AccreditationId = Guid.NewGuid(),
                Status = TaskStatuses.Completed
            };

            // Act
            var task = handler.Handle(command, CancellationToken.None);

            // Assert
            task.IsCompleted.Should().BeTrue();         
            return Task.CompletedTask;
        }

        [TestMethod]
        [DataRow(TaskStatuses.Completed, "Completed")]
        [DataRow(TaskStatuses.Queried, "Queried")]
        [DataRow(TaskStatuses.Queried, "Completed")]

        public async Task Handle_ThrowsInvalidOperationException_WhenRepositoryThrows(TaskStatuses commnandStatus, string taskStatus)
        {
            // Arrange
            var mockRepo = new Mock<IAccreditationTaskStatusRepository>();

            mockRepo.Setup(r => r.GetTaskStatusAsync(It.IsAny<string>(), It.IsAny<Guid>()))
          .ReturnsAsync(new Data.DataModels.Accreditations.AccreditationTaskStatus
          {
              TaskStatus = new LookupTaskStatus { Name = taskStatus },

              Accreditation = new()

          });
            mockRepo.Setup(r => r.UpdateStatusAsync(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<TaskStatuses>()))
                .ThrowsAsync(new InvalidOperationException("Test exception"));

            var handler = new UpdateAccreditationTaskHandler(mockRepo.Object);
            var command = new UpdateAccreditationTaskCommand
            {
                TaskName = "AnyTask",
                AccreditationId = Guid.NewGuid(),
                Status = commnandStatus// TaskStatuses.Completed
            };

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>().WithMessage($"Cannot set task status to {commnandStatus} as it is already {taskStatus}");
        }


        [TestMethod]
        public async Task Handle_ThrowsInvalidOperationException_WhenTaskStatusIsNull()
        {
    
            // Arrange
            var mockRepo = new Mock<IAccreditationTaskStatusRepository>();

            mockRepo.Setup(r => r.GetTaskStatusAsync(It.IsAny<string>(), It.IsAny<Guid>()))
          .ReturnsAsync((AccreditationTaskStatus)null);
            mockRepo.Setup(r => r.UpdateStatusAsync(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<TaskStatuses>()))
                .ThrowsAsync(new InvalidOperationException("Test exception"));

            var handler = new UpdateAccreditationTaskHandler(mockRepo.Object);
            var command = new UpdateAccreditationTaskCommand
            {
                TaskName = "AnyTask",
                AccreditationId = Guid.NewGuid(),
                Status = TaskStatuses.Completed
            };

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>().WithMessage($"Task with name {command.TaskName} and accreditation ID {command.AccreditationId} not found.");
        }
    }
}
