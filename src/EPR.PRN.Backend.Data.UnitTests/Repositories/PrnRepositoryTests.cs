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

        var result = await _repository.GetObligationSummary(organisationId, 2026);

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
                    new PrnObligationSummaryDto("PRN-008", 0, 8, 1),
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

        var result = await _repository.GetObligationSummary(organisationId, 2026);

        result.Should().BeEquivalentTo([new PrnObligationSummaryDto("Plastic", 1, 20, 4)]);
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
                false
            ),
            new(
                "PRN-008",
                organisationId,
                (int)EprnStatus.AWAITINGACCEPTANCE,
                otherYear,
                previousYear,
                true,
                true
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
