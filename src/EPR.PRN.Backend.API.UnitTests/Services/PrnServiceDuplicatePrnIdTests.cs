using System.Diagnostics.CodeAnalysis;
using AutoFixture;
using AwesomeAssertions;
using EPR.PRN.Backend.API.Common.Dto;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Helpers;
using EPR.PRN.Backend.API.Repositories;
using EPR.PRN.Backend.API.Services;
using EPR.PRN.Backend.Data;
using EPR.PRN.Backend.Data.DataModels;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace EPR.PRN.Backend.API.UnitTests.Services;

// Runs the real PrnService and Repository against SQLite. The mocked repository in
// PrnServiceTests cannot show what the database keeps when a batch names one PRN twice.
[ExcludeFromCodeCoverage]
[TestClass]
public class PrnServiceDuplicatePrnIdTests
{
    private SqliteConnection _connection;
    private DbContextOptions<EprContext> _contextOptions;
    private Fixture _fixture;
    private Guid _orgId;
    private Guid _userId;

    [TestInitialize]
    public async Task Initialize()
    {
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();
        _contextOptions = new DbContextOptionsBuilder<EprContext>().UseSqlite(_connection).Options;
        _fixture = new Fixture();
        _orgId = Guid.NewGuid();
        _userId = Guid.NewGuid();

        await using var context = new EprContext(_contextOptions);
        await context.Database.EnsureCreatedAsync(CancellationToken.None);
    }

    [TestCleanup]
    public void Cleanup() => _connection.Dispose();

    [TestMethod]
    public async Task DuplicatePrnIdInAGuardedBatch_ShouldBeRejectedAndLeaveTheStoredPrnUnchanged()
    {
        var prn = await SeedPrnAsync(EprnStatus.AWAITINGACCEPTANCE);
        var updates = new List<PrnUpdateStatusDto>
        {
            new()
            {
                PrnId = prn.ExternalId,
                Status = EprnStatus.ACCEPTED,
                ObligationYear = "2027",
                ExpectedStatus = EprnStatus.AWAITINGACCEPTANCE,
            },
            new() { PrnId = prn.ExternalId, Status = EprnStatus.REJECTED },
        };

        var act = () => CreateService().UpdateStatus(_orgId, _userId, updates);

        await act.Should().ThrowAsync<ConflictException>();

        await using var check = new EprContext(_contextOptions);
        var stored = await check.Prn.Include(x => x.PrnStatusHistories).SingleAsync(x => x.Id == prn.Id);
        stored.PrnStatusId.Should().Be((int)EprnStatus.AWAITINGACCEPTANCE);
        stored.ObligationYear.Should().Be("2026");
        stored.PrnStatusHistories.Should().BeEmpty();
    }

    [TestMethod]
    public async Task DuplicatePrnIdWithTheSameGuardedUpdateTwice_ShouldBeRejectedAndLeaveTheStoredPrnUnchanged()
    {
        var prn = await SeedPrnAsync(EprnStatus.AWAITINGACCEPTANCE);
        var update = new PrnUpdateStatusDto
        {
            PrnId = prn.ExternalId,
            Status = EprnStatus.ACCEPTED,
            ObligationYear = "2027",
            ExpectedStatus = EprnStatus.AWAITINGACCEPTANCE,
        };

        var act = () => CreateService().UpdateStatus(_orgId, _userId, [update, update]);

        await act.Should().ThrowAsync<ConflictException>();

        await using var check = new EprContext(_contextOptions);
        var stored = await check.Prn.Include(x => x.PrnStatusHistories).SingleAsync(x => x.Id == prn.Id);
        stored.PrnStatusId.Should().Be((int)EprnStatus.AWAITINGACCEPTANCE);
        stored.ObligationYear.Should().Be("2026");
        stored.PrnStatusHistories.Should().BeEmpty();
    }

    [TestMethod]
    public async Task UnguardedRejectThenGuardedAccept_ForTheSamePrn_ShouldBeRejectedAndLeaveTheStoredPrnUnchanged()
    {
        var prn = await SeedPrnAsync(EprnStatus.AWAITINGACCEPTANCE);
        var updates = new List<PrnUpdateStatusDto>
        {
            new() { PrnId = prn.ExternalId, Status = EprnStatus.REJECTED },
            new()
            {
                PrnId = prn.ExternalId,
                Status = EprnStatus.ACCEPTED,
                ObligationYear = "2027",
                ExpectedStatus = EprnStatus.AWAITINGACCEPTANCE,
            },
        };

        var act = () => CreateService().UpdateStatus(_orgId, _userId, updates);

        await act.Should().ThrowAsync<ConflictException>();

        await using var check = new EprContext(_contextOptions);
        var stored = await check.Prn.Include(x => x.PrnStatusHistories).SingleAsync(x => x.Id == prn.Id);
        stored.PrnStatusId.Should().Be((int)EprnStatus.AWAITINGACCEPTANCE);
        stored.ObligationYear.Should().Be("2026");
        stored.PrnStatusHistories.Should().BeEmpty();
    }

    [TestMethod]
    public async Task DuplicatePrnIdWithoutAGuard_ShouldBeRejectedAndLeaveTheStoredPrnUnchanged()
    {
        var prn = await SeedPrnAsync(EprnStatus.AWAITINGACCEPTANCE);
        var updates = new List<PrnUpdateStatusDto>
        {
            new() { PrnId = prn.ExternalId, Status = EprnStatus.REJECTED },
            new() { PrnId = prn.ExternalId, Status = EprnStatus.ACCEPTED },
        };

        var act = () => CreateService().UpdateStatus(_orgId, _userId, updates);

        await act.Should().ThrowAsync<ConflictException>();

        await using var check = new EprContext(_contextOptions);
        var stored = await check.Prn.Include(x => x.PrnStatusHistories).SingleAsync(x => x.Id == prn.Id);
        stored.PrnStatusId.Should().Be((int)EprnStatus.AWAITINGACCEPTANCE);
        stored.PrnStatusHistories.Should().BeEmpty();
    }

    private PrnService CreateService()
    {
        var context = new EprContext(_contextOptions);
        var configuration = new ConfigurationBuilder().Build();
        var repository = new Repository(context, Mock.Of<ILogger<Repository>>(), configuration);
        return new PrnService(repository, Mock.Of<ILogger<PrnService>>(), configuration);
    }

    private async Task<Eprn> SeedPrnAsync(EprnStatus status)
    {
        var prn = _fixture.Build<Eprn>().Without(x => x.PrnStatusHistories).Create();
        prn.OrganisationId = _orgId;
        prn.PrnStatusId = (int)status;
        prn.ObligationYear = "2026";

        await using var context = new EprContext(_contextOptions);
        context.Prn.Add(prn);
        await context.SaveChangesAsync();
        return prn;
    }
}
