using System.Diagnostics.CodeAnalysis;
using EPR.PRN.Backend.API.Common.Enums;

namespace EPR.PRN.Backend.API.Dto.Regulator;

[ExcludeFromCodeCoverage]
public class MaterialsPermitTypeDto : IdNamePairDto

{
    public bool HasPermitNumber => ((MaterialPermitType)Id != MaterialPermitType.WasteExemption && (MaterialPermitType)Id != MaterialPermitType.None);
}
