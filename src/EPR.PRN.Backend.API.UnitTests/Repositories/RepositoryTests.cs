using AutoFixture;
using EPR.PRN.Backend.API.Common.DTO;
using EPR.PRN.Backend.API.Repositories;
using EPR.PRN.Backend.API.Repositories.Interfaces;
using EPR.PRN.Backend.Data;
using EPR.PRN.Backend.Data.DataModels;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Diagnostics.CodeAnalysis;
using Moq.EntityFrameworkCore;

namespace EPR.PRN.Backend.API.UnitTests.Repositories;

[ExcludeFromCodeCoverage]
[TestClass]
public class RepositoryTests
{
    private SqliteConnection _connection;
    private DbContextOptions<EprContext> _contextOptions;
    private IFixture _fixture;
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
    public async Task GetSearchPrnsForOrganisation_Returns_EmptyResponse_WhenNoResults()
    {
        // Arrange
        var request = _fixture.Create<PaginatedRequestDto>();
        var orgId = Guid.NewGuid();
        _mockContext.Setup(c => c.Prn).Returns(new List<Eprn>().AsQueryable().BuildMockDbSet().Object);
        // Act
        var result = await _repository.GetSearchPrnsForOrganisation(orgId, request);
        // Assert
        result.Should().NotBeNull();
        result.TotalItems.Should().Be(0);
        result.Items.Should().BeEmpty();
    }
    [TestMethod]
    [Ignore]
    public async Task GetSearchPrnsForOrganisation_Applies_FilterBy()
    {
        // Arrange
        var request = _fixture.Create<PaginatedRequestDto>();
        var orgId = Guid.NewGuid();
        request.FilterBy = "someFilter";
        request.PageSize = 10;
        var prns = _fixture.CreateMany<Eprn>().AsQueryable();
        _mockContext.Setup(c => c.Prn).Returns(prns.BuildMockDbSet().Object);
        // Act
        var result = await _repository.GetSearchPrnsForOrganisation(orgId, request);
        // Assert
        result.Should().NotBeNull();
        result.Items.Should().AllSatisfy(prn => prn.OrganisationName.Should().Be("someFilter"));

    }

    [TestMethod]
    [Ignore]
    public async Task GetSearchPrnsForOrganisation_Applies_Search()
    {
        // Arrange
        var request = _fixture.Create<PaginatedRequestDto>();
        var orgId = Guid.NewGuid();
        request.Search = "searchTerm";
        var prns = _fixture.CreateMany<Eprn>().AsQueryable();
        _mockContext.Setup(c => c.Prn).Returns(prns.BuildMockDbSet().Object);
        // Act
        var result = await _repository.GetSearchPrnsForOrganisation(orgId, request);
        // Assert
        result.Should().NotBeNull();
        result.Items.Should().OnlyContain(prn => prn.PrnNumber.Contains("searchTerm"));
    }
    
    [TestMethod]
    public async Task GetSearchPrnsForOrganisation_Applies_Sorting()
    {
        // Arrange
        var request = _fixture.Create<PaginatedRequestDto>();
        var orgId = Guid.NewGuid();
        request.SortBy = "1";
        var prns = _fixture.CreateMany<Eprn>().AsQueryable();
        _mockContext.Setup(c => c.Prn).Returns(prns.BuildMockDbSet().Object);
        // Act
        var result = await _repository.GetSearchPrnsForOrganisation(orgId, request);
        // Assert
        result.Should().NotBeNull();
        result.Items.Should().BeInAscendingOrder(prn => prn.PrnNumber);
    }

    [TestMethod]
    [Ignore]
    public async Task GetSearchPrnsForOrganisation_Applies_Pagination()
    {
        // Arrange
        var request = _fixture.Create<PaginatedRequestDto>();
        var orgId = Guid.NewGuid();
        request.Page = 2;
        request.PageSize = 10;
        // Create 30 PRN entities with the same OrganisationId to ensure there are enough records for pagination
        var prns = _fixture.Build<Eprn>()
            .With(p => p.OrganisationId, orgId)
            .CreateMany(30)
            .AsQueryable();
        // Use Moq.EntityFrameworkCore to setup the mock context with async support
        _mockContext.Setup(c => c.Prn).ReturnsDbSet(prns);
        // Act
        var result = await _repository.GetSearchPrnsForOrganisation(orgId, request);
        // Assert
        result.Should().NotBeNull();
        result.Items.Count.Should().Be(10); // Expecting 10 items on the second page
        result.CurrentPage.Should().Be(2);
        result.PageSize.Should().Be(10);
    }
}
