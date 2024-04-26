using EPR.Accreditation.API.Common.Dtos;
using EPR.Accreditation.API.Helpers;
using EPR.Accreditation.API.Repositories.Interfaces;
using EPR.Accreditation.API.Services;
using Moq;

namespace EPR.Accreditation.API.UnitTests.Services
{
    [TestClass]
    public class AccreditationServiceTests
    {
        private AccreditationService _accreditationService;
        private Mock<IRepository> _mockRepository;

        [TestInitialize]
        public void Init()
        {
            _mockRepository = new Mock<IRepository>();
            _accreditationService = new AccreditationService(_mockRepository.Object);
        }

        [TestMethod]
        public async Task GetAccreditation_WithValidId_ReturnsAccreditation()
        {
            // Arrange
            var id = Guid.NewGuid();
            var expectedAccreditation = new Common.Dtos.Accreditation();

            _mockRepository.Setup(r => r.GetAccreditation(id)).ReturnsAsync(expectedAccreditation);

            // Act
            var result = await _accreditationService.GetAccreditation(id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedAccreditation, result);

            _mockRepository.Verify(r => r.GetAccreditation(id), Times.Once());
        }

        [TestMethod]
        public async Task GetAccreditation_WithInvalidId_ReturnsNull()
        {
            // Arrange
            var id = Guid.NewGuid();

            _mockRepository.Setup(r => r.GetAccreditation(id)).ReturnsAsync((Common.Dtos.Accreditation)null);

            // Act
            var result = await _accreditationService.GetAccreditation(id);

            // Assert
            Assert.IsNull(result);

            _mockRepository.Verify(r => r.GetAccreditation(id), Times.Once);
        }

        [TestMethod]
        public async Task UpdateAccreditation_CallsRepositoryWithCorrectParameters()
        {
            // Arrange
            var id = Guid.NewGuid();
            var accreditation = new Common.Dtos.Accreditation();

            // Act
            await _accreditationService.UpdateAccreditation(id, accreditation);

            // Assert
            _mockRepository.Verify(r => r.UpdateAccreditation(id, accreditation), Times.Once);
        }

        [TestMethod]
        public async Task GetMaterial_CallsRepositoryWithCorrectParameters()
        {
            // Arrange
            var id = Guid.NewGuid();
            var materialid = Guid.NewGuid();

            // Act
            await _accreditationService.GetMaterial(
                id,
                materialid);

            // Assert
            _mockRepository.Verify(r =>
                r.GetMaterial(
                    id,
                    materialid),
                Times.Once);
        }

        [TestMethod]
        public async Task UpdateMaterial_NullWastesCodes_CallsRepositoryWithCorrectParameters()
        {
            // Arrange
            var id = Guid.NewGuid();
            var materialid = Guid.NewGuid();
            var accreditationMaterial = new Common.Dtos.AccreditationMaterial();

            // Act
            await _accreditationService
                .UpdateMaterail(
                id,
                materialid,
                accreditationMaterial);

            // Assert
            _mockRepository.Verify(r =>
                r.UpdateMaterial(
                    id,
                    materialid,
                    accreditationMaterial),
                Times.Once);
        }

        [TestMethod]
        public async Task UpdateMaterial_EmptyWastesCodes_CallsRepositoryWithCorrectParameters()
        {
            // Arrange
            var id = Guid.NewGuid();
            var materialid = Guid.NewGuid();
            var accreditationMaterial = new Common.Dtos.AccreditationMaterial
            {
                WasteCodes = new List<WasteCode>()
            };

            // Act
            await _accreditationService.UpdateMaterail(
                id,
                materialid,
                accreditationMaterial);

            // Assert
            _mockRepository.Verify(r =>
                r.UpdateMaterial(
                    id,
                    materialid,
                    accreditationMaterial),
                Times.Once);
        }

        [TestMethod]
        public async Task UpdateMaterial_SingleItemWastesCodes_CallsRepositoryWithCorrectParameters()
        {
            // Arrange
            var id = Guid.NewGuid();
            var overseasSiteId = Guid.NewGuid();
            var materialid = Guid.NewGuid();
            var wasteCodeValue = "VALUE";
            var accreditationMaterial = new Common.Dtos.AccreditationMaterial
            {
                WasteCodes = new List<WasteCode>
                {
                    new()
                    {
                        Code = wasteCodeValue
                    }
                }
            };

            // Act
            await _accreditationService.UpdateMaterail(
                id,
                materialid,
                accreditationMaterial);

            // Assert
            _mockRepository.Verify(r =>
                r.UpdateMaterial(
                    id,
                    materialid,
                    It.Is<AccreditationMaterial>(p =>
                        p.WasteCodes != null &&
                        p.WasteCodes.First().Code == wasteCodeValue)),
                Times.Once);
        }

        [TestMethod]
        public async Task UpdateMaterial_MultipleIncludingDuplicatesItemWastesCodes_CallsRepositoryWithCorrectParameters()
        {
            // Arrange
            var id = Guid.NewGuid();
            var overseasSiteId = Guid.NewGuid();
            var materialid = Guid.NewGuid();
            var wasteCodeValue = "VALUE";
            var wasteCodeValue2 = "VALUE2";
            var accreditationMaterial = new Common.Dtos.AccreditationMaterial
            {
                WasteCodes = new List<WasteCode>
                {
                    new()
                    {
                        Code = wasteCodeValue
                    },
                    new()
                    {
                        Code = wasteCodeValue
                    },
                    new()
                    {
                        Code = wasteCodeValue2
                    }
                }
            };

            // Act
            await _accreditationService.UpdateMaterail(
                id,
                materialid,
                accreditationMaterial);

            // Assert
            _mockRepository.Verify(r =>
                r.UpdateMaterial(
                    id,
                    materialid,
                    It.Is<AccreditationMaterial>(p =>
                        p.WasteCodes != null &&
                        p.WasteCodes.ToList().Count == 2 &&
                        p.WasteCodes.ToList()[0].Code == wasteCodeValue &&
                        p.WasteCodes.ToList()[1].Code == wasteCodeValue2)),
                Times.Once);
        }

