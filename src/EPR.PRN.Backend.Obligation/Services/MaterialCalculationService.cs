using EPR.PRN.Backend.Obligation.Interfaces;

namespace EPR.PRN.Backend.Obligation.Services
{
    public class MaterialCalculationService : IMaterialCalculationService
    {
        public int Calculate(double target, double tonnage)
        {
            return (int)Math.Round(target * tonnage, 0, MidpointRounding.AwayFromZero);
        }

        public (int remelt, int remainder) CalculateGlass(double target, double remeltTarget, double tonnage)
        {
            var initialTarget = (int)Math.Round(target * tonnage, 0, MidpointRounding.AwayFromZero);
            var remelt = (int)Math.Round(remeltTarget * initialTarget, 0, MidpointRounding.ToPositiveInfinity);

            return (remelt, initialTarget - remelt);
        }
    }
}
