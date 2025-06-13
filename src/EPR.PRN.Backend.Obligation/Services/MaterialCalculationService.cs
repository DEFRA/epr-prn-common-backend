using EPR.PRN.Backend.Obligation.Interfaces;

namespace EPR.PRN.Backend.Obligation.Services
{
    public class MaterialCalculationService : IMaterialCalculationService
    {
        public int Calculate(double target, int tonnage)
        {
            return (int)Math.Ceiling(target * tonnage);
        }

        public (int remelt, int remainder) CalculateGlass(double target, double remeltTarget, int tonnage)
        {
			var initialTarget = target * tonnage;
			var glassRemelt = (int)Math.Ceiling(remeltTarget * initialTarget);
			var glassTotal = (int)Math.Ceiling(initialTarget);
			var glassOther = glassTotal - glassRemelt;

			return (glassRemelt, glassOther);
		}
    }
}
