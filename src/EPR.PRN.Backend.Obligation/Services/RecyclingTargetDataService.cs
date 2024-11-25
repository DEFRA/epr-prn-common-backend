using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Data.Interfaces;
using EPR.PRN.Backend.Obligation.Helpers;
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
                var recyclingTargets = (await _recyclingTargetRepository.GetAllAsync()) ?? new List<RecyclingTarget>();
                _recyclingTargets = recyclingTargets.GroupBy(target => target.Year)
                                                    .ToDictionary(
                                                        group => group.Key,
                                                        group => TransformTargets(group.ToList())
                                                    );
            }

            return _recyclingTargets;
        }

        private static Dictionary<MaterialType, double> TransformTargets(List<RecyclingTarget> recyclingTargets)
        {
            var dictionary = new Dictionary<MaterialType, double>();

            foreach (var target in recyclingTargets)
            {
                var materialType = EnumHelper.ConvertStringToEnum<MaterialType>(target.MaterialNameRT);
                if (materialType == null)
                {
                    throw new ArgumentException($"Invalid material name '{target.MaterialNameRT}' in recycling targets.");
                }

                dictionary[materialType.Value] = target.Target;
            }

            return dictionary;
        }
    }
}
