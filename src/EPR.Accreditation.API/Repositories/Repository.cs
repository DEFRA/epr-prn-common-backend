using AutoMapper;
using EPR.Accreditation.API.Common.Data;
using EPR.Accreditation.API.Common.Data.DataModels;
using EPR.Accreditation.API.Common.Data.Enums;
using EPR.Accreditation.API.Helpers;
using EPR.Accreditation.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Data = EPR.Accreditation.API.Common.Data.DataModels;
using DTO = EPR.Accreditation.API.Common.Dtos;

namespace EPR.Accreditation.API.Repositories
{
    public class Repository : IRepository
    {
        protected IMapper _mapper;
        protected readonly AccreditationContext _accreditationContext;

        public Repository(
            IMapper mapper,
            AccreditationContext accreditationContext)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _accreditationContext = accreditationContext ?? throw new ArgumentNullException(nameof(accreditationContext));
        }

        public async Task<Guid> AddAccreditation(DTO.Accreditation accreditation)
        {
            var entity = _mapper.Map<Data.Accreditation>(accreditation);
            _accreditationContext.Accreditation.Add(entity);
            await _accreditationContext.SaveChangesAsync();

            return entity.ExternalId;
        }

        public async Task<Guid> AddAccreditationMaterial(
            Guid externalId,
            Guid? overseasSiteId,
            DTO.AccreditationMaterial material)
        {
            var entity = _mapper.Map<Data.AccreditationMaterial>(material);

            if (overseasSiteId == null)
            {
                entity.SiteId = await _accreditationContext
                    .Accreditation
                    .Where(a =>
                        a.ExternalId == externalId &&
                        a.SiteId.HasValue)
                    .Select(a => a.SiteId)
                    .SingleAsync();
            }
            else
            {
                entity.OverseasReprocessingSiteId = await _accreditationContext
                    .OverseasReprocessingSite
                    .Where(o =>
                        o.Accreditation.ExternalId == externalId &&
                        o.ExternalId == overseasSiteId)
                    .Select(o => o.Id)
                    .SingleAsync();
            }

            await _accreditationContext.AccreditationMaterial.AddAsync(entity);
            await _accreditationContext.SaveChangesAsync();
            return entity.ExternalId;
        }

        public async Task UpdateAccreditation(
            Guid externalId,
            DTO.Accreditation accreditation)
        {
            var entity = await _accreditationContext
                .Accreditation
                    .Include(a => a.WastePermit)
                .Where(a => a.ExternalId == externalId)
                .FirstOrDefaultAsync();

            if (entity == null)
                throw new NotFoundException($"Accreditation record not found for External ID: {externalId}");

            // map updates on top of existing database entity
            entity = _mapper.Map(accreditation, entity);

            // ensure foreign keys are set
            if (entity.OverseasReprocessingSites != null)
            {
                foreach (var o in entity.OverseasReprocessingSites)
                {
                    if (o.Id == default)
                    {
                        o.ExternalId = Guid.NewGuid();
                        await _accreditationContext.OverseasReprocessingSite.AddAsync(o);
                    }

                    if (o.WastePermit != null &&
                        o.WastePermit.Id == default)
                    {
                        await _accreditationContext.WastePermit.AddAsync(o.WastePermit);
                    }
                }
            }

            if (entity.Site != null &&
                entity.Site.Id == default)
            {
                entity.Site.ExternalId = Guid.NewGuid();
                await _accreditationContext.Site.AddAsync(entity.Site);
            }

            if (entity.WastePermit == null &&
                entity.WastePermit.Id == default)
                await _accreditationContext.WastePermit.AddAsync(entity.WastePermit);

            await _accreditationContext.SaveChangesAsync();
        }

        public async Task<DTO.Accreditation> GetById(Guid externalId)
        {
            var accreditation = await _accreditationContext
                .Accreditation
                    .Include(a => a.Site)
                    .Include(a => a.OverseasReprocessingSites)
                    .Include(a => a.WastePermit)
                .Where(a => a.ExternalId == externalId)
                .Select(a =>
                    _mapper.Map<DTO.Accreditation>(a)
                )
                .FirstOrDefaultAsync();

            return accreditation;
        }

