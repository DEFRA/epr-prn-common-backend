using System.Diagnostics.CodeAnalysis;
using AutoFixture;
using EPR.PRN.Backend.API.Repositories;
using EPR.PRN.Backend.Data;
using EPR.PRN.Backend.Data.DataModels;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace EPR.PRN.Backend.API.UnitTests.Repositories;

[ExcludeFromCodeCoverage]
[TestClass]
public class RepositoryTests
{
    private SqliteConnection _connection;
    private DbContextOptions<EprContext> _contextOptions;
    private Fixture _fixture;
    private Mock<EprContext> _mockContext;
    private Mock<ILogger<Repository>> _mockLogger;
    private Mock<IConfiguration> _configurationMock;
    private Repository _repository;

    [TestInitialize]
    public void TestInitialize()
    {
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        // These options will be used by the context instances in this test suite, including the connection opened above.
        _contextOptions = new DbContextOptionsBuilder<EprContext>()
           .UseSqlite(_connection)
           .Options;

        _fixture = new Fixture();
        _mockContext = new Mock<EprContext>();
        
        _mockLogger = new Mock<ILogger<Repository>>();
        _configurationMock = new Mock<IConfiguration>();
        _configurationMock.Setup(c => c["LogPrefix"]).Returns("[EPR.PRN.Backend]");

        _repository = new Repository(_mockContext.Object, _mockLogger.Object, _configurationMock.Object);
    }

    [TestMethod]
    public async Task GetAllPrnByOrganisationId_Returns_Prns()
    {
        // Arrange
        var data = _fixture.CreateMany<Eprn>().ToArray();
        data[0].PrnStatusId = data[1].PrnStatusId = data[2].PrnStatusId = 1;

        using var context = new EprContext(_contextOptions);
        if (await context.Database.EnsureCreatedAsync(CancellationToken.None))
        {
            context.AddRange(data);
            await context.SaveChangesAsync(CancellationToken.None);
        }

        // Act
        var repo = new Repository(context, _mockLogger.Object, _configurationMock.Object);

        // Assert
        var prns = await repo.GetAllPrnByOrganisationId(data[0].OrganisationId);

        prns.Should().ContainSingle();
        prns[0].Should().BeSameAs(data[0]);
    }

    [TestMethod]
    public async Task GetPrnForOrganisationById_Returns_Prn()
    {
        //Arrange
        var data = _fixture.CreateMany<Eprn>().ToArray();
        data[0].PrnStatusId = data[1].PrnStatusId = data[2].PrnStatusId = 1;

        using var context = new EprContext(_contextOptions);
        if (await context.Database.EnsureCreatedAsync(CancellationToken.None))
        {
            context.AddRange(data);
            await context.SaveChangesAsync(CancellationToken.None);
        }
        //Act
        var repo = new Repository(context, _mockLogger.Object, _configurationMock.Object);

        //Assert
        var prn = await repo.GetPrnForOrganisationById(data[0].OrganisationId, data[0].ExternalId);
        prn.Should().BeSameAs(data[0]);
    }

    [TestMethod]
    public async Task SaveTransaction_SavesDataInDB()
    {
        //Arrange
        var data = _fixture.CreateMany<Eprn>().ToArray();
        data[0].PrnStatusId = data[1].PrnStatusId = data[2].PrnStatusId = 2;

        using var context = new EprContext(_contextOptions);
        if (await context.Database.EnsureCreatedAsync(CancellationToken.None))
        {
            context.AddRange(data);
            await context.SaveChangesAsync(CancellationToken.None);
        }
        //Act
        var repo = new Repository(context, _mockLogger.Object, _configurationMock.Object);

        var transaction = repo.BeginTransaction();
        var updatingPrn = await repo.GetAllPrnByOrganisationId(data[0].OrganisationId);
        updatingPrn[0].PrnStatusId = 3;
        await repo.SaveTransaction(transaction);

        var prn = await repo.GetPrnForOrganisationById(data[0].OrganisationId, data[0].ExternalId);

        //Asset
        prn.PrnStatusId.Should().Be(3);
    }

