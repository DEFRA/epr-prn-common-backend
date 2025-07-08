using Azure.Core;
using System;
using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.DTO;
using EPR.PRN.Backend.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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

        public async Task SaveOverseasReprocessingSites(UpdateOverseasAddressDto overseasAddressSubmission)
        {
            await using var transaction = await context.Database.BeginTransactionAsync();

            try
            {
                await SaveOverseasSitesTransaction(overseasAddressSubmission);
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("SaveOverseasReprocessingSites trandsaction failed", ex);
            }
        }

        public async Task SaveOverseasSitesTransaction(UpdateOverseasAddressDto overseasAddressSubmission)
        {
            var overseasAddressIds = overseasAddressSubmission.OverseasAddresses.Select(x => x.ExternalId).ToList();
            var registrationMaterial = await GetRegistrationMaterial(overseasAddressSubmission.RegistrationMaterialId);
            var registrationId = registrationMaterial.RegistrationId;
            var registrationMaterialId = registrationMaterial.Id;

            await DeleteOverseasReprocessingSites(overseasAddressIds, registrationId);

            var overseasAddressesAfterDelete = await context.OverseasAddress
                .Where(x => x.RegistrationId == registrationId)
                .ToListAsync();

            await UpdateOverseasReprocessingSites(overseasAddressesAfterDelete, overseasAddressSubmission.OverseasAddresses);

            var overseasAddressesAfterUpdate = await context.OverseasAddress
               .Where(x => x.RegistrationId == registrationId)
               .ToListAsync();

            await CreateOverseasReprocessingSites(overseasAddressSubmission.OverseasAddresses, overseasAddressesAfterUpdate, registrationId, registrationMaterialId);
        }


        private async Task DeleteOverseasReprocessingSites(List<Guid> overseasAddressIds, int registrationId)
        {
            var overseasAddresses = await context.OverseasAddress
                .Where(x => x.RegistrationId == registrationId)
                .ToListAsync();

            var overseasAddressesToDeleteExternalId = overseasAddresses.Where(x => x.RegistrationId == registrationId
                        && !overseasAddressIds.Contains(x.ExternalId)).Select(x => x.ExternalId).ToList();

            var overseasAddressesToDeleteId = overseasAddresses.Where(x => x.RegistrationId == registrationId
                        && !overseasAddressIds.Contains(x.ExternalId)).Select(x => x.Id).ToList();

            if (overseasAddressesToDeleteExternalId.Count > 0)
            {
                var overseasSitesToDelete = await context.OverseasAddress
                .Where(x => overseasAddressesToDeleteExternalId.Contains(x.ExternalId))
                .ToListAsync();

                var overseasMaterialReprocessingSitesToDelete = await context.OverseasMaterialReprocessingSite
                    .Where(x => overseasAddressesToDeleteId.Contains(x.Id) && x.RegistrationMaterialId == registrationId)
                    .ToListAsync();

                context.OverseasAddress.RemoveRange(overseasSitesToDelete);
                context.OverseasMaterialReprocessingSite.RemoveRange(overseasMaterialReprocessingSitesToDelete);
                await context.SaveChangesAsync();
            }
        }

        private async Task UpdateOverseasReprocessingSites(List<OverseasAddress> overseasAddresses, List<OverseasAddressDto> updatedAddresses)
        {
            foreach (var overseasAddress in overseasAddresses)
            {
                var updatedAddress = updatedAddresses.Where(x => x.ExternalId == overseasAddress.ExternalId)
                    .FirstOrDefault()!;
                overseasAddress.AddressLine1 = updatedAddress.AddressLine1;
                overseasAddress.AddressLine2 = updatedAddress.AddressLine2;
                overseasAddress.CityOrTown = updatedAddress.CityOrTown;
                overseasAddress.StateProvince = updatedAddress.StateProvince;
                overseasAddress.PostCode = updatedAddress.PostCode;
                overseasAddress.SiteCoordinates = updatedAddress.SiteCoordinates;
                overseasAddress.OrganisationName = updatedAddress.OrganisationName;
                overseasAddress.UpdatedBy = updatedAddress.UpdatedBy;
                overseasAddress.UpdatedOn = DateTime.UtcNow;

                foreach (var contact in updatedAddress.OverseasAddressContacts)
                {
                    var existingContact = overseasAddress.OverseasAddressContacts.FirstOrDefault(c => c.CreatedBy == contact.CreatedBy);
                    if (existingContact != null)
                    {
                        existingContact.FullName = contact.FullName;
                        existingContact.Email = contact.Email;
                        existingContact.PhoneNumber = contact.PhoneNumber;
                    }
                    else
                    {
                        overseasAddress.OverseasAddressContacts.Add(new OverseasAddressContact
                        {
                            FullName = contact.FullName,
                            Email = contact.Email,
                            PhoneNumber = contact.PhoneNumber,
                            CreatedBy = contact.CreatedBy
                        });
                    }
                }
                foreach (var wasteCode in updatedAddress.OverseasAddressWasteCodes)
                {
                    var existingWasteCode = overseasAddress.OverseasAddressWasteCodes.FirstOrDefault(wc => wc.ExternalId == wasteCode.ExternalId);
                    if (existingWasteCode != null)
                    {
                        existingWasteCode.CodeName = wasteCode.CodeName;
                    }
                    else
                    {
                        overseasAddress.OverseasAddressWasteCodes.Add(new OverseasAddressWasteCode
                        {
                            CodeName = wasteCode.CodeName,
                        });
                    }
                }
            }
            await context.SaveChangesAsync();
        }

        private async Task CreateOverseasReprocessingSites(List<OverseasAddressDto> overseasAddresses, List<OverseasAddress> overseasAddressesAfterDb, int registrationId, int registrationMaterialId)
        {
            var overseasAddressesToCreate = overseasAddresses
                .Where(x => !overseasAddressesAfterDb.Any(y => y.ExternalId == x.ExternalId))
                .Select(x => new OverseasAddress
                {
                    ExternalId = Guid.NewGuid(),
                    OrganisationName = x.OrganisationName,
                    AddressLine1 = x.AddressLine1,
                    AddressLine2 = x.AddressLine2,
                    CityOrTown = x.CityOrTown,
                    StateProvince = x.StateProvince,
                    PostCode = x.PostCode,
                    SiteCoordinates = x.SiteCoordinates,
                    RegistrationId = registrationId,
                    CreatedBy = x.CreatedBy,
                    CountryId = context.LookupCountries
                        .Where(c => c.Name == x.CountryName)
                        .Select(c => c.Id)
                        .FirstOrDefault(),
                    OverseasAddressContacts = [.. x.OverseasAddressContacts.Select(c => new OverseasAddressContact
                {
                    FullName = c.FullName,
                    Email = c.Email,
                    PhoneNumber = c.PhoneNumber,
                    CreatedBy = c.CreatedBy
                })],
                    OverseasAddressWasteCodes = [.. x.OverseasAddressWasteCodes.Select(wc => new OverseasAddressWasteCode
                {
                    ExternalId = Guid.NewGuid(),
                    CodeName = wc.CodeName,                    
                })]
                }).ToList();

            var overseasMaterialReprocessingSites = overseasAddressesToCreate.Select(x => new OverseasMaterialReprocessingSite
            {
                OverseasAddressId = x.Id,
                RegistrationMaterialId = registrationMaterialId,
                ExternalId = Guid.NewGuid(),
            }).ToList();

            await context.OverseasAddress.AddRangeAsync(overseasAddressesToCreate);
            await context.OverseasMaterialReprocessingSite.AddRangeAsync(overseasMaterialReprocessingSites);
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

        private async Task<RegistrationMaterial> GetRegistrationMaterial(Guid registrationMaterialId)
        {
            var registrationMaterial = await context.RegistrationMaterials.Where(x => x.ExternalId == registrationMaterialId).SingleOrDefaultAsync();

            if (registrationMaterial == null)
                throw new KeyNotFoundException("RegistrationMaterial not found");

            return registrationMaterial;
        }
    }
}
