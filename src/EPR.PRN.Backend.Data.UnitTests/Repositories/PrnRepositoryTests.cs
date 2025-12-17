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
    private readonly SavePrnDetailsRequestV2 _validSavePrnDetailsRequestV2 = new()
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
        ProcessToBeUsed = "R4"
    };
    [TestInitialize]
    public void Setup()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open(); // Keep the connection open for the lifetime of the context

        var options = new DbContextOptionsBuilder<EprContext>()
            .UseSqlite(connection)
            .Options;

        _context = new EprContext(options);
        _context.Database.EnsureCreated(); 

        _repository = new PrnRepository(_context);
    }

    [TestMethod]
    public async Task CanAddValidSavePrnDetailsRequestV2()
    {
        var prn = PrnProfile.CreateMapper().Map<Eprn>(_validSavePrnDetailsRequestV2);
        var added = await _context.AddAsync(prn, CancellationToken.None);
        added.Entity.Should().BeEquivalentTo(prn);
    }
    
    [TestMethod]
    public async Task GetAcceptedAndAwaitingPrnsByYearAsync_ReturnsFilteredPrns()
    {
        // Arrange
        var organisationId = Guid.NewGuid();
        var year = 2023;
        var fixture = new Fixture();
        var prnStatuses = new[]
        {
            new PrnStatus {  StatusName = nameof(EprnStatus.ACCEPTED) },
            new PrnStatus {  StatusName = nameof(EprnStatus.AWAITINGACCEPTANCE) }
        };
        await _context.PrnStatus.AddRangeAsync(prnStatuses, CancellationToken.None);
        await _context.SaveChangesAsync(CancellationToken.None);
        var prns = fixture.CreateMany<Eprn>(10).ToList();
        prns[0].OrganisationId = organisationId;
        prns[0].PrnStatusId = prnStatuses[0].Id; // ACCEPTED
        prns[0].ObligationYear = year.ToString();
        prns[1].OrganisationId = organisationId;
        prns[1].PrnStatusId = prnStatuses[1].Id; // AWAITINGACCEPTANCE
        prns[1].ObligationYear = year.ToString();
        await _context.Prn.AddRangeAsync(prns, CancellationToken.None);
        await _context.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = _repository.GetAcceptedAndAwaitingPrnsByYear(organisationId, year).ToList();

        // Assert
        result.Should().HaveCount(2);
        result.Should().ContainSingle(r => r.Status.StatusName == nameof(EprnStatus.ACCEPTED));
        result.Should().ContainSingle(r => r.Status.StatusName == nameof(EprnStatus.AWAITINGACCEPTANCE));
    }
}