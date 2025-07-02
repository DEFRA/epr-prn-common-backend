using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using EPR.PRN.Backend.API.Handlers;
using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.Data.Interfaces;
using EPR.PRN.Backend.API.Common.Enums;

namespace EPR.PRN.Backend.API.UnitTests.Handlers
{
    [TestClass]
    public class UpdateApplicantRegistrationTaskStatusHandlerTests
    {
        [TestMethod]
        public async Task Handle_CallsUpdateApplicantRegistrationTaskStatusAsync()
        {
            // Arrange
            var mockRepo = new Mock<IMaterialRepository>();
            var handler = new UpdateApplicantRegistrationTaskStatusHandler(mockRepo.Object);
            var command = new UpdateApplicantRegistrationTaskStatusCommand
            {
                TaskName = "SomeTask",
                RegistrationId = Guid.NewGuid(),
                Status = TaskStatuses.Completed
            };

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            mockRepo.Verify(r => r.UpdateApplicantRegistrationTaskStatusAsync(command.TaskName, command.RegistrationId, command.Status), Times.Once);
        }
    }
}

