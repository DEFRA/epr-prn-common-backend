using EPR.PRN.Backend.API.Constants;
using EPR.PRN.Backend.API.Queries;
using FluentValidation;

namespace EPR.PRN.Backend.API.Validators.Accreditation
{
    public class GetAccreditationsOverviewByOrgIdQueryValidator : AbstractValidator<GetAccreditationsOverviewByOrgIdQuery>
    {
        public GetAccreditationsOverviewByOrgIdQueryValidator()
        {
            RuleFor(x => x.OrganisationId)
            .NotEmpty()
            .NotEqual(Guid.Empty)
            .WithMessage(ValidationMessages.AccreditationOrganisationIdRequired);
        }
    }
}