    [TestMethod]
    public async Task AddPrnHistory()
    {
        //Arrange
        var data = _fixture.CreateMany<Eprn>().ToArray();
        var statusHistory = _fixture.Create<PrnStatusHistory>();
        data[0].PrnStatusId = data[1].PrnStatusId = data[2].PrnStatusId = 2;
        statusHistory.PrnIdFk = data[0].Id;
        statusHistory.PrnStatusIdFk = data[0].PrnStatusId;
        using var context = new EprContext(_contextOptions);
        if (await context.Database.EnsureCreatedAsync(CancellationToken.None))
        {
            context.AddRange(data);
            await context.SaveChangesAsync(CancellationToken.None);
        }
        //Act
        var repo = new Repository(context, _mockLogger.Object, _configurationMock.Object);

        var transaction = repo.BeginTransaction();
        repo.AddPrnStatusHistory(statusHistory);
        await repo.SaveTransaction(transaction);

        var history = await context.PrnStatusHistory.Where(p => p.CreatedByUser == statusHistory.CreatedByUser).ToListAsync(CancellationToken.None);
        history.Should().HaveCount(1);
    }

    [TestMethod]
    public async Task GetModifiedPrnsbyDate_ReturnsMappedPrnUpdateStatuses()
    {
        //Arrange
        var fromDate = new DateTime(2021, 11, 22, 0, 0, 0, DateTimeKind.Utc);
        var toDate = new DateTime(2024, 11, 24, 0, 0, 0, DateTimeKind.Utc);

        var data = _fixture.CreateMany<Eprn>().ToArray();
        data[0].PrnNumber = "PRN001";
        data[0].StatusUpdatedOn = new DateTime(2024, 11, 23, 0, 0, 0, DateTimeKind.Utc);
        data[0].PrnStatusId = 1;
        data[0].AccreditationYear = "2023";

        data[1].PrnNumber = "PRN002";
        data[1].StatusUpdatedOn = new DateTime(2024, 11, 22, 0, 0, 0, DateTimeKind.Utc);
        data[1].PrnStatusId = 2;
        data[1].AccreditationYear = "2024";

		data[2].PrnNumber = "PRN003";
		data[2].StatusUpdatedOn = new DateTime(2024, 12, 12, 0, 0, 0, DateTimeKind.Utc);
		data[2].PrnStatusId = 2;
		data[2].AccreditationYear = "2024";

		using var context = new EprContext(_contextOptions);
        if (await context.Database.EnsureCreatedAsync(CancellationToken.None))
        {
            context.AddRange(data);
            await context.SaveChangesAsync(CancellationToken.None);
        }

        //Act
        var repo = new Repository(context, _mockLogger.Object, _configurationMock.Object);
        var result = await repo.GetModifiedPrnsbyDate(fromDate, toDate);

        //Assert
        Assert.IsNotNull(result);
        Assert.HasCount(2, result);

        var firstPrn = result[0];
        Assert.AreEqual("PRN001", firstPrn.EvidenceNo);
        Assert.AreEqual("2023", firstPrn.AccreditationYear);
        Assert.AreEqual("EV-ACCEP", firstPrn.EvidenceStatusCode);

        var secondPrn = result[1];
        Assert.AreEqual("PRN002", secondPrn.EvidenceNo);
        Assert.AreEqual("2024", secondPrn.AccreditationYear);
        Assert.AreEqual("EV-ACANCEL", secondPrn.EvidenceStatusCode);
    }

