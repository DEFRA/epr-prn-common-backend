using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using EPR.PRN.Backend.Data.Repositories.Regulator;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace EPR.PRN.Backend.Data.UnitTests.Repositories.Regulator
{ 
    [TestClass]
    public class RegulatorRegistrationTaskStatusRepositoryTests
    {
        private Mock<EprRegistrationsContext> _contextMock;
        private IRegulatorRegistrationTaskStatusRepository _repository;

        [TestInitialize]
        public void Setup()
        {
            _contextMock = new Mock<EprRegistrationsContext>();
            _repository = new RegulatorRegistrationTaskStatusRepository(_contextMock.Object);
        }

        [TestMethod]
        public async Task UpdateStatusAsync_ShouldUpdateStatus_WhenRegulatorRegistrationTaskStatusExists()
        {
            // Arrange
            var id = 1;
            var status = StatusTypes.Complete;
            var comments = "Test comments";
            var taskStatus = new RegulatorRegistrationTaskStatus { Id = id };

            var dbSetMock = new Mock<DbSet<RegulatorRegistrationTaskStatus>>();
            dbSetMock.Setup(m => m.FindAsync(id)).ReturnsAsync(taskStatus);
            _contextMock.Setup(c => c.RegulatorRegistrationTaskStatus).Returns(dbSetMock.Object);

            // Act
            await _repository.UpdateStatusAsync(id, status, comments);

            // Assert
            taskStatus.TaskStatusId.Should().Be((int)status);
            taskStatus.Comments.Should().Be(comments);
            _contextMock.Verify(c => c.SaveChangesAsync(default), Times.Once);
        }

        [TestMethod]
        public async Task UpdateStatusAsync_ShouldThrowException_WhenRegulatorRegistrationTaskStatusDoesNotExist()
        {
            // Arrange
            var id = 1;
            var status = StatusTypes.Complete;
            var comments = "Test comments";

            var dbSetMock = new Mock<DbSet<RegulatorRegistrationTaskStatus>>();
            dbSetMock.Setup(m => m.FindAsync(id)).ReturnsAsync((RegulatorRegistrationTaskStatus)null);
            _contextMock.Setup(c => c.RegulatorRegistrationTaskStatus).Returns(dbSetMock.Object);

            // Act
            Func<Task> act = async () => await _repository.UpdateStatusAsync(id, status, comments);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage($"Regulator registration task status not found: {id}");
            _contextMock.Verify(c => c.SaveChangesAsync(default), Times.Never);
        }
    }
}
