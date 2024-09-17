using AutoFixture;
using EPR.PRN.Backend.API.Common.DTO;
using EPR.PRN.Backend.API.Helpers;
using EPR.PRN.Backend.API.Repositories.Interfaces;
using EPR.PRN.Backend.API.Services;
using EPR.PRN.Backend.Data.DataModels;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.UnitTests.Services;

[ExcludeFromCodeCoverage]
[TestClass]
public class PrnServiceTests
{
    private PrnService _systemUnderTest;
    private Mock<IRepository> _mockRepository;
    private static readonly IFixture _fixture = new Fixture();

    [TestInitialize]
    public void Init()
    {
        _mockRepository = new Mock<IRepository>();
        _systemUnderTest = new PrnService(_mockRepository.Object);
    }

    [TestMethod]
    public async Task GetPrnForOrganisationById_WithValidId_ReturnsExpectedDto()
    {
        var prnId = Guid.NewGuid();
        var orgId = Guid.NewGuid();
        var expectedPrn = _fixture.Create<Eprn>();

        _mockRepository.Setup(r => r.GetPrnForOrganisationById(orgId, prnId)).ReturnsAsync(expectedPrn);

        var result = await _systemUnderTest.GetPrnForOrganisationById(orgId, prnId);

        result.Should().BeEquivalentTo((PrnDto)expectedPrn);
    }

    [TestMethod]
    public async Task GetPrnForOrganisationById_WithInValidId_ReturnsNull()
    {
        var prnId = Guid.NewGuid();
        var orgId = Guid.NewGuid();

        _mockRepository.Setup(r => r.GetPrnForOrganisationById(orgId, prnId)).ReturnsAsync((Eprn)null);

        var result = await _systemUnderTest.GetPrnForOrganisationById(orgId, prnId);

        result.Should().BeNull();
    }

    [TestMethod]
    public async Task GetAllPrnByOrganisationId_WithInValidId_ReturnsNull()
    {
        var orgId = Guid.NewGuid();
        var expectedPrns = _fixture.CreateMany<Eprn>().ToList();
        _mockRepository.Setup(r => r.GetAllPrnByOrganisationId(orgId)).ReturnsAsync(expectedPrns);

        var result = await _systemUnderTest.GetAllPrnByOrganisationId(orgId);

        result.Should().BeEquivalentTo(expectedPrns);
    }

    [TestMethod]
    public async Task UpdateStatus_throwsNotFoundIfNoPrnRecordsForOrg()
    {
        var orgId = Guid.NewGuid();
        var prnUpdates = _fixture.CreateMany<PrnUpdateStatusDto>().ToList();

        _mockRepository.Setup(r => r.GetAllPrnByOrganisationId(orgId)).ReturnsAsync([]);

        await _systemUnderTest
            .Invoking(x => x.UpdateStatus(orgId, Guid.NewGuid(), prnUpdates))
            .Should()
            .ThrowAsync<NotFoundException>();
    }

    [TestMethod]
    public async Task UpdateStatus_throwsNotFoundIfPrnInUpdateDoesntExists()
    {
        var orgId = Guid.NewGuid();
        var availablePrns = _fixture.CreateMany<Eprn>().ToList();
        var prnUpdates = _fixture.CreateMany<PrnUpdateStatusDto>().ToList();

        _mockRepository.Setup(r => r.GetAllPrnByOrganisationId(orgId)).ReturnsAsync(availablePrns);

        await _systemUnderTest
            .Invoking(x => x.UpdateStatus(orgId, Guid.NewGuid(), prnUpdates))
            .Should()
            .ThrowAsync<NotFoundException>();
    }

    [TestMethod]
    public async Task UpdateStatus_throwsConflictExceptionIfStatusNotInAwaitingAcceptanceOrNotSame()
    {
        var orgId = Guid.NewGuid();
        var availablePrns = _fixture.CreateMany<Eprn>().ToList();
        var prnUpdates = _fixture.CreateMany<PrnUpdateStatusDto>().ToList();

        availablePrns[0].ExternalId = prnUpdates[0].PrnId;
        availablePrns[1].ExternalId = prnUpdates[1].PrnId;
        availablePrns[2].ExternalId = prnUpdates[2].PrnId;
        availablePrns[0].PrnStatusId = (int)EprnStatus.ACCEPTED;
        prnUpdates[0].Status = EprnStatus.REJECTED;

        _mockRepository.Setup(r => r.GetAllPrnByOrganisationId(orgId)).ReturnsAsync(availablePrns);

        await _systemUnderTest
            .Invoking(x => x.UpdateStatus(orgId, Guid.NewGuid(), prnUpdates))
            .Should()
            .ThrowAsync<ConflictException>();
    }

