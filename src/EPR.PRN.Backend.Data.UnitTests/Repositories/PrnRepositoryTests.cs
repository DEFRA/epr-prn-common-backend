using AutoFixture;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Data.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace EPR.PRN.Backend.Data.UnitTests.Repositories
{
    [TestClass]
    public class PrnRepositoryTests
    {
        private EprContext _context;
        private PrnRepository _repository;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<EprContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new EprContext(options);
            _repository = new PrnRepository(_context);
        }

        [TestMethod]
        public async Task GetAcceptedAndAwaitingPrnsByYearAsync_ReturnsFilteredPrns()
        {
            // Arrange
            var organisationId = Guid.NewGuid();
            var year = 2023;
            var fixture = new Fixture();
            var prns = fixture.CreateMany<Eprn>(10).ToList();
            prns[0].OrganisationId = organisationId;
            prns[0].PrnStatusId = (int)EprnStatus.ACCEPTED;
            prns[0].ObligationYear = year.ToString();
            prns[1].OrganisationId = organisationId;
            prns[1].PrnStatusId = (int)EprnStatus.AWAITINGACCEPTANCE;
            prns[1].ObligationYear = year.ToString();
            await _context.Prn.AddRangeAsync(prns, CancellationToken.None);
            var prnStatuses = new[]
            {
            new PrnStatus { Id = (int)EprnStatus.ACCEPTED, StatusName = EprnStatus.ACCEPTED.ToString() },
            new PrnStatus { Id = (int)EprnStatus.AWAITINGACCEPTANCE, StatusName = EprnStatus.AWAITINGACCEPTANCE.ToString() }
            };
            await _context.PrnStatus.AddRangeAsync(prnStatuses, CancellationToken.None);
            await _context.SaveChangesAsync(CancellationToken.None);

            // Act
            var result = _repository.GetAcceptedAndAwaitingPrnsByYear(organisationId, year);

            // Assert
            result.Should().HaveCount(2);
            result.Should().ContainSingle(r => r.Status.StatusName == EprnStatus.ACCEPTED.ToString());
            result.Should().ContainSingle(r => r.Status.StatusName == EprnStatus.AWAITINGACCEPTANCE.ToString());
        }
    }
}