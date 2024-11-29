using AutoFixture;
using EPR.PRN.Backend.API.Repositories;
using EPR.PRN.Backend.Data;
using EPR.PRN.Backend.Data.DataModels;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.UnitTests.Repositories;

[ExcludeFromCodeCoverage]
[TestClass]
public class RepositoryTests
{
    private SqliteConnection _connection;
    private DbContextOptions<EprContext> _contextOptions;
    private Fixture _fixture;
    private Mock<EprContext> _mockContext;
    private Repository _repository;
    private Mock<ILogger<EprContext>> _mockLogger;

    [TestInitialize]
    public void TestInitialize()
    {
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();
        _mockLogger = new Mock<ILogger<EprContext>>();
        // These options will be used by the context instances in this test suite, including the connection opened above.
        _contextOptions = new DbContextOptionsBuilder<EprContext>()
           .UseSqlite(_connection)
           .Options;

        _fixture = new Fixture();
        _mockContext = new Mock<EprContext>();
        _repository = new Repository(_mockContext.Object, _mockLogger.Object);
    }

    [TestMethod]
    public async Task GetAllPrnByOrganisationId_Returns_Prns()
    {
        //Arrange
        var data = _fixture.CreateMany<Eprn>().ToArray();
        data[0].PrnStatusId = data[1].PrnStatusId = data[2].PrnStatusId = 1;

        using var context = new EprContext(_contextOptions);
        if (await context.Database.EnsureCreatedAsync())
        {
            context.AddRange(data);
            await context.SaveChangesAsync();
        }
        //Act
        var repo = new Repository(context, _mockLogger.Object);

        //Assert
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
        if (await context.Database.EnsureCreatedAsync())
        {
            context.AddRange(data);
            await context.SaveChangesAsync();
        }
        //Act
        var repo = new Repository(context, _mockLogger.Object);

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
        if (await context.Database.EnsureCreatedAsync())
        {
            context.AddRange(data);
            await context.SaveChangesAsync();
        }
        //Act
        var repo = new Repository(context, _mockLogger.Object);

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
        if (await context.Database.EnsureCreatedAsync())
        {
            context.AddRange(data);
            await context.SaveChangesAsync();
        }
        //Act
        var repo = new Repository(context, _mockLogger.Object);

        var transaction = repo.BeginTransaction();
        repo.AddPrnStatusHistory(statusHistory);
        await repo.SaveTransaction(transaction);

        var history = await context.PrnStatusHistory.Where(p => p.CreatedByUser == statusHistory.CreatedByUser).ToListAsync();
        history.Should().HaveCount(1);
    }

    [TestMethod]
    public async Task GetModifiedPrnsbyDate_ReturnsMappedPrnUpdateStatuses()
    {
        //Arrange
        var fromDate = new DateTime(2024, 11, 19);
        var toDate = new DateTime(2024, 11, 24);

        var data = _fixture.CreateMany<Eprn>().ToArray();
        data[0].PrnNumber = "PRN001";
        data[0].StatusUpdatedOn = new DateTime(2024, 11, 23);
        data[0].PrnStatusId = 1;
        data[0].AccreditationYear = "2023";

        data[1].PrnNumber = "PRN002";
        data[1].StatusUpdatedOn = new DateTime(2024, 11, 20);
        data[1].PrnStatusId = 2;
        data[1].AccreditationYear = "2024";

        using var context = new EprContext(_contextOptions);
        if (await context.Database.EnsureCreatedAsync())
        {
            context.AddRange(data);
            await context.SaveChangesAsync();
        }

        //Act
        var repo = new Repository(context, _mockLogger.Object);
        var result = await repo.GetModifiedPrnsbyDate(fromDate, toDate);

        //Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(2, result.Count);

        var firstPrn = result[0];
        Assert.AreEqual("PRN001", firstPrn.EvidenceNo);
        Assert.AreEqual("2023", firstPrn.AccreditationYear);
        Assert.AreEqual("EV-ACCEP", firstPrn.EvidenceStatusCode);

        var secondPrn = result[1];
        Assert.AreEqual("PRN002", secondPrn.EvidenceNo);
        Assert.AreEqual("2024", secondPrn.AccreditationYear);
        Assert.AreEqual("EV-ACANCEL", secondPrn.EvidenceStatusCode);
    }
}
