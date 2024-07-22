
namespace EPR.PRN.Backend.Obligation.Services
{
    public class ObligationCalculatorService
    {
        public int Calculate(double target, int tonnage)
        {
            return (int)Math.Round(target * tonnage, 0, MidpointRounding.AwayFromZero);
        }

        public (int remelt, int remainder) CalculateGlass(double target, double remeltTarget, int tonnage)
        {
            var initialTarget = (int)Math.Round(target * tonnage, 0, MidpointRounding.AwayFromZero);
            var remelt = (int)Math.Round(remeltTarget * initialTarget, 0, MidpointRounding.ToPositiveInfinity);

            return (remelt, initialTarget - remelt);
        }
    }
}
