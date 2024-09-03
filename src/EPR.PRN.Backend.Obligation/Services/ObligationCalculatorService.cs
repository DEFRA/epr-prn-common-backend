using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Data.Interfaces;
using EPR.PRN.Backend.Obligation.DTO;
using EPR.PRN.Backend.Obligation.Interfaces;
using EPR.PRN.Backend.Obligation.Models;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.Obligation.Services
{
    public class ObligationCalculatorService : IObligationCalculatorService
    {
        private readonly IObligationCalculationRepository _obligationCalculationRepository;

        public ObligationCalculatorService(IObligationCalculationRepository obligationCalculationRepository)
        {
            _obligationCalculationRepository = obligationCalculationRepository;
        }

        [ExcludeFromCodeCoverage]
        public async Task ProcessApprovedPomData(Guid id, SubmissionCalculationRequest request)
        {
            // Pom Data will be provided by the function app request - still work in progress

            var calculations = new List<ObligationCalculation>();

            await _obligationCalculationRepository.AddObligationCalculation(calculations);
        }

        public async Task<List<ObligationCalculationDto>?> GetObligationCalculationByOrganisationId(int id)
        {
            var result = await _obligationCalculationRepository.GetObligationCalculationByOrganisationId(id);

            return result?.Select(item => new ObligationCalculationDto
            {
                MaterialName = item.MaterialName,
                MaterialObligationValue = item.MaterialObligationValue,
                OrganisationId = item.OrganisationId,
                Year = item.Year
            }).ToList();
        }
    }
}