        [TestMethod]
        public async Task GetSite_WithValidId_ReturnsSite()
        {
            // Arrange
            var id = Guid.NewGuid();
            var expectedSite = new Common.Dtos.Site();

            _mockRepository.Setup(r => r.GetSite(id)).ReturnsAsync(expectedSite);

            // Act
            var result = await _accreditationService.GetSite(id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedSite, result);

            _mockRepository.Verify(r => r.GetSite(id), Times.Once());
        }

        [TestMethod]
        public async Task UpdateSite_CallsRepositoryWithCorrectParameters()
        {
            // Arrange
            var id = Guid.NewGuid();
            var site = new Common.Dtos.Site();

            // Act
            await _accreditationService.UpdateSite(id, site);

            //Assert
            _mockRepository.Verify(r => r.UpdateSite(id, site), Times.Once);
        }

        [TestMethod]
        public async Task GetSaveAndComeBack_WithValidId_ReturnsSaveAndComeBack()
        {
            // Arrange
            var id = Guid.NewGuid();
            var expectedSaveAndComeBack = new Common.Dtos.SaveAndComeBack();

            _mockRepository.Setup(r => r.GetSaveAndComeBack(id)).ReturnsAsync(expectedSaveAndComeBack);

            // Act
            var result = await _accreditationService.GetSaveAndComeBack(id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedSaveAndComeBack, result);

            _mockRepository.Verify(r => r.GetSaveAndComeBack(id), Times.Once());
        }

        [TestMethod]
        public async Task AddSaveAndComeBack_CallsRepositoryWithCorrectParameters()
        {
            // Arrange
            var id = Guid.NewGuid();
            var saveAndComeBack = new Common.Dtos.SaveAndComeBack();

            // Act
            await _accreditationService.AddSaveAndComeBack(id, saveAndComeBack);

            //Assert
            _mockRepository.Verify(r => r.AddSaveAndComeBack(id, saveAndComeBack), Times.Once);
        }

        [TestMethod]
        public async Task DeleteSaveAndComeBack_CallsRepositoryWithCorrectParameters()
        {
            // Arrange
            var id = Guid.NewGuid();

            // Act
            await _accreditationService.DeleteSaveAndComeBack(id);

            //Assert
            _mockRepository.Verify(r => r.DeleteSaveAndComeBack(id), Times.Once);
        }

        [TestMethod]
        public async Task CreateOverseasSite_ExporterAccreditationExists_ReturnsNewGuid()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetAccreditation(It.IsAny<Guid>())).ReturnsAsync(
                new Common.Dtos.Accreditation
                {
                    OperatorTypeId = Common.Enums.OperatorType.Exporter
                });

            var id = Guid.NewGuid();
            var materialId = Guid.NewGuid();
            var overseasReprocessingSite = new OverseasReprocessingSite();
            var expectedGuid = Guid.NewGuid();

            _mockRepository.Setup(r =>
                r.CreateOverseasSite(
                    id,
                    materialId,
                    overseasReprocessingSite))
                .ReturnsAsync(expectedGuid);

            // Act
            var result = await _accreditationService.CreateOverseasSite(
                id,
                materialId,
                overseasReprocessingSite);

            // Assert
            Assert.AreEqual(expectedGuid, result);

            _mockRepository.Verify(
                r =>
                    r.CreateOverseasSite(
                        id,
                        materialId,
                        overseasReprocessingSite)
                    , Times.Once);
        }

        [TestMethod]
        public async Task GetOverseasSite_ExistingSite_ReturnsSite()
        {
            // Arrange
            var id = Guid.NewGuid();
            var overseasSiteid = Guid.NewGuid();
            var expectedSite = new OverseasReprocessingSite();

            _mockRepository.Setup(r =>
                r.GetOverseasSite(
                    id,
                    overseasSiteid))
                .ReturnsAsync(expectedSite);

            // Act
            var result = await _accreditationService.GetOverseasSite(id, overseasSiteid);

            // Assert
            Assert.AreEqual(expectedSite, result);

            _mockRepository.Verify(r => r.GetOverseasSite(id, overseasSiteid), Times.Once);
        }

        [TestMethod]
        public async Task GetOverseasSite_NonExistingSite_ReturnsNull()
        {
            // Arrange
            var id = Guid.NewGuid();
            var overseasSiteid = Guid.NewGuid();

            _mockRepository.Setup(r =>
                r.GetOverseasSite(
                    id,
                    overseasSiteid))
                .ReturnsAsync((OverseasReprocessingSite)null);

            // Act
            var result = await _accreditationService.GetOverseasSite(id, overseasSiteid);

            // Assert
            Assert.IsNull(result);

            _mockRepository.Verify(r => r.GetOverseasSite(id, overseasSiteid), Times.Once);
        }

        [TestMethod]
        public async Task UpdateOverseasSite_ValidData_CallsRepositoryWithCorrectParameters()
        {
            // Arrange
            var id = Guid.NewGuid();
            var siteId = Guid.NewGuid();
            var overseasReprocessingSite = new OverseasReprocessingSite();

            // Act
            await _accreditationService.UpdateOverseasSite(
                id,
                siteId,
                overseasReprocessingSite);

            // Assert
            _mockRepository.Verify(r =>
                r.UpdateOverseasSite(
                    id,
                    siteId,
                    overseasReprocessingSite),
                Times.Once);
        }
    }
}
