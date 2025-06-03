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
    public class RegulatorRegistrationTaskStatusRepositoryTests
    {
        private Mock<ILogger<RegulatorRegistrationTaskStatusRepository>> _loggerMock;
        private EprContext _context;
        private RegulatorRegistrationTaskStatusRepository _repository;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<EprContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new EprContext(options);
            _loggerMock = new Mock<ILogger<RegulatorRegistrationTaskStatusRepository>>();
            _repository = new RegulatorRegistrationTaskStatusRepository(_context, _loggerMock.Object);
        }

        [TestMethod]
        public async Task GetTaskStatusAsync_ShouldReturnTaskStatus_WhenTaskExists()
        {
            // Arrange
            var taskName = "TestTask";
            var registrationId = Guid.Parse("4bac12f7-f7a9-4df4-b7b5-9c4221860c4d");

            var taskStatus = new RegulatorRegistrationTaskStatus
            {
                Task = new LookupRegulatorTask { Name = taskName },
                Registration = new Registration { ExternalId = registrationId },
                TaskStatus = new LookupTaskStatus { Name = RegulatorTaskStatus.Completed.ToString() }
            };

            _context.RegulatorRegistrationTaskStatus.Add(taskStatus);
            _context.SaveChanges();

            // Act
            var result = await _repository.GetTaskStatusAsync(taskName, registrationId);

            // Assert
            result.Should().NotBeNull();
            result!.Task.Name.Should().Be(taskName);
            result.Registration.ExternalId.Should().Be(registrationId);
        }

        [TestMethod]
        public async Task GetTaskStatusAsync_ShouldReturnNull_WhenTaskDoesNotExist()
        {
            // Arrange
            var taskName = "NonExistentTask";
            var registrationId = Guid.Parse("4bac12f7-f7a9-4df4-b7b5-9c4221860c4d");

            // Act
            var result = await _repository.GetTaskStatusAsync(taskName, registrationId);

            // Assert
            result.Should().BeNull();
        }

        [TestMethod]
        public async Task UpdateStatusAsync_ShouldAddNewTaskStatus_WhenTaskDoesNotExist()
        {
            // Arrange
            var taskName = "NewTask";
            var registrationId = Guid.Parse("4bac12f7-f7a9-4df4-b7b5-9c4221860c4d");
            var status = RegulatorTaskStatus.Started;
            var comments = "Task started";
            var user = Guid.NewGuid();

            _context.LookupTaskStatuses.Add(new LookupTaskStatus { Id = 2, Name = "Started" });

            _context.LookupTasks.Add(new LookupRegulatorTask { Id = 1, Name = taskName, IsMaterialSpecific = false, ApplicationTypeId = 1 });

            _context.Registrations.Add(new Registration { ExternalId = registrationId, ApplicationTypeId = 1 });

            _context.SaveChanges();

            // Act
            await _repository.UpdateStatusAsync(taskName, registrationId, status, comments, user);

            // Assert
            var taskStatus = _context.RegulatorRegistrationTaskStatus.FirstOrDefault();

            taskStatus.Should().NotBeNull();
            taskStatus.Task.Name.Should().Be(taskName);
            taskStatus.Registration.ExternalId.Should().Be(registrationId);
            taskStatus.TaskStatus.Name.Should().Be(status.ToString());
            taskStatus.StatusCreatedBy.Should().Be(user);
            taskStatus.StatusUpdatedBy.Should().Be(user);
        }

        [TestMethod]
        public async Task UpdateStatusAsync_ShouldAddNewTaskStatus_WhenStatusQueried()
        {
            // Arrange
            var taskName = "NewTask";
            var registrationId = Guid.Parse("4bac12f7-f7a9-4df4-b7b5-9c4221860c4d");
            var status = RegulatorTaskStatus.Queried;
            var comments = "Task Queried";
            var user = Guid.NewGuid();

            _context.LookupTaskStatuses.Add(new LookupTaskStatus { Id = 4, Name = "Queried" });

            _context.LookupTasks.Add(new LookupRegulatorTask { Id = 1, Name = taskName, IsMaterialSpecific = false, ApplicationTypeId = 1 });

            _context.Registrations.Add(new Registration { ExternalId = registrationId, ApplicationTypeId = 1 });

            _context.SaveChanges();

            // Act
            await _repository.UpdateStatusAsync(taskName, registrationId, status, comments, user);

            // Assert
            var taskStatus = _context.RegulatorRegistrationTaskStatus.FirstOrDefault();

            taskStatus.Should().NotBeNull();
            taskStatus.Task.Name.Should().Be(taskName);
            taskStatus.Registration.ExternalId.Should().Be(registrationId);
            taskStatus.TaskStatus.Name.Should().Be(status.ToString());
            taskStatus.StatusCreatedBy.Should().Be(user);
            taskStatus.StatusUpdatedBy.Should().Be(user);
        }

        [TestMethod]
        public async Task UpdateStatusAsync_ShouldUpdateExistingTaskStatus_WhenTaskExists()
        {
            // Arrange
            var taskName = "ExistingTask";
            var registrationId = Guid.Parse("4bac12f7-f7a9-4df4-b7b5-9c4221860c4d");
            var existingStatus = RegulatorTaskStatus.Queried;
            var status = RegulatorTaskStatus.Completed;
            var comments = "Task completed";
            var existingUser = Guid.NewGuid();
            var user = Guid.NewGuid();

            _context.LookupTaskStatuses.Add(new LookupTaskStatus { Id = 2, Name = status.ToString() });

            _context.RegulatorRegistrationTaskStatus.Add(new RegulatorRegistrationTaskStatus
            {
                Task = new LookupRegulatorTask { Name = taskName },
                Registration = new Registration { ExternalId = registrationId },
                TaskStatus = new LookupTaskStatus { Name = existingStatus.ToString() },
                StatusCreatedBy = existingUser,
                StatusUpdatedBy = existingUser
            });

            _context.Registrations.Add(new Registration { ExternalId = registrationId, ApplicationTypeId = 1 });

            _context.SaveChanges();

            // Act
            await _repository.UpdateStatusAsync(taskName, registrationId, status, comments, user);

            // Assert
            var taskStatus = _context.RegulatorRegistrationTaskStatus.FirstOrDefault();

            taskStatus.Should().NotBeNull();
            taskStatus.Task.Name.Should().Be(taskName);
            taskStatus.Registration.ExternalId.Should().Be(registrationId);
            taskStatus.TaskStatus.Name.Should().Be(status.ToString());
            taskStatus.StatusCreatedBy.Should().Be(existingUser);
            taskStatus.StatusUpdatedBy.Should().Be(user);
        }

        [TestMethod]
        public async Task UpdateStatusAsync_ShouldThrowKeyNotFoundException_WhenRegistrationMaterialNotFound()
        {
            // Arrange
            var taskName = "NewTask";
            var registrationId = Guid.Parse("4bac12f7-f7a9-4df4-b7b5-9c4221860c4d");
            var status = RegulatorTaskStatus.Started;
            var comments = "Task started";
            var user = Guid.NewGuid();

            _context.LookupTaskStatuses.Add(new LookupTaskStatus { Id = 2, Name = "Started" });

            _context.LookupTasks.Add(new LookupRegulatorTask { Id = 1, Name = taskName, IsMaterialSpecific = true, ApplicationTypeId = 1 });

            _context.SaveChanges();

            // Act
            Func<Task> act = async () => await _repository.UpdateStatusAsync(taskName, registrationId, status, comments, user);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>();
        }

        [TestMethod]
        public async Task UpdateStatusAsync_ShouldThrowRegulatorInvalidOperationException_WhenTaskNotFound()
        {
            // Arrange
            var taskName = "NewTask";
            var registrationId = Guid.Parse("4bac12f7-f7a9-4df4-b7b5-9c4221860c4d");
            var status = RegulatorTaskStatus.Started;
            var comments = "Task started";
            var user = Guid.NewGuid();

            _context.LookupTaskStatuses.Add(new LookupTaskStatus { Id = 2, Name = "Started" });

            _context.Registrations.Add(new Registration { ExternalId = registrationId, ApplicationTypeId = 1 });

            _context.SaveChanges();

            // Act
            Func<Task> act = async () => await _repository.UpdateStatusAsync(taskName, registrationId, status, comments, user);

            // Assert
            await act.Should().ThrowAsync<RegulatorInvalidOperationException>();
        }
        [TestMethod]
        public async Task AddRegistrationTaskQueryNoteAsync_ShouldAddNote_WhenTaskStatusIsValid()
        {
            var externalId = Guid.NewGuid();
            var queryBy = Guid.NewGuid();
            var note = "Test query note";

            var taskStatus = new RegulatorRegistrationTaskStatus
            {
                ExternalId = externalId,
                Id = 1,
                TaskStatusId = (int)RegulatorTaskStatus.Queried
            };

            _context.RegulatorRegistrationTaskStatus.Add(taskStatus);
            _context.SaveChanges();

            await _repository.AddRegistrationTaskQueryNoteAsync(externalId, queryBy, note);

            var savedNote = _context.QueryNote.FirstOrDefault();
            savedNote.Should().NotBeNull();
            savedNote!.Notes.Should().Be(note);
            savedNote.CreatedBy.Should().Be(queryBy);

            var link = _context.RegistrationTaskStatusQueryNotes.FirstOrDefault();
            link.Should().NotBeNull();
            link!.RegulatorRegistrationTaskStatus.Id.Should().Be(taskStatus.Id);
        }

        [TestMethod]
        public async Task AddRegistrationTaskQueryNoteAsync_ShouldThrow_WhenTaskStatusNotFound()
        {
            var invalidId = Guid.NewGuid();
            var queryBy = Guid.NewGuid();

            Func<Task> act = async () => await _repository.AddRegistrationTaskQueryNoteAsync(invalidId, queryBy, "Note");

            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("Regulator Registration Task Status not found.");
        }

        [TestMethod]
        public async Task AddRegistrationTaskQueryNoteAsync_ShouldThrow_WhenStatusIsCompleted()
        {
            var externalId = Guid.NewGuid();
            var queryBy = Guid.NewGuid();

            var taskStatus = new RegulatorRegistrationTaskStatus
            {
                ExternalId = externalId,
                Id = 1,
                TaskStatusId = (int)RegulatorTaskStatus.Completed
            };

            _context.RegulatorRegistrationTaskStatus.Add(taskStatus);
            _context.SaveChanges();

            Func<Task> act = async () => await _repository.AddRegistrationTaskQueryNoteAsync(externalId, queryBy, "Note");

            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("Cannot insert query because the Regulator Registration Task Status is completed.");
        }
        [TestMethod]
        public async Task GetTaskStatusAsync_ShouldThrowArgumentNullException_WhenTaskNameIsNull()
        {
            var registrationId = Guid.NewGuid();

            Func<Task> act = async () => await _repository.GetTaskStatusAsync(null!, registrationId);

            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [TestMethod]
        public async Task UpdateStatusAsync_ShouldNotAddNote_WhenCommentsIsNull_AndStatusIsNotQueried()
        {
            var taskName = "TaskWithNullComment";
            var registrationId = Guid.NewGuid();
            var user = Guid.NewGuid();

            _context.LookupTaskStatuses.Add(new LookupTaskStatus { Id = 2, Name = RegulatorTaskStatus.Started.ToString() });
            _context.LookupTasks.Add(new LookupRegulatorTask { Id = 1, Name = taskName, IsMaterialSpecific = false, ApplicationTypeId = 1 });
            _context.Registrations.Add(new Registration { ExternalId = registrationId, ApplicationTypeId = 1 });
            _context.SaveChanges();

            await _repository.UpdateStatusAsync(taskName, registrationId, RegulatorTaskStatus.Started, null, user);

            _context.QueryNote.Should().BeEmpty("because comments were null and status was not 'Queried'");
        }
        [TestMethod]
        public async Task UpdateStatusAsync_ShouldNotAddNote_WhenCommentsIsNull_AndStatusIsQueried()
        {
            var taskName = "QueriedTaskNullComment";
            var registrationId = Guid.NewGuid();
            var user = Guid.NewGuid();

            _context.LookupTaskStatuses.Add(new LookupTaskStatus { Id = 3, Name = RegulatorTaskStatus.Queried.ToString() });
            _context.LookupTasks.Add(new LookupRegulatorTask { Id = 2, Name = taskName, IsMaterialSpecific = false, ApplicationTypeId = 1 });
            _context.Registrations.Add(new Registration { ExternalId = registrationId, ApplicationTypeId = 1 });
            _context.SaveChanges();

            await _repository.UpdateStatusAsync(taskName, registrationId, RegulatorTaskStatus.Queried, null, user);

            _context.QueryNote.Should().BeEmpty("because comments were null even though status was 'Queried'");
        }
        [TestMethod]
        public async Task UpdateStatusAsync_ShouldThrowException_WhenLookupTaskStatusIsMissing()
        {
            var taskName = "MissingStatus";
            var registrationId = Guid.NewGuid();
            var user = Guid.NewGuid();

            _context.LookupTasks.Add(new LookupRegulatorTask { Id = 1, Name = taskName, IsMaterialSpecific = false, ApplicationTypeId = 1 });
            _context.Registrations.Add(new Registration { ExternalId = registrationId, ApplicationTypeId = 1 });
            _context.SaveChanges();

            Func<Task> act = async () => await _repository.UpdateStatusAsync(taskName, registrationId, RegulatorTaskStatus.Completed, "Some note", user);

            await act.Should().ThrowAsync<InvalidOperationException>("because the LookupTaskStatus does not exist in DB");
        }
        [TestMethod]
        public async Task AddRegistrationTaskQueryNoteAsync_ShouldAddNote_WhenStatusIsStarted()
        {
            var externalId = Guid.NewGuid();
            var queryBy = Guid.NewGuid();
            var note = "Started task note";

            _context.RegulatorRegistrationTaskStatus.Add(new RegulatorRegistrationTaskStatus
            {
                ExternalId = externalId,
                Id = 1,
                TaskStatusId = (int)RegulatorTaskStatus.Started
            });

            _context.SaveChanges();

            await _repository.AddRegistrationTaskQueryNoteAsync(externalId, queryBy, note);

            var savedNote = _context.QueryNote.FirstOrDefault();
            savedNote.Should().NotBeNull();
            savedNote!.Notes.Should().Be(note);
            savedNote.CreatedBy.Should().Be(queryBy);
        }
        [TestMethod]
        public async Task AddRegistrationTaskQueryNoteAsync_ShouldThrow_WhenQueryByIsEmptyGuid()
        {
            var externalId = Guid.NewGuid();
            var note = "Invalid user";

            _context.RegulatorRegistrationTaskStatus.Add(new RegulatorRegistrationTaskStatus
            {
                ExternalId = externalId,
                Id = 1,
                TaskStatusId = (int)RegulatorTaskStatus.Started
            });

            _context.SaveChanges();

            Func<Task> act = async () => await _repository.AddRegistrationTaskQueryNoteAsync(externalId, Guid.Empty, note);

            await act.Should().ThrowAsync<ArgumentException>("because queryBy is an empty Guid (invalid user)");
        }
    }
}
