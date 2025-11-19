using EPR.PRN.Backend.Obligation.Interfaces;

namespace EPR.PRN.Backend.Obligation.Services
{
    public class MaterialCalculationService : IMaterialCalculationService
    {
        public int Calculate(double target, int tonnage)
        {
            var result = (decimal)target * tonnage;
			return (int)Math.Ceiling(result);
		}

		public (int remelt, int remainder) CalculateGlass(double target, double remeltTarget, int tonnage)
        {
            var initialTarget = (decimal)target * tonnage;
            var glassRemelt = (int)Math.Ceiling((decimal)remeltTarget * initialTarget);
            var glassTotal = (int)Math.Ceiling(initialTarget);
            var glassOther = glassTotal - glassRemelt;

            return (glassRemelt, glassOther);
        }
    }
}
