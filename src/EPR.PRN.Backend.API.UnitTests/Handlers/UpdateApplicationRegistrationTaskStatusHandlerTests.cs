using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Handlers;
using EPR.PRN.Backend.Data.Interfaces;
using Moq;

namespace EPR.PRN.Backend.API.UnitTests.Handlers
{
    [TestClass]
    public class UpdateApplicationRegistrationTaskStatusHandlerTests
    {
        [TestMethod]
        public async Task Handle_CallsUpdateApplicationRegistrationTaskStatusAsync()
        {
            // Arrange
            var mockRepo = new Mock<IMaterialRepository>();
            var handler = new UpdateApplicationRegistrationTaskStatusHandler(mockRepo.Object);
            var command = new UpdateApplicationRegistrationTaskStatusCommand
            {
                TaskName = "SomeTask",
                RegistrationId = Guid.NewGuid(),
                Status = TaskStatuses.Completed
            };

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            mockRepo.Verify(r => r.UpdateApplicationRegistrationTaskStatusAsync(command.TaskName, command.RegistrationId, command.Status), Times.Once);
        }
    }
}

