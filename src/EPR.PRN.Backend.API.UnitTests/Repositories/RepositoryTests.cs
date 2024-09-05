using AutoFixture.MSTest;
using EPR.PRN.Backend.API.Repositories;
using EPR.PRN.Backend.Data;
using EPR.PRN.Backend.Data.DataModels;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.UnitTests.Repositories;

[ExcludeFromCodeCoverage]
[TestClass]
public class RepositoryTests
{
    private SqliteConnection _connection;
    private DbContextOptions<EprContext> _contextOptions;

    [TestInitialize]
    public void TestInitialize()
    {
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        // These options will be used by the context instances in this test suite, including the connection opened above.
        _contextOptions = new DbContextOptionsBuilder<EprContext>()
           .UseSqlite(_connection)
           .Options;
    }

    [TestMethod]
    [AutoData]
    public async Task GetAllPrnByOrganisationId_Returns_Prns(List<Eprn> data)
    {
        //Arrange
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
    [AutoData]
    public async Task GetPrnForOrganisationById_Returns_Prn(List<Eprn> data)
    {
        //Arrange
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
    [AutoData]
    public async Task SaveTransaction_SavesDataInDB(List<Eprn> data)
    {
        //Arrange
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
    [AutoData]
    public async Task AddPrnHistory(List<Eprn> data, PrnStatusHistory statusHistory)
    {
        //Arrange
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
}
