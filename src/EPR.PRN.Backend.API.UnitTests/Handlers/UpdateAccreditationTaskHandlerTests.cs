using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Handlers;
using EPR.PRN.Backend.API.Handlers.Accreditation;
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
        public async Task Handle_CallsUpdateAccreditationTaskAsync()
        {
            // Arrange
            var mockRepo = new Mock<IAccreditationTaskStatusRepository>();
            mockRepo.Setup(r => r.GetTaskStatusAsync(It.IsAny<string>(), It.IsAny<Guid>()))
                .ReturnsAsync(new Data.DataModels.Accreditations.AccreditationTaskStatus
                {
                    TaskStatus = new LookupTaskStatus { Name = "InProgress" },

                    Accreditation = new()

                });
            mockRepo.Setup(r => r.UpdateStatusAsync(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<TaskStatuses>()))
                .Returns(Task.CompletedTask);
            var handler = new UpdateAccreditationTaskHandler(mockRepo.Object);
            var command = new UpdateAccreditationTaskCommand
            {
                TaskName = "Completed",
                AccreditationId = Guid.NewGuid(),
                Status = TaskStatuses.Completed
            };

            // Act
            var task = handler.Handle(command, CancellationToken.None);

            // Assert
            task.IsCompleted.Should().BeTrue();          
        }
    }
}
