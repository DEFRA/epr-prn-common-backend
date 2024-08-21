using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Data.Interfaces;
using FluentAssertions;
using Moq;
using Moq.EntityFrameworkCore;

namespace EPR.PRN.Backend.Data.Repositories.Tests
{
    [TestClass()]
    public class ObligationCalculationRepositoryTests
    {
        private Mock<EprContext> _mockEprContext;
        private int organisationId = 1;
        private List<ObligationCalculation> obligationCalculation = new List<ObligationCalculation>
            {
                new ObligationCalculation { OrganisationId = 1, MaterialName = "Paper", MaterialObligationValue = 75 },
                new ObligationCalculation { OrganisationId = 1, MaterialName = "Glass", MaterialObligationValue = 75 },
                new ObligationCalculation { OrganisationId = 1, MaterialName = "Aluminium", MaterialObligationValue = 75 },
                new ObligationCalculation { OrganisationId = 1, MaterialName = "Steel", MaterialObligationValue = 75 },
                new ObligationCalculation { OrganisationId = 1, MaterialName = "Plastic", MaterialObligationValue = 75 }
            };

        [TestInitialize]
        public void TestInitialize()
        {
            _mockEprContext = new Mock<EprContext>();
            _mockEprContext.Setup(context => context.ObligationCalculations).ReturnsDbSet(obligationCalculation);
            _mockEprContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        }

        [TestMethod()]
        public void GetObligationCalculationByOrganisationIdTest()
        {
            // Arrange
            var obligationCalculationRepository = new ObligationCalculationRepository(_mockEprContext.Object);

            // Act
            var result = obligationCalculationRepository.GetObligationCalculationByOrganisationId(organisationId).Result;

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(5);
            result.Should().Contain(x => x.MaterialName == "Paper");
            result.Should().Contain(x => x.MaterialName == "Glass");
            result.Should().Contain(x => x.MaterialName == "Aluminium");
            result.Should().Contain(x => x.MaterialName == "Steel");
            result.Should().Contain(x => x.MaterialName == "Plastic");
        }

        [TestMethod()]
        public void AddObligationCalculationTest()
        {
            // Arrange
            var obligationCalculationRepository = new ObligationCalculationRepository(_mockEprContext.Object);

            List<ObligationCalculation> obligationCalculationAdd = new List<ObligationCalculation>
            {
                new ObligationCalculation { OrganisationId = 1, MaterialName = "Wood", MaterialObligationValue = 75 },
                new ObligationCalculation { OrganisationId = 1, MaterialName = "GlassRemelt", MaterialObligationValue = 75 }
            };

            // Act
            obligationCalculationRepository.AddObligationCalculation(obligationCalculationAdd).Wait();

            // Assert
            _mockEprContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}