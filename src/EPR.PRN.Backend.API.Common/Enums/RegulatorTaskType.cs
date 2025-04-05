using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPR.PRN.Backend.API.Common.Enums;
public enum RegulatorTaskType
{
    [Description("Site Address and Contact Details")]
    SiteAddressAndContactDetails = 1,

    [Description("Waste Licenses, Permits, and Exemptions")]
    WasteLicensesPermitsAndExemptions = 2,

    [Description("Reprocessing Inputs and Outputs")]
    ReprocessingInputsAndOutputs = 3,

    [Description("Sampling and Inspection Plan")]
    SamplingAndInspectionPlan = 4,

    [Description("Registration Duly Made")]
    RegistrationDulyMade = 5,

    [Description("Assign Officer")]
    AssignOfficer = 6,

    [Description("Materials Authorised on Site")]
    MaterialsAuthorisedOnSite = 7,

    [Description("Material Details and Contact")]
    MaterialDetailsAndContact = 8,

    [Description("Overseas Reprocessor and Interim Site Details")]
    nOverseasReprocessorAndInterimSiteDetails = 9,

    [Description("Business Address")]
    BusinessAddress = 10
}