    [TestMethod]
    public async Task UpdateStatus_ShouldNotThrowsConflictExceptionIfStatusInAwaitingAcceptanceOrSame()
    {
        var orgId = Guid.NewGuid();
        var availablePrns = _fixture.CreateMany<Eprn>().ToList();
        var prnUpdates = _fixture.CreateMany<PrnUpdateStatusDto>().ToList();

        availablePrns[0].ExternalId = prnUpdates[0].PrnId;
        availablePrns[1].ExternalId = prnUpdates[1].PrnId;
        availablePrns[2].ExternalId = prnUpdates[2].PrnId;
        availablePrns[0].PrnStatusId = availablePrns[1].PrnStatusId = (int)EprnStatus.ACCEPTED;
        availablePrns[2].PrnStatusId = (int)EprnStatus.AWAITINGACCEPTANCE;

        prnUpdates[0].Status = prnUpdates[1].Status = prnUpdates[2].Status = EprnStatus.ACCEPTED;

        _mockRepository.Setup(r => r.GetAllPrnByOrganisationId(orgId)).ReturnsAsync(availablePrns);

        await _systemUnderTest
            .Invoking(x => x.UpdateStatus(orgId, Guid.NewGuid(), prnUpdates))
            .Should()
            .NotThrowAsync<ConflictException>();
    }

    [TestMethod]
    public async Task UpdateStatus_ShouldThrowsConflictExceptionIfSamePrnIsTriedToUpdateMultiple()
    {
        var orgId = Guid.NewGuid();
        var availablePrns = _fixture.CreateMany<Eprn>().ToList();
        var prnUpdates = _fixture.CreateMany<PrnUpdateStatusDto>().ToList();

        availablePrns[0].ExternalId = prnUpdates[0].PrnId = prnUpdates[1].PrnId;
        availablePrns[2].ExternalId = prnUpdates[2].PrnId;
        
        availablePrns[0].PrnStatusId = availablePrns[1].PrnStatusId =
        availablePrns[2].PrnStatusId = (int)EprnStatus.AWAITINGACCEPTANCE;

        prnUpdates[0].Status = EprnStatus.ACCEPTED; 
        prnUpdates[1].Status = EprnStatus.AWAITINGACCEPTANCE;
        prnUpdates[2].Status = EprnStatus.ACCEPTED;

        _mockRepository.Setup(r => r.GetAllPrnByOrganisationId(orgId)).ReturnsAsync(availablePrns);

        await _systemUnderTest
            .Invoking(x => x.UpdateStatus(orgId, Guid.NewGuid(), prnUpdates))
            .Should()
            .ThrowAsync<ConflictException>();
    }

    [TestMethod]
    public async Task UpdateStatus_CallsUpdateToDB()
    {
        var orgId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var availablePrns = _fixture.CreateMany<Eprn>().ToList();
        var prnUpdates = _fixture.CreateMany<PrnUpdateStatusDto>().ToList();

        availablePrns[0].ExternalId = prnUpdates[0].PrnId;
        availablePrns[1].ExternalId = prnUpdates[1].PrnId;
        availablePrns[2].ExternalId = prnUpdates[2].PrnId;
        
        availablePrns[0].PrnStatusId = availablePrns[1].PrnStatusId =
            availablePrns[2].PrnStatusId = (int)EprnStatus.AWAITINGACCEPTANCE;

        prnUpdates[0].Status = prnUpdates[1].Status =
            prnUpdates[2].Status =  EprnStatus.ACCEPTED;

        _mockRepository.Setup(r => r.GetAllPrnByOrganisationId(orgId)).ReturnsAsync(availablePrns);

        await _systemUnderTest.UpdateStatus(orgId, userId, prnUpdates);

        availablePrns.Should().AllSatisfy(x => x.PrnStatusId.Should().Be((int)EprnStatus.ACCEPTED));
        availablePrns.Should().AllSatisfy(x => x.LastUpdatedBy.Should().Be(userId));
        _mockRepository.Verify(x => x.SaveTransaction(It.IsAny<IDbContextTransaction>()), Times.Once());
        _mockRepository.Verify(x => x.AddPrnStatusHistory(It.IsAny<PrnStatusHistory>()), Times.Exactly(3));
    }
}
