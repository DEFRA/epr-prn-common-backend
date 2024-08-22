using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Data.Interfaces;
using EPR.PRN.Backend.Obligation.Enums;
using EPR.PRN.Backend.Obligation.Interfaces;

namespace EPR.PRN.Backend.Obligation.Services
{
    public class RecyclingTargetDataService : IRecyclingTargetDataService
    {
        private Dictionary<int, Dictionary<MaterialType, double>> _recyclingTargets;
        private readonly IRecyclingTargetRepository _recyclingTargetRepository;

        public RecyclingTargetDataService(IRecyclingTargetRepository recyclingTargetRepository)
        {
            _recyclingTargetRepository = recyclingTargetRepository;
            _recyclingTargets = [];
        }

        public async Task<Dictionary<int, Dictionary<MaterialType, double>>> GetRecyclingTargetsAsync()
        {
            if (_recyclingTargets.Count == 0)
            {
                var recyclingTargets = (await _recyclingTargetRepository.GetAllAsync()).ToList();
                _recyclingTargets = recyclingTargets.Select(x => new KeyValuePair<int, Dictionary<MaterialType, double>>(x.Year, TransformTargets(x))).ToDictionary();
            }

            return _recyclingTargets;
        }

        private static Dictionary<MaterialType, double> TransformTargets(RecyclingTarget recyclingTarget)
        {
            var dictionary = new Dictionary<MaterialType, double>(7)
            {
                { MaterialType.Aluminium, recyclingTarget.AluminiumTarget },
                { MaterialType.Glass, recyclingTarget.GlassTarget },
                { MaterialType.GlassRemelt, recyclingTarget.GlassRemeltTarget },
                { MaterialType.Paper, recyclingTarget.PaperTarget },
                { MaterialType.Plastic, recyclingTarget.PlasticTarget },
                { MaterialType.Steel, recyclingTarget.SteelTarget },
                { MaterialType.Wood, recyclingTarget.WoodTarget }
            };

            return dictionary;
        }
    }
}
