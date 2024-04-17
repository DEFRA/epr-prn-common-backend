using EPR.Accreditation.API.Common.Dtos;
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
            var externalId = Guid.NewGuid();
            var expectedAccreditation = new Common.Dtos.Accreditation();

            _mockRepository.Setup(r => r.GetById(externalId)).ReturnsAsync(expectedAccreditation);

            // Act
            var result = await _accreditationService.GetAccreditation(externalId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedAccreditation, result);

            _mockRepository.Verify(r => r.GetById(externalId), Times.Once());
        }

        [TestMethod]
        public async Task GetAccreditation_WithInvalidId_ReturnsNull()
        {
            // Arrange
            var externalId = Guid.NewGuid();

            _mockRepository.Setup(r => r.GetById(externalId)).ReturnsAsync((Common.Dtos.Accreditation)null);

            // Act
            var result = await _accreditationService.GetAccreditation(externalId);

            // Assert
            Assert.IsNull(result);

            _mockRepository.Verify(r => r.GetById(externalId), Times.Once);
        }

        [TestMethod]
        public async Task UpdateAccreditation_CallsRepositoryWithCorrectParameters()
        {
            // Arrange
            var externalId = Guid.NewGuid();
            var accreditation = new Common.Dtos.Accreditation();

            // Act
            await _accreditationService.UpdateAccreditation(externalId, accreditation);

            // Assert
            _mockRepository.Verify(r => r.UpdateAccreditation(externalId, accreditation), Times.Once);
        }

        [TestMethod]
        public async Task GetMaterial_CallsRepositoryWithCorrectParameters()
        {
            // Arrange
            var externalId = Guid.NewGuid();
            var overseasSiteExternalId = Guid.NewGuid();
            var materialExternalId = Guid.NewGuid();

            // Act
            await _accreditationService.GetMaterial(
                externalId,
                overseasSiteExternalId,
                materialExternalId);

            // Assert
            _mockRepository.Verify(r =>
                r.GetMaterial(
                    externalId,
                    overseasSiteExternalId,
                    materialExternalId), Times.Once);
        }

        [TestMethod]
        public async Task UpdateMaterial_CallsRepositoryWithCorrectParameters()
        {
            // Arrange
            var externalId = Guid.NewGuid();
            var overseasSiteId = Guid.NewGuid();
            var materialExternalId = Guid.NewGuid();
            var accreditationMaterial = new Common.Dtos.AccreditationMaterial();

            // Act
            await _accreditationService
                .UpdateMaterail(
                externalId,
                overseasSiteId,
                materialExternalId,
                accreditationMaterial);

            //Assert
            _mockRepository.Verify(r =>
            r.UpdateMaterial(
                externalId,
                overseasSiteId,
                materialExternalId,
                accreditationMaterial), Times.Once);
        }

        [TestMethod]
        public async Task GetSite_WithValidId_ReturnsSite()
        {
            // Arrange
            var externalId = Guid.NewGuid();
            var expectedSite = new Common.Dtos.Site();

            _mockRepository.Setup(r => r.GetSite(externalId)).ReturnsAsync(expectedSite);

            // Act
            var result = await _accreditationService.GetSite(externalId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedSite, result);

            _mockRepository.Verify(r => r.GetSite(externalId), Times.Once());
        }

        [TestMethod]
        public async Task UpdateSite_CallsRepositoryWithCorrectParameters()
        {
            // Arrange
            var externalId = Guid.NewGuid();
            var site = new Common.Dtos.Site();

            // Act
            await _accreditationService.UpdateSite(externalId, site);

            //Assert
            _mockRepository.Verify(r => r.UpdateSite(externalId, site), Times.Once);
        }

        [TestMethod]
        public async Task GetSaveAndComeBack_WithValidId_ReturnsSaveAndComeBack()
        {
            // Arrange
            var externalId = Guid.NewGuid();
            var expectedSaveAndComeBack = new Common.Dtos.SaveAndComeBack();

            _mockRepository.Setup(r => r.GetSaveAndComeBack(externalId)).ReturnsAsync(expectedSaveAndComeBack);

            // Act
            var result = await _accreditationService.GetSaveAndComeBack(externalId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedSaveAndComeBack, result);

            _mockRepository.Verify(r => r.GetSaveAndComeBack(externalId), Times.Once());
        }

        [TestMethod]
        public async Task AddSaveAndComeBack_CallsRepositoryWithCorrectParameters()
        {
            // Arrange
            var externalId = Guid.NewGuid();
            var saveAndComeBack = new Common.Dtos.SaveAndComeBack();

            // Act
            await _accreditationService.AddSaveAndComeBack(externalId, saveAndComeBack);

            //Assert
            _mockRepository.Verify(r => r.AddSaveAndComeBack(externalId, saveAndComeBack), Times.Once);
        }

        [TestMethod]
        public async Task DeleteSaveAndComeBack_CallsRepositoryWithCorrectParameters()
        {
            // Arrange
            var externalId = Guid.NewGuid();

            // Act
            await _accreditationService.DeleteSaveAndComeBack(externalId);

            //Assert
            _mockRepository.Verify(r => r.DeleteSaveAndComeBack(externalId), Times.Once);
        }

        [TestMethod]
        public async Task CreateOverseasSite_ValidData_ReturnsNewGuid()
        {
            // Arrange
            var externalId = Guid.NewGuid();
            var overseasReprocessingSite = new OverseasReprocessingSite();
            var expectedGuid = Guid.NewGuid();

            _mockRepository.Setup(r =>
                r.CreateOverseasSite(
                    externalId,
                    overseasReprocessingSite))
                .ReturnsAsync(expectedGuid);

            // Act
            var result = await _accreditationService.CreateOverseasSite(externalId, overseasReprocessingSite);

            // Assert
            Assert.AreEqual(expectedGuid, result);

            _mockRepository.Verify(r => r.CreateOverseasSite(externalId, overseasReprocessingSite), Times.Once);
        }

        [TestMethod]
        public async Task GetOverseasSite_ExistingSite_ReturnsSite()
        {
            // Arrange
            var externalId = Guid.NewGuid();
            var overseasSiteExternalId = Guid.NewGuid();
            var expectedSite = new OverseasReprocessingSite();

            _mockRepository.Setup(r =>
                r.GetOverseasSite(
                    externalId,
                    overseasSiteExternalId))
                .ReturnsAsync(expectedSite);

            // Act
            var result = await _accreditationService.GetOverseasSite(externalId, overseasSiteExternalId);

            // Assert
            Assert.AreEqual(expectedSite, result);

            _mockRepository.Verify(r => r.GetOverseasSite(externalId, overseasSiteExternalId), Times.Once);
        }

        [TestMethod]
        public async Task GetOverseasSite_NonExistingSite_ReturnsNull()
        {
            // Arrange
            var externalId = Guid.NewGuid();
            var overseasSiteExternalId = Guid.NewGuid();

            _mockRepository.Setup(r =>
                r.GetOverseasSite(
                    externalId,
                    overseasSiteExternalId))
                .ReturnsAsync((OverseasReprocessingSite)null);

            // Act
            var result = await _accreditationService.GetOverseasSite(externalId, overseasSiteExternalId);

            // Assert
            Assert.IsNull(result);

            _mockRepository.Verify(r => r.GetOverseasSite(externalId, overseasSiteExternalId), Times.Once);
        }

        [TestMethod]
        public async Task UpdateOverseasSite_ValidData_CallsRepositoryWithCorrectParameters()
        {
            // Arrange
            var overseasReprocessingSite = new OverseasReprocessingSite();

            // Act
            await _accreditationService.UpdateOverseasSite(overseasReprocessingSite);

            // Assert
            _mockRepository.Verify(x => x.UpdateOverseasSite(overseasReprocessingSite), Times.Once);
        }
    }
}
