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
        var organisationId = Guid.NewGuid();
        var fixture = new Fixture();
        var prns = fixture.CreateMany<Eprn>(11).ToList();
        
        prns[0].OrganisationId = organisationId;
        prns[0].PrnNumber = "PRN-001-NPWD";
        prns[0].PrnStatusId = (int)EprnStatus.AWAITINGACCEPTANCE;
        prns[0].AccreditationYear = "2025";
        prns[0].ObligationYear = "2025";
        prns[0].DecemberWaste = false;
        
        prns[1].OrganisationId = organisationId;
        prns[1].PrnNumber = "PRN-002-NPWD-DEC";
        prns[1].PrnStatusId = (int)EprnStatus.AWAITINGACCEPTANCE;
        prns[1].AccreditationYear = "2025";
        prns[1].ObligationYear = "2025";
        prns[1].DecemberWaste = true;
        
        prns[2].OrganisationId = organisationId;
        prns[2].PrnNumber = "PRN-003-RREPW";
        prns[2].PrnStatusId = (int)EprnStatus.AWAITINGACCEPTANCE;
        prns[2].AccreditationYear = "2026";
        prns[2].ObligationYear = "2026";
        prns[2].DecemberWaste = false;
        
        prns[3].OrganisationId = organisationId;
        prns[3].PrnNumber = "PRN-004-RREPW-DEC";
        prns[3].PrnStatusId = (int)EprnStatus.AWAITINGACCEPTANCE;
        prns[3].AccreditationYear = "2026";
        prns[3].ObligationYear = "2026";
        prns[3].DecemberWaste = true;
        
        prns[4].OrganisationId = organisationId;
        prns[4].PrnNumber = "PRN-005-OLD";
        prns[4].PrnStatusId = (int)EprnStatus.ACCEPTED;
        prns[4].AccreditationYear = "2024";
        prns[4].ObligationYear = "2025";
        prns[4].DecemberWaste = false;
        
        prns[5].OrganisationId = organisationId;
        prns[5].PrnNumber = "PRN-006-OLD-DEC";
        prns[5].PrnStatusId = (int)EprnStatus.ACCEPTED;
        prns[5].AccreditationYear = "2024";
        prns[5].ObligationYear = "2025";
        prns[5].DecemberWaste = true;
        
        prns[6].OrganisationId = organisationId;
        prns[6].PrnNumber = "PRN-007-OLD-DEC";
        prns[6].PrnStatusId = (int)EprnStatus.ACCEPTED;
        prns[6].AccreditationYear = "2025";
        prns[6].ObligationYear = "2025";
        prns[6].DecemberWaste = true;
        
        prns[7].OrganisationId = organisationId;
        prns[7].PrnNumber = "PRN-008-CURRENT";
        prns[7].PrnStatusId = (int)EprnStatus.ACCEPTED;
        prns[7].AccreditationYear = "2026";
        prns[7].ObligationYear = "2026";
        prns[7].DecemberWaste = false;
        
        prns[8].OrganisationId = organisationId;
        prns[8].PrnNumber = "PRN-009-RREPW";
        prns[8].PrnStatusId = (int)EprnStatus.AWAITINGACCEPTANCE;
        prns[8].AccreditationYear = "2027";
        prns[8].ObligationYear = "2027";
        prns[8].DecemberWaste = false;
        
        prns[9].OrganisationId = organisationId;
        prns[9].PrnNumber = "PRN-010-RREPW-DEC";
        prns[9].PrnStatusId = (int)EprnStatus.AWAITINGACCEPTANCE;
        prns[9].AccreditationYear = "2027";
        prns[9].ObligationYear = "2027";
        prns[9].DecemberWaste = true;
        
        prns[10].OrganisationId = organisationId;
        prns[10].PrnNumber = "PRN-011-OLD-DEC";
        prns[10].PrnStatusId = (int)EprnStatus.AWAITINGACCEPTANCE;
        prns[10].AccreditationYear = "2024";
        prns[10].ObligationYear = "2025";
        prns[10].DecemberWaste = true;
        
        await _context.Prn.AddRangeAsync(prns, CancellationToken.None);
        await _context.SaveChangesAsync(CancellationToken.None);

        var result = _repository.GetAcceptedAndAwaitingPrnsByYear(organisationId, 2026).ToList();

        result.Should().HaveCount(4);
        result.Should().ContainSingle(x => x.Eprn.PrnNumber == "PRN-002-NPWD-DEC");
        result.Should().ContainSingle(x => x.Eprn.PrnNumber == "PRN-003-RREPW");
        result.Should().ContainSingle(x => x.Eprn.PrnNumber == "PRN-004-RREPW-DEC");
        result.Should().ContainSingle(x => x.Eprn.PrnNumber == "PRN-008-CURRENT");
        
        result = _repository.GetAcceptedAndAwaitingPrnsByYear(organisationId, 2025).ToList();

        result.Should().HaveCount(6);
        result.Should().ContainSingle(x => x.Eprn.PrnNumber == "PRN-001-NPWD");
        result.Should().ContainSingle(x => x.Eprn.PrnNumber == "PRN-002-NPWD-DEC");
        result.Should().ContainSingle(x => x.Eprn.PrnNumber == "PRN-005-OLD");
        result.Should().ContainSingle(x => x.Eprn.PrnNumber == "PRN-006-OLD-DEC");
        result.Should().ContainSingle(x => x.Eprn.PrnNumber == "PRN-007-OLD-DEC");
        result.Should().ContainSingle(x => x.Eprn.PrnNumber == "PRN-011-OLD-DEC");
    }
}
