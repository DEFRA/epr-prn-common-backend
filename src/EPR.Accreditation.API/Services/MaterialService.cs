using EPR.Accreditation.API.Common.Dtos;
using EPR.Accreditation.API.Repositories.Interfaces;
using EPR.Accreditation.API.Services.Interfaces;

namespace EPR.Accreditation.API.Services
{
    public class MaterialService : IMaterialService
    {
        protected readonly IRepository _repository;

        public MaterialService(IRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<IEnumerable<Material>> GetMaterialList()
        {
            return await _repository.GetMaterials();
        }
    }
}
