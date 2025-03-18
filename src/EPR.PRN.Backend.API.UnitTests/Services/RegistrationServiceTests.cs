using AutoFixture;
using EPR.PRN.Backend.API.Common.Constants;
using EPR.PRN.Backend.API.Services;
using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Data.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.UnitTests.Services
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class RegistrationServiceTests
    {
        private RegistrationService _systemUnderTest;
        private Mock<IRegistrationRepository> _mockRepository;
        private Mock<ILogger<RegistrationService>> _mockLogger;
        private static readonly IFixture _fixture = new Fixture();

        [TestInitialize]
        public void Init()
        {
            _mockRepository = new Mock<IRegistrationRepository>();

            _mockLogger = new Mock<ILogger<RegistrationService>>();

            _systemUnderTest = new RegistrationService(_mockRepository.Object, _mockLogger.Object);
        }


        [TestMethod]
        public async Task GetRegistrationById_WithValidId_ReturnsExpectedDto()
        {
            var registrationId = 1;
            var organisationId = Guid.NewGuid();

            var expectedRegistration = new Registration { Id= 1, 
                ApplicationTypeId = 1, 
                OrganisationId = organisationId,
                BusinessAddress = new Address { 
                    Id = 1, 
                    AddressLine1 = "Business Address", 
                    AddressLine2 = "", 
                    County = "Country", 
                    Postcode = "Postcode", 
                    TownCity = "TownCity", 
                    NationId = 1 
                },
                ReprocessingSiteAddress =  new Address
                {
                    Id = 1,
                    AddressLine1 = "ReprocessingSiteAddress",
                    AddressLine2 = "",
                    County = "Country",
                    Postcode = "Postcode",
                    TownCity = "TownCity",
                    NationId = 1
                }
            };

            _mockRepository.Setup(r => r.GetByIdAsync(registrationId)).ReturnsAsync(expectedRegistration);

            var result = await _systemUnderTest.GetByIdAsync(registrationId);

            result.ApplicationTypeId.Should().Be(1);
            result.OrganisatonId.Should().Be(organisationId);
            result.Addresses.Should().HaveCount(2);

            _mockRepository.Verify(r => r.GetByIdAsync(registrationId), Times.Once);
            _mockRepository.VerifyNoOtherCalls();
        }

        [TestMethod]
        public async Task GetRegistrationById_WithInValidId_ReturnsNull()
        {
            var registrationId = 1;
            var organisationId = Guid.NewGuid();

            _mockRepository.Setup(r => r.GetByIdAsync(registrationId)).ReturnsAsync((Registration)null);

            var result = await _systemUnderTest.GetByIdAsync(registrationId);

            result.Should().BeNull();

            _mockRepository.Verify(r => r.GetByIdAsync(registrationId), Times.Once);
            _mockRepository.VerifyNoOtherCalls();
        }

        [TestMethod]
        public async Task GetRegistrationById_WithValidId_ReturnsExpectedAddresses()
        {
            var registrationId = 1;
            var organisationId = Guid.NewGuid();

            var expectedRegistration = new Registration
            {
                Id = 1,
                ApplicationTypeId = 1,
                OrganisationId = organisationId,
                BusinessAddress = new Address
                {
                    Id = 1,
                    AddressLine1 = "Business Address",
                    AddressLine2 = "",
                    County = "Country",
                    Postcode = "Postcode",
                    TownCity = "TownCity",
                    NationId = 1
                },
                ReprocessingSiteAddress = new Address
                {
                    Id = 1,
                    AddressLine1 = "ReprocessingSiteAddress",
                    AddressLine2 = "",
                    County = "Country",
                    Postcode = "Postcode",
                    TownCity = "TownCity",
                    NationId = 1
                },
                LegalDocumentAddress = new Address
                {
                    Id = 1,
                    AddressLine1 = "LegalDocumentAddress",
                    AddressLine2 = "",
                    County = "Country",
                    Postcode = "Postcode",
                    TownCity = "TownCity",
                    NationId = 1
                }
            };

            _mockRepository.Setup(r => r.GetByIdAsync(registrationId)).ReturnsAsync(expectedRegistration);

            var result = await _systemUnderTest.GetByIdAsync(registrationId);

            result.Addresses.Should().HaveCount(3);
            result.Addresses[0].AddressType.Should().Be(RegistrationAddressConstants.BusinessAddress);
            result.Addresses[1].AddressType.Should().Be(RegistrationAddressConstants.ReprocessingSiteAddress);
            result.Addresses[2].AddressType.Should().Be(RegistrationAddressConstants.LegalDocumentAddress);

            _mockRepository.Verify(r => r.GetByIdAsync(registrationId), Times.Once);
            _mockRepository.VerifyNoOtherCalls();
        }
    }
}
