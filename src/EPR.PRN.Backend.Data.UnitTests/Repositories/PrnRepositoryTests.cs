using AutoFixture;
using EPR.PRN.Backend.API.Common.Dto;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Profiles;
using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Data.Repositories;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace EPR.PRN.Backend.Data.UnitTests.Repositories;

[TestClass]
public class PrnRepositoryTests
{
    private EprContext _context;
    private PrnRepository _repository;
    private readonly SavePrnDetailsRequest _validSavePrnDetailsRequestV2 = new()
    {
        PrnNumber = "PRN123",
        OrganisationId = Guid.NewGuid(),
        OrganisationName = "Org",
        ReprocessorExporterAgency = "Reprocessor",
        PrnStatusId = 1,
        TonnageValue = 0,
        MaterialName = "Plastic",
        IssuerNotes = "Notes",
        PrnSignatory = "Sig",
        PrnSignatoryPosition = "Role",
        DecemberWaste = true,
        StatusUpdatedOn = DateTime.UtcNow,
        IssuedByOrg = "Issuer",
        AccreditationNumber = "ACC123",
        ReprocessingSite = "Site",
        AccreditationYear = "2024",
        IsExport = false,
        SourceSystemId = "SYS",
        ProcessToBeUsed = "R4",
        ObligationYear = "2025",
    };

    [TestInitialize]
    public void Setup()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open(); // Keep the connection open for the lifetime of the context

        var options = new DbContextOptionsBuilder<EprContext>().UseSqlite(connection).Options;

        _context = new EprContext(options);
        _context.Database.EnsureCreated();

        _repository = new PrnRepository(_context);
    }
    
    [TestMethod]
    public async Task CanAddValidSavePrnDetailsRequestV2()
    {
        var prn = PrnMapper.CreateMapper().Map<Eprn>(_validSavePrnDetailsRequestV2);
        var added = await _context.AddAsync(prn, CancellationToken.None);
        added.Entity.Should().BeEquivalentTo(prn);
    }

    [TestMethod]
    public async Task GetAcceptedAndAwaitingPrnsByYearAsync_WhenDecemberWaste_ReturnsFilteredPrns()
    {
        // Arrange
        var organisationId = Guid.NewGuid();
        var year = 2026;
        var fixture = new Fixture();
        var prns = fixture.CreateMany<Eprn>(10).ToList();
        prns[0].OrganisationId = organisationId;
        prns[0].PrnStatusId = (int)EprnStatus.ACCEPTED;
        prns[0].ObligationYear = year.ToString();
        prns[1].OrganisationId = organisationId;
        prns[1].PrnStatusId = (int)EprnStatus.AWAITINGACCEPTANCE;
        prns[1].ObligationYear = year.ToString();
        prns[2].OrganisationId = organisationId;
        prns[2].PrnStatusId = (int)EprnStatus.ACCEPTED;
        prns[2].ObligationYear = "2025";
        prns[2].DecemberWaste = true;
        prns[3].OrganisationId = organisationId;
        prns[3].PrnStatusId = (int)EprnStatus.AWAITINGACCEPTANCE;
        prns[3].ObligationYear = "2025";
        prns[3].DecemberWaste = true;
        prns[4].OrganisationId = organisationId;
        prns[4].PrnStatusId = (int)EprnStatus.AWAITINGACCEPTANCE;
        prns[4].ObligationYear = "2025";
        prns[4].DecemberWaste = false;
        prns[5].OrganisationId = organisationId;
        prns[5].PrnStatusId = (int)EprnStatus.AWAITINGACCEPTANCE;
        prns[5].ObligationYear = "2027";
        prns[5].DecemberWaste = true;
        prns[6].OrganisationId = organisationId;
        prns[6].PrnStatusId = (int)EprnStatus.AWAITINGACCEPTANCE;
        prns[6].ObligationYear = "2027";
        prns[6].DecemberWaste = false;
        await _context.Prn.AddRangeAsync(prns, CancellationToken.None);
        await _context.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = _repository.GetAcceptedAndAwaitingPrnsByYear(organisationId, year).ToList();

        // Assert
        result.Should().HaveCount(4);
        result.Should().ContainSingle(r => r.Status.StatusName == nameof(EprnStatus.ACCEPTED) && r.Eprn.ObligationYear == year.ToString());
        result.Should().ContainSingle(r => r.Status.StatusName == nameof(EprnStatus.AWAITINGACCEPTANCE) && r.Eprn.ObligationYear == year.ToString());
        result.Should().ContainSingle(r => r.Status.StatusName == nameof(EprnStatus.ACCEPTED) && r.Eprn.ObligationYear == "2025");
        result.Should().ContainSingle(r => r.Status.StatusName == nameof(EprnStatus.AWAITINGACCEPTANCE) && r.Eprn.ObligationYear == "2025");
    }
}
