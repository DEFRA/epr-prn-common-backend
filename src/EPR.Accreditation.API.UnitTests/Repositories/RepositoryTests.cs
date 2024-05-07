namespace EPR.Accreditation.API.UnitTests.Repositories
{
    using System;
    using System.Net.Sockets;
    using System.Reflection.Metadata;
    using System.Threading.Tasks;
    using AutoFixture;
    using AutoMapper;
    using EPR.Accreditation.API.Common.Data;
    using EPR.Accreditation.API.Common.Data.DataModels;
    using EPR.Accreditation.API.Common.Dtos;
    using EPR.Accreditation.API.Common.Enums;
    using EPR.Accreditation.API.Repositories;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Moq.EntityFrameworkCore;

    [TestClass]
    public class RepositoryTests
    {
        private Repository TestRepository { get; set; }

        private IMapper Mapper { get; set; }

        private Mock<AccreditationContext> MockAccreditationContext { get; set; }

        /// <summary>
        /// Gets or sets some test data to go in the mock database.
        /// </summary>
        private IList<PackageRecyclingNote> TestPrnData { get; set; }

        private IList<PrnStatusHistory> TestHistoryData { get; set; }

        [TestInitialize]
        public void SetUp()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PackageRecyclingNote, PackageRecyclingNoteResponse>();
                cfg.CreateMap<PackageRecyclingNoteRequest, PackageRecyclingNote>();
                cfg.CreateMap<PrnStatusHistoryRequest, PrnStatusHistory>();
            });

            this.Mapper = new Mapper(config);

            this.MockAccreditationContext = new Mock<AccreditationContext>();

            this.GenerateFakePrnData();
            this.MockAccreditationContext.Setup(mock => mock.PackageRecyclingNote)
                .ReturnsDbSet(this.TestPrnData);

            this.GenerateFakeHistoryData();
            this.MockAccreditationContext.Setup(mock => mock.PrnStatusHistories)
                .ReturnsDbSet(this.TestHistoryData);

            this.TestRepository = new Repository(this.Mapper, this.MockAccreditationContext.Object);
        }

        private void GenerateFakePrnData()
        {
            var fix = new Fixture()
                .Build<PackageRecyclingNote>()
                .Without(f => f.Site)
                .Without(f => f.PrnStatus)
                .Without(f => f.PrnType);

            this.TestPrnData = new List<PackageRecyclingNote>();
            this.TestPrnData.AddMany(() => fix.Create(), 10);
        }

        private void GenerateFakeHistoryData()
        {
            var fix = new Fixture()
                .Build<PrnStatusHistory>()
                .Without(f => f.Prn)
                .Without(f => f.PrnStatus);

            this.TestHistoryData = new List<PrnStatusHistory>();
            this.TestHistoryData.AddMany(() => fix.Create(), 10);
        }

        [TestMethod]
        public async Task AddPackageRecyclingNote_Success()
        {
            // Arrange
            var newPrn = new Fixture()
                .Build<PackageRecyclingNoteRequest>()
                .With(f => f.OperatorTypeId, OperatorType.Exporter)
                .Create();

            // Act
            var result = await this.TestRepository.AddPackageRecyclingNote(newPrn);

            // Assert
            this.MockAccreditationContext.Verify(
                mock => mock.PackageRecyclingNote.Add(
                It.Is<PackageRecyclingNote>(record =>
                    (int)record.OperatorTypeId == (int)newPrn.OperatorTypeId &&
                    record.AccreditationId == newPrn.AccreditationId &&
                    record.AccreditationReference == newPrn.AccreditationReference &&
                    record.Note == newPrn.Note)),
                Times.Once());
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Checks that the AddPackageRecyclingNote method throws an ArgumentException if the new PRN data isn't supplied.
        /// </summary>
        /// <returns>A <see cref="Task"/>.</returns>
        [TestMethod]
        public async Task AddPackageRecyclingNote_NullPrn()
        {
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(()
                => this.TestRepository.AddPackageRecyclingNote(default));
        }

        [TestMethod]
        public async Task UpdatePrn_Success()
        {
            // Arrange
            var prnId = this.TestPrnData[0].ExternalId;

            var updatedData = new Fixture()
                .Build<PrnUpdateRequest>()
                .Create();

            // Act
            await this.TestRepository.UpdatePrn(prnId, updatedData);

            // Assert
            this.MockAccreditationContext.Verify(
               mock => mock.PackageRecyclingNote.Update(
               It.Is<PackageRecyclingNote>(record =>
                   record.ExternalId == prnId &&
                   record.AccreditationId == updatedData.Prn.AccreditationId &&
                   record.AccreditationReference == updatedData.Prn.AccreditationReference &&
                   record.Note == updatedData.Prn.Note)),
               Times.Once());

            this.MockAccreditationContext.Verify(
               mock => mock.PrnStatusHistories.Add(It.IsAny<PrnStatusHistory>()));
        }

        /// <summary>
        /// Checks that the UpdatePrn method throws an ArgumentException if no update data is supplied.
        /// </summary>
        /// <returns>A <see cref="Task"/>.</returns>
        [TestMethod]
        public async Task CallUpdatePrn_NullRequest()
        {
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(()
                => this.TestRepository.UpdatePrn(
                    new Guid("72039cfe-5a38-4d54-ab2e-3b7fea6ad599"),
                    default));
        }

        [TestMethod]
        public async Task GetPackageRecyclingNote_Success()
        {
            // Arrange
            var id = this.TestPrnData[0].ExternalId;
            var expectedResult = this.Mapper.Map<PackageRecyclingNoteResponse>(this.TestPrnData[0]);

            // Act
            var result = await this.TestRepository.GetPackageRecyclingNote(id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public async Task GetPrnsForOrganisation_Success()
        {
            // Arrange
            var organisationId = new Guid("ebc46f56-3c20-4ad4-8956-c68b18990e06");
            var expectedNoOfMatchingRecords = 3;
            var matchingRecordsFixture = new Fixture()
                .Build<PackageRecyclingNote>()
                .Without(f => f.Site)
                .Without(f => f.PrnStatus)
                .Without(f => f.PrnType)
                .With(f => f.OrganisationId, organisationId);
            this.TestPrnData.AddMany(() => matchingRecordsFixture.Create(), expectedNoOfMatchingRecords);

            // Act
            var results = this.TestRepository.GetPrnsForOrganisation(organisationId).Result.ToList();
            var expectedResults = this.TestPrnData.Skip(this.TestPrnData.Count - expectedNoOfMatchingRecords)
                .Select(record => record.ExternalId).ToList();

            // Assert
            CollectionAssert.AreEqual(expectedResults, results);
        }

        private Mock<IMapper> _mapper;
        private AccreditationContext _accreditationContext;

        [TestMethod]
        public async Task UpdatePrnStatus_Success()
        {
            // Arrange
            var prnId = this.TestPrnData[0].ExternalId;
            var newStatus = new PrnStatusHistoryRequest
            {
                CreatedByUser = new Guid("a55fd4ce-d8be-4f63-8e33-4b19eb100138"),
                CreatedByOrganisationId = new Guid("5039cc14-30c7-43f4-9e57-5395af1826b9"),
                OrganisationName = "TestValue1046057102",
                PrnStatusId = 2111279239,
                Comment = "TestValue800230845",
            };

            // Act
            await this.TestRepository.UpdatePrnStatus(prnId, newStatus);

            // Assert
            this.MockAccreditationContext.Verify(
                mock => mock.PrnStatusHistories.Add(
                    It.Is<PrnStatusHistory>(record =>
                        record.PrnId == this.TestPrnData[0].Id &&
                        record.CreatedByUser == newStatus.CreatedByUser &&
                        record.CreatedByOrganisationId == newStatus.CreatedByOrganisationId &&
                        record.OrganisationName == newStatus.OrganisationName &&
                        record.Comment == newStatus.Comment)),
                Times.Once());
            Assert.AreEqual(this.TestPrnData[0].PrnStatusId, newStatus.PrnStatusId);
        }

        /// <summary>
        /// Check that UpdatePrnStatus throws an ArgumentNullException if no status update info is supplied.
        /// </summary>
        /// <returns>A <see cref="Task"/>.</returns>
        [TestMethod]
        public async Task UpdatePrnStatus_NullStatus()
        {
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(()
                => this.TestRepository.UpdatePrnStatus(
                    Guid.NewGuid(),
                    default));
        }

        /// <summary>
        /// Check that the CanCallDeletePrn method 
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task DeletePrn_Success()
        {
            // Arrange

            // Act
            await this.TestRepository.DeletePrn(this.TestPrnData[0].ExternalId);

            // Assert
            this.MockAccreditationContext.Verify(
                mock => mock.PackageRecyclingNote.Remove(this.TestPrnData[0]),
                Times.Once());
        }
    }
}