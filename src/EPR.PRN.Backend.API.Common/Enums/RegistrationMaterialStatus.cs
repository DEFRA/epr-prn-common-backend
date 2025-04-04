using System.ComponentModel;

namespace EPR.PRN.Backend.API.Common.Enums;
public enum RegistrationMaterialStatus
{
    GRANTED = 1,
    REFUSED = 2
}
public enum ApplicationOrganisationType
{
    Reprocessor,
    Exporter
}
public enum RegulatorTaskStatus
{
    [Description("Not Started")]
    NotStarted,
    [Description("Started")]
    Started,
    [Description("Completed")]
    Completed,
    [Description("Can Not Start Yet")]
    CannotStartYet,
    [Description("Queried")]
    Queried,
    [Description("Approved")]
    Approved
}