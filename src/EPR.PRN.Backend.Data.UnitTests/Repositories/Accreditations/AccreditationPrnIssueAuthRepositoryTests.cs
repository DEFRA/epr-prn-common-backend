using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EPR.PRN.Backend.Data.DataModels.Accreditations;
using EPR.PRN.Backend.Data.Repositories.Accreditations;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EPR.PRN.Backend.Data.UnitTests.Repositories.Accreditations;

[TestClass]
public class AccreditationPrnIssueAuthRepositoryTests
{
    private DbContextOptions<EprAccreditationContext> _dbContextOptions;
    private EprAccreditationContext _context;
    private AccreditationPrnIssueAuthRepository _repository;

    [TestInitialize]
    public void Setup()
    {
        _dbContextOptions = new DbContextOptionsBuilder<EprAccreditationContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new EprAccreditationContext(_dbContextOptions);
        _repository = new AccreditationPrnIssueAuthRepository(_context);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [TestMethod]
    public async Task GetByAccreditationId_ReturnsEntities_WhenTheyExist()
    {
        // Arrange
        var accreditationId = Guid.NewGuid();
        var accreditation = new Accreditation { Id = 1, ExternalId = accreditationId, OrganisationId = Guid.NewGuid() };
        var auth = new AccreditationPrnIssueAuth
        {
            ExternalId = Guid.NewGuid(),
            AccreditationExternalId = accreditationId,
            AccreditationId = 1,
            PersonExternalId = Guid.NewGuid()
        };
        _context.Accreditations.Add(accreditation);
        _context.AccreditationPrnIssueAuths.Add(auth);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByAccreditationId(accreditationId);

        // Assert
        result.Should().NotBeNull();
        result.Should().ContainSingle();
        result![0].AccreditationExternalId.Should().Be(accreditationId);
    }

    [TestMethod]
    public async Task GetByAccreditationId_ReturnsEmptyList_WhenNoneExist()
    {
        // Arrange
        var accreditationId = Guid.NewGuid();

        // Act
        var result = await _repository.GetByAccreditationId(accreditationId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [TestMethod]
    public async Task ReplaceAllByAccreditationId_RemovesOldAndAddsNew()
    {
        // Arrange
        var accreditationId = Guid.NewGuid();
        var accreditation = new Accreditation { Id = 2, ExternalId = accreditationId, OrganisationId = Guid.NewGuid() };
        var oldAuth = new AccreditationPrnIssueAuth
        {
            ExternalId = Guid.NewGuid(),
            AccreditationExternalId = accreditationId,
            AccreditationId = 2,
            PersonExternalId = Guid.NewGuid()
        };
        _context.Accreditations.Add(accreditation);
        _context.AccreditationPrnIssueAuths.Add(oldAuth);
        await _context.SaveChangesAsync();

        var newAuth = new AccreditationPrnIssueAuth
        {
            PersonExternalId = Guid.NewGuid()
        };
        var newList = new List<AccreditationPrnIssueAuth> { newAuth };

        // Act
        await _repository.ReplaceAllByAccreditationId(accreditationId, newList);

        // Assert
        var all = await _context.AccreditationPrnIssueAuths.Where(x => x.AccreditationExternalId == accreditationId).ToListAsync();
        all.Should().ContainSingle();
        all[0].PersonExternalId.Should().Be(newAuth.PersonExternalId);
        all[0].AccreditationId.Should().Be(accreditation.Id);
        all[0].AccreditationExternalId.Should().Be(accreditationId);
    }
}
