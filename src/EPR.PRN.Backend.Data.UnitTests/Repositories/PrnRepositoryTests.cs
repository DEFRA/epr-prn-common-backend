//using AutoFixture;
//using EPR.PRN.Backend.Data.DataModels;
//using EPR.PRN.Backend.Data.Repositories;
//using FluentAssertions;
//using Microsoft.EntityFrameworkCore;
//using Moq;
//using Moq.EntityFrameworkCore;

//namespace EPR.PRN.Backend.Data.UnitTests.Repositories
//{
//    [TestClass]
//    public class PrnRepositoryTests
//    {
//        private Mock<EprContext> _mockContext;
//        private Guid organisationId = Guid.NewGuid();

//        [TestInitialize]
//        public void Initialize()
//        {
//            var dbContextOptions = new DbContextOptionsBuilder<EprContext>().Options;
//            _mockContext = new Mock<EprContext>(dbContextOptions);
//            var fixture = new Fixture();
//            var prns = fixture.CreateMany<Eprn>(10).ToList();
//            for (int i = 0; i < 2; i++)
//            {
//                prns[i].MaterialName = "Plastic";
//                prns[i].PrnStatusId = 1;
//                prns[i].TonnageValue = 300;
//                prns[i].OrganisationId = organisationId;
//            }
//            for (int i = 2; i < 5; i++)
//            {
//                prns[i].MaterialName = "Plastic";
//                prns[i].PrnStatusId = 4;
//                prns[i].TonnageValue = 500;
//                prns[i].OrganisationId = organisationId;
//            }
//            for (int i = 5; i < 7; i++)
//            {
//                prns[i].MaterialName = "Wood";
//                prns[i].PrnStatusId = 1;
//                prns[i].TonnageValue = 100;
//                prns[i].OrganisationId = organisationId;
//            }
//            for (int i = 7; i < prns.Count; i++)
//            {
//                prns[i].MaterialName = "Wood";
//                prns[i].PrnStatusId = 4;
//                prns[i].TonnageValue = 200;
//                prns[i].OrganisationId = organisationId;
//            }
//            _mockContext.Setup(context => context.Prn).ReturnsDbSet(prns);
//            var prnStatuses = new List<PrnStatus>
//            {
//                new() { Id = 1, StatusName = EprnStatus.ACCEPTED.ToString(), StatusDescription = "Prn Accepted"},
//                new() { Id = 2, StatusName = EprnStatus.REJECTED.ToString(), StatusDescription = "Prn Rejected"},
//                new() { Id = 3, StatusName = EprnStatus.CANCELLED.ToString(), StatusDescription = "Prn Cancelled"},
//                new() { Id = 4, StatusName = EprnStatus.AWAITINGACCEPTANCE.ToString(), StatusDescription = "Prn Awaiting Acceptance"}
//            };
//            _mockContext.Setup(context => context.PrnStatus).ReturnsDbSet(prnStatuses);
//        }

//        [TestMethod]
//        [DataRow("ACCEPTED")]
//        [DataRow("AWAITINGACCEPTANCE")]
//        [DataRow("CANCELLED")]
//        public async Task GetSumOfTonnageForMaterials_ReturnsExpectedResult(string statusName)
//        {
//            // Arrange
//            var prnRepository = new PrnRepository(_mockContext.Object);

//            // Act
//            var result = await prnRepository.GetSumOfTonnageForMaterialsAsync(organisationId, statusName);

//            // Assert
//            if (statusName == EprnStatus.ACCEPTED.ToString())
//            {
//                result.Should().NotBeNull();
//                result.Should().HaveCount(2);
//                result.Should().ContainSingle(r => r.MaterialName == "Plastic" && r.TotalTonnage == 600);
//                result.Should().ContainSingle(r => r.MaterialName == "Wood" && r.TotalTonnage == 200);
//            }
//            if (statusName == EprnStatus.AWAITINGACCEPTANCE.ToString())
//            {
//                result.Should().NotBeNull();
//                result.Should().HaveCount(2);
//                result.Should().ContainSingle(r => r.MaterialName == "Plastic" && r.TotalTonnage == 1500);
//                result.Should().ContainSingle(r => r.MaterialName == "Wood" && r.TotalTonnage == 600);
//            }
//            if (statusName == EprnStatus.CANCELLED.ToString())
//            {
//                result.Should().NotBeNull();
//                result.Should().HaveCount(0);
//            }
//        }

//    }
//}
