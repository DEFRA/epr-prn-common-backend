using System.ComponentModel.DataAnnotations;
using AwesomeAssertions;
using EPR.PRN.Backend.API.Common.Dto;

namespace EPR.PRN.Backend.API.UnitTests.Dto;

[TestClass]
public class PaginatedRequestDtoTests
{
    private static List<ValidationResult> Validate(PaginatedRequestDto dto)
    {
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(dto, new ValidationContext(dto), results, validateAllProperties: true);

        return results;
    }

    [TestMethod]
    [DataRow(null)]
    [DataRow(2023)]
    [DataRow(2026)]
    [DataRow(2050)]
    public void AcceptableIntoYear_WhenAbsentOrInRange_IsValid(int? year)
    {
        Validate(new PaginatedRequestDto { AcceptableIntoYear = year }).Should().BeEmpty();
    }

    [TestMethod]
    [DataRow(0)]
    [DataRow(-1)]
    [DataRow(2022)]
    [DataRow(2051)]
    [DataRow(int.MaxValue)]
    public void AcceptableIntoYear_WhenOutOfRange_IsInvalid(int year)
    {
        var results = Validate(new PaginatedRequestDto { AcceptableIntoYear = year });

        results.Should().ContainSingle().Which.MemberNames.Should().Contain(nameof(PaginatedRequestDto.AcceptableIntoYear));
    }
}