    [TestMethod]
    public async Task GetSyncStatus_ReturnsMappedPrnStatusSyncs()
    {
        // Arrange
        var fromDate = new DateTime(2024, 11, 22, 0, 0, 0, DateTimeKind.Local);
        var toDate = new DateTime(2024, 11, 24, 0, 0, 0, DateTimeKind.Local);

        // Create the Eprn entities
        var prnData = _fixture.CreateMany<Eprn>().ToArray();
        prnData[0].PrnStatusId = 1;
        prnData[0].Id = 1;
        prnData[0].PrnNumber = "PRN001";
        prnData[0].StatusUpdatedOn = new DateTime(2024, 11, 23, 0, 0, 0, DateTimeKind.Local);
        prnData[0].OrganisationName = "Org1";

        prnData[1].PrnStatusId = 2;
        prnData[1].Id = 2;
        prnData[1].PrnNumber = "PRN002";
        prnData[1].StatusUpdatedOn = new DateTime(2024, 11, 23, 0, 0, 0, DateTimeKind.Local);
        prnData[1].OrganisationName = "Org2";

        // Create the PEprNpwdSync entities
        var syncData = new List<PEprNpwdSync>
    {
        new PEprNpwdSync { PRNId = 1, PRNStatusId = 1, CreatedOn= new DateTime(2024, 11, 23, 0, 0, 0, DateTimeKind.Local), Id=1 },
        new PEprNpwdSync { PRNId = 2, PRNStatusId = 2, CreatedOn = new DateTime(2024, 11, 23, 0, 0, 0, DateTimeKind.Local),Id=2 }
    };

        using var context = new EprContext(_contextOptions);
        if (await context.Database.EnsureCreatedAsync(CancellationToken.None))
        {
            await context.AddRangeAsync(prnData, CancellationToken.None);  // Add Eprn entities
            await context.AddRangeAsync(syncData, CancellationToken.None); // Add PEprNpwdSync entities
            await context.SaveChangesAsync(CancellationToken.None);
        }

        // Act
        var repo = new Repository(context, _mockLogger.Object, _configurationMock.Object);
        var result = await repo.GetSyncStatuses(fromDate, toDate);

        // Assert
        Assert.IsNotNull(result);
        Assert.HasCount(2, result);

        var firstSync = result.First(x => x.PrnNumber == "PRN001");
        Assert.AreEqual("PRN001", firstSync.PrnNumber);
        Assert.AreEqual("Org1", firstSync.OrganisationName);
        Assert.AreEqual("EV-ACCEP", firstSync.StatusName);

        var secondSync = result.First(x => x.PrnNumber == "PRN002");
        Assert.AreEqual("PRN002", secondSync.PrnNumber);
        Assert.AreEqual("Org2", secondSync.OrganisationName);
        Assert.AreEqual("EV-ACANCEL", secondSync.StatusName);
    }

    [TestMethod]
    public async Task GetPrnsForPrnNumbers_ReturnMatchingPrns()
    {
        var prns = _fixture.CreateMany<Eprn>().ToList();
        using var context = new EprContext(_contextOptions);
        if (await context.Database.EnsureCreatedAsync(CancellationToken.None))
        {
            context.AddRange(prns);
            await context.SaveChangesAsync(CancellationToken.None);
        }
        _repository = new Repository(context, _mockLogger.Object, _configurationMock.Object);
        var result = await _repository.GetPrnsForPrnNumbers([prns[0].PrnNumber, prns[1].PrnNumber]);

        result.Count.Should().Be(2);
        result.Should().BeEquivalentTo([prns[0], prns[1]], o => o.Excluding(prn => prn.PrnStatusHistories));
    }

    [TestMethod]
    public async Task InsertPeprNpwdSyncPrns_ReturnMatchingPrns()
    {
        var syncPepr = _fixture.CreateMany<PEprNpwdSync>().ToList();
        using var context = new EprContext(_contextOptions);
        await context.Database.EnsureCreatedAsync(CancellationToken.None);

        _repository = new Repository(context, _mockLogger.Object, _configurationMock.Object);
        await _repository.InsertPeprNpwdSyncPrns(syncPepr);

        context.PEprNpwdSync.Count().Should().Be(syncPepr.Count);
    }
}
