using EPR.Accreditation.API.Common.Dtos;
using EPR.Accreditation.API.Common.Enums;
using EPR.Accreditation.API.Repositories.Interfaces;
using EPR.Accreditation.API.Services;
using Moq;

namespace EPR.Accreditation.API.UnitTests.Services
{
    /// <summary>
    /// Unit tests for <see cref="PackageRecyclingNoteService"/>.
    /// </summary>
    [TestClass]
    public class PackageRecyclingNoteServiceTests
    {
        private PackageRecyclingNoteService PrnService { get; set; }

        private Mock<IRepository> MockRepository { get; set; }

        [TestInitialize]
        public void SetUp()
        {
            this.MockRepository = new Mock<IRepository>();
            this.PrnService = new PackageRecyclingNoteService(this.MockRepository.Object);
        }

        /// <summary>
        /// Check the constructor runs.
        /// </summary>
        [TestMethod]
        public void Constructor()
        {
            // Act
            var instance = new PackageRecyclingNoteService(this.MockRepository.Object);

            // Assert
            Assert.IsNotNull(instance);
        }

        /// <summary>
        /// Check that the constructor does not allow a service object to be instanciated without a repository.
        /// </summary>
        [TestMethod]
        public void Constructor_NullRepository()
            => Assert.ThrowsException<ArgumentNullException>(() => new PackageRecyclingNoteService(default));

        /// <summary>
        /// Check that the CreatePackageRecyclingNote method runs, passes the parameters on to the repository
        /// and passes the ID back to the caller.
        /// </summary>
        /// <returns>A <see cref="Task"/>.</returns>
        [TestMethod]
        public async Task CreatePackageRecyclingNote_Success()
        {
            // Arrange
            var prn = new PackageRecyclingNoteRequest
            {
                OperatorTypeId = OperatorType.Exporter,
                ReferenceNumber = "TestValue1440206571",
                OrganisationId = new Guid("f1df7bc7-6a8a-4807-a21c-dd6cdaee3813"),
                PrnStatusId = 79260415,
                SiteId = 807862745,
                CreatedBy = new Guid("072498f8-ef12-486b-bf5b-6f0c1235d025"),
                LastUpdatedBy = new Guid("a9c43851-2cfe-485b-b74d-a4c8909410b7"),
                Note = "TestValue1168176283",
                MaterialId = 1244534559,
                IsDecember = false,
                TonnageValue = 1506697343,
                ProducerId = 1837362528,
                ProducerName = "TestValue368209418",
                AccreditationId = 2064331101,
                AccreditationReference = "TestValue916535714",
                IsActive = true,
            };

            var expectedGuid = new Guid("6f634faf-50b2-48e8-a653-39a1745acea3");

            this.MockRepository.Setup(mock => mock.AddPackageRecyclingNote(It.IsAny<PackageRecyclingNoteRequest>()))
                .ReturnsAsync(expectedGuid);

            // Act
            var result = await this.PrnService.CreatePackageRecyclingNote(prn);

            // Assert
            this.MockRepository.Verify(mock => mock.AddPackageRecyclingNote(It.IsAny<PackageRecyclingNoteRequest>()));
            Assert.AreEqual(expectedGuid, result);
        }

        /// <summary>
        /// Check that the UpdatePrn method runs, passes the parameters on to the repository.
        /// </summary>
        /// <returns>A <see cref="Task"/>.</returns>
        [TestMethod]
        public async Task UpdatePrn_Success()
        {
            // Arrange
            var prnId = new Guid("e920150a-51cd-4365-809b-948aaeceef6c");
            var newData = new PrnUpdateRequest
            {
                Prn = new PackageRecyclingNoteRequest
                {
                    OperatorTypeId = OperatorType.Reprocessor,
                    ReferenceNumber = "TestValue1439557443",
                    OrganisationId = new Guid("59c8c140-bd3d-4894-9790-2a3349596b14"),
                    PrnStatusId = 2140533017,
                    SiteId = 1793023827,
                    CreatedBy = new Guid("2b778841-7da1-4feb-8144-fba0c7a158f1"),
                    LastUpdatedBy = new Guid("d461028b-4f00-439e-8c41-eed72423518c"),
                    Note = "TestValue1554863258",
                    MaterialId = 1141517379,
                    IsDecember = true,
                    TonnageValue = 392059562,
                    ProducerId = 1626085919,
                    ProducerName = "TestValue1755210105",
                    AccreditationId = 1135070727,
                    AccreditationReference = "TestValue867867394",
                    IsActive = true,
                },
                Status = new PrnStatusHistoryRequest
                {
                    CreatedByUser = new Guid("4f9553e1-bc60-45e2-ab6a-dc529915b582"),
                    CreatedByOrganisationId = new Guid("b559378d-130c-480d-9a61-2bd386df9d47"),
                    OrganisationName = "TestValue1271900432",
                    PrnStatusId = 1559603764,
                    Comment = "TestValue1493940704",
                },
            };

            // Act
            await this.PrnService.UpdatePrn(prnId, newData);

            // Assert
            this.MockRepository.Verify(mock => mock.UpdatePrn(prnId, newData));
        }

