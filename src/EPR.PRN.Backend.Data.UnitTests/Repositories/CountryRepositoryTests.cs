using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EPR.PRN.Backend.Data;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Repositories.Lookup;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EPR.PRN.Backend.Data.UnitTests.Repositories
{
    [TestClass]
    public class CountryRepositoryTests
    {
        private EprContext _context;
        private CountryRepository _repository;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<EprContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;
            _context = new EprContext(options);

            // Seed data
            _context.Set<LookupCountry>().AddRange(
                new LookupCountry { Id = 1, Name = "UK" },
                new LookupCountry { Id = 2, Name = "France" }
            );
            _context.SaveChanges();

            _repository = new CountryRepository(_context);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [TestMethod]
        public async Task GetAllAsync_ReturnsAllCountries()
        {
            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Select(c => c.Name).Should().Contain(new[] { "UK", "France" });
        }
    }
}
