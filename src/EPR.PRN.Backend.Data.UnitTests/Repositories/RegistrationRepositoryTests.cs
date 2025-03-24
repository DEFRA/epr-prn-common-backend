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
    public class RegistrationRepositoryTests
    {
        private RegistrationRepository _repository;
        private Mock<EprContext> _mockEprContext;

        private readonly List<Registration> _registrations =
        [
            new Registration { Id = 1, OrganisationId = Guid.NewGuid(), ApplicationTypeId = 1, BusinessAddress = new Address { AddressLine1 = "Test", TownCity = "London", NationId = 1, Postcode = "N00LIO" } },
            new Registration { Id = 2, OrganisationId = Guid.NewGuid(), ApplicationTypeId = 1, BusinessAddress = new Address { AddressLine1 = "Test", TownCity = "London", NationId = 1, Postcode = "N00LIO" } },
            new Registration { Id = 3, OrganisationId = Guid.NewGuid(), ApplicationTypeId = 1, BusinessAddress = new Address { AddressLine1 = "Test", TownCity = "London", NationId = 1, Postcode = "N00LIO" } },
            new Registration { Id = 4, OrganisationId = Guid.NewGuid(), ApplicationTypeId = 2, ReprocessingSiteAddress = new Address { AddressLine1 = "Test2", TownCity = "London", NationId = 1, Postcode = "EE8965" } },
        ];

        [TestInitialize]
        public void Setup()
        {
            var dbContextOptions = new DbContextOptionsBuilder<EprContext>().Options;
            _mockEprContext = new Mock<EprContext>(dbContextOptions);
            _mockEprContext.Setup(context => context.Registration).ReturnsDbSet(_registrations);
            _repository = new RegistrationRepository(_mockEprContext.Object);
        }

        [TestMethod]
        public async Task GetRegistrationsById_ShouldReturnRegistration()
        {
            var expectedRegistrationId = 1;
            // Act
            var result = await _repository.GetByIdAsync(1);

            // Assert
            result.Should().NotBeNull();         
            result.Id.Should().Be(expectedRegistrationId);
        }
    }
}
