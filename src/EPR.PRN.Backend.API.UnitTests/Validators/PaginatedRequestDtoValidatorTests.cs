using AwesomeAssertions;
using EPR.PRN.Backend.API.Common.Dto;
using EPR.PRN.Backend.API.Validators;

namespace EPR.PRN.Backend.API.UnitTests.Validators;

[TestClass]
public class PaginatedRequestDtoValidatorTests
{
    private readonly PaginatedRequestDtoValidator _validator = new();

    [TestMethod]
    [DataRow(null)]
    [DataRow(2023)]
    [DataRow(2026)]
    [DataRow(2050)]
    public void AcceptableIntoYear_WhenAbsentOrInRange_IsValid(int? year)
    {
        _validator.Validate(new PaginatedRequestDto { AcceptableIntoYear = year }).IsValid.Should().BeTrue();
    }

    [TestMethod]
    [DataRow(0)]
    [DataRow(-1)]
    [DataRow(2022)]
    [DataRow(2051)]
    [DataRow(int.MaxValue)]
    public void AcceptableIntoYear_WhenOutOfRange_IsInvalid(int year)
    {
        var result = _validator.Validate(new PaginatedRequestDto { AcceptableIntoYear = year });

        result.Errors.Should().ContainSingle().Which.PropertyName.Should().Be(nameof(PaginatedRequestDto.AcceptableIntoYear));
    }
}
