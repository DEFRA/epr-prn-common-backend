﻿using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Obligation.Interfaces;
using EPR.PRN.Backend.Obligation.Strategies;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.Obligation.UnitTests.Strategies;

[ExcludeFromCodeCoverage]
[TestClass]
public class MaterialCalculationStrategyResolverTests
{
    private Mock<ILogger<MaterialCalculationStrategyResolver>> _loggerMock;
    private MaterialCalculationStrategyResolver _resolver;
    private List<Mock<IMaterialCalculationStrategy>> _strategyMocks;

    [TestInitialize]
    public void SetUp()
    {
        _loggerMock = new Mock<ILogger<MaterialCalculationStrategyResolver>>();
        _strategyMocks = new List<Mock<IMaterialCalculationStrategy>>();
    }

    [TestMethod]
    public void Resolve_ShouldReturnCorrectStrategy_WhenStrategyExists()
    {
        // Arrange
        var materialType = MaterialType.Plastic;
        var strategyMock = new Mock<IMaterialCalculationStrategy>();
        strategyMock.Setup(s => s.CanHandle(materialType)).Returns(true);
        _strategyMocks.Add(strategyMock);

        _resolver = new MaterialCalculationStrategyResolver(_strategyMocks.Select(m => m.Object), _loggerMock.Object);

        // Act
        var result = _resolver.Resolve(materialType);

        // Assert
        result.Should().NotBeNull();
		result.Should().Be(strategyMock.Object);
    }

    [TestMethod]
    public void Resolve_ShouldReturnNull_AndLogError_WhenNoStrategyExists()
    {
        // Arrange
        var materialType = MaterialType.Plastic;
        _resolver = new MaterialCalculationStrategyResolver(_strategyMocks.Select(m => m.Object), _loggerMock.Object);

        // Act
        var result = _resolver.Resolve(materialType);

        // Assert
        result.Should().BeNull();
        _loggerMock.Verify(
            x => x.Log(
                It.Is<LogLevel>(logLevel => logLevel == LogLevel.Error),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains($"No strategy found for material type: {materialType}")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    [TestMethod]
    public void Resolve_ShouldReturnFirstMatchingStrategy_WhenMultipleStrategiesExist()
    {
        // Arrange
        var materialType = MaterialType.Plastic;

        var firstStrategyMock = new Mock<IMaterialCalculationStrategy>();
        firstStrategyMock.Setup(s => s.CanHandle(materialType)).Returns(true);

        var secondStrategyMock = new Mock<IMaterialCalculationStrategy>();
        secondStrategyMock.Setup(s => s.CanHandle(materialType)).Returns(true);

        _strategyMocks.Add(firstStrategyMock);
        _strategyMocks.Add(secondStrategyMock);

        _resolver = new MaterialCalculationStrategyResolver(_strategyMocks.Select(m => m.Object), _loggerMock.Object);

        // Act
        var result = _resolver.Resolve(materialType);

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(firstStrategyMock.Object, "The first strategy that can handle the material should be returned.");
    }
}
