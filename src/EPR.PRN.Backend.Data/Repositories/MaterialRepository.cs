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

        public async Task<RegistrationReprocessingIOResponseDto> UpsertRegistrationReprocessingDetailsAsync(Guid registrationMaterialId, RegistrationReprocessingIO registrationReprocessingIO)
        {
            var registrationMaterial = await context.RegistrationMaterials
               .Include(rm => rm.RegistrationReprocessingIO)
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
                await context.RegistrationReprocessingIO.AddAsync(registrationReprocessingIO);
            }
            else
            {
                context.RegistrationReprocessingIO.Update(registrationReprocessingIO);
            }

            await context.SaveChangesAsync();

            return new RegistrationReprocessingIOResponseDto
            {
                Id = registrationReprocessingIO.Id,
            };
        }
    }
}
