using EPR.PRN.Backend.API.Constants;
using EPR.PRN.Backend.API.Queries;
using FluentValidation;

namespace EPR.PRN.Backend.API.Validators.Registration;

public class GetRegistrationsOverviewByOrgIdValidator : AbstractValidator<GetRegistrationsOverviewByOrgIdQuery>
{
    public GetRegistrationsOverviewByOrgIdValidator()
    {
        RuleFor(x => x.OrganisationId)
            .NotEmpty()
            .NotEqual(Guid.Empty)
            .WithMessage(ValidationMessages.RegistrationOrganisationIdRequired);
    }
}