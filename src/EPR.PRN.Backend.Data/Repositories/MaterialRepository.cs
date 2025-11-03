using EPR.PRN.Backend.API.Common.Constants;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Common.Exceptions;
using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.DTO;
using EPR.PRN.Backend.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EPR.PRN.Backend.Data.Repositories
{
    public class MaterialRepository(EprContext context, ILogger<MaterialRepository> logger) : IMaterialRepository
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

        public async Task UpdateMaterialNotReprocessingReason(Guid registrationMaterialId, string materialNotReprocessingReason)
        {
            var registrationMaterial = await context.RegistrationMaterials
              .SingleOrDefaultAsync(rm => rm.ExternalId == registrationMaterialId);

            if (registrationMaterial is null)
            {
                throw new KeyNotFoundException("Registration material not found.");
            }

            registrationMaterial.ReasonforNotreg = materialNotReprocessingReason;

            await context.SaveChangesAsync();
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
                logger.LogError(ex, "An error occurred while saving overseas reprocessing sites.");
                throw; // Properly re-throw the exception without altering the stack trace
            }
        }

        private async Task SaveOverseasSitesTransaction(UpdateOverseasAddressDto overseasAddressSubmission)
        {
            var overseasAddressIds = overseasAddressSubmission.OverseasAddresses.Select(x => x.ExternalId).ToList();
            var registrationMaterial = await GetRegistrationMaterial(overseasAddressSubmission.RegistrationMaterialId);
            var registrationId = registrationMaterial.RegistrationId;
            var registrationMaterialId = registrationMaterial.Id;

            await DeleteOverseasReprocessingSites(overseasAddressIds, registrationId);

            var overseasAddressesAfterDelete = await context.OverseasAddress
                .Where(x => x.RegistrationId == registrationId)
                .Include(x => x.OverseasAddressContacts)
                .Include(x => x.OverseasAddressWasteCodes)
                .ToListAsync();

            await UpdateOverseasReprocessingSites(overseasAddressesAfterDelete, overseasAddressSubmission.OverseasAddresses);

            var overseasAddressesAfterUpdate = await context.OverseasAddress
               .Where(x => x.RegistrationId == registrationId)
               .ToListAsync();

            await CreateOverseasReprocessingSites(overseasAddressSubmission.OverseasAddresses, overseasAddressesAfterUpdate, registrationId, registrationMaterialId);

            await UpdateApplicationRegistrationTaskStatusAsync(ApplicantRegistrationTaskNames.OverseasReprocessorSiteDetails, registrationMaterial.ExternalId, TaskStatuses.Completed);
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
                var updatedAddress = updatedAddresses
                    .Find(x => x.ExternalId == overseasAddress.ExternalId)!;
                overseasAddress.AddressLine1 = updatedAddress.AddressLine1;
                overseasAddress.AddressLine2 = updatedAddress.AddressLine2;
                overseasAddress.CityOrTown = updatedAddress.CityOrTown;
                overseasAddress.StateProvince = updatedAddress.StateProvince;
                overseasAddress.PostCode = updatedAddress.PostCode;
                overseasAddress.SiteCoordinates = updatedAddress.SiteCoordinates;
                overseasAddress.OrganisationName = updatedAddress.OrganisationName;
                overseasAddress.UpdatedBy = updatedAddress.UpdatedBy;
                overseasAddress.UpdatedOn = DateTime.UtcNow;

                UpsertOverseasAddressContact(overseasAddress, updatedAddress.OverseasAddressContacts.FirstOrDefault()!);
                UpsertOverseasAddressWasteCodes(overseasAddress, updatedAddress.OverseasAddressWasteCodes);
            }
            await context.SaveChangesAsync();
        }

        private static void UpsertOverseasAddressContact(OverseasAddress overseasAddress, OverseasAddressContactDto contactDto)
        {
            if (contactDto == null)
            {
                overseasAddress.OverseasAddressContacts.Clear();
                return;
            }

            var existingContact = overseasAddress.OverseasAddressContacts.SingleOrDefault();

            if (existingContact != null)
            {
                existingContact.FullName = contactDto.FullName;
                existingContact.Email = contactDto.Email;
                existingContact.PhoneNumber = contactDto.PhoneNumber;
            }
            else
            {
                overseasAddress.OverseasAddressContacts.Add(new OverseasAddressContact
                {
                    ExternalId = Guid.NewGuid(),
                    FullName = contactDto.FullName,
                    Email = contactDto.Email,
                    PhoneNumber = contactDto.PhoneNumber,
                    CreatedBy = contactDto.CreatedBy
                });
            }
        }

        private static void UpsertOverseasAddressWasteCodes(OverseasAddress overseasAddress, List<OverseasAddressWasteCodeDto> updatedWasteCodes)
        {
            var deletedWastCodes = overseasAddress.OverseasAddressWasteCodes
                .Where(wc => !updatedWasteCodes.Exists(uwc => uwc.ExternalId == wc.ExternalId))
                .ToList();

            overseasAddress.OverseasAddressWasteCodes.RemoveAll(wc => deletedWastCodes.Contains(wc));

            foreach (var wasteCode in updatedWasteCodes)
            {
                if (wasteCode.ExternalId == Guid.Empty)
                {
                    overseasAddress.OverseasAddressWasteCodes.Add(new OverseasAddressWasteCode
                    {
                        ExternalId = Guid.NewGuid(),
                        CodeName = wasteCode.CodeName,
                    });
                }
                else
                {
                    var existingWasteCode = overseasAddress.OverseasAddressWasteCodes.Find(wc => wc.ExternalId == wasteCode.ExternalId);
                    if (existingWasteCode != null)
                    {
                        existingWasteCode.CodeName = wasteCode.CodeName;
                    }
                }
            }
        }

        private async Task CreateOverseasReprocessingSites(List<OverseasAddressDto> overseasAddresses, List<OverseasAddress> overseasAddressesAfterDb, int registrationId, int registrationMaterialId)
        {
            var overseasAddressesToCreate = overseasAddresses
                .Where(x => !overseasAddressesAfterDb.Exists(y => y.ExternalId == x.ExternalId))
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
                  ExternalId = Guid.NewGuid(),
                  FullName = c.FullName,
                  Email = c.Email,
                  PhoneNumber = c.PhoneNumber,
                  CreatedBy = c.CreatedBy
              })],
                    OverseasAddressWasteCodes = [.. x.OverseasAddressWasteCodes.Select(wc => new OverseasAddressWasteCode
              {
                  ExternalId = Guid.NewGuid(),
                  CodeName = wc.CodeName,
              })],
                    OverseasMaterialReprocessingSites = [
                        new OverseasMaterialReprocessingSite
                  {
                      RegistrationMaterialId = registrationMaterialId,
                      ExternalId = Guid.NewGuid(),
                  }
                    ]
                }).ToList();

            await context.OverseasAddress.AddRangeAsync(overseasAddressesToCreate);
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

        public async Task UpdateApplicationRegistrationTaskStatusAsync(string taskName, Guid registrationMaterialId, TaskStatuses status)
        {
            logger.LogInformation("Updating application status for task with TaskName {TaskName} And registrationMaterialId {registrationMaterialId} to {Status}", taskName, registrationMaterialId, status);

            var statusEntity = await context.LookupTaskStatuses.SingleAsync(lts => lts.Name == status.ToString());

            var taskStatus = await GetTaskStatusAsync(taskName, registrationMaterialId);
            if (taskStatus is null)
            {
                var registrationMaterial = await context.RegistrationMaterials.FirstOrDefaultAsync(o => o.ExternalId == registrationMaterialId);
                if (registrationMaterial is null)
                {
                    throw new KeyNotFoundException();
                }

                var registration = await context.Registrations.FirstOrDefaultAsync(o => o.Id == registrationMaterial.RegistrationId);
                if (registration is null)
                {
                    throw new KeyNotFoundException();
                }

                var task = await context
                    .LookupApplicantRegistrationTasks
                    .SingleOrDefaultAsync(t => t.Name == taskName && t.IsMaterialSpecific && t.ApplicationTypeId == registration.ApplicationTypeId);

                if (task is null)
                {
                    throw new RegulatorInvalidOperationException($"No Valid Task Exists: {taskName}");
                }

                // Create a new entity if it doesn't exist
                taskStatus = new ApplicantRegistrationTaskStatus
                {
                    ExternalId = Guid.NewGuid(),
                    RegistrationId = registration.Id,
                    Task = task,
                    TaskStatus = statusEntity,
                    RegistrationMaterialId = registrationMaterial.Id
                };

                await context.RegistrationTaskStatus.AddAsync(taskStatus);
            }
            else
            {
                // Update the existing entity
                taskStatus.TaskStatus = statusEntity;
                context.RegistrationTaskStatus.Update(taskStatus);
            }
            await context.SaveChangesAsync();

            logger.LogInformation("Successfully updated status for task with TaskName {TaskName} And RegistrationMaterialId {registrationMaterialId} to {Status}", taskName, registrationMaterialId, status);
        }

        public async Task<ApplicantRegistrationTaskStatus?> GetTaskStatusAsync(string taskName, Guid registrationMaterialId)
        {
            var taskStatus = await context
                .RegistrationTaskStatus
                .Include(ts => ts.TaskStatus)
                .Include(o => o.Registration)
                .FirstOrDefaultAsync(x => x.Task.Name == taskName && x.RegistrationMaterial.ExternalId == registrationMaterialId);

            return taskStatus;
        }

        private async Task<RegistrationMaterial> GetRegistrationMaterial(Guid registrationMaterialId)
        {
            var registrationMaterial = await context.RegistrationMaterials.Where(x => x.ExternalId == registrationMaterialId).SingleOrDefaultAsync();

            if (registrationMaterial == null)
                throw new KeyNotFoundException("RegistrationMaterial not found");

            return registrationMaterial;
        }

        public async Task<IList<OverseasMaterialReprocessingSite>> GetOverseasMaterialReprocessingSites(Guid registrationMaterialId)
        {
            var result = await context.OverseasMaterialReprocessingSite
                .Where(s => s.RegistrationMaterial != null && s.RegistrationMaterial!.ExternalId == registrationMaterialId)
                .Include(s => s.OverseasAddress)
                .ThenInclude(oa => oa!.OverseasAddressContacts)
                .Include(s => s.OverseasAddress)
                .ThenInclude(oa => oa!.Country)
                .Include(s => s.OverseasAddress)
                .ThenInclude(oa => oa!.OverseasAddressWasteCodes)
                .Include(s => s.OverseasAddress)
                .ThenInclude(oa => oa!.ChildInterimConnections)
                .ThenInclude(child_oa => child_oa.OverseasAddress)
                .ThenInclude(child_oa => child_oa.OverseasAddressContacts)
                .Include(s => s.OverseasAddress)
                .ThenInclude(oa => oa!.ChildInterimConnections)
                .ThenInclude(child_oa => child_oa.OverseasAddress)
                .ThenInclude(child_oa => child_oa.Country)
                .AsSplitQuery()
                .ToListAsync();

            return result;
        }

        public async Task SaveInterimSitesAsync(SaveInterimSitesRequestDto requestDto)
        {
            await using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                var registrationMaterial = await context.RegistrationMaterials
                    .Include(rm => rm.Registration)
                    .FirstOrDefaultAsync(rm => rm.ExternalId == requestDto.RegistrationMaterialId)
                    ?? throw new KeyNotFoundException("RegistrationMaterial not found");

                var registrationMaterialId = registrationMaterial.Id;
                var registrationId = registrationMaterial.Registration.Id;

                var existingInterimSites = await GetExistingInterimSites(registrationId);

                var incomingInterimAddresses = requestDto.OverseasMaterialReprocessingSites
                    .Where(x => x.InterimSiteAddresses != null)
                    .SelectMany(x => x.InterimSiteAddresses!)
                    .ToList();

                await DeleteObsoleteInterimSites(existingInterimSites, incomingInterimAddresses, registrationMaterialId);

                foreach (var dto in incomingInterimAddresses)
                {
                    var entity = await UpsertSingleInterimSite(dto, registrationId, registrationMaterialId, requestDto.UserId!.Value, existingInterimSites);
                    await CreateInterimConnection(dto, entity.ExternalId);
                }

                await UpdateApplicationRegistrationTaskStatusAsync(ApplicantRegistrationTaskNames.InterimSites, registrationMaterial.ExternalId, TaskStatuses.Completed);
                await context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while saving interim sites for RegistrationMaterialId: {RegistrationMaterialId}", requestDto.RegistrationMaterialId);
                await transaction.RollbackAsync();
                throw;
            }
        }

        private async Task<List<OverseasAddress>> GetExistingInterimSites(int registrationId)
        {
            return await context.OverseasAddress
                .Where(oa => oa.IsInterimSite == true && oa.RegistrationId == registrationId)
                .Include(oa => oa.OverseasAddressContacts)
                .Include(oa => oa.InterimConnections)
                .ToListAsync();
        }

        private async Task DeleteObsoleteInterimSites(List<OverseasAddress> existingSites, List<InterimSiteAddressDto> incomingDtos, int registrationMaterialId)
        {
            var incomingExternalIds = incomingDtos
                .Where(x => x.ExternalId != Guid.Empty)
                .Select(x => x.ExternalId)
                .ToHashSet();

            var toDelete = existingSites
                .Where(x => !incomingExternalIds.Contains(x.ExternalId))
                .ToList();

            if (toDelete.Count == 0) return;

            var deleteSiteIds = toDelete.Select(x => x.Id).ToList();

            var toDeleteConnections = await context.InterimOverseasConnections
                .Where(c => deleteSiteIds.Contains(c.InterimSiteId))
                .ToListAsync();

            var toDeleteMaterialSites = await context.OverseasMaterialReprocessingSite
                .Where(r => deleteSiteIds.Contains(r.OverseasAddressId) && r.RegistrationMaterialId == registrationMaterialId)
                .ToListAsync();

            context.InterimOverseasConnections.RemoveRange(toDeleteConnections);
            context.OverseasMaterialReprocessingSite.RemoveRange(toDeleteMaterialSites);
            context.OverseasAddress.RemoveRange(toDelete);
        }

        private async Task<OverseasAddress> UpsertSingleInterimSite(InterimSiteAddressDto dto, int registrationId, int registrationMaterialId, Guid userId, List<OverseasAddress> existingInterimSites)
        {
            var isNew = dto.ExternalId == Guid.Empty;
            var externalId = isNew ? Guid.NewGuid() : dto.ExternalId;

            var countryId = await context.LookupCountries
                .Where(c => c.Name == dto.CountryName)
                .Select(c => c.Id)
                .FirstOrDefaultAsync();

            OverseasAddress entity;

            if (isNew)
            {
                entity = new OverseasAddress
                {
                    ExternalId = externalId,
                    OrganisationName = dto.OrganisationName,
                    AddressLine1 = dto.AddressLine1,
                    AddressLine2 = dto.AddressLine2,
                    CityOrTown = dto.CityOrTown,
                    StateProvince = dto.StateProvince,
                    PostCode = dto.PostCode,
                    CountryId = countryId,
                    RegistrationId = registrationId,
                    IsInterimSite = true,
                    CreatedBy = userId,
                    CreatedOn = DateTime.UtcNow,
                    OverseasAddressContacts = dto.InterimAddressContact.Select(c => new OverseasAddressContact
                    {
                        FullName = c.FullName,
                        Email = c.Email,
                        PhoneNumber = c.PhoneNumber,
                        CreatedBy = userId
                    }).ToList(),
                    OverseasMaterialReprocessingSites = new List<OverseasMaterialReprocessingSite>
            {
                new() { RegistrationMaterialId = registrationMaterialId, ExternalId = Guid.NewGuid() }
            }
                };

                await context.OverseasAddress.AddAsync(entity);
                await context.SaveChangesAsync();
            }
            else
            {
                entity = existingInterimSites.First(x => x.ExternalId == externalId);

                entity.OrganisationName = dto.OrganisationName;
                entity.AddressLine1 = dto.AddressLine1;
                entity.AddressLine2 = dto.AddressLine2;
                entity.CityOrTown = dto.CityOrTown;
                entity.StateProvince = dto.StateProvince;
                entity.PostCode = dto.PostCode;
                entity.CountryId = countryId;
                entity.UpdatedBy = userId;
                entity.UpdatedOn = DateTime.UtcNow;

                context.OverseasAddressContact.RemoveRange(entity.OverseasAddressContacts);
                entity.OverseasAddressContacts = dto.InterimAddressContact.Select(c => new OverseasAddressContact
                {
                    FullName = c.FullName,
                    Email = c.Email,
                    PhoneNumber = c.PhoneNumber,
                    CreatedBy = userId
                }).ToList();
            }

            return entity;
        }

        private async Task CreateInterimConnection(InterimSiteAddressDto dto, Guid interimExternalId)
        {
            if (!dto.ParentExternalId.HasValue) return;

            var parentEntityId = await context.OverseasAddress
                .Where(p => p.ExternalId == dto.ParentExternalId && p.IsInterimSite != true)
                .Select(p => p.Id)
                .FirstOrDefaultAsync();

            var interimEntityId = await context.OverseasAddress
                .Where(i => i.ExternalId == interimExternalId && i.IsInterimSite == true)
                .Select(i => i.Id)
                .FirstOrDefaultAsync();

            if (parentEntityId == 0 || interimEntityId == 0) return;

            var exists = await context.InterimOverseasConnections
                .AnyAsync(c => c.InterimSiteId == interimEntityId && c.ParentOverseasAddressId == parentEntityId);

            if (!exists)
            {
                await context.InterimOverseasConnections.AddAsync(new InterimOverseasConnections
                {
                    ExternalId = Guid.NewGuid(),
                    InterimSiteId = interimEntityId,
                    ParentOverseasAddressId = parentEntityId
                });
            }
        }
    }
}
