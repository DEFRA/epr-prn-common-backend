using EPR.PRN.Backend.API.Validators;
using EPR.PRN.Backend.Data.DTO;
using FluentValidation.TestHelper;

namespace EPR.PRN.Backend.API.UnitTests.Validators
{
    [TestClass]
    public class RegistrationReprocessingIORequestValidatorTests
    {
        private RegistrationReprocessingIORequestValidator _validator;

        [TestInitialize]
        public void Setup()
        {
            _validator = new RegistrationReprocessingIORequestValidator();
        }

        [TestMethod]
        public void ShouldNotHaveError_WhenTypeOfSuppliersIsSet()
        {
            var model = new RegistrationReprocessingIORequestDto { TypeOfSuppliers = "Supplier" };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveAnyValidationErrors();
        }

        [TestMethod]
        public void ShouldHaveError_WhenTypeOfSuppliersIsNotSet()
        {
            var model = new RegistrationReprocessingIORequestDto { TypeOfSuppliers = string.Empty };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.TypeOfSuppliers);
        }

        [TestMethod]
        public void ShouldNotHaveError_WhenTypeOfSuppliersIsExactly500Characters()
        {
            var value = new string('A', 500);
            var model = new RegistrationReprocessingIORequestDto { TypeOfSuppliers = value };

            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.TypeOfSuppliers);
        }

        [TestMethod]
        public void ShouldHaveError_WhenTypeOfSuppliersExceeds500Characters()
        {
            var value = new string('A', 501);
            var model = new RegistrationReprocessingIORequestDto { TypeOfSuppliers = value };

            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.TypeOfSuppliers)
                  .WithErrorMessage("Type of suppliers must not exceed 500 characters");
        }
    }
}