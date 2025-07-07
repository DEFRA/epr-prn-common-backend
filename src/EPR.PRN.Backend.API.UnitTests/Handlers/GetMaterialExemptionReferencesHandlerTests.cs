using AutoMapper;
using EPR.PRN.Backend.API.Dto;
using EPR.PRN.Backend.API.Handlers;
using EPR.PRN.Backend.API.Profiles.Regulator;
using EPR.PRN.Backend.API.Queries;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using FluentAssertions;
using Moq;

namespace EPR.PRN.Backend.API.UnitTests.Handlers
{
    [TestClass]
    public class GetMaterialExemptionReferencesHandlerTests
    {
        private Mock<IRegistrationMaterialRepository> _mockRegistrationMaterialRepository;
        private GetMaterialExemptionReferencesHandler _handler;
        private GetMaterialExemptionReferencesQuery _query;
        private readonly Guid _registrationMateriaId = Guid.NewGuid();

        [TestInitialize]
        public void Setup()
        {
            _mockRegistrationMaterialRepository = new Mock<IRegistrationMaterialRepository>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<RegistrationMaterialProfile>();
            });
            var mapper = config.CreateMapper();
            _handler = new GetMaterialExemptionReferencesHandler(_mockRegistrationMaterialRepository.Object, mapper);
            _query = new GetMaterialExemptionReferencesQuery()
            {
                RegistrationMateriaId = _registrationMateriaId
            };
        }

        [TestMethod]
        public async Task Handle_ShouldReturnMappedDto_WhenMaterialExemptionExists()
        {
            // Arrange
            var exemptionRefernece = new List<MaterialExemptionReference>() { new() { Id = 1, ReferenceNo = "REF", RegistrationMaterial = new RegistrationMaterial { ExternalId = _registrationMateriaId } } };
            var expectedExemptionRefernece = new List<GetMaterialExemptionReferenceDto>() { new() { ReferenceNo = "REF" } };
            _mockRegistrationMaterialRepository.Setup(r => r.GetMaterialExemptionReferences(_registrationMateriaId))
                             .ReturnsAsync(exemptionRefernece);

            var query = new GetMaterialExemptionReferencesQuery { RegistrationMateriaId = _registrationMateriaId };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(expectedExemptionRefernece);
        }

    }
}
