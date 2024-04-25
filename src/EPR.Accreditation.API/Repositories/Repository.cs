using AutoMapper;
using EPR.Accreditation.API.Common.Data;
using EPR.Accreditation.API.Common.Data.Enums;
using EPR.Accreditation.API.Helpers;
using EPR.Accreditation.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Data = EPR.Accreditation.API.Common.Data.DataModels;
using DTO = EPR.Accreditation.API.Common.Dtos;
using Enums = EPR.Accreditation.API.Common.Enums;

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

        public async Task<DTO.Accreditation> GetAccreditation(Guid id)
        {
            var accreditation = await _accreditationContext
                .Accreditation
                    .Include(a => a.Site)
                    .Include(a => a.OverseasReprocessingSites)
                    .Include(a => a.WastePermit)
                .Where(a => a.ExternalId == id)
                .Select(a =>
                    _mapper.Map<DTO.Accreditation>(a)
                )
                .FirstOrDefaultAsync();

            return accreditation;
        }

        public async Task<Guid> AddAccreditation(DTO.Accreditation accreditation)
        {
            var entity = _mapper.Map<Data.Accreditation>(accreditation);
            entity.ExternalId = Guid.NewGuid();
            _accreditationContext.Accreditation.Add(entity);
            await _accreditationContext.SaveChangesAsync();

            return entity.ExternalId;
        }

        public async Task<Guid> AddAccreditationMaterial(
            Guid id,
            Enums.OperatorType accreditationOperatorType,
            DTO.AccreditationMaterial material)
        {
            var entity = _mapper.Map<Data.AccreditationMaterial>(material);
            entity.ExternalId = Guid.NewGuid();

            // materials for overseas sites are added before the site
            // therefore if the accreditation is a reprocessor, we should use
            // that site id
            var accreditationEntity = await _accreditationContext
                .Accreditation
                .Where(a =>
                    a.ExternalId == id &&
                    a.OperatorTypeId == _mapper.Map<OperatorType>(accreditationOperatorType))
                .Select(a => new
                {
                    a.Id,
                    a.SiteId
                })
                .SingleOrDefaultAsync();

            if (accreditationEntity == null)
            {
                throw new NotFoundException($"No accreditation record found with id: {id}");
            }

            if (accreditationOperatorType == Enums.OperatorType.Reprocessor)
            {
                entity.SiteId = accreditationEntity.SiteId ?? throw new NotFoundException($"Reprocessor accreditation does not have a site. Accreditation ID: {id}"); // if this is an exporter the site id will be null
            }

            entity.AccreditationId = accreditationEntity.Id;

            await _accreditationContext.AccreditationMaterial.AddAsync(entity);
            await _accreditationContext.SaveChangesAsync();
            return entity.ExternalId;
        }

        public async Task UpdateAccreditation(
            Guid id,
            DTO.Accreditation accreditation)
        {
            var entity = await _accreditationContext
                .Accreditation
                    .Include(a => a.WastePermit)
                .Where(a => a.ExternalId == id)
                .FirstOrDefaultAsync();

            if (entity == null)
                throw new NotFoundException($"Accreditation record not found for External ID: {id}");

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

        public async Task DeleteFile(Guid id, Guid fileId)
        {
            var file = await _accreditationContext
                .FileUpload
                .Where(f =>
                    f.Accreditation.ExternalId == id &&
                    f.FileId == fileId)
                .FirstOrDefaultAsync();

            // TODO need to handle an entity that's not found better here
            if (file == null)
                throw new NullReferenceException(nameof(file));

            file.Status = FileUploadStatus.Deleted;

            await _accreditationContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<DTO.AccreditationTaskProgress>> GetTaskProgress(Guid id)
        {
            var taskProgressList = await _accreditationContext
                .AccreditationTaskProgress
                    .Include(p => p.AccreditationTaskProgressMaterials)
                .Where(p => p.Accreditation.ExternalId == id)
                .Select(p => p)
                .ToListAsync();

            // need to include the materials and their progress now as well
            return _mapper.Map<List<DTO.AccreditationTaskProgress>>(taskProgressList);
        }

        public async Task<IEnumerable<DTO.FileUpload>> GetFileRecords(Guid id)
        {
            return await _accreditationContext
                .FileUpload
                .Where(f => f.Accreditation.ExternalId == id)
                .Select(f => _mapper.Map<DTO.FileUpload>(f))
                .ToListAsync();
        }

        public async Task AddFile(
            Guid id,
            DTO.FileUpload fileUpload)
        {
            var entity = _mapper.Map<Data.FileUpload>(fileUpload);

            // get the id of the accreditation that this upload is related to
            var accreditationId =
                await _accreditationContext
                .Accreditation
                .Where(a => a.ExternalId == id)
                .Select(a => a.Id)
                .SingleOrDefaultAsync();

            // TODO need to handle an entity that's not found better here
            if (accreditationId == default)
                throw new NotFoundException($"Accreditation record does not exist for External ID: {id}");

            entity.AccreditationId = accreditationId;
            entity.DateUploaded = DateTime.UtcNow;

            await _accreditationContext.FileUpload.AddAsync(entity);
            await _accreditationContext.SaveChangesAsync();
        }

        public async Task<DTO.AccreditationMaterial> GetMaterial(
            Guid id,
            Guid materialid)
        {
            var entity = await GetAccreditationSiteMaterial(
                id,
                materialid);

            return _mapper.Map<DTO.AccreditationMaterial>(entity);
        }

        public async Task UpdateMaterial(
            Guid id,
            Guid materialid,
            DTO.AccreditationMaterial material,
            Guid? overseasSiteId)
        {
            var entity = await GetAccreditationSiteMaterial(
                id,
                materialid);

            // waste last year has changed, therefore MaterialReprocessorDetails
            // are invalid and should be cleared down
            if (material.WasteLastYear.HasValue &&
                entity.WasteLastYear != material.WasteLastYear &&
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

            if (entity.Accreditation.OperatorTypeId == OperatorType.Exporter &&
                overseasSiteId.HasValue)
            {
                var overseasSiteEntityId = await _accreditationContext.OverseasReprocessingSite
                    .Where(os => os.ExternalId == overseasSiteId.Value)
                    .Select(os => (int?)os.Id)
                    .FirstOrDefaultAsync();

                if (overseasSiteEntityId == null)
                {
                    throw new NotFoundException($"No overseas reprocessing site found for external id: {overseasSiteId}");
                }

                entity.OverseasReprocessingSiteId = overseasSiteEntityId.Value;
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
                .Include(a => a.Site)
                    .ThenInclude(s => s.ExemptionReferences)
                .Where(a => a.ExternalId == id && a.SiteId.HasValue)
                .Select(a => _mapper.Map<DTO.Site>(a.Site))
                .SingleOrDefaultAsync();
        }

        public async Task<Guid> CreateSite(
            Guid id,
            DTO.Site site)
        {
            // add the site id to the accreditation record
            var accreditionEntity = await _accreditationContext
                .Accreditation
                .Where(a => a.ExternalId == id)
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
            Guid id,
            DTO.Site site)
        {
            // get site from the accreditation
            var entity = await _accreditationContext
                .Accreditation
                .Include(a => a.Site)
                    .ThenInclude(s => s.ExemptionReferences)
                .Where(a => a.ExternalId == id
                    && a.SiteId.HasValue)
                .Select(a => a.Site)
                .SingleOrDefaultAsync()
                ?? throw new NotFoundException();

            if (entity.ExemptionReferences.Any())
                entity.ExemptionReferences.Clear();

            foreach (var reference in site.ExemptionReferences.Where(x => !string.IsNullOrWhiteSpace(x)))
                entity.ExemptionReferences.Add(new Data.ExemptionReference { Reference = reference });

            await _accreditationContext.SaveChangesAsync();
        }

        public async Task<DTO.OverseasReprocessingSite> GetOverseasSite(
            Guid id,
            Guid overseasSiteid)
        {
            var site = await _accreditationContext
                .OverseasReprocessingSite
                .Include(x => x.OverseasAddress)
                .Where(o => o.ExternalId == overseasSiteid)
                .SingleOrDefaultAsync();

            var siteDto = _mapper.Map<DTO.OverseasReprocessingSite>(site);
            return siteDto;
        }

        public async Task<Guid> CreateOverseasSite(
            Guid id,
            DTO.OverseasReprocessingSite site)
        {
            var accreditationId = await _accreditationContext
                .Accreditation
                .Where(
                    a =>
                        a.ExternalId == id &&
                        a.OperatorTypeId == OperatorType.Exporter) // cannot add an overseas site to a reprocessor
                .Select(a => (int?)a.Id)
                .SingleOrDefaultAsync();

            if (accreditationId == null)
            {
                throw new NotFoundException();
            }

            var entity = _mapper.Map<Data.OverseasReprocessingSite>(site);
            entity.AccreditationId = accreditationId.Value;
            entity.ExternalId = Guid.NewGuid();

            await _accreditationContext.OverseasReprocessingSite.AddAsync(entity);
            await _accreditationContext.SaveChangesAsync();

            return entity.ExternalId;
        }

        public async Task UpdateOverseasSite(
            Guid id,
            Guid overseasSiteId,
            DTO.OverseasReprocessingSite site)
        {
            // get overseas site
            var entity = await _accreditationContext
                .OverseasReprocessingSite
                .Include(s => s.OverseasAddress)
                .Where(os =>
                    os.Accreditation.ExternalId == id &&
                    os.Accreditation.OperatorTypeId == OperatorType.Exporter &&
                    os.ExternalId == overseasSiteId)
                .SingleOrDefaultAsync()
                 ?? throw new NotFoundException();

            if (entity == null)
            {
                throw new NotFoundException($"Overseas site not found for Accreditation External ID: {id}, Overseas External Site Id: {overseasSiteId}");
            }

            _mapper.Map(site, entity);

            await _accreditationContext.SaveChangesAsync();
        }

        public async Task<DTO.SaveAndComeBack> GetSaveAndComeBack(Guid id)
        {
            var saveAndContinue = await _accreditationContext
                .SaveAndComeBack
                .Where(s => s.Accreditation.ExternalId == id)
                .Select(s =>
                    _mapper.Map<DTO.SaveAndComeBack>(s)
                )
                .SingleOrDefaultAsync();

            return saveAndContinue;
        }

        public async Task DeleteSaveAndComeBack(Guid id)
        {
            var saveAndContinueId = await _accreditationContext
                .SaveAndComeBack
                .Where(s => s.Accreditation.ExternalId == id)
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
            Guid id,
            DTO.SaveAndComeBack saveAndContinue)
        {
            var entity = _mapper.Map<Data.SaveAndComeBack>(saveAndContinue);

            // get the id of the accreditation that this save and continue record is related to
            var accreditationId =
                await _accreditationContext
                .Accreditation
                .Where(a => a.ExternalId == id)
                .Select(a => a.Id)
                .SingleOrDefaultAsync();

            // TODO need to handle an entity that's not found better here
            if (accreditationId == default)
                throw new NotFoundException($"Save and continue record does not exist for External ID: {id}");

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
            Guid id,
            Guid materialid)
        {
            var entity = await _accreditationContext
                .AccreditationMaterial
                .Include(am => am.Accreditation)
                .Include(am => am.Material)
                    .Include(am => am.WasteCodes)
                    .Include(am => am.MaterialReprocessorDetails)
                        .ThenInclude(mrp => mrp.ReprocessorSupportingInformation)
                .Where(
                    am =>
                        am.Accreditation.ExternalId == id &&
                        am.ExternalId == materialid)
                .SingleOrDefaultAsync();

            if (entity == null)
            {
                throw new NotFoundException($"Material not found for External ID: {id}, Material External ID: {materialid}");
            }

            return entity;
        }
    }
}
