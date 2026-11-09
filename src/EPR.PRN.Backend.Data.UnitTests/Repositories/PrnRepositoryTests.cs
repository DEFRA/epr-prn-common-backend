using AutoFixture;
using AwesomeAssertions;
using EPR.PRN.Backend.API.Common.Dto;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Profiles;
using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Data.Dto;
using EPR.PRN.Backend.Data.Repositories;
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
    public async Task GetObligationSummary_ReturnsExpectedAggregationForSelectionDecisionTable()
    {
        var organisationId = Guid.NewGuid();
        var otherOrganisationId = Guid.NewGuid();
        var testCases = CreateSelectionTestCases(organisationId, otherOrganisationId);
        var fixture = new Fixture();
        var prns = CreatePrns(fixture, testCases, testCase => testCase.PrnNumber);

        await _context.Prn.AddRangeAsync(prns, CancellationToken.None);
        await _context.SaveChangesAsync(CancellationToken.None);

        var result = await new PrnRepository(_context, DecemberFifteenth2026).GetObligationSummary(organisationId, 2026);

        result
            .Select(summary => summary.MaterialName)
            .Should()
            .BeEquivalentTo(
                testCases
                    .Where(testCase => testCase.IsSelected)
                    .Select(testCase => testCase.PrnNumber)
            );
        result
            .Should()
            .BeEquivalentTo(
                [
                    new PrnObligationSummaryDto("PRN-001", 1, 0, 0),
                    new PrnObligationSummaryDto("PRN-003", 0, 3, 1),
                    new PrnObligationSummaryDto("PRN-004", 0, 4, 1),
                    new PrnObligationSummaryDto("PRN-005", 0, 5, 1),
                    new PrnObligationSummaryDto("PRN-007", 0, 7, 1),
                ]
            );
    }

    [TestMethod]
    public async Task GetObligationSummary_AggregatesTheSelectionDecisionTableWithoutDuplicates()
    {
        var organisationId = Guid.NewGuid();
        var testCases = CreateSelectionTestCases(organisationId, Guid.NewGuid());
        var prns = CreatePrns(new Fixture(), testCases, _ => "Plastic");

        await _context.Prn.AddRangeAsync(prns, CancellationToken.None);
        await _context.SaveChangesAsync(CancellationToken.None);

        var result = await new PrnRepository(_context, DecemberFifteenth2026).GetObligationSummary(organisationId, 2026);

        result.Should().BeEquivalentTo([new PrnObligationSummaryDto("Plastic", 1, 19, 4)]);
    }

    [TestMethod]
    [DataRow(2026, 3)]
    [DataRow(2027, 1)]
    [DataRow(2025, 0)]
    public async Task GetAwaitingAcceptanceCount_CountsOnlyNotesAcceptableIntoTheYearNow(int year, int expected)
    {
        var organisationId = Guid.NewGuid();
        var repository = new PrnRepository(
            _context,
            new FixedTimeProvider(new DateTimeOffset(2026, 12, 15, 12, 0, 0, TimeSpan.Zero))
        );
        var prns = new[]
        {
            CreateAwaitingPrn("STD-2026", organisationId, "2026", false),
            CreateAwaitingPrn("STD-2025", organisationId, "2025", false),
            CreateAwaitingPrn("DEC-2026", organisationId, "2026", true),
            CreateAwaitingPrn("DEC-2025", organisationId, "2025", true),
            CreateAwaitingPrn("OTHER-ORG", Guid.NewGuid(), "2026", false),
            CreateAwaitingPrn("ACCEPTED", organisationId, "2026", false, EprnStatus.ACCEPTED),
        };
        await _context.Prn.AddRangeAsync(prns, CancellationToken.None);
        await _context.SaveChangesAsync(CancellationToken.None);

        var result = await repository.GetAwaitingAcceptanceCount(organisationId, year);

        result.Should().Be(expected);
    }

    [TestMethod]
    public async Task GetAwaitingAcceptanceCount_BeforeTheDecemberWindow_ExcludesNextYearDecemberWaste()
    {
        var organisationId = Guid.NewGuid();
        var repository = new PrnRepository(
            _context,
            new FixedTimeProvider(new DateTimeOffset(2026, 6, 15, 12, 0, 0, TimeSpan.Zero))
        );
        await _context.Prn.AddAsync(CreateAwaitingPrn("DEC-2026", organisationId, "2026", true), CancellationToken.None);
        await _context.SaveChangesAsync(CancellationToken.None);

        (await repository.GetAwaitingAcceptanceCount(organisationId, 2026)).Should().Be(1);
        (await repository.GetAwaitingAcceptanceCount(organisationId, 2027)).Should().Be(0);
    }

    [TestMethod]
    [DataRow(2026, 12, 15, 2026, 18, 4)]
    [DataRow(2026, 12, 15, 2027, 5, 1)]
    [DataRow(2027, 2, 1, 2026, 0, 0)]
    [DataRow(2027, 2, 1, 2027, 5, 1)]
    [DataRow(2026, 11, 15, 2027, 0, 0)]
    public async Task GetObligationSummary_AwaitingTonnageCoversOnlyTheNotesTheCountCounts(
        int nowYear,
        int nowMonth,
        int nowDay,
        int year,
        int expectedTonnage,
        int expectedCount
    )
    {
        var organisationId = Guid.NewGuid();
        var repository = new PrnRepository(
            _context,
            new FixedTimeProvider(new DateTimeOffset(nowYear, nowMonth, nowDay, 12, 0, 0, TimeSpan.Zero))
        );
        var prns = new[]
        {
            CreateAwaitingPrn("STD-2026", organisationId, "2026", false, tonnage: 10),
            CreateAwaitingPrn("DEC-2026", organisationId, "2026", true, tonnage: 5),
            CreateAwaitingPrn("STD-2025", organisationId, "2025", false, tonnage: 3),
            CreateAwaitingPrn("DEC-2025", organisationId, "2025", true, tonnage: 2),
            // Accredited in 2026 for an obligation year of 2025: the old rule picked it up for 2027, the year rule does not.
            CreateAwaitingPrn("DEC-2025-ACCREDITED-2026", organisationId, "2025", true, tonnage: 1),
        };
        prns[4].AccreditationYear = "2026";
        foreach (var prn in prns)
        {
            prn.MaterialName = "Plastic";
        }
        await _context.Prn.AddRangeAsync(prns, CancellationToken.None);
        await _context.SaveChangesAsync(CancellationToken.None);

        var summary = await repository.GetObligationSummary(organisationId, year);
        var count = await repository.GetAwaitingAcceptanceCount(organisationId, year);

        summary.Sum(item => item.AwaitingAcceptanceTonnage).Should().Be(expectedTonnage);
        summary.Sum(item => item.AwaitingAcceptanceCount).Should().Be(expectedCount);
        count.Should().Be(expectedCount);
    }

    private static readonly FixedTimeProvider DecemberFifteenth2026 = new(
        new DateTimeOffset(2026, 12, 15, 12, 0, 0, TimeSpan.Zero)
    );

    private static readonly Fixture AwaitingPrnFixture = new();

    private static Eprn CreateAwaitingPrn(
        string prnNumber,
        Guid organisationId,
        string obligationYear,
        bool decemberWaste,
        EprnStatus status = EprnStatus.AWAITINGACCEPTANCE,
        int tonnage = 1
    ) =>
        AwaitingPrnFixture
            .Build<Eprn>()
            .With(prn => prn.OrganisationId, organisationId)
            .With(prn => prn.PrnNumber, prnNumber)
            .With(prn => prn.PrnStatusId, (int)status)
            .With(prn => prn.ObligationYear, obligationYear)
            .With(prn => prn.AccreditationYear, obligationYear)
            .With(prn => prn.DecemberWaste, decemberWaste)
            .With(prn => prn.TonnageValue, tonnage)
            .Create();

    private sealed class FixedTimeProvider(DateTimeOffset now) : TimeProvider
    {
        public override DateTimeOffset GetUtcNow() => now;
    }

    private static PrnSelectionCase[] CreateSelectionTestCases(
        Guid organisationId,
        Guid otherOrganisationId
    )
    {
        const string currentYear = "2026";
        const string previousYear = "2025";
        const string otherYear = "2024";

        return
        [
            new(
                "PRN-001",
                organisationId,
                (int)EprnStatus.ACCEPTED,
                currentYear,
                previousYear,
                false,
                true
            ),
            new(
                "PRN-002",
                organisationId,
                (int)EprnStatus.ACCEPTED,
                previousYear,
                previousYear,
                true,
                false
            ),
            new(
                "PRN-003",
                organisationId,
                (int)EprnStatus.AWAITINGACCEPTANCE,
                currentYear,
                otherYear,
                false,
                true
            ),
            new(
                "PRN-004",
                organisationId,
                (int)EprnStatus.AWAITINGACCEPTANCE,
                currentYear,
                previousYear,
                true,
                true
            ),
            new(
                "PRN-005",
                organisationId,
                (int)EprnStatus.AWAITINGACCEPTANCE,
                previousYear,
                previousYear,
                true,
                true
            ),
            new(
                "PRN-006",
                organisationId,
                (int)EprnStatus.AWAITINGACCEPTANCE,
                previousYear,
                previousYear,
                false,
                false
            ),
            new(
                "PRN-007",
                organisationId,
                (int)EprnStatus.AWAITINGACCEPTANCE,
                previousYear,
                currentYear,
                true,
                true
            ),
            new(
                "PRN-008",
                organisationId,
                (int)EprnStatus.AWAITINGACCEPTANCE,
                otherYear,
                previousYear,
                true,
                false
            ),
            new(
                "PRN-009",
                organisationId,
                (int)EprnStatus.REJECTED,
                currentYear,
                previousYear,
                true,
                false
            ),
            new(
                "PRN-010",
                organisationId,
                (int)EprnStatus.CANCELLED,
                currentYear,
                previousYear,
                true,
                false
            ),
            new(
                "PRN-011",
                otherOrganisationId,
                (int)EprnStatus.ACCEPTED,
                currentYear,
                previousYear,
                false,
                false
            ),
            new(
                "PRN-012",
                otherOrganisationId,
                (int)EprnStatus.AWAITINGACCEPTANCE,
                currentYear,
                previousYear,
                true,
                false
            ),
        ];
    }

    private static List<Eprn> CreatePrns(
        Fixture fixture,
        IEnumerable<PrnSelectionCase> testCases,
        Func<PrnSelectionCase, string> getMaterialName
    )
    {
        return testCases
            .Select(
                (testCase, index) =>
                    fixture
                        .Build<Eprn>()
                        .With(prn => prn.OrganisationId, testCase.OrganisationId)
                        .With(prn => prn.PrnNumber, testCase.PrnNumber)
                        .With(prn => prn.PrnStatusId, testCase.PrnStatusId)
                        .With(prn => prn.ObligationYear, testCase.ObligationYear)
                        .With(prn => prn.AccreditationYear, testCase.AccreditationYear)
                        .With(prn => prn.DecemberWaste, testCase.DecemberWaste)
                        .With(prn => prn.MaterialName, getMaterialName(testCase))
                        .With(prn => prn.TonnageValue, index + 1)
                        .Create()
            )
            .ToList();
    }

    private sealed record PrnSelectionCase(
        string PrnNumber,
        Guid OrganisationId,
        int PrnStatusId,
        string ObligationYear,
        string AccreditationYear,
        bool DecemberWaste,
        bool IsSelected
    );
}
