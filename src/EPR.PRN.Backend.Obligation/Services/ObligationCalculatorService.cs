
using EPR.PRN.Backend.Obligation.Enums;
using EPR.PRN.Backend.Obligation.Interfaces;

namespace EPR.PRN.Backend.Obligation.Services
{
    public class ObligationCalculatorService
    {
        private readonly IRecyclingTargetDataService _recyclingTargetDataService;

        public ObligationCalculatorService(IRecyclingTargetDataService recyclingTargetDataService)
        {
            _recyclingTargetDataService = recyclingTargetDataService;
        }

        public async Task ProcessApprovedPomData(int year, MaterialType materialType, int tonnage)
        {
            // last check date (where is this coming from?)
            // POM data(TBC)
            var recyclingTargets = await _recyclingTargetDataService.GetRecyclingTargetsAsync();

            switch (materialType)
            {
                case MaterialType.Glass:
                    var (remelt, remainder) = CalculateGlass(recyclingTargets[year][materialType], recyclingTargets[year][MaterialType.GlassRemelt], tonnage);
                    break;
                default:
                    var calculation = Calculate(recyclingTargets[year][materialType], tonnage);
                    break;
            }

            // store the calculations
        }

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
