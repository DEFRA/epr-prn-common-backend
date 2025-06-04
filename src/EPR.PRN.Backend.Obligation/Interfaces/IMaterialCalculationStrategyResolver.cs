using EPR.PRN.Backend.API.Common.Enums;

namespace EPR.PRN.Backend.Obligation.Interfaces
{
    public interface IMaterialCalculationStrategyResolver
    {
        IMaterialCalculationStrategy? Resolve(MaterialType materialType);
    }
}
