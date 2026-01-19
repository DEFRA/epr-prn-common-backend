using AutoFixture;
using EPR.PRN.Backend.API.Common.Dto;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Dto;
using EPR.PRN.Backend.API.Helpers;
using EPR.PRN.Backend.API.Models;
using EPR.PRN.Backend.API.Repositories.Interfaces;
using EPR.PRN.Backend.API.Services;
using EPR.PRN.Backend.Data.DataModels;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
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
    private Mock<IConfiguration> _configurationMock;
    private static readonly IFixture _fixture = new Fixture();

    [TestInitialize]
    public void Init()
    {
        _mockRepository = new Mock<IRepository>();

        _mockLogger = new Mock<ILogger<PrnService>>();
        _configurationMock = new Mock<IConfiguration>();
        _configurationMock.Setup(c => c["LogPrefix"]).Returns("[EPR.PRN.Backend]");

        _systemUnderTest = new PrnService(_mockRepository.Object, _mockLogger.Object, _configurationMock.Object);
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

        result.Should().BeEquivalentTo(expectedPrns, o => o.Excluding(p => p.PrnStatusHistories));
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
    public async Task GetSyncStatuses_ReturnsList()
    {
        DateTime fromDate = DateTime.UtcNow;
        DateTime toDate = DateTime.UtcNow;
        List<PrnStatusSync> prnStatusSyncs = new List<PrnStatusSync>();
        _mockRepository.Setup(r => r.GetSyncStatuses(fromDate, toDate)).ReturnsAsync(prnStatusSyncs);

        var result = await _systemUnderTest.GetSyncStatuses(fromDate, toDate);

        result.Should().NotBeNull();
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
            new() { EvidenceNo = "123", EvidenceStatusCode = "Modified", AccreditationYear= "2014"},
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
            .ReturnsAsync(null as List<PrnUpdateStatus>);

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
            AccreditationYear = 2018,
            CancelledDate = DateTime.UtcNow.AddDays(-1),
            DecemberWaste = true,
            EvidenceMaterial = "Aluminium",
            EvidenceNo = Guid.NewGuid().ToString(),
            EvidenceStatusCode = EprnStatus.AWAITINGACCEPTANCE,
            EvidenceTonnes = 5000,
            IssueDate = DateTime.UtcNow.AddDays(-5),
            IssuedByNPWDCode = "NPWD367742",
            IssuedByOrgName = "ANB",
            IssuedToEPRId = Guid.NewGuid(),
            IssuedToNPWDCode = "NPWD557742",
            IssuedToOrgName = "ZNZ",
            IssuerNotes = "no notes",
            IssuerRef = "ANB-1123",
            MaterialOperationCode = "R-PLA",
            ObligationYear = 2025,
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
            AccreditationYear = 2018,
            CancelledDate = DateTime.UtcNow.AddDays(-1),
            DecemberWaste = true,
            EvidenceMaterial = "Aluminium",
            EvidenceNo = Guid.NewGuid().ToString(),
            EvidenceStatusCode = EprnStatus.AWAITINGACCEPTANCE,
            EvidenceTonnes = 5000,
            IssueDate = DateTime.UtcNow.AddDays(-5),
            IssuedByNPWDCode = "NPWD367742",
            IssuedByOrgName = "ANB",
            IssuedToEPRId = Guid.NewGuid(),
            IssuedToNPWDCode = "NPWD557742",
            IssuedToOrgName = "ZNZ",
            IssuerNotes = "no notes",
            IssuerRef = "ANB-1123",
            MaterialOperationCode = "R-PLA",
            ObligationYear = 2025,
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

    [TestMethod]
    [DataRow("EA26899222", false)]
    [DataRow("EX26899222", true)]
    [DataRow("SX26899222", true)]
    public async Task SavePrnDetails_SetsIsExportCorrectly_BeforeSaving(string evidenceNo, bool expectedIsExportValue)
    {
        Eprn createdEntity = null;

        _mockRepository.Setup(s => s.SavePrnDetails(It.IsAny<Eprn>())).Callback<Eprn>(x => createdEntity = x);

        var dto = new SavePrnDetailsRequest()
        {
            AccreditationNo = "ABC",
            AccreditationYear = 2018,
            CancelledDate = DateTime.UtcNow.AddDays(-1),
            DecemberWaste = true,
            EvidenceMaterial = "Aluminium",
            EvidenceNo = Guid.NewGuid().ToString(),
            EvidenceStatusCode = EprnStatus.AWAITINGACCEPTANCE,
            EvidenceTonnes = 5000,
            IssueDate = DateTime.UtcNow.AddDays(-5),
            IssuedByNPWDCode = "NPWD367742",
            IssuedByOrgName = "ANB",
            IssuedToEPRId = Guid.NewGuid(),
            IssuedToNPWDCode = "NPWD557742",
            IssuedToOrgName = "ZNZ",
            IssuerNotes = "no notes",
            IssuerRef = "ANB-1123",
            MaterialOperationCode = "R-PLA",
            ObligationYear = 2025,
            PrnSignatory = "Pat Anderson",
            PrnSignatoryPosition = "Director",
            ProducerAgency = "TTL",
            RecoveryProcessCode = "N11",
            ReprocessorAgency = "BEX",
            StatusDate = DateTime.UtcNow,
            CreatedByUser = "UserTest",
        };

        // OVerride Evidence no with input argument from Test data
        dto.EvidenceNo = evidenceNo;

        await _systemUnderTest.SavePrnDetails(dto);
        _mockRepository.Verify(x => x.SavePrnDetails(It.IsAny<Eprn>()), Times.Once());

        createdEntity.IsExport.Should().Be(expectedIsExportValue);
    }

    [TestMethod]
    public async Task SavePrnDetails_CheckIsExport_ReturningFalse()
    {
        Eprn createdEntity = null;
        string evidenceNo = string.Empty;
        bool expectedIsExportValue = false;

        _mockRepository.Setup(s => s.SavePrnDetails(It.IsAny<Eprn>())).Callback<Eprn>(x => createdEntity = x);

        var dto = new SavePrnDetailsRequest()
        {
            AccreditationNo = "ABC",
            AccreditationYear = 2018,
            CancelledDate = DateTime.UtcNow.AddDays(-1),
            DecemberWaste = true,
            EvidenceMaterial = "Aluminium",
            EvidenceNo = string.Empty,
            EvidenceStatusCode = EprnStatus.AWAITINGACCEPTANCE,
            EvidenceTonnes = 5000,
            IssueDate = DateTime.UtcNow.AddDays(-5),
            IssuedByNPWDCode = "NPWD367742",
            IssuedByOrgName = "ANB",
            IssuedToEPRId = Guid.NewGuid(),
            IssuedToNPWDCode = "NPWD557742",
            IssuedToOrgName = "ZNZ",
            IssuerNotes = "no notes",
            IssuerRef = "ANB-1123",
            MaterialOperationCode = "R-PLA",
            ObligationYear = 2025,
            PrnSignatory = "Pat Anderson",
            PrnSignatoryPosition = "Director",
            ProducerAgency = "TTL",
            RecoveryProcessCode = "N11",
            ReprocessorAgency = "BEX",
            StatusDate = DateTime.UtcNow,
            CreatedByUser = "UserTest",
        };

        // OVerride Evidence no with input argument from Test data
        dto.EvidenceNo = evidenceNo;

        await _systemUnderTest.SavePrnDetails(dto);
        _mockRepository.Verify(x => x.SavePrnDetails(It.IsAny<Eprn>()), Times.Once());

        createdEntity.IsExport.Should().Be(expectedIsExportValue);
    }

    [TestMethod]
    public async Task SavePrnDetails_SetsDatesCorrectly_BeforeSaving()
    {
        Eprn prn = null;
        DateTime? issuedDate = DateTime.UtcNow.AddDays(-5);
        DateTime statusUpdatedDate = DateTime.UtcNow.AddDays(-2);

        _mockRepository.Setup(s => s.SavePrnDetails(It.IsAny<Eprn>())).Callback<Eprn>(x => prn = x);

        var dto = _fixture.Create<SavePrnDetailsRequest>();
        dto.EvidenceStatusCode = EprnStatus.AWAITINGACCEPTANCE;
        dto.IssueDate = issuedDate;
        dto.StatusDate = statusUpdatedDate;

        await _systemUnderTest.SavePrnDetails(dto);

        prn.CreatedOn.Should().Be(default);
        prn.IssueDate.Should().Be(issuedDate);
        prn.LastUpdatedDate.Should().Be(default);
        prn.StatusUpdatedOn.Should().Be(statusUpdatedDate);
    }

    [TestMethod]
    public async Task SavePrnDetails_WhenCancelled_SetsDatesCorrectly_BeforeSaving()
    {
        Eprn prn = null;
        DateTime? issuedDate = DateTime.UtcNow.AddDays(-5);
        DateTime statusUpdatedDate = DateTime.UtcNow.AddDays(-2);

        _mockRepository.Setup(s => s.SavePrnDetails(It.IsAny<Eprn>())).Callback<Eprn>(x => prn = x);

        var dto = _fixture.Create<SavePrnDetailsRequest>();
        dto.CancelledDate = statusUpdatedDate.AddDays(1);
        dto.EvidenceStatusCode = EprnStatus.CANCELLED;
        dto.IssueDate = issuedDate;
        dto.StatusDate = statusUpdatedDate;
 
        await _systemUnderTest.SavePrnDetails(dto);

        prn.CreatedOn.Should().Be(default);
        prn.IssueDate.Should().Be(issuedDate);
        prn.LastUpdatedDate.Should().Be(default);
        prn.StatusUpdatedOn.Should().Be(statusUpdatedDate.AddDays(1));
    }

    [TestMethod]
    public async Task InsertPeprNpwdSyncPrns_throwsNotFoundIfNoPrnRecordsForEvidence()
    {
        var syncPrns = _fixture.CreateMany<InsertSyncedPrn>().ToList();

        _mockRepository.Setup(r => r.GetPrnsForPrnNumbers(It.IsAny<List<string>>())).ReturnsAsync([]);

        await _systemUnderTest
            .Invoking(x => x.InsertPeprNpwdSyncPrns(syncPrns))
            .Should()
            .ThrowAsync<NotFoundException>();
    }

    [TestMethod]
    public async Task InsertPeprNpwdSyncPrns_CallsDBWithCorrectData()
    {
        var expectedPeprSync = new List<PEprNpwdSync>();
        var syncPrns = _fixture.CreateMany<InsertSyncedPrn>().ToList();
        var prns = _fixture.CreateMany<Eprn>().ToList();
        prns[0].PrnNumber = syncPrns[0].EvidenceNo;
        prns[1].PrnNumber = syncPrns[1].EvidenceNo;
        prns[2].PrnNumber = syncPrns[2].EvidenceNo;

        _mockRepository.Setup(r => r.GetPrnsForPrnNumbers(It.IsAny<List<string>>())).ReturnsAsync(prns);
        _mockRepository.Setup(r => r.InsertPeprNpwdSyncPrns(It.IsAny<List<PEprNpwdSync>>()))
            .Callback<List<PEprNpwdSync>>(p => expectedPeprSync = p);

        await _systemUnderTest.InsertPeprNpwdSyncPrns(syncPrns);

        _mockRepository.Verify(x => x.InsertPeprNpwdSyncPrns(It.IsAny<List<PEprNpwdSync>>()), Times.Once());

        expectedPeprSync.Count.Should().Be(3);
        expectedPeprSync.Select(p => p.PRNId).Should().BeEquivalentTo(prns.Select(p => p.Id));
        expectedPeprSync.Select(p => p.PRNStatusId).Should().BeEquivalentTo(syncPrns.Select(p => (int)p.EvidenceStatus));
    }

    [TestMethod]
    public async Task UpdateStatus_WithObligationYear_SetsObligationYearOnPrnAndHistory()
    {
        // Arrange
        var orgId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var availablePrns = _fixture.CreateMany<Eprn>(1).ToList();
        var prnUpdates = new List<PrnUpdateStatusDto>
        {
            new PrnUpdateStatusDto
            {
                PrnId = availablePrns[0].ExternalId,
                Status = EprnStatus.ACCEPTED,
                ObligationYear = "2025"
            }
        };

        availablePrns[0].PrnStatusId = (int)EprnStatus.AWAITINGACCEPTANCE;
        availablePrns[0].ObligationYear = "2024";

        PrnStatusHistory capturedHistory = null;
        _mockRepository.Setup(r => r.GetAllPrnByOrganisationId(orgId)).ReturnsAsync(availablePrns);
        _mockRepository.Setup(r => r.AddPrnStatusHistory(It.IsAny<PrnStatusHistory>()))
            .Callback<PrnStatusHistory>(h => capturedHistory = h);

        // Act
        await _systemUnderTest.UpdateStatus(orgId, userId, prnUpdates);

        // Assert
        availablePrns[0].ObligationYear.Should().Be("2025");
        capturedHistory.Should().NotBeNull();
        capturedHistory.ObligationYear.Should().Be("2025");
    }

    [TestMethod]
    public async Task UpdateStatus_WithNullObligationYear_DoesNotChangePrnObligationYear()
    {
        // Arrange
        var orgId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var availablePrns = _fixture.CreateMany<Eprn>(1).ToList();
        var prnUpdates = new List<PrnUpdateStatusDto>
        {
            new PrnUpdateStatusDto
            {
                PrnId = availablePrns[0].ExternalId,
                Status = EprnStatus.ACCEPTED,
                ObligationYear = null
            }
        };

        availablePrns[0].PrnStatusId = (int)EprnStatus.AWAITINGACCEPTANCE;
        availablePrns[0].ObligationYear = "2024";

        PrnStatusHistory capturedHistory = null;
        _mockRepository.Setup(r => r.GetAllPrnByOrganisationId(orgId)).ReturnsAsync(availablePrns);
        _mockRepository.Setup(r => r.AddPrnStatusHistory(It.IsAny<PrnStatusHistory>()))
            .Callback<PrnStatusHistory>(h => capturedHistory = h);

        // Act
        await _systemUnderTest.UpdateStatus(orgId, userId, prnUpdates);

        // Assert
        availablePrns[0].ObligationYear.Should().Be("2024");
        capturedHistory.Should().NotBeNull();
        capturedHistory.ObligationYear.Should().BeNull();
    }

    [TestMethod]
    public async Task UpdateStatus_WithEmptyObligationYear_DoesNotChangePrnObligationYear()
    {
        // Arrange
        var orgId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var availablePrns = _fixture.CreateMany<Eprn>(1).ToList();
        var prnUpdates = new List<PrnUpdateStatusDto>
        {
            new PrnUpdateStatusDto
            {
                PrnId = availablePrns[0].ExternalId,
                Status = EprnStatus.ACCEPTED,
                ObligationYear = ""
            }
        };

        availablePrns[0].PrnStatusId = (int)EprnStatus.AWAITINGACCEPTANCE;
        availablePrns[0].ObligationYear = "2024";

        PrnStatusHistory capturedHistory = null;
        _mockRepository.Setup(r => r.GetAllPrnByOrganisationId(orgId)).ReturnsAsync(availablePrns);
        _mockRepository.Setup(r => r.AddPrnStatusHistory(It.IsAny<PrnStatusHistory>()))
            .Callback<PrnStatusHistory>(h => capturedHistory = h);

        // Act
        await _systemUnderTest.UpdateStatus(orgId, userId, prnUpdates);

        // Assert
        availablePrns[0].ObligationYear.Should().Be("2024");
        capturedHistory.Should().NotBeNull();
        capturedHistory.ObligationYear.Should().Be("");
    }

    [TestMethod]
    public async Task UpdateStatus_MultipleUpdates_SetsObligationYearCorrectlyPerPrn()
    {
        // Arrange
        var orgId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var availablePrns = _fixture.CreateMany<Eprn>(3).ToList();
        var prnUpdates = new List<PrnUpdateStatusDto>
        {
            new PrnUpdateStatusDto
            {
                PrnId = availablePrns[0].ExternalId,
                Status = EprnStatus.ACCEPTED,
                ObligationYear = "2025"
            },
            new PrnUpdateStatusDto
            {
                PrnId = availablePrns[1].ExternalId,
                Status = EprnStatus.ACCEPTED,
                ObligationYear = null
            },
            new PrnUpdateStatusDto
            {
                PrnId = availablePrns[2].ExternalId,
                Status = EprnStatus.ACCEPTED,
                ObligationYear = "2026"
            }
        };

        availablePrns[0].PrnStatusId = (int)EprnStatus.AWAITINGACCEPTANCE;
        availablePrns[0].ObligationYear = "2024";
        availablePrns[1].PrnStatusId = (int)EprnStatus.AWAITINGACCEPTANCE;
        availablePrns[1].ObligationYear = "2024";
        availablePrns[2].PrnStatusId = (int)EprnStatus.AWAITINGACCEPTANCE;
        availablePrns[2].ObligationYear = "2024";

        var capturedHistories = new List<PrnStatusHistory>();
        _mockRepository.Setup(r => r.GetAllPrnByOrganisationId(orgId)).ReturnsAsync(availablePrns);
        _mockRepository.Setup(r => r.AddPrnStatusHistory(It.IsAny<PrnStatusHistory>()))
            .Callback<PrnStatusHistory>(h => capturedHistories.Add(h));

        // Act
        await _systemUnderTest.UpdateStatus(orgId, userId, prnUpdates);

        // Assert
        availablePrns[0].ObligationYear.Should().Be("2025");
        availablePrns[1].ObligationYear.Should().Be("2024");
        availablePrns[2].ObligationYear.Should().Be("2026");

        capturedHistories.Count.Should().Be(3);
        capturedHistories[0].ObligationYear.Should().Be("2025");
        capturedHistories[1].ObligationYear.Should().BeNull();
        capturedHistories[2].ObligationYear.Should().Be("2026");
    }
}
