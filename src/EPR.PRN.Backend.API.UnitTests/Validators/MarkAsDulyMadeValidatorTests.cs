using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Constants;
using EPR.PRN.Backend.API.Validators;

namespace EPR.PRN.Backend.API.UnitTests.Validators
{
    [TestClass]
    public class MarkAsDulyMadeValidatorTests
    {
        private MarkAsDulyMadeValidator _validator;

        [TestInitialize]
        public void TestInitialize()
        {
            _validator = new MarkAsDulyMadeValidator();
        }

        [TestMethod]
        public void Should_Not_Have_Validation_Errors_When_Command_Is_Valid()
        {
            var command = new RegistrationMaterialsMarkAsDulyMadeCommand
            {
                RegistrationMaterialId = 1,
                DulyMadeDate = DateTime.UtcNow.Date,
                DeterminationDate = DateTime.UtcNow.Date.AddDays(84),
                DulyMadeBy = Guid.NewGuid()
            };

            var result = _validator.Validate(command);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Should_Have_Error_When_RegistrationMaterialId_Is_Zero()
        {
            var command = new RegistrationMaterialsMarkAsDulyMadeCommand
            {
                RegistrationMaterialId = 0
            };

            var result = _validator.Validate(command);

            Assert.IsTrue(result.Errors.Any(e =>
                e.PropertyName == "RegistrationMaterialId" &&
                e.ErrorMessage == ValidationMessages.RegistrationMaterialIdGreaterThanZero));
        }

        [TestMethod]
        public void Should_Have_Error_When_DulyMadeDate_Is_MinValue()
        {
            var command = new RegistrationMaterialsMarkAsDulyMadeCommand
            {
                DulyMadeDate = DateTime.MinValue
            };

            var result = _validator.Validate(command);

            Assert.IsTrue(result.Errors.Any(e =>
                e.PropertyName == "DulyMadeDate" &&
                e.ErrorMessage == ValidationMessages.InvalidDulyMadeDate));
        }

        [TestMethod]
        public void Should_Have_Error_When_DeterminationDate_Is_MinValue()
        {
            var command = new RegistrationMaterialsMarkAsDulyMadeCommand
            {
                DulyMadeDate = DateTime.UtcNow.Date,
                DeterminationDate = DateTime.MinValue
            };

            var result = _validator.Validate(command);

            Assert.IsTrue(result.Errors.Any(e =>
                e.PropertyName == "DeterminationDate" &&
                e.ErrorMessage == ValidationMessages.InvalidDeterminationDate));
        }

        [TestMethod]
        public void Should_Have_Error_When_DeterminationDate_Is_Less_Than_12_Weeks_From_DulyMadeDate()
        {
            var command = new RegistrationMaterialsMarkAsDulyMadeCommand
            {
                DulyMadeDate = DateTime.UtcNow.Date,
                DeterminationDate = DateTime.UtcNow.Date.AddDays(83)
            };

            var result = _validator.Validate(command);

            Assert.IsTrue(result.Errors.Any(e =>
                e.PropertyName == "DeterminationDate" &&
                e.ErrorMessage == ValidationMessages.DeterminationDate12WeekRule));
        }

        [TestMethod]
        public void Should_Not_Have_Error_When_DeterminationDate_Is_Exactly_12_Weeks_From_DulyMadeDate()
        {
            var command = new RegistrationMaterialsMarkAsDulyMadeCommand
            {
                DulyMadeDate = DateTime.UtcNow.Date,
                DeterminationDate = DateTime.UtcNow.Date.AddDays(84)
            };

            var result = _validator.Validate(command);

            Assert.IsFalse(result.Errors.Any(e => e.PropertyName == "DeterminationDate"));
        }

        [TestMethod]
        public void Should_Have_Error_When_DulyMadeBy_Is_Empty()
        {
            var command = new RegistrationMaterialsMarkAsDulyMadeCommand
            {
                DulyMadeBy = Guid.Empty
            };

            var result = _validator.Validate(command);

            Assert.IsTrue(result.Errors.Any(e =>
                e.PropertyName == "DulyMadeBy" &&
                e.ErrorMessage == ValidationMessages.DulyMadeByRequired));
        }
    }
}
