using System;
using System.Collections.Generic;
using EPR.PRN.Backend.API.Commands.ExporterJourney;
using EPR.PRN.Backend.API.Dto.ExporterJourney;
using EPR.PRN.Backend.API.Validators.ExportJourney;
using FluentValidation.TestHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EPR.PRN.Backend.API.UnitTests.Validators.ExportJourney
{
    [TestClass]
    public class UpdateCarrierBrokerDealerPermitsCommandValidatorTests
    {
        private UpdateCarrierBrokerDealerPermitsCommandValidator _validator;

        [TestInitialize]
        public void Setup()
        {
            _validator = new UpdateCarrierBrokerDealerPermitsCommandValidator();
        }

        [TestMethod]
        public void WasteCarrierBrokerDealerRegistration_ExceedsMaxLength_ShouldHaveValidationError()
        {
            var dto = new UpdateCarrierBrokerDealerPermitsDto
            {
                WasteCarrierBrokerDealerRegistration = new string('A', 17)
            };
            var command = new UpdateCarrierBrokerDealerPermitsCommand { Dto = dto };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Dto.WasteCarrierBrokerDealerRegistration);
        }

        [TestMethod]
        public void WasteLicenseOrPermitNumber_ExceedsMaxLength_ShouldHaveValidationError()
        {
            var dto = new UpdateCarrierBrokerDealerPermitsDto
            {
                WasteLicenseOrPermitNumber = new string('B', 21)
            };
            var command = new UpdateCarrierBrokerDealerPermitsCommand { Dto = dto };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Dto.WasteLicenseOrPermitNumber);
        }

        [TestMethod]
        public void PpcNumber_ExceedsMaxLength_ShouldHaveValidationError()
        {
            var dto = new UpdateCarrierBrokerDealerPermitsDto
            {
                PpcNumber = new string('C', 21)
            };
            var command = new UpdateCarrierBrokerDealerPermitsCommand { Dto = dto };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Dto.PpcNumber);
        }

        [TestMethod]
        public void WasteExemptionReference_Null_ShouldHaveValidationError()
        {
            var dto = new UpdateCarrierBrokerDealerPermitsDto
            {
                WasteExemptionReference = null
            };
            var command = new UpdateCarrierBrokerDealerPermitsCommand { Dto = dto };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Dto.WasteExemptionReference);
        }

        [TestMethod]
        public void WasteExemptionReference_TooManyItems_ShouldHaveValidationError()
        {
            var dto = new UpdateCarrierBrokerDealerPermitsDto
            {
                WasteExemptionReference = new List<string> { "1", "2", "3", "4", "5", "6" }
            };
            var command = new UpdateCarrierBrokerDealerPermitsCommand { Dto = dto };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Dto.WasteExemptionReference);
        }

        [TestMethod]
        public void WasteExemptionReference_ItemExceedsMaxLength_ShouldHaveValidationError()
        {
            var dto = new UpdateCarrierBrokerDealerPermitsDto
            {
                WasteExemptionReference = new List<string> { new string('D', 21) }
            };
            var command = new UpdateCarrierBrokerDealerPermitsCommand { Dto = dto };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor("Dto.WasteExemptionReference[0]");
        }

        [TestMethod]
        public void AllFields_Valid_ShouldNotHaveValidationError()
        {
            var dto = new UpdateCarrierBrokerDealerPermitsDto
            {
                WasteCarrierBrokerDealerRegistration = "ValidReg",
                WasteLicenseOrPermitNumber = "ValidPermit",
                PpcNumber = "ValidPpc",
                WasteExemptionReference = new List<string> { "Ref1", "Ref2" }
            };
            var command = new UpdateCarrierBrokerDealerPermitsCommand { Dto = dto };

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
