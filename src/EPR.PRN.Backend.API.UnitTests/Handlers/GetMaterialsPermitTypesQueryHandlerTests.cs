using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Dto.Regulator;
using EPR.PRN.Backend.API.Handlers;
using EPR.PRN.Backend.API.Queries;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;

namespace EPR.PRN.Backend.API.UnitTests.Handlers;

[TestClass]
public class GetMaterialsPermitTypesQueryHandlerTests
{
    private Mock<IRegistrationMaterialRepository> _repositoryMock;
    private GetMaterialsPermitTypesQueryHandler _handler;
    private GetMaterialsPermitTypesQuery _query;

    [TestInitialize]
    public void TestInitialize()
    {
        _repositoryMock = new Mock<IRegistrationMaterialRepository>();
        _handler = new GetMaterialsPermitTypesQueryHandler(_repositoryMock.Object);
        _query = new GetMaterialsPermitTypesQuery();
    }

    [TestMethod]
    public async Task Handle_ShouldReturnMappedDto_WhenMaterialExists()
    {
        // Arrange
        var materialPermitTypes = Enum.GetValues(typeof(MaterialPermitType))
            .Cast<MaterialPermitType>()
            .Select(e => new LookupMaterialPermit
            {
                Id = (int)e,
                Name = e.ToString()
            })
            .Where(x => x.Id != 0)
            .ToList();

        var expectedMaterialPermitTypes = Enum.GetValues(typeof(MaterialPermitType))
            .Cast<MaterialPermitType>()
            .Select(e => new MaterialsPermitTypeDto
            {
                Id = (int)e,
                Name = e.ToString()
            })
            .Where(x => x.Id != 0)
            .ToList();

        _repositoryMock
            .Setup(o => o.GetMaterialPermitTypes())
            .ReturnsAsync(materialPermitTypes);

        // Act
        var result = await _handler.Handle(_query, CancellationToken.None);

        // Assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedMaterialPermitTypes);
        }
    }
}