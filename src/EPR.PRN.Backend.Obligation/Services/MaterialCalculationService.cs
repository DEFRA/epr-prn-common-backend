using EPR.PRN.Backend.Obligation.Interfaces;

namespace EPR.PRN.Backend.Obligation.Services
{
    public class MaterialCalculationService : IMaterialCalculationService
    {
        public int Calculate(double target, double tonnage)
        {
            return (int)Math.Ceiling(target * tonnage);
        }

        public (int remelt, int remainder) CalculateGlass(double target, double remeltTarget, double tonnage)
        {
            var initialTarget = (int)Math.Ceiling(target * tonnage);
            var remelt = (int)Math.Ceiling(remeltTarget * initialTarget);

            return (remelt, initialTarget - remelt);
        }
    }
}
