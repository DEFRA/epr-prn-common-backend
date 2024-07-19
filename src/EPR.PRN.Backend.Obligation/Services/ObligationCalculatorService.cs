
namespace EPR.PRN.Backend.Obligation.Services
{
    public class ObligationCalculatorService
    {
        public int Calculate(double target, int tonnage)
        {
            return (int)Math.Round(target * tonnage, 0, MidpointRounding.AwayFromZero);
        }
    }
}
