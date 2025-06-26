using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces.Registrationreprocessingio;
using Microsoft.EntityFrameworkCore;

namespace EPR.PRN.Backend.Data.Repositories;

public class RegistrationReprocessorIORepository(EprContext eprContext) : IRegistrationReprocessorIORepository
{
    public async Task CreateReprocessorOutputAsync(Guid ReprocessorOutputId, int registrationMaterialId, decimal SentToOtherSiteTonnes,
    decimal ContaminantTonnes, decimal ProcessLossTonnes, decimal TotalOutput, List<KeyValuePair<string, decimal>> MaterialorProducts)
    {
        // Check if material exists
        var material = await eprContext.RegistrationMaterials.FirstOrDefaultAsync(rm => rm.Id == registrationMaterialId);
        if (material is null) throw new KeyNotFoundException("Material not found.");

        // Check if ReprocessorOutput exists
        var reprocessorOutput = await eprContext.RegistrationReprocessingIO
            .Include(r => r.RawMaterialorProducts) 
            .FirstOrDefaultAsync(rm => rm.ExternalId == ReprocessorOutputId &&  rm.RegistrationMaterialId== registrationMaterialId);

        if (reprocessorOutput is null) throw new KeyNotFoundException("ReprocessorIO not found.");

        // Update fields
      //  reprocessorOutput.RegistrationMaterialId = registrationMaterialId;
        reprocessorOutput.SenttoOtherSiteTonne = SentToOtherSiteTonnes;
        reprocessorOutput.ContaminantsTonne = ContaminantTonnes;
        reprocessorOutput.ProcessLossTonne = ProcessLossTonnes;
        reprocessorOutput.TotalOutputs = TotalOutput;

        // Clear existing RawMaterialorProducts if needed (optional based on requirement)
        reprocessorOutput.RawMaterialorProducts.Clear();

        // Add new RawMaterialorProducts
        foreach (var materialorProduct in MaterialorProducts)
        {
            reprocessorOutput.RawMaterialorProducts.Add(new ReprocessingIORawMaterialorProducts
            {
                ExternalID=new Guid(),
                RawMaterialNameorProductName = materialorProduct.Key,
                TonneValue = materialorProduct.Value,
                IsInput = false
            });
        }

        // Save changes to the database
        await eprContext.SaveChangesAsync();
    }

}
