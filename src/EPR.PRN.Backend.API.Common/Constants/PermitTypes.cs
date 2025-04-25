using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.Common.Constants;

[ExcludeFromCodeCoverage]
public static class PermitTypes
{
    public const string WasteExemption = "Waste Exemption";
    public const string PollutionPreventionAndControlPermit = "Pollution,Prevention and Contril(PPC) permit";
    public const string WasteManagementLicense = "Waste Management License";
    public const string InstallationPermit = "Installation Permit";
    public const string EnvironmentalPermitOrWasteManagementLicense = "Environmental permit or waste management license";
}