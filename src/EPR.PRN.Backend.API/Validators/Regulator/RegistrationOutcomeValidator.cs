﻿using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Constants;
using FluentValidation;

namespace EPR.PRN.Backend.API.Validators.Regulator;

public class RegistrationOutcomeValidator : AbstractValidator<RegistrationMaterialsOutcomeCommand>
{
    public RegistrationOutcomeValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage(ValidationMessages.RegistrationOutcomeIdRequired);

        RuleFor(x => x.Status)
            .IsInEnum()
            .WithMessage(ValidationMessages.InvalidRegistrationOutcomeStatus);

        RuleFor(x => x.Comments)
            .MaximumLength(500).WithMessage(ValidationMessages.RegistrationOutcomeCommentsMaxLength);

        RuleFor(x => x.Comments)
            .NotEmpty()
            .WithMessage(ValidationMessages.RegistrationOutcomeCommentsRequired)
            .When(x => x.Status == RegistrationMaterialStatus.Refused);
        RuleFor(x => x.User)
            .NotEmpty()
            .NotEqual(Guid.Empty)
            .WithMessage(ValidationMessages.RegistrationOutcomeUserRequired);
    }
}