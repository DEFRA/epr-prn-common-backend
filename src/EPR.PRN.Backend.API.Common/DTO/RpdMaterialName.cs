using System.Diagnostics.CodeAnalysis;

namespace EprPrnIntegration.Common.Models.Rpd;

[ExcludeFromCodeCoverage]
public static class RpdMaterialName
{
    public const string Aluminium = "Aluminium";
    public const string Fibre = "Fibre";
    public const string GlassRemelt = "Glass Re-melt";
    public const string GlassOther = "Glass Other";
    public const string PaperBoard = "Paper/board";
    public const string Plastic = "Plastic";
    public const string Steel = "Steel";
    public const string Wood = "Wood";

    private static readonly List<string> _all =
    [
        Aluminium,
        Fibre,
        GlassRemelt,
        GlassOther,
        PaperBoard,
        Plastic,
        Steel,
        Wood,
    ];

    public static List<string> GetAll()
    {
        return _all;
    }
}
