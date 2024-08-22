using EPR.PRN.Backend.Obligation.Enums;

namespace EPR.PRN.Backend.Obligation.Interfaces
{
    public interface IMaterialCalculationStrategyResolver
    {
        IMaterialCalculationStrategy? Resolve(MaterialType materialType);
    }
}
