using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EPR.PRN.Backend.Data.Repositories
{
    public class MaterialRepository(EprContext context) : IMaterialRepository
    {
		public async Task<IEnumerable<Material>> GetCalculableMaterials()
		{
			return await context.Material.Where(m => m.IsCaculable).ToListAsync();
		}
		public async Task<IEnumerable<Material>> GetVisibleToObligationMaterials()
		{
			return await context.Material.Where(m => m.IsVisibleToObligation).ToListAsync();
		}
	}
}
