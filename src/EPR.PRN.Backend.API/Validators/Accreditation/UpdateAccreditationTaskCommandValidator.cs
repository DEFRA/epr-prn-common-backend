﻿using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Common.Enums;
using FluentValidation;

namespace EPR.PRN.Backend.API.Validators.Accreditation
{
    public class UpdateAccreditationTaskCommandValidator : AbstractValidator<UpdateAccreditationTaskCommand>
    {
        public UpdateAccreditationTaskCommandValidator()
        {
            var allowedStatuses = new[] { TaskStatuses.Started, TaskStatuses.Completed };

            RuleFor(x => x.Status)
                .Must(status => allowedStatuses.Contains(status))
                .WithMessage("Invalid Status value");
        }
    }
}