        public async Task DeleteFile(Guid externalId, Guid fileId)
        {
            var file = await _accreditationContext
                .FileUpload
                .Where(f =>
                    f.Accreditation.ExternalId == externalId &&
                    f.FileId == fileId)
                .FirstOrDefaultAsync();

            // TODO need to handle an entity that's not found better here
            if (file == null)
                throw new NullReferenceException(nameof(file));

            file.Status = FileUploadStatus.Deleted;

            await _accreditationContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<DTO.AccreditationTaskProgress>> GetTaskProgress(Guid externalId)
        {
            var taskProgressList = await _accreditationContext
                .AccreditationTaskProgress
                    .Include(p => p.AccreditationTaskProgressMaterials)
                .Where(p => p.Accreditation.ExternalId == externalId)
                .Select(p => p)
                .ToListAsync();

            // need to include the materials and their progress now as well
            return _mapper.Map<List<DTO.AccreditationTaskProgress>>(taskProgressList);
        }

        public async Task<IEnumerable<DTO.FileUpload>> GetFileRecords(Guid externalId)
        {
            return await _accreditationContext
                .FileUpload
                .Where(f => f.Accreditation.ExternalId == externalId)
                .Select(f => _mapper.Map<DTO.FileUpload>(f))
                .ToListAsync();
        }

        public async Task AddFile(
            Guid externalId,
            DTO.FileUpload fileUpload)
        {
            var entity = _mapper.Map<Data.FileUpload>(fileUpload);

            // get the id of the accreditation that this upload is related to
            var accreditationId =
                await _accreditationContext
                .Accreditation
                .Where(a => a.ExternalId == externalId)
                .Select(a => a.Id)
                .SingleOrDefaultAsync();

            // TODO need to handle an entity that's not found better here
            if (accreditationId == default)
                throw new NotFoundException($"Accreditation record does not exist for External ID: {externalId}");

            entity.AccreditationId = accreditationId;
            entity.DateUploaded = DateTime.UtcNow;

            await _accreditationContext.FileUpload.AddAsync(entity);
            await _accreditationContext.SaveChangesAsync();
        }

        

        public async Task<DTO.AccreditationMaterial> GetMaterial(
            Guid externalId,
            Guid? overseasSiteExternalId,
            Guid materialExternalId)
        {
            var entity = await GetAccreditationSiteMaterial(
                externalId, 
                overseasSiteExternalId, 
                materialExternalId);

            return _mapper.Map<DTO.AccreditationMaterial>(entity);
        }

        public async Task UpdateMaterial(
            Guid externalId,
            Guid? overseasSiteExternalId,
            Guid materialExternalId,
            DTO.AccreditationMaterial material)
        {
            var entity = await GetAccreditationSiteMaterial(
                externalId, 
                overseasSiteExternalId, 
                materialExternalId);

            if (entity == null)
                throw new NotFoundException($"Material not found for External ID: {externalId}, Overseas External ID: {overseasSiteExternalId}, Material External ID: {materialExternalId}");

            // waste last year has changed, therefore MaterialReprocessorDetails
            // are invalid and should be cleared down
            if (entity.WasteLastYear !=
                material.WasteLastYear &&
                entity.MaterialReprocessorDetails != null)
            {
                _accreditationContext.Remove(entity.MaterialReprocessorDetails);
            }

            // copy the updates over to the db entity
            entity = _mapper.Map(material, entity);

            if (entity.WasteCodes != null)
            {
                foreach (var wasteCode in entity.WasteCodes)
                {
                    if (wasteCode.Id == default)
                        _accreditationContext.WasteCodes.Add(wasteCode);
                }
            }

            // save the changes
            await _accreditationContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<DTO.Country>> GetCountries()
        {
            return await _accreditationContext
                .Country
                .OrderBy(c => c.Name)
                .Select(c => _mapper.Map<DTO.Country>(c))
                .ToListAsync();
        }

        public async Task<DTO.Site> GetSite(
            Guid id)
        {
            return await _accreditationContext
                .Accreditation
                .Where(a => a.ExternalId == id && a.SiteId.HasValue)
                .Select(a => _mapper.Map<DTO.Site>(a.Site))
                .SingleOrDefaultAsync();
        }

        public async Task<Guid> CreateSite(
            Guid externalId,
            DTO.Site site)
        {
            // add the site id to the accreditation record
            var accreditionEntity = await _accreditationContext
                .Accreditation
                .Where(a => a.ExternalId == externalId)
                .SingleOrDefaultAsync();

            if (accreditionEntity == null)
                throw new NotFoundException("Accreditation entity not found");

            var entity = _mapper.Map<Data.Site>(site);

            entity.ExternalId = Guid.NewGuid();
            await _accreditationContext.Site.AddAsync(entity);

            // perform a save so that we have the id of the site
            await _accreditationContext.SaveChangesAsync();

            accreditionEntity.SiteId = entity.Id;

            await _accreditationContext.SaveChangesAsync();

            return entity.ExternalId;
        }

        public async Task UpdateSite(
            Guid externalId,
            DTO.Site site)
        {
            // get site from the accreditation
            var entity = await _accreditationContext
                .Accreditation
                .Where(a => a.ExternalId == externalId
                    && a.SiteId.HasValue)
                .Select(a => a.Site)
                .SingleOrDefaultAsync();

            if (entity == null)
                throw new NotFoundException();

            entity = _mapper.Map(site, entity);
            await _accreditationContext.SaveChangesAsync();
        }

        public async Task<DTO.OverseasReprocessingSite> GetOverseasSite(
            Guid id,
            Guid overseasSiteExternalId)
        {
            return await _accreditationContext
                .OverseasReprocessingSite
                .Where(o => o.Accreditation.ExternalId == id && o.ExternalId == overseasSiteExternalId)
                .Select(o => _mapper.Map<DTO.OverseasReprocessingSite>(o))
                .SingleOrDefaultAsync();
        }

        public async Task<Guid> CreateOverseasSite(
            Guid id,
            DTO.OverseasReprocessingSite site)
        {
            var accreditationId = await _accreditationContext
                .Accreditation
                .Where(a => a.ExternalId == id)
                .Select(a => (int?)a.Id)
                .SingleOrDefaultAsync();

            if (accreditationId == null)
                throw new NotFoundException();

            var entity = _mapper.Map<Data.OverseasReprocessingSite>(site);
            entity.AccreditationId = accreditationId.Value;
            entity.ExternalId = Guid.NewGuid();

            await _accreditationContext.OverseasReprocessingSite.AddAsync(entity);
            await _accreditationContext.SaveChangesAsync();

            return entity.ExternalId;
        }

        public async Task UpdateOverseasSite(DTO.OverseasReprocessingSite site)
        {
            throw new NotImplementedException();
        }

        public async Task<DTO.SaveAndComeBack> GetSaveAndComeBack(Guid externalId)
        {
            var saveAndContinue = await _accreditationContext
                .SaveAndComeBack
                .Where(s => s.Accreditation.ExternalId == externalId)
                .Select(s =>
                    _mapper.Map<DTO.SaveAndComeBack>(s)
                )
                .SingleOrDefaultAsync();

            return saveAndContinue;
        }

        public async Task DeleteSaveAndComeBack(Guid externalId)
        {
            var saveAndContinueId = await _accreditationContext
                .SaveAndComeBack
                .Where(s => s.Accreditation.ExternalId == externalId)
                .Select(s => s.Id)
                .FirstOrDefaultAsync();

            if (saveAndContinueId == default)
                return;

            // Create a new instance of the entity with the primary key set
            var entityToDelete = new Data.SaveAndComeBack
            {
                Id = saveAndContinueId
            };

            // Attach the entity to the context and mark it as deleted
            _accreditationContext.Entry(entityToDelete).State = EntityState.Deleted;

            // Save changes to the database
            await _accreditationContext.SaveChangesAsync();
        }

        public async Task AddSaveAndComeBack(
            Guid externalId,
            DTO.SaveAndComeBack saveAndContinue)
        {
            var entity = _mapper.Map<Data.SaveAndComeBack>(saveAndContinue);

            // get the id of the accreditation that this save and continue record is related to
            var accreditationId =
                await _accreditationContext
                .Accreditation
                .Where(a => a.ExternalId == externalId)
                .Select(a => a.Id)
                .SingleOrDefaultAsync();

            // TODO need to handle an entity that's not found better here
            if (accreditationId == default)
                throw new NotFoundException($"Save and continue record does not exist for External ID: {externalId}");

            entity.AccreditationId = accreditationId;

            await _accreditationContext.SaveAndComeBack.AddAsync(entity);
            await _accreditationContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<DTO.Material>> GetMaterials()
        {
            return await _accreditationContext
                .Material
                .Select(m => _mapper.Map<DTO.Material>(m))
                .ToListAsync();
        }

        protected async Task<Data.AccreditationMaterial> GetAccreditationSiteMaterial(
            Guid externalId,
            Guid? overseasSiteExternalId,
            Guid materialExternalId)
        {
            var entity = default(Data.AccreditationMaterial);

            if (overseasSiteExternalId == null)
            {
                // get the site based material
                entity = await _accreditationContext
                    .Accreditation
                    .Include(a => a.Site.AccreditationMaterials)
                        .ThenInclude(am => am.Material)
                    .Include(a => a.Site.AccreditationMaterials)
                        .ThenInclude(am => am.MaterialReprocessorDetails)
                    .Where(a =>
                        a.ExternalId == externalId &&
                        a.Site != null &&
                        a.Site.AccreditationMaterials.Any(m => m.ExternalId == materialExternalId))
                    .Select(a =>
                        a.Site.AccreditationMaterials.FirstOrDefault(m => m.ExternalId == materialExternalId))
                    .SingleOrDefaultAsync();
            }
            else
            {
                // get the overseas site based material
                entity = await _accreditationContext
                    .AccreditationMaterial
                    .Include(am => am.Material)
                    .Include(am => am.MaterialReprocessorDetails)
                    .Where(m =>
                        m.ExternalId == materialExternalId &&
                        m.OverseasReprocessingSite != null &&
                        m.OverseasReprocessingSite.ExternalId == overseasSiteExternalId &&
                        m.OverseasReprocessingSite.Accreditation.ExternalId == externalId)
                    .Select(m => m)
                    .SingleOrDefaultAsync();
            }

            return entity;
        }
    }
}
