using AutoFixture;
using Castle.Core.Logging;
using EPR.PRN.Backend.API.Common.DTO;
using EPR.PRN.Backend.API.Helpers;
using EPR.PRN.Backend.API.Repositories.Interfaces;
using EPR.PRN.Backend.API.Services;
using EPR.PRN.Backend.Data;
using EPR.PRN.Backend.Data.DataModels;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Moq;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.UnitTests.Services;

[ExcludeFromCodeCoverage]
[TestClass]
public class PrnServiceTests
{
    private PrnService _systemUnderTest;
    private Mock<IRepository> _mockRepository;
    private Mock<ILogger<PrnService>> _mockLogger;
    private static readonly IFixture _fixture = new Fixture();

    [TestInitialize]
    public void Init()
    {
        _mockRepository = new Mock<IRepository>();
        _mockLogger = new Mock<ILogger<PrnService>>();
        _systemUnderTest = new PrnService(_mockRepository.Object, _mockLogger.Object);
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
            prnUpdates[2].Status = EprnStatus.ACCEPTED;

        _mockRepository.Setup(r => r.GetAllPrnByOrganisationId(orgId)).ReturnsAsync(availablePrns);

        await _systemUnderTest.UpdateStatus(orgId, userId, prnUpdates);

        availablePrns.Should().AllSatisfy(x => x.PrnStatusId.Should().Be((int)EprnStatus.ACCEPTED));
        availablePrns.Should().AllSatisfy(x => x.LastUpdatedBy.Should().Be(userId));
        availablePrns.Should().AllSatisfy(x => x.StatusUpdatedOn.Should().BeCloseTo(DateTime.UtcNow, new TimeSpan(0, 10, 0)));
        availablePrns.Should().AllSatisfy(x => x.StatusUpdatedOn.Should().BeCloseTo(DateTime.UtcNow, new TimeSpan(0, 10, 0)));
        _mockRepository.Verify(x => x.SaveTransaction(It.IsAny<IDbContextTransaction>()), Times.Once());
        _mockRepository.Verify(x => x.AddPrnStatusHistory(It.IsAny<PrnStatusHistory>()), Times.Exactly(3));
    }

    [TestMethod]
    public async Task GetSearchPrnsForOrganisation_ReturnsRepsoneGotFromRepo()
    {
        var orgId = Guid.NewGuid();
        var request = _fixture.Create<PaginatedRequestDto>();
        var repoResponse = _fixture.Create<PaginatedResponseDto<PrnDto>>();

        _mockRepository.Setup(s => s.GetSearchPrnsForOrganisation(orgId, request)).ReturnsAsync(repoResponse);

        var result = await _systemUnderTest.GetSearchPrnsForOrganisation(orgId, request);
        result.Should().Be(repoResponse);
    }

    [TestMethod]
    public async Task GetModifiedPrnsbyDate_ReturnsModifiedPrns_WhenDataExists()
    {
        // Arrange
        var fromDate = DateTime.UtcNow.AddDays(-7);
        var toDate = DateTime.UtcNow;
        var mockPrns = new List<PrnUpdateStatus>
    {
        new() { EvidenceNo = "123", EvidenceStatusCode = "Modified", AccreditationYear= "2014" },
        new() { EvidenceNo = "456", EvidenceStatusCode = "Unchanged", AccreditationYear= "2014" }
    };

        _mockRepository
            .Setup(repo => repo.GetModifiedPrnsbyDate(fromDate, toDate))
            .ReturnsAsync(mockPrns);

        // Act
        var result = await _systemUnderTest.GetModifiedPrnsbyDate(fromDate, toDate);

        // Assert
        Assert.IsNotNull(result);
        CollectionAssert.AreEqual(mockPrns, result);
        _mockRepository.Verify(repo => repo.GetModifiedPrnsbyDate(fromDate, toDate), Times.Once);
    }

