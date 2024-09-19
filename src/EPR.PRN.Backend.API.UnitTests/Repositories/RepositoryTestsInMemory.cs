using EPR.PRN.Backend.API.Common.DTO;
using EPR.PRN.Backend.API.Repositories;
using EPR.PRN.Backend.Data;
using EPR.PRN.Backend.Data.DataModels;
using Microsoft.EntityFrameworkCore;
namespace EPR.PRN.Backend.API.UnitTests.Repositories;
[TestClass]
public class RepositoryTestsInMemory
{
    private EprContext _context;
    private Repository _repository;
    [TestInitialize]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<EprContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        _context = new EprContext(options);
        _repository = new Repository(_context);
        var orgId = Guid.NewGuid();
        var eprns = new List<Eprn>
        {
            CreateEprn(orgId, "searchTerm1", "Org1", "OrgName1", "Material1", "ProducerAgency1", "ReprocessorExporterAgency1", "IssuerReference1", "AccreditationNumber1", "ReprocessingSite1", 2023, 2023, "PackagingProducer1"),
            CreateEprn(orgId, "searchTerm2", "Org2", "OrgName2", "Material2", "ProducerAgency2", "ReprocessorExporterAgency2", "IssuerReference2", "AccreditationNumber2", "ReprocessingSite2", 2023, 2023, "PackagingProducer2")
        };
        _context.Prn.AddRange(eprns);
        _context.SaveChanges();
    }
    [TestCleanup]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
    [TestMethod]
    public async Task GetSearchPrnsForOrganisation_WithSearchTerm_FiltersResults()
    {
        // Arrange
        var orgId = _context.Prn.First().OrganisationId;
        var request = new PaginatedRequestDto
        {
            Page = 1,
            PageSize = 10,
            Search = "searchTerm"
        };
        // Act
        var result = await _repository.GetSearchPrnsForOrganisation(orgId, request);
        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(2, result.Items.Count);
        Assert.IsTrue(result.Items.Any(i => i.PrnNumber.Contains("searchTerm")));
    }
    [TestMethod]
    public async Task GetSearchPrnsForOrganisation_Returns_CorrectResults()
    {
        var orgId = _context.Prn.First().OrganisationId;
        var request = new PaginatedRequestDto
        {
            Page = 1,
            PageSize = 10,
            Search = "searchTerm1"
        };
        var result = await _repository.GetSearchPrnsForOrganisation(orgId, request);
        Assert.AreEqual(1, result.Items.Count);
        Assert.AreEqual("searchTerm1", result.Items.First().PrnNumber);
    }
    [TestMethod]
    public async Task GetSearchPrnsForOrganisation_Returns_Empty_When_NoMatch()
    {
        var orgId = _context.Prn.First().OrganisationId;
        var request = new PaginatedRequestDto
        {
            Page = 1,
            PageSize = 10,
            Search = "nonexistent"
        };
        var result = await _repository.GetSearchPrnsForOrganisation(orgId, request);
        Assert.AreEqual(0, result.Items.Count);
    }
    [TestMethod]
    public async Task GetSearchPrnsForOrganisation_Paginates_Correctly()
    {
        var orgId = _context.Prn.First().OrganisationId;
        var request = new PaginatedRequestDto
        {
            Page = 1,
            PageSize = 1
        };
        var result = await _repository.GetSearchPrnsForOrganisation(orgId, request);
        Assert.AreEqual(1, result.Items.Count);
        Assert.AreEqual(2, result.TotalItems);
    }
    [TestMethod]
    public async Task GetSearchPrnsForOrganisation_Filters_By_SearchTerm()
    {
        var orgId = _context.Prn.First().OrganisationId;
        var request = new PaginatedRequestDto
        {
            Page = 1,
            PageSize = 10,
            Search = "Org2"
        };
        var result = await _repository.GetSearchPrnsForOrganisation(orgId, request);
        Assert.AreEqual(1, result.Items.Count);
        Assert.AreEqual("Org2", result.Items.First().IssuedByOrg);
    }
    [TestMethod]
    public async Task GetSearchPrnsForOrganisation_ReturnsEmptyResponse_WhenNoResults()
    {
        // Arrange
        var orgId = Guid.NewGuid(); // Use a new GUID to ensure no matching records
        var request = new PaginatedRequestDto
        {
            Page = 1,
            PageSize = 10,
            Search = null,
            FilterBy = null,
            SortBy = null
        };
        // Act
        var result = await _repository.GetSearchPrnsForOrganisation(orgId, request);
        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(0, result.Items.Count);
        Assert.AreEqual(0, result.TotalItems);
        Assert.AreEqual(request.Page, result.CurrentPage);
        Assert.AreEqual(request.PageSize, result.PageSize);
        Assert.AreEqual(request.Search, result.SearchTerm);
        Assert.AreEqual(request.FilterBy, result.FilterBy);
        Assert.AreEqual(request.SortBy, result.SortBy);
    }
    private static Eprn CreateEprn(Guid orgId, string prnNumber, string issuedByOrg, string organisationName, string materialName, string producerAgency, string reprocessorExporterAgency, string issuerReference, string accreditationNumber, string reprocessingSite, int accreditationYear, int obligationYear, string packagingProducer)
    {
        return new Eprn
        {
            OrganisationId = orgId,
            PrnNumber = prnNumber,
            IssuedByOrg = issuedByOrg,
            ExternalId = Guid.NewGuid(),
            OrganisationName = organisationName,
            MaterialName = materialName,
            ProducerAgency = producerAgency,
            ReprocessorExporterAgency = reprocessorExporterAgency,
            IssuerReference = issuerReference,
            AccreditationNumber = accreditationNumber,
            ReprocessingSite = reprocessingSite,
            AccreditationYear = accreditationYear.ToString(),
            ObligationYear = obligationYear.ToString(),
            PackagingProducer = packagingProducer,
            IssueDate = DateTime.Now,
            CreatedOn = DateTime.Now
        };
    }
}
