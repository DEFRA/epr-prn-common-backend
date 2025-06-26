using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Constants;
using FluentValidation;

namespace EPR.PRN.Backend.API.Validators.RegistrationReprocessingIO;

public class CreateReprocessorOutputValidator : AbstractValidator<CreateReprocessorOutputCommand>
{
    public CreateReprocessorOutputValidator()
    {
        RuleFor(x => x.RegistrationMaterialId)
             .GreaterThan(0)
             .WithMessage("Registration material ID must be greater than 0.");

        RuleFor(x => x.SentToOtherSiteTonnes)
            .GreaterThanOrEqualTo(0).WithMessage("Sent to other site tonnes cannot be negative.");

        RuleFor(x => x.ContaminantTonnes)
            .GreaterThanOrEqualTo(0).WithMessage("Contaminant tonnes cannot be negative.");

        RuleFor(x => x.ProcessLossTonnes)
            .GreaterThanOrEqualTo(0).WithMessage("Process loss tonnes cannot be negative.");

        RuleFor(x => x.TotalOutputTonnes)
            .GreaterThanOrEqualTo(0).WithMessage("Total output tonnes cannot be negative.");

        RuleForEach(x => x.RawMaterialorProducts)
            .SetValidator(new RawMaterialorProductsValidator());
    }
}
