using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Repositories.Regulator;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace EPR.PRN.Backend.Data.UnitTests.Repositories.Regulator
{
    [TestClass]
    public class RegulatorApplicationTaskStatusRepositoryTests
    {
        private Mock<EprRegistrationsContext> _contextMock;
        private RegulatorApplicationTaskStatusRepository _repository;

        [TestInitialize]
        public void Setup()
        {
            _contextMock = new Mock<EprRegistrationsContext>();
            _repository = new RegulatorApplicationTaskStatusRepository(_contextMock.Object);
        }

        [TestMethod]
        public async Task UpdateStatusAsync_ShouldUpdateStatus_WhenRecordExists()
        {
            // Arrange
            var id = 1;
            var status = StatusTypes.Complete;
            var comments = "Test comments";
            var taskStatus = new RegulatorApplicationTaskStatus { Id = id };

            var dbSetMock = new Mock<DbSet<RegulatorApplicationTaskStatus>>();
            dbSetMock.Setup(m => m.FindAsync(id)).ReturnsAsync(taskStatus);
            _contextMock.Setup(c => c.RegulatorApplicationTaskStatus).Returns(dbSetMock.Object);

            // Act
            await _repository.UpdateStatusAsync(id, status, comments);

            // Assert
            taskStatus.TaskStatusId.Should().Be((int)status);
            taskStatus.Comments.Should().Be(comments);
            _contextMock.Verify(c => c.SaveChangesAsync(default), Times.Once);
        }

        [TestMethod]
        public async Task UpdateStatusAsync_ShouldThrowException_WhenRecordDoesNotExist()
        {
            // Arrange
            var id = 1;
            var status = StatusTypes.Complete;
            var comments = "Test comments";

            var dbSetMock = new Mock<DbSet<RegulatorApplicationTaskStatus>>();
            dbSetMock.Setup(m => m.FindAsync(id)).ReturnsAsync((RegulatorApplicationTaskStatus)null);
            _contextMock.Setup(c => c.RegulatorApplicationTaskStatus).Returns(dbSetMock.Object);

            // Act
            Func<Task> act = async () => await _repository.UpdateStatusAsync(id, status, comments);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Regulator application task status not found");
            _contextMock.Verify(c => c.SaveChangesAsync(default), Times.Never);
        }
    }
}