        /// <summary>
        /// Check that the GetPackageRecyclingNote method runs, passes the parameters on to the repository
        /// and passes the result back to the caller.
        /// </summary>
        /// <returns>A <see cref="Task"/>.</returns>
        [TestMethod]
        public async Task GetPackageRecyclingNote_Success()
        {
            // Arrange
            var externalId = new Guid("9f4810ec-6d46-42e2-a0cd-7ee32eebe4ca");
            var expectedResult = new PackageRecyclingNoteResponse
            {
                Note = "Test response.",
            };
            this.MockRepository.Setup(mock => mock.GetPackageRecyclingNote(externalId))
               .ReturnsAsync(expectedResult);

            // Act
            var result = await this.PrnService.GetPackageRecyclingNote(externalId);

            // Assert
            this.MockRepository.Verify(mock => mock.GetPackageRecyclingNote(externalId));
            Assert.AreEqual(expectedResult, result);
        }

        /// <summary>
        /// Check that the CreatePackageRecyclingNote method runs, passes the parameters on to the repository
        /// and passes the list of IDs back to the caller.
        /// </summary>
        /// <returns>A <see cref="Task"/>.</returns>
        [TestMethod]
        public async Task GetPrnsForOrganisation_Success()
        {
            // Arrange
            var organisationId = new Guid("10872e15-2145-4450-ae35-34d7bab02361");
            var expectedResult = new Guid[]
            {
               new Guid("1ed50b1c-bba1-4528-8ebd-0e396ba7ea68"),
               new Guid("52a75dbd-9bc5-4c80-9bf9-9e29db78c22a"),
            };
            this.MockRepository.Setup(mock => mock.GetPrnsForOrganisation(organisationId))
               .ReturnsAsync(expectedResult);

            // Act
            var result = await this.PrnService.GetPrnsForOrganisation(organisationId);

            // Assert
            this.MockRepository.Verify(mock => mock.GetPrnsForOrganisation(organisationId));
            Assert.AreEqual(expectedResult, result);
        }

        /// <summary>
        /// Check that the UpdatePrnStatus method runs and passes the parameters on to the repository.
        /// </summary>
        /// <returns>A <see cref="Task"/>.</returns>
        [TestMethod]
        public async Task UpdatePrnStatus_Success()
        {
            // Arrange
            var prnId = new Guid("1711d9f7-7c0d-4385-b6db-3927ccead53c");
            var status = new PrnStatusHistoryRequest
            {
                CreatedByUser = new Guid("34e184d9-9daa-4679-afa7-88ecae60128f"),
                CreatedByOrganisationId = new Guid("4ee7cf95-1e3d-4381-8680-bc84054c5643"),
                OrganisationName = "TestValue1260998827",
                PrnStatusId = 2017033254,
                Comment = "TestValue1392743064",
            };

            // Act
            await this.PrnService.UpdatePrnStatus(prnId, status);

            // Assert
            this.MockRepository.Verify(mock => mock.UpdatePrnStatus(prnId, status));
        }

        /// <summary>
        /// Check that the DeletePrn method runs and passes the parameters on to the repository.
        /// </summary>
        /// <returns>A <see cref="Task"/>.</returns>
        [TestMethod]
        public async Task DeletePrn_Success()
        {
            // Arrange
            var prnId = new Guid("d72f05d7-51d4-4273-8471-1e48fe07e7bf");

            // Act
            await this.PrnService.DeletePrn(prnId);

            // Assert
            this.MockRepository.Verify(mock => mock.DeletePrn(prnId));
        }
    }
}