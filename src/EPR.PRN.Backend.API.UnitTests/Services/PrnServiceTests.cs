using AutoFixture.MSTest;
using EPR.PRN.Backend.API.Common.DTO;
using EPR.PRN.Backend.API.Helpers;
using EPR.PRN.Backend.API.Repositories.Interfaces;
using EPR.PRN.Backend.API.Services;
using EPR.PRN.Backend.Data.DataModels;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;

namespace EPR.PRN.Backend.API.UnitTests.Services
{
    [TestClass]
    public class PrnServiceTests
    {
        private PrnService _systemUnderTest;
        private Mock<IRepository> _mockRepository;

        [TestInitialize]
        public void Init()
        {
            _mockRepository = new Mock<IRepository>();
            _systemUnderTest = new PrnService(_mockRepository.Object);
        }

        [TestMethod]
        [AutoData]
        public async Task GetPrnForOrganisationById_WithValidId_ReturnsExpectedDto(Guid prnId, Guid orgId, EPRN expectedPrn)
        {
            _mockRepository.Setup(r => r.GetPrnForOrganisationById(orgId, prnId)).ReturnsAsync(expectedPrn);

            var result = await _systemUnderTest.GetPrnForOrganisationById(orgId, prnId);

            result.Should().BeEquivalentTo((PrnDto)expectedPrn);
        }

        [TestMethod]
        [AutoData]
        public async Task GetPrnForOrganisationById_WithInValidId_ReturnsNull(Guid prnId, Guid orgId)
        {
            _mockRepository.Setup(r => r.GetPrnForOrganisationById(orgId, prnId)).ReturnsAsync((EPRN)null);

            var result = await _systemUnderTest.GetPrnForOrganisationById(orgId, prnId);

            result.Should().BeNull();
        }

        [TestMethod]
        [AutoData]
        public async Task GetAllPrnByOrganisationId_WithInValidId_ReturnsNull(Guid orgId, List<EPRN> expectedPrns)
        {
            _mockRepository.Setup(r => r.GetAllPrnByOrganisationId(orgId)).ReturnsAsync(expectedPrns);

            var result = await _systemUnderTest.GetAllPrnByOrganisationId(orgId);

            result.Should().BeEquivalentTo(expectedPrns);
        }

        [TestMethod]
        [AutoData]
        public async Task UpdateStatus_throwsNotFoundIfNoPrnRecordsForOrg(Guid orgId, List<PrnUpdateStatusDto> prnUpdates)
        {
            _mockRepository.Setup(r => r.GetAllPrnByOrganisationId(orgId)).ReturnsAsync([]);

            await _systemUnderTest
                .Invoking(x => x.UpdateStatus(orgId, Guid.NewGuid(), prnUpdates))
                .Should()
                .ThrowAsync<NotFoundException>();
        }

        [TestMethod]
        [AutoData]
        public async Task UpdateStatus_throwsNotFoundIfPrnInUpdateDoesntExists(Guid orgId, List<EPRN> availablePrns,List<PrnUpdateStatusDto> prnUpdates)
        {
            _mockRepository.Setup(r => r.GetAllPrnByOrganisationId(orgId)).ReturnsAsync(availablePrns);

            await _systemUnderTest
                .Invoking(x => x.UpdateStatus(orgId, Guid.NewGuid(), prnUpdates))
                .Should()
                .ThrowAsync<NotFoundException>();
        }

        [TestMethod]
        [AutoData]
        public async Task UpdateStatus_throwsConflictExceptionIfStatusNotInAwaitingAcceptanceOrNotSame(Guid orgId, List<EPRN> availablePrns, List<PrnUpdateStatusDto> prnUpdates)
        {
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
        [AutoData]
        public async Task UpdateStatus_ShouldNotThrowsConflictExceptionIfStatusInAwaitingAcceptanceOrSame(Guid orgId, List<EPRN> availablePrns, List<PrnUpdateStatusDto> prnUpdates)
        {
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
        [AutoData]
        public async Task UpdateStatus_ShouldThrowsConflictExceptionIfSamePrnIsTriedToUpdateMultiple(Guid orgId, List<EPRN> availablePrns, List<PrnUpdateStatusDto> prnUpdates)
        {
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
        [AutoData]
        public async Task UpdateStatus_CallsUpdateToDB(Guid orgId, Guid userId, List<EPRN> availablePrns, List<PrnUpdateStatusDto> prnUpdates)
        {
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
}
