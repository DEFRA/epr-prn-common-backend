namespace EprPrnIntegration.Common.Models.Rpd;

public static class AgencyName
{
    public const string EnvironmentAgency = "Environment Agency";
    public const string NaturalResourcesWales = "Natural Resources Wales";
    public const string NorthernIrelandEnvironmentAgency = "Northern Ireland Environment Agency";
    public const string ScottishEnvironmentProtectionAgency = "Scottish Environment Protection Agency";
    private static readonly List<string> _all =
    [
        EnvironmentAgency,
        NaturalResourcesWales,
        NorthernIrelandEnvironmentAgency,
        ScottishEnvironmentProtectionAgency,
    ];

    public static List<string> GetAll()
    {
        return _all;
    }
}
