using EPR.PRN.Backend.Data.DataModels.Accreditations;
using EPR.PRN.Backend.Data.Repositories.Accreditations;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EPR.PRN.Backend.Data.UnitTests.Repositories.Accreditations;

[TestClass]
public class OverseasAccreditationSiteRepositoryTests
{
    private DbContextOptions<EprAccreditationContext> _dbContextOptions;
    private EprAccreditationContext _context;
    private OverseasAccreditationSiteRepository _repository;

    [TestInitialize]
    public void Setup()
    {
        _dbContextOptions = new DbContextOptionsBuilder<EprAccreditationContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new EprAccreditationContext(_dbContextOptions);
        _repository = new OverseasAccreditationSiteRepository(_context);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [TestMethod]
    public async Task GetAllByAccreditationId_ReturnsEntities_WhenTheyExist()
    {
        // Arrange
        var accreditationId = Guid.NewGuid();
        var accreditation = new AccreditationEntity { Id = 1, ExternalId = accreditationId, OrganisationId = Guid.NewGuid() };
        var site = new OverseasAccreditationSite
        {
            ExternalId = accreditationId,
            AccreditationId = 1,
            OrganisationName = "Hun Manet Recycler Ltd",
            MeetConditionsOfExportId = 1,
            SiteCheckStatusId = 2
        };
        _context.Accreditations.Add(accreditation);
        _context.OverseasAccreditationSites.Add(site);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllByAccreditationId(accreditationId);

        // Assert
        result.Should().NotBeNull();
        result.Should().ContainSingle();
        result[0].ExternalId.Should().Be(accreditationId);
    }

    [TestMethod]
    public async Task GetAllByAccreditationId_ReturnsEmptyList_WhenNoneExist()
    {
        // Arrange
        var accreditationId = Guid.NewGuid();

        // Act
        var result = await _repository.GetAllByAccreditationId(accreditationId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [TestMethod]
    public async Task PostByAccreditationId_CreatesNewOverseasSite()
    {
        // Arrange
        var accreditationId = Guid.NewGuid();
        var accreditation = new AccreditationEntity { Id = 1, ExternalId = accreditationId, OrganisationId = Guid.NewGuid() };
        var site = new OverseasAccreditationSite
        {
            ExternalId = accreditationId,
            AccreditationId = 2,
            OrganisationName = "Hun Manet Recycler Ltd",
            MeetConditionsOfExportId = 2,
            SiteCheckStatusId = 2
        };
        _context.Accreditations.Add(accreditation);
        await _context.SaveChangesAsync();

        // Act
        await _repository.PostByAccreditationId(accreditationId, site);

        // Assert
        var newSite = await _context.OverseasAccreditationSites.Where(x => x.ExternalId == accreditationId).ToListAsync();
        newSite.Should().ContainSingle();
        newSite[0].AccreditationId.Should().Be(accreditation.Id);
        newSite[0].ExternalId.Should().Be(accreditationId);
    }
}
