using EPR.PRN.Backend.API.Commands;
using FluentValidation;

public class RawMaterialorProductsValidator : AbstractValidator<ReprocessorRawMaterialorProducts>
{
    public RawMaterialorProductsValidator()
    {
        RuleFor(x => x.RawMaterialNameorProductName)
            .MaximumLength(50)
            .WithMessage("Material or product name must not exceed 50 characters.");

            When(x => !string.IsNullOrWhiteSpace(x.RawMaterialNameorProductName), () =>
            {
                RuleFor(x => x.TonneValue)
                    .GreaterThan(0)
                    .WithMessage("Tonne value must be greater than zero when material or product name is provided.");

                RuleFor(x => x.TonneValue)
                    .InclusiveBetween(0.01M, 99999999.99M)
                    .WithMessage("Tonne value must be a valid decimal with maximum 10 digits in total and 2 decimal places.");

                RuleFor(x => x.TonneValue)
                    .Must(v => decimal.Round(v, 2) == v)
                    .WithMessage("Tonne value cannot have more than 2 decimal places.");
            });
    }
}
