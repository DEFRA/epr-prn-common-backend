using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Data.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.Data.UnitTests.Repositories
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class SaveAndContinueRepositoryTests
    {
        private SaveAndContinueRepository _repository;
        private Mock<EprContext> _mockEprContext;
        private List<Registration> registrations = new List<Registration>();
        private List<SaveAndContinue> saveAndContinues = new List<SaveAndContinue>();


        [TestInitialize]
        public void Setup()
        {
            registrations = new List<Registration> {
                new Registration { Id = 1},
                new Registration { Id = 2 },
            };
            saveAndContinues =
                [
                  new SaveAndContinue{ Id = 1, Action = "Action", Controller = "Controller", Area = "Registration", RegistrationId = 1, CreatedOn = DateTime.UtcNow.AddDays(-1)},
                  new SaveAndContinue{ Id = 2, Action = "Action2", Controller = "Controller", Area = "Registration", RegistrationId = 1, CreatedOn = DateTime.UtcNow},
                  new SaveAndContinue{ Id = 3, Action = "Action", Controller = "Controller", Area = "Account", RegistrationId = 2}
                ];

            var dbContextOptions = new DbContextOptionsBuilder<EprContext>().Options;
            _mockEprContext = new Mock<EprContext>(dbContextOptions);
            _mockEprContext.Setup(context => context.Registration).ReturnsDbSet(registrations);
            _mockEprContext.Setup(context => context.SaveAndContinue).ReturnsDbSet(saveAndContinues);
            _repository = new SaveAndContinueRepository(_mockEprContext.Object);
        }

        [TestMethod]
        public async Task AddAsync_ShouldAddModel()
        {
            var model = new SaveAndContinue();
            // Act
            await _repository.AddAsync(model);

            // Assert
            _mockEprContext.Verify(x=>x.SaveAndContinue.AddAsync(It.IsAny<SaveAndContinue>(), It.IsAny<CancellationToken>()), Times.Once());
            _mockEprContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
        }

        [TestMethod]
        public async Task GetAsync_ShouldReturnResult()
        {
            // Act
            var result = await _repository.GetAsync(1, "Registration");

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(2);
            result.Area.Should().Be("Registration");
            result.Action.Should().Be("Action2");
        }

        [TestMethod]
        public async Task GetAllAsync_ShouldReturnResult()
        {
            // Act
            var result = await _repository.GetAllAsync(1, "Registration");

            // Assert
            result.Should().NotBeNull();
            result.Count.Should().Be(2);
        }

        [TestMethod]
        public async Task GetAllAsync_ShouldReturnResultInDescendingOrder()
        {
            // Act
            var result = await _repository.GetAllAsync(1, "Registration");

            // Assert
            result.Should().NotBeNull();
            result.Count.Should().Be(2);
            result[0].CreatedOn.Date.Should().BeAfter(result[1].CreatedOn.Date);
        }
    }
}
