using AutoMapper;
using EPR.Accreditation.API.Common.Data;
using EPR.Accreditation.API.Common.Data.Enums;
using EPR.Accreditation.API.Common.Dtos;
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
            Guid? siteId,
            Guid? overseasSiteId,
            DTO.AccreditationMaterial material)
        {
            var entity = _mapper.Map<Data.AccreditationMaterial>(material);

            if (siteId != null)
            {
                entity.SiteId = await _accreditationContext
                    .Accreditation
                    .Where(a => 
                        a.ExternalId == externalId &&
                        a.SiteId.HasValue &&
                        a.Site.ExternalId == siteId)
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
                        await _accreditationContext.OverseasReprocessingSite.AddAsync(o);

                    if (o.WastePermit != null &&
                        o.WastePermit.Id == default)
                    {
                        await _accreditationContext.WastePermit.AddAsync(o.WastePermit);
                    }
                }
            }

            if (entity.Site != null &&
                entity.Site.Id == default)
                await _accreditationContext.Site.AddAsync(entity.Site);

            if (entity.WastePermit != null &&
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

        public async Task UpdateMaterial(
            Guid externalId,
            Guid? siteExternalId,
            Guid? overseasSiteExternalId,
            Guid materialExternalId,
            DTO.AccreditationMaterial material)
        {
            var entity = default(Data.AccreditationMaterial);

            if (siteExternalId != null)
            {
                // get  the site based material
                entity = await _accreditationContext
                    .Accreditation
                    .Where(a =>
                        a.ExternalId == externalId &&
                        a.Site != null &&
                        a.Site.ExternalId == siteExternalId &&
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
                    .Where(m =>
                        m.ExternalId == materialExternalId &&
                        m.OverseasReprocessingSite != null &&
                        m.OverseasReprocessingSite.ExternalId == overseasSiteExternalId &&
                        m.OverseasReprocessingSite.Accreditation.ExternalId == externalId)
                    .Select(m => m)
                    .SingleOrDefaultAsync();
            }

            // TODO need to handle an entity that's not found better here
            if (entity == null)
                throw new NotFoundException($"Material not found for External ID: {externalId}, Site External ID: {siteExternalId}, Overseas External ID: {overseasSiteExternalId}, Material External ID: {materialExternalId}");

            // copy the updates over to the db entity
            entity = _mapper.Map(material, entity);

            foreach(var wasteCode in entity.WasteCodes)
            {
                if (wasteCode.Id == default)
                    _accreditationContext.WasteCodes.Add(wasteCode);
            }

            // save the changes
            await _accreditationContext.SaveChangesAsync();
        }

        public async Task<DTO.AccreditationMaterial> GetMaterial(
            Guid externalId,
            Guid? siteExternalId,
            Guid? overseasSiteExternalId,
            Guid materialExternalId)
        {
            var entity = default(Data.AccreditationMaterial);

            if (siteExternalId != null)
            {
                // get the site based material
                entity = await _accreditationContext
                    .Accreditation
                    .Where(a =>
                        a.ExternalId == externalId &&
                    a.Site != null &&
                        a.Site.ExternalId == siteExternalId &&
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
                    .Where(m =>
                        m.ExternalId == materialExternalId &&
                        m.OverseasReprocessingSite != null &&
                        m.OverseasReprocessingSite.ExternalId == overseasSiteExternalId &&
                        m.OverseasReprocessingSite.Accreditation.ExternalId == externalId)
                    .Select(m => m)
                    .SingleOrDefaultAsync();
            }

            return _mapper.Map<DTO.AccreditationMaterial>(entity);
        }

        public async Task<IEnumerable<Country>> GetCountries()
        {
            return await _accreditationContext
                .Country
                .OrderBy(c => c.Name)
                .Select(c => _mapper.Map<DTO.Country>(c))
                .ToListAsync();
        }
    }
}
