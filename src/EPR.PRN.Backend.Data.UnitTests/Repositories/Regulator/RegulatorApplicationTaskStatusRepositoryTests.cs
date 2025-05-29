using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Common.Exceptions;
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
        private EprContext _context;
        private RegulatorApplicationTaskStatusRepository _repository;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<EprContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new EprContext(options);
            _loggerMock = new Mock<ILogger<RegulatorApplicationTaskStatusRepository>>();
            _repository = new RegulatorApplicationTaskStatusRepository(_context, _loggerMock.Object);
        }

        [TestMethod]
        public async Task GetTaskStatusAsync_ShouldReturnTaskStatus_WhenTaskExists()
        {
            // Arrange
            var taskName = "TestTask";
            var RegistrationMaterialId = Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a9");

            var taskStatus = new RegulatorApplicationTaskStatus
            {
                Task = new LookupRegulatorTask { Name = taskName },
                RegistrationMaterial = new RegistrationMaterial { ExternalId = RegistrationMaterialId }
            };

            _context.RegulatorApplicationTaskStatus.Add(taskStatus);
            _context.SaveChanges();
            // Act
            var result = await _repository.GetTaskStatusAsync(taskName, RegistrationMaterialId);
            // Assert
            result.Should().NotBeNull();
            result!.Task.Name.Should().Be(taskName);
            result.RegistrationMaterial.ExternalId.Should().Be(RegistrationMaterialId);
        }

        [TestMethod]
        public async Task GetTaskStatusAsync_ShouldReturnNull_WhenTaskDoesNotExist()
        {
            var taskName = "NonExistentTask";
            var registrationMaterialId = Guid.NewGuid();

            var result = await _repository.GetTaskStatusAsync(taskName, registrationMaterialId);

            result.Should().BeNull();
        }

        [TestMethod]
        public async Task UpdateStatusAsync_ShouldAddNewTaskStatus_WhenTaskDoesNotExist()
        {
            var taskName = "NewTask";
            var registrationMaterialId = Guid.NewGuid();
            var registrationId = Guid.NewGuid();
            var status = RegulatorTaskStatus.Started;
            var comments = "Task started";
            var user = Guid.NewGuid();

            _context.LookupTaskStatuses.Add(new LookupTaskStatus { Id = 2, Name = "Started" });
            _context.LookupTasks.Add(new LookupRegulatorTask { Id = 1, Name = taskName, IsMaterialSpecific = true, ApplicationTypeId = 1 });
            _context.RegistrationMaterials.Add(new RegistrationMaterial
            {
                ExternalId = registrationMaterialId,
                Registration = new Registration { ExternalId = registrationId, ApplicationTypeId = 1 }
            });

            _context.SaveChanges();

            await _repository.UpdateStatusAsync(taskName, registrationMaterialId, status, comments, user);

            var taskStatus = _context.RegulatorApplicationTaskStatus.FirstOrDefault();

            taskStatus.Should().NotBeNull();
            taskStatus!.ExternalId.Should().NotBe(Guid.Empty);
            taskStatus.Task.Name.Should().Be(taskName);
            taskStatus.RegistrationMaterial.ExternalId.Should().Be(registrationMaterialId);
            taskStatus.TaskStatus.Name.Should().Be(status.ToString());
            taskStatus.StatusCreatedBy.Should().Be(user);
            taskStatus.StatusUpdatedBy.Should().Be(user);
        }

        [TestMethod]
        public async Task UpdateStatusAsync_ShouldUpdateExistingTaskStatus_WhenTaskExists()
        {
            var taskName = "ExistingTask";
            var registrationMaterialId = Guid.NewGuid();
            var status = RegulatorTaskStatus.Completed;
            var comments = "Task completed";
            var existingUser = Guid.NewGuid();
            var user = Guid.NewGuid();

            _context.LookupTaskStatuses.Add(new LookupTaskStatus { Id = 2, Name = status.ToString() });

            _context.RegulatorApplicationTaskStatus.Add(new RegulatorApplicationTaskStatus
            {
                Task = new LookupRegulatorTask { Name = taskName },
                RegistrationMaterial = new RegistrationMaterial { ExternalId = registrationMaterialId },
                TaskStatus = new LookupTaskStatus { Name = RegulatorTaskStatus.Queried.ToString() },
                StatusCreatedBy = existingUser
            });

            _context.SaveChanges();

            await _repository.UpdateStatusAsync(taskName, registrationMaterialId, status, comments, user);

            var taskStatus = _context.RegulatorApplicationTaskStatus.FirstOrDefault();

            taskStatus.Should().NotBeNull();
            taskStatus!.Task.Name.Should().Be(taskName);
            taskStatus.RegistrationMaterial.ExternalId.Should().Be(registrationMaterialId);
            taskStatus.TaskStatus.Name.Should().Be(status.ToString());
            taskStatus.StatusCreatedBy.Should().Be(existingUser);
            taskStatus.StatusUpdatedBy.Should().Be(user);
        }

        [TestMethod]
        public async Task UpdateStatusAsync_ShouldThrowKeyNotFoundException_WhenRegistrationMaterialNotFound()
        {
            var taskName = "NewTask";
            var registrationMaterialId = Guid.NewGuid();
            var status = RegulatorTaskStatus.Started;
            var comments = "Task started";
            var user = Guid.NewGuid();

            _context.LookupTaskStatuses.Add(new LookupTaskStatus { Id = 2, Name = "Started" });
            _context.LookupTasks.Add(new LookupRegulatorTask { Id = 1, Name = taskName, IsMaterialSpecific = true, ApplicationTypeId = 1 });

            _context.SaveChanges();

            Func<Task> act = async () => await _repository.UpdateStatusAsync(taskName, registrationMaterialId, status, comments, user);

            await act.Should().ThrowAsync<KeyNotFoundException>();
        }

        [TestMethod]
        public async Task UpdateStatusAsync_ShouldThrowRegulatorInvalidOperationException_WhenTaskNotFound()
        {
            var taskName = "NewTask";
            var registrationMaterialId = Guid.NewGuid();
            var registrationId = Guid.NewGuid();
            var status = RegulatorTaskStatus.Started;
            var comments = "Task started";
            var user = Guid.NewGuid();

            _context.LookupTaskStatuses.Add(new LookupTaskStatus { Id = 2, Name = "Started" });
            _context.RegistrationMaterials.Add(new RegistrationMaterial
            {
                ExternalId = registrationMaterialId,
                Registration = new Registration { ExternalId = registrationId, ApplicationTypeId = 1 }
            });

            _context.SaveChanges();

            Func<Task> act = async () => await _repository.UpdateStatusAsync(taskName, registrationMaterialId, status, comments, user);

            await act.Should().ThrowAsync<RegulatorInvalidOperationException>();
        }

        [TestMethod]
        public async Task AddApplicationTaskQueryNoteAsync_ShouldAddNote_WhenTaskStatusIsValid()
        {
            var externalId = Guid.NewGuid();
            var queryBy = Guid.NewGuid();
            var note = "Test query note";

            var taskStatus = new RegulatorApplicationTaskStatus
            {
                ExternalId = externalId,
                Id = 1,
                TaskStatusId = (int)RegulatorTaskStatus.Queried
            };

            _context.RegulatorApplicationTaskStatus.Add(taskStatus);
            _context.SaveChanges();

            await _repository.AddApplicationTaskQueryNoteAsync(externalId, queryBy, note);

            var savedNote = _context.QueryNote.FirstOrDefault();
            savedNote.Should().NotBeNull();
            savedNote!.Notes.Should().Be(note);
            savedNote.CreatedBy.Should().Be(queryBy);

            var link = _context.ApplicationTaskStatusQueryNotes.FirstOrDefault();
            link.Should().NotBeNull();
            link!.RegulatorApplicationTaskStatusId.Should().Be(taskStatus.Id);
        }

        [TestMethod]
        public async Task AddApplicationTaskQueryNoteAsync_ShouldThrow_WhenTaskStatusNotFound()
        {
            var invalidId = Guid.NewGuid();
            var queryBy = Guid.NewGuid();

            Func<Task> act = async () => await _repository.AddApplicationTaskQueryNoteAsync(invalidId, queryBy, "Note");

            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("Regulator Application Task Status not found.");
        }

        [TestMethod]
        public async Task AddApplicationTaskQueryNoteAsync_ShouldThrow_WhenStatusIsCompleted()
        {
            var externalId = Guid.NewGuid();
            var queryBy = Guid.NewGuid();

            var taskStatus = new RegulatorApplicationTaskStatus
            {
                ExternalId = externalId,
                Id = 1,
                TaskStatusId = (int)RegulatorTaskStatus.Completed
            };

            _context.RegulatorApplicationTaskStatus.Add(taskStatus);
            _context.SaveChanges();

            Func<Task> act = async () => await _repository.AddApplicationTaskQueryNoteAsync(externalId, queryBy, "Note");

            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("Cannot insert query because the Regulator Application Task Status is completed.");
        }
    }
}
