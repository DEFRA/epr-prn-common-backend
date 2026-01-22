using EPR.PRN.Backend.Obligation.Interfaces;

namespace EPR.PRN.Backend.Obligation.Services
{
	public class MaterialCalculationService : IMaterialCalculationService
	{
		public int Calculate(double target, int tonnage, int? numberOfDaysObligated, int complianceYear)
		{
			int daysInYear = DateTime.IsLeapYear(complianceYear) ? 366 : 365;
			decimal scale = numberOfDaysObligated is not null
				? (decimal)numberOfDaysObligated.Value / daysInYear
				: 1m;

			int result = (int)Math.Ceiling((decimal)target * tonnage);
			return (int)Math.Ceiling(result * scale);
		}

		public (int remelt, int remainder) CalculateGlass(double target, double remeltTarget, int tonnage, int? numberOfDaysObligated, int complianceYear)
		{
			int daysInYear = DateTime.IsLeapYear(complianceYear) ? 366 : 365;
			decimal scale = numberOfDaysObligated is not null
				? (decimal)numberOfDaysObligated.Value / daysInYear
				: 1m;

			var initialTarget = (decimal)target * tonnage;
			var glassRemelt = (int)Math.Ceiling((decimal)remeltTarget * initialTarget);
			var glassTotal = (int)Math.Ceiling(initialTarget);
			var glassOther = glassTotal - glassRemelt;

			return ((int)Math.Ceiling(glassRemelt * scale), (int)Math.Ceiling(glassOther * scale));
		}
	}
}
