using EPR.PRN.Backend.API.Common.Dto;
using FluentValidation;

namespace EPR.PRN.Backend.API.Validators;

/// <summary>
/// The application turns off DataAnnotations validation (see Startup), so the search request is checked here.
/// </summary>
public class PaginatedRequestDtoValidator : AbstractValidator<PaginatedRequestDto>
{
    public PaginatedRequestDtoValidator()
    {
        RuleFor(x => x.AcceptableIntoYear)
            .InclusiveBetween(
                PaginatedRequestDto.MinimumAcceptableIntoYear,
                PaginatedRequestDto.MaximumAcceptableIntoYear
            )
            .When(x => x.AcceptableIntoYear.HasValue);
    }
}
