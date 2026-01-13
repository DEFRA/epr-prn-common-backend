namespace EprPrnIntegration.Common.Models.Rpd;

public static class AgencyName
{
    public const string EnvironmentAgency = "Environment Agency";
    public const string NaturalResourcesWales = "Natural Resources Wales";
    public const string NorthernIrelandEnvironmentAgency = "Northern Ireland Environment Agency";
    public const string ScottishEnvironmentProtectionAge = "Scottish Environment Protection Age";
    private static readonly List<string> _all =
    [
        EnvironmentAgency,
        NaturalResourcesWales,
        NorthernIrelandEnvironmentAgency,
        ScottishEnvironmentProtectionAge,
    ];

    public static List<string> GetAll()
    {
        return _all;
    }
}
