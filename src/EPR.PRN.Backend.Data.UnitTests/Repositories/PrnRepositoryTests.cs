using AutoFixture;
using EPR.PRN.Backend.Data;
using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Data.DTO;
using EPR.PRN.Backend.Data.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

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
        prns[0].PrnStatusId = 1; // ACCEPTED
        prns[0].ObligationYear = year.ToString();
        prns[1].OrganisationId = organisationId;
        prns[1].PrnStatusId = 2; // AWAITINGACCEPTANCE
        prns[1].ObligationYear = year.ToString();
        await _context.Prn.AddRangeAsync(prns);
        var prnStatuses = new[]
        {
        new PrnStatus { Id = 1, StatusName = EprnStatus.ACCEPTED.ToString() },
        new PrnStatus { Id = 2, StatusName = EprnStatus.AWAITINGACCEPTANCE.ToString() }
        };
        await _context.PrnStatus.AddRangeAsync(prnStatuses);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAcceptedAndAwaitingPrnsByYearAsync(organisationId, year);

        // Assert
        result.Should().HaveCount(2);
        result.Should().ContainSingle(r => r.Status.StatusName == EprnStatus.ACCEPTED.ToString());
        result.Should().ContainSingle(r => r.Status.StatusName == EprnStatus.AWAITINGACCEPTANCE.ToString());
    }

    [TestMethod]
    [DataRow("ACCEPTED", 1)]
    [DataRow("AWAITINGACCEPTANCE", 2)]
    [DataRow("REJECTED", 1)]
    [DataRow("ACCEPTED", 0)]
    public void GetPrnStatusCount_ReturnsCorrectCount(string status, int expectedCount)
    {
        // Arrange
        var fixture = new Fixture();
        var prns = new List<EprnResultsDto>
        {
            new EprnResultsDto { Eprn = fixture.Create<Eprn>(), Status = new PrnStatus { StatusName = "ACCEPTED" } },
            new EprnResultsDto { Eprn = fixture.Create<Eprn>(), Status = new PrnStatus { StatusName = "AWAITINGACCEPTANCE" } },
            new EprnResultsDto { Eprn = fixture.Create<Eprn>(), Status = new PrnStatus { StatusName = "AWAITINGACCEPTANCE" } },
            new EprnResultsDto { Eprn = fixture.Create<Eprn>(), Status = new PrnStatus { StatusName = "REJECTED" } }
        };

        // Adjust list based on DataRow
        if (expectedCount == 0 && status == "ACCEPTED")
        {
            prns = new List<EprnResultsDto>(); // Empty list for specific case
        }

        // Act
        var result = _repository.GetPrnStatusCount(prns, status);

        // Assert
        result.Should().Be(expectedCount);
    }

    [TestMethod]
    [DataRow("ACCEPTED", 150)]
    [DataRow("AWAITINGACCEPTANCE", 750)]
    [DataRow("REJECTED", 640)]
    public void GetSumOfTonnageForMaterials_ReturnsSumOfTonnage(string status, int expectedTonnageSum)
    {
        // Arrange
        var fixture = new Fixture();
        var prns = new List<EprnResultsDto>
        {
            new EprnResultsDto { Eprn = fixture.Build<Eprn>().With(e => e.TonnageValue, 100)
            .With(e => e.MaterialName, "Plastic").Create(), Status = new PrnStatus { StatusName = "ACCEPTED" } },
            new EprnResultsDto { Eprn = fixture.Build<Eprn>().With(e => e.TonnageValue, 50)
            .With(e => e.MaterialName, "Plastic").Create(), Status = new PrnStatus { StatusName = "ACCEPTED" } },
            new EprnResultsDto { Eprn = fixture.Build<Eprn>().With(e => e.TonnageValue, 600)
            .With(e => e.MaterialName, "Wood").Create(), Status = new PrnStatus { StatusName = "AWAITINGACCEPTANCE" } },
            new EprnResultsDto { Eprn = fixture.Build<Eprn>().With(e => e.TonnageValue, 50)
            .With(e => e.MaterialName, "Steel").Create(), Status = new PrnStatus { StatusName = "AWAITINGACCEPTANCE" } },
            new EprnResultsDto { Eprn = fixture.Build<Eprn>().With(e => e.TonnageValue, 100)
            .With(e => e.MaterialName, "Plastic").Create(), Status = new PrnStatus { StatusName = "AWAITINGACCEPTANCE" } },
            new EprnResultsDto { Eprn = fixture.Build<Eprn>().With(e => e.TonnageValue, 150)
            .With(e => e.MaterialName, "Wood").Create(), Status = new PrnStatus { StatusName = "AWAITINGACCEPTANCE" } },
            new EprnResultsDto { Eprn = fixture.Build<Eprn>().With(e => e.TonnageValue, 100)
            .With(e => e.MaterialName, "Aluminium").Create(), Status = new PrnStatus { StatusName = "REJECTED" } },
            new EprnResultsDto { Eprn = fixture.Build<Eprn>().With(e => e.TonnageValue, 540)
            .With(e => e.MaterialName, "Aluminium").Create(), Status = new PrnStatus { StatusName = "REJECTED" } }
        };

        // Act
        var result = _repository.GetSumOfTonnageForMaterials(prns, status);

        // Assert
        result.FirstOrDefault().TotalTonnage.Should().Be(expectedTonnageSum);
    }
}