    [TestMethod]
    public async Task GetModifiedPrnsbyDate_ReturnsNull_WhenNoDataExists()
    {
        // Arrange
        var fromDate = DateTime.UtcNow.AddDays(-7);
        var toDate = DateTime.UtcNow;

        _mockRepository
            .Setup(repo => repo.GetModifiedPrnsbyDate(fromDate, toDate))
            .ReturnsAsync((List<PrnUpdateStatus>?)null);

        // Act
        var result = await _systemUnderTest.GetModifiedPrnsbyDate(fromDate, toDate);

        // Assert
        Assert.IsNull(result);
        _mockRepository.Verify(repo => repo.GetModifiedPrnsbyDate(fromDate, toDate), Times.Once);
    }


    [TestMethod]
    public async Task SavePrnDetails_ReturnsWithoutError_OnSuccessfullySave()
    {
        _mockRepository.Setup(s => s.SavePrnDetails(It.IsAny<Eprn>())).Returns(Task.CompletedTask);

        var dto = new SavePrnDetailsRequest()
        {
            AccreditationNo = "ABC",
            AccreditationYear = "2018",
            CancelledDate = DateTime.UtcNow.AddDays(-1),
            DecemberWaste = true,
            EvidenceMaterial = "Aluminium",
            EvidenceNo = Guid.NewGuid().ToString(),
            EvidenceStatusCode = Common.Enums.PrnStatus.AwaitingAcceptance,
            EvidenceTonnes = 5000,
            ExternalId = Guid.NewGuid(),
            IssueDate = DateTime.UtcNow.AddDays(-5),
            IssuedByNPWDCode = Guid.NewGuid(),
            IssuedByOrgName = "ANB",
            IssuedToEPRId = Guid.NewGuid(),
            IssuedToNPWDCode = Guid.NewGuid(),
            IssuedToOrgName = "ZNZ",
            IssuerNotes = "no notes",
            IssuerRef = "ANB-1123",
            MaterialOperationCode = Guid.NewGuid(),
            ObligationYear = "2025",
            PrnSignatory = "Pat Anderson",
            PrnSignatoryPosition = "Director",
            ProducerAgency = "TTL",
            RecoveryProcessCode = "N11",
            ReprocessorAgency = "BEX",
            StatusDate = DateTime.UtcNow,
        };

        await _systemUnderTest.SavePrnDetails(dto);
        _mockRepository.Verify(x => x.SavePrnDetails(It.IsAny<Eprn>()), Times.Once());
    }

    [TestMethod]
    public async Task SavePrnDetails_ThrowsException_OnRepositoryError()
    {
        _mockRepository.Setup(s => s.SavePrnDetails(It.IsAny<Eprn>())).Throws<Exception>();

        var dto = new SavePrnDetailsRequest()
        {
            AccreditationNo = "ABC",
            AccreditationYear = "2018",
            CancelledDate = DateTime.UtcNow.AddDays(-1),
            DecemberWaste = true,
            EvidenceMaterial = "Aluminium",
            EvidenceNo = Guid.NewGuid().ToString(),
            EvidenceStatusCode = Common.Enums.PrnStatus.AwaitingAcceptance,
            EvidenceTonnes = 5000,
            ExternalId = Guid.NewGuid(),
            IssueDate = DateTime.UtcNow.AddDays(-5),
            IssuedByNPWDCode = Guid.NewGuid(),
            IssuedByOrgName = "ANB",
            IssuedToEPRId = Guid.NewGuid(),
            IssuedToNPWDCode = Guid.NewGuid(),
            IssuedToOrgName = "ZNZ",
            IssuerNotes = "no notes",
            IssuerRef = "ANB-1123",
            MaterialOperationCode = Guid.NewGuid(),
            ObligationYear = "2025",
            PrnSignatory = "Pat Anderson",
            PrnSignatoryPosition = "Director",
            ProducerAgency = "TTL",
            RecoveryProcessCode = "N11",
            ReprocessorAgency = "BEX",
            StatusDate = DateTime.UtcNow,
        };

        var call = () => _systemUnderTest.SavePrnDetails(dto);
        await call.Should().ThrowAsync<Exception>();
        _mockRepository.Verify(x => x.SavePrnDetails(It.IsAny<Eprn>()), Times.Once());
    }
}
