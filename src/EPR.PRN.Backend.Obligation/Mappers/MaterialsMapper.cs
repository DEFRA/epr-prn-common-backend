using EPR.PRN.Backend.Data.DTO;

namespace EPR.PRN.Backend.Obligation.Mappers;

public static class MaterialsMapper
{

    public static IQueryable<EprnResultsDto> AdjustPrnMaterialNames(IQueryable<EprnResultsDto> prns)
    {
        List<EprnResultsDto> prnsWithAdjustedMaterialNames = [];
        foreach (var p in prns)
        {
            p.Eprn.MaterialName = MaterialsMapper.MapMaterialName(p.Eprn.MaterialName);
            prnsWithAdjustedMaterialNames.Add(p);
        }
        return prnsWithAdjustedMaterialNames.AsQueryable();
    }

    // the material names must match those on the material table
    public static string MapMaterialName(string materialName)
    {
        switch (materialName)
        {
            case "Glass Other":
                return "Glass";
            case "Glass Re-melt":
                return "GlassRemelt";
            case "Paper/board":
                return "Paper";
            default:
                return materialName;
        }
    }
}
