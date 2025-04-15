using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Repositories.Regulator;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace EPR.PRN.Backend.Data.UnitTests.Repositories.Regulator
{
    [TestClass]
    public class RegulatorApplicationTaskStatusRepositoryTests
    {
        private Mock<ILogger<RegulatorApplicationTaskStatusRepository>> _loggerMock;
        private EprRegistrationsContext _context;
        private RegulatorApplicationTaskStatusRepository _repository;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<EprRegistrationsContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new EprRegistrationsContext(options);
            _loggerMock = new Mock<ILogger<RegulatorApplicationTaskStatusRepository>>();
            _repository = new RegulatorApplicationTaskStatusRepository(_context, _loggerMock.Object);
        }

        [TestMethod]
        public async Task GetTaskStatusAsync_ShouldReturnTaskStatus_WhenTaskExists()
        {
            // Arrange
            var taskName = "TestTask";
            var RegistrationMaterialId = 1;

            var taskStatus = new RegulatorApplicationTaskStatus
            {
                Task = new LookupRegulatorTask { Name = taskName },
                RegistrationMaterialId = RegistrationMaterialId
            };

            _context.RegulatorApplicationTaskStatus.Add(taskStatus);
            _context.SaveChanges();

            // Act
            var result = await _repository.GetTaskStatusAsync(taskName, RegistrationMaterialId);

            // Assert
            result.Should().NotBeNull();
            result!.Task.Name.Should().Be(taskName);
            result.RegistrationMaterialId.Should().Be(RegistrationMaterialId);
        }

        [TestMethod]
        public async Task GetTaskStatusAsync_ShouldReturnNull_WhenTaskDoesNotExist()
        {
            // Arrange
            var taskName = "NonExistentTask";
            var ApplicationId = 1;

            // Act
            var result = await _repository.GetTaskStatusAsync(taskName, ApplicationId);

            // Assert
            result.Should().BeNull();
        }

        [TestMethod]
        public async Task UpdateStatusAsync_ShouldAddNewTaskStatus_WhenTaskDoesNotExist()
        {
            // Arrange
            var taskName = "NewTask";
            var RegistrationMaterialId = 1;
            var RegistrationId = 1;
            var status = StatusTypes.Started;
            var comments = "Task started";

            _context.LookupTaskStatuses.Add(new LookupTaskStatus { Id = 2, Name = "Started" });

            _context.LookupTasks.Add(new LookupRegulatorTask { Id = 1, Name = taskName, IsMaterialSpecific = false, ApplicationTypeId = 1 });

            _context.Registrations.Add(new Registration{ Id = RegistrationId, ExternalId = "", ApplicationTypeId = 1 });

            _context.RegistrationMaterials.Add(new RegistrationMaterial { Id = RegistrationMaterialId, RegistrationId = RegistrationId });

            _context.SaveChanges();

            // Act
            await _repository.UpdateStatusAsync(taskName, RegistrationMaterialId, status, comments);

            // Assert
            _context.RegulatorApplicationTaskStatus.First().Should().NotBeNull();
            _context.RegulatorApplicationTaskStatus.First().Task.Name.Should().Be(taskName);
            _context.RegulatorApplicationTaskStatus.First().RegistrationMaterialId.Should().Be(RegistrationMaterialId);
            _context.RegulatorApplicationTaskStatus.First().TaskStatus.Name.Should().Be(status.ToString());
            _context.RegulatorApplicationTaskStatus.First().Comments.Should().Be(comments);
        }

        [TestMethod]
        public async Task UpdateStatusAsync_ShouldUpdateExistingTaskStatus_WhenTaskExists()
        {
            // Arrange
            var taskName = "ExistingTask";
            var RegistrationMaterialId = 1;
            var RegistrationId = 1;
            var status = StatusTypes.Completed;
            var comments = "Task completed";

            _context.RegulatorApplicationTaskStatus.Add(new RegulatorApplicationTaskStatus
            {
                Task = new LookupRegulatorTask { Name = taskName },
                RegistrationMaterialId = RegistrationMaterialId,
                TaskStatus = new LookupTaskStatus { Name = StatusTypes.Completed.ToString() },
                Comments = "Task started"
            });

            _context.Registrations.Add(new Registration { Id = RegistrationId, ExternalId = "", ApplicationTypeId = 1 });

            _context.RegistrationMaterials.Add(new RegistrationMaterial { Id = RegistrationMaterialId, RegistrationId = RegistrationId });

            _context.SaveChanges();

            // Act
            await _repository.UpdateStatusAsync(taskName, RegistrationMaterialId, status, comments);

            // Assert
            _context.RegulatorApplicationTaskStatus.First().Should().NotBeNull();
            _context.RegulatorApplicationTaskStatus.First().Task.Name.Should().Be(taskName);
            _context.RegulatorApplicationTaskStatus.First().RegistrationMaterialId.Should().Be(RegistrationMaterialId);
            _context.RegulatorApplicationTaskStatus.First().TaskStatus.Name.Should().Be(status.ToString());
            _context.RegulatorApplicationTaskStatus.First().Comments.Should().Be(comments);
        }
    }
}
