using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Data.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;

namespace EPR.PRN.Backend.Data.UnitTests.Repositories;

[TestClass]
public class ObligationCalculationOrganisationSubmitterTypeRepositoryTests
{
    private ObligationCalculationOrganisationSubmitterTypeRepository _submitterTypeRepository;
    private Mock<EprContext> _mockEprContext;
    private readonly List<ObligationCalculationOrganisationSubmitterType> _submitterTypes =
        [
            new ObligationCalculationOrganisationSubmitterType { Id = 1, TypeName = ObligationCalculationOrganisationSubmitterTypeName.ComplianceScheme.ToString() },
            new ObligationCalculationOrganisationSubmitterType { Id = 2, TypeName = ObligationCalculationOrganisationSubmitterTypeName.DirectRegistrant.ToString() },
        ];

    [TestInitialize]
    public void Setup()
    {
        var dbContextOptions = new DbContextOptionsBuilder<EprContext>().Options;
        _mockEprContext = new Mock<EprContext>(dbContextOptions);
        _mockEprContext
            .Setup(context => context.ObligationCalculationOrganisationSubmitterType)
            .ReturnsDbSet(_submitterTypes);
        _submitterTypeRepository = new ObligationCalculationOrganisationSubmitterTypeRepository(_mockEprContext.Object);
    }

    [TestMethod]
    public async Task GetSubmitterTypeIdByTypeName_ShouldReturnMaterialId_ForComplianceScheme()
    {
        // Act
        var result = await _submitterTypeRepository.GetSubmitterTypeIdByTypeName(ObligationCalculationOrganisationSubmitterTypeName.ComplianceScheme);

        // Assert
        result.Should().Be(1);
    }

    [TestMethod]
    public async Task GetSubmitterTypeIdByTypeName_ShouldReturnMaterialId_ForDirectRegistrant()
    {
        // Act
        var result = await _submitterTypeRepository.GetSubmitterTypeIdByTypeName(ObligationCalculationOrganisationSubmitterTypeName.DirectRegistrant);

        // Assert
        result.Should().Be(2);
    }
}
