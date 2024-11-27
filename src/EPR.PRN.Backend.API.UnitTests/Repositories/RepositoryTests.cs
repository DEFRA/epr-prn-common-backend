using AutoFixture;
using EPR.PRN.Backend.API.Repositories;
using EPR.PRN.Backend.Data;
using EPR.PRN.Backend.Data.DataModels;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
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
        _repository = new Repository(_mockContext.Object);
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
        var repo = new Repository(context);

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
        var repo = new Repository(context);

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
        var repo = new Repository(context);

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
        var repo = new Repository(context);

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
        var fromDate = DateTime.UtcNow.AddDays(-7);
        var toDate = DateTime.UtcNow;

        using var context = new EprContext(_contextOptions);
        if (await context.Database.EnsureCreatedAsync())
        { }

        //Act
        var repo = new Repository(context);
        var result = await repo.GetModifiedPrnsbyDate(fromDate, toDate);

        //Assert
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public void MapStatusCode_ReturnsCorrectValues()
    {
        // Arrange
        var privateMethod = typeof(Repository)
            .GetMethod("MapStatusCode", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        // Act & Assert
        var acceptedResult = privateMethod.Invoke(_repository, new object[] { EprnStatus.ACCEPTED, "2024" });
        var rejectedResult = privateMethod.Invoke(_repository, new object[] { EprnStatus.REJECTED, "2024" });
        var cancelledResult = privateMethod.Invoke(_repository, new object[] { EprnStatus.CANCELLED, "2024" });
        var awaiting2024Result = privateMethod.Invoke(_repository, new object[] { EprnStatus.AWAITINGACCEPTANCE, "2024" });
        var awaiting2025Result = privateMethod.Invoke(_repository, new object[] { EprnStatus.AWAITINGACCEPTANCE, "2025" });
        var awaitingOtherResult = privateMethod.Invoke(_repository, new object[] { EprnStatus.AWAITINGACCEPTANCE, "2023" });

        Assert.AreEqual("EV-ACCEP", acceptedResult);
        Assert.AreEqual("EV-ACANCEL", rejectedResult);
        Assert.AreEqual("EV-CANCEL", cancelledResult);
        Assert.AreEqual("EV-AWACCEP", awaiting2024Result);
        Assert.AreEqual("EV-AWACCEP-EPR", awaiting2025Result);
        Assert.AreEqual("EV-AWACCEP", awaitingOtherResult);
    }
}
