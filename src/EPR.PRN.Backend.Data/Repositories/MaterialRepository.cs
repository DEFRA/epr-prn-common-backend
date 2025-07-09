using Azure.Core;
using System;
using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.DTO;
using EPR.PRN.Backend.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EPR.PRN.Backend.Data.Repositories
{
    public class MaterialRepository(EprContext context) : IMaterialRepository
    {
		public async Task<IEnumerable<Material>> GetAllMaterials()
		{
			return await context.Material
								.AsNoTracking()
								.Include(m => m.PrnMaterialMappings)
								.ToListAsync();
		}

        public async Task<RegistrationMaterialContact> UpsertRegistrationMaterialContact(Guid registrationMaterialId, Guid userId)
        {
            var registrationMaterial = await context.RegistrationMaterials
                .Include(rm => rm.RegistrationMaterialContact)
                .SingleOrDefaultAsync(rm => rm.ExternalId == registrationMaterialId);

            if (registrationMaterial is null)
            {
                throw new KeyNotFoundException("Registration material not found.");
            }

            var registrationMaterialContact = registrationMaterial.RegistrationMaterialContact;

            if (registrationMaterialContact is null)
            {
                registrationMaterialContact = new RegistrationMaterialContact
                {
                    ExternalId = Guid.NewGuid(),
                    RegistrationMaterialId = registrationMaterial.Id,
                    UserId = userId
                };

                await context.RegistrationMaterialContacts.AddAsync(registrationMaterialContact);
            }
            else
            {
                registrationMaterialContact.UserId = userId;
            }

            await context.SaveChangesAsync();

            return registrationMaterialContact;
        }

        public async Task UpsertRegistrationReprocessingDetailsAsync(Guid registrationMaterialId, RegistrationReprocessingIO registrationReprocessingIO)
        {
            var registrationMaterial = await context.RegistrationMaterials
               .Include(rm => rm.RegistrationReprocessingIO!)
                .ThenInclude(io => io.RegistrationReprocessingIORawMaterialOrProducts)
               .SingleOrDefaultAsync(rm => rm.ExternalId == registrationMaterialId);

            if (registrationMaterial is null)
            {
                throw new KeyNotFoundException("Registration material not found.");
            }

            var existingRegistrationReprocessingIO = registrationMaterial.RegistrationReprocessingIO?.SingleOrDefault();

            if (existingRegistrationReprocessingIO is null)
            {
                registrationReprocessingIO.RegistrationMaterialId = registrationMaterial.Id;
                registrationReprocessingIO.ExternalId = Guid.NewGuid();
                if (registrationReprocessingIO.RegistrationReprocessingIORawMaterialOrProducts != null)
                {
                    foreach (var item in registrationReprocessingIO.RegistrationReprocessingIORawMaterialOrProducts)
                    {
                        item.ExternalId = Guid.NewGuid();
                    }
                }
                await context.RegistrationReprocessingIO.AddAsync(registrationReprocessingIO);
            }
            else
            {
                existingRegistrationReprocessingIO.TypeOfSuppliers = registrationReprocessingIO.TypeOfSuppliers;
                existingRegistrationReprocessingIO.ReprocessingPackagingWasteLastYearFlag = registrationReprocessingIO.ReprocessingPackagingWasteLastYearFlag;
                existingRegistrationReprocessingIO.TotalInputs = registrationReprocessingIO.TotalInputs;
                existingRegistrationReprocessingIO.TotalOutputs = registrationReprocessingIO.TotalOutputs;
                existingRegistrationReprocessingIO.UKPackagingWasteTonne = registrationReprocessingIO.UKPackagingWasteTonne;
                existingRegistrationReprocessingIO.NonUKPackagingWasteTonne = registrationReprocessingIO.NonUKPackagingWasteTonne;
                existingRegistrationReprocessingIO.NotPackingWasteTonne = registrationReprocessingIO.NotPackingWasteTonne;
                existingRegistrationReprocessingIO.ContaminantsTonne = registrationReprocessingIO.ContaminantsTonne;
                existingRegistrationReprocessingIO.SenttoOtherSiteTonne = registrationReprocessingIO.SenttoOtherSiteTonne;
                existingRegistrationReprocessingIO.ProcessLossTonne = registrationReprocessingIO.ProcessLossTonne;
                existingRegistrationReprocessingIO.PlantEquipmentUsed = registrationReprocessingIO.PlantEquipmentUsed;

                if (registrationReprocessingIO.RegistrationReprocessingIORawMaterialOrProducts != null)
                {
                    context.RegistrationReprocessingIORawMaterialOrProducts
                        .RemoveRange(existingRegistrationReprocessingIO.RegistrationReprocessingIORawMaterialOrProducts ?? new List<RegistrationReprocessingIORawMaterialOrProducts>());

                    foreach (var newItem in registrationReprocessingIO.RegistrationReprocessingIORawMaterialOrProducts)
                    {
                        newItem.ExternalId = Guid.NewGuid();
                        newItem.RegistrationReprocessingIOId = existingRegistrationReprocessingIO.Id;
                        context.RegistrationReprocessingIORawMaterialOrProducts.Add(newItem);
                    }
                }
            }

            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Material>> GetMaterialsByRegistrationIdQuery(Guid registrationId)
        {
            var materialsInRegistrationMaterial = await context.RegistrationMaterials.Where(m => m.Registration.ExternalId == registrationId)
                                                                                     .Select(m => m.MaterialId)
                                                                                     .ToListAsync();

            var materialsNotInRegistrationMaterial = await context.Material.Where(m => !materialsInRegistrationMaterial.Contains(m.Id)).ToListAsync();

            return materialsNotInRegistrationMaterial;
        }
    }
}
