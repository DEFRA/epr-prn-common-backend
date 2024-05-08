namespace EPR.Accreditation.API.Services
{
    using EPR.Accreditation.API.Common.Dtos;
    using EPR.Accreditation.API.Common.Enums;
    using EPR.Accreditation.API.Helpers;
    using EPR.Accreditation.API.Helpers.Comparers;
    using EPR.Accreditation.API.Repositories.Interfaces;
    using EPR.Accreditation.API.Services.Interfaces;
    using DTO = EPR.Accreditation.API.Common.Dtos;

    public class AccreditationService : IAccreditationService
    {
        protected readonly IRepository _repository;

        public AccreditationService(IRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<Guid> CreateAccreditation(DTO.Accreditation accreditation)
        {
            // perform checks against the accreditation
            if (accreditation == null)
                throw new ArgumentNullException(nameof(accreditation));

            // set the external id for the accreditation
            accreditation.AccreditationStatusId = Common.Enums.AccreditationStatus.Started;

            return await _repository.AddAccreditation(accreditation);
        }

        public async Task<Guid> CreateMaterial(
            Guid id,
            OperatorType accreditationOperatorType,
            AccreditationMaterial accreditationMaterial)
        {
            return await _repository.AddAccreditationMaterial(
                id,
                accreditationOperatorType,
                accreditationMaterial);
        }

        public async Task UpdateAccreditation(
            Guid id,
            DTO.Accreditation accreditation)
        {
            await _repository.UpdateAccreditation(
                id,
                accreditation);
        }

        public async Task<DTO.Accreditation> GetAccreditation(Guid id)
        {
            var accreditation = await _repository.GetAccreditation(id);

            return accreditation;
        }

        public async Task<IEnumerable<FileUpload>> GetFileRecords(Guid id)
        {
            var fileUploadRecords = await _repository.GetFileRecords(id);

            return fileUploadRecords;
        }

        public async Task DeleteFile(
            Guid id,
            Guid fileId)
        {
            await _repository.DeleteFile(
                id,
                fileId);
        }

        public async Task<IEnumerable<AccreditationTaskProgress>> GetTaskProgress(Guid id)
        {
            return await _repository.GetTaskProgress(id);
        }

        public async Task AddFile(
            Guid id,
            DTO.FileUpload fileUpload)
        {
            await _repository.AddFile(
                id, fileUpload);
        }

        public async Task UpdateMaterail(
            Guid id,
            Guid materialid,
            AccreditationMaterial accreditationMaterial)
        {
            if (accreditationMaterial.WasteCodes != null &&
                accreditationMaterial.WasteCodes.Any())
            {
                accreditationMaterial.WasteCodes =
                    accreditationMaterial
                        .WasteCodes
                        .Where(wc => !string.IsNullOrWhiteSpace(wc.Code))
                        .Distinct(new WasteCodeDtoComparer());
            }

            await _repository.UpdateMaterial(
                id,
                materialid,
                accreditationMaterial);
        }

        public async Task<AccreditationMaterial> GetMaterial(
            Guid id,
            Guid materialid)
        {
            return await _repository.GetMaterial(
                id,
                materialid);
        }

        public async Task<DTO.Site> GetSite(
            Guid id)
        {
            return await _repository.GetSite(id);
        }

        public async Task<Guid> CreateSite(
            Guid id,
            Site site)
        {
            return await _repository.CreateSite(
                id,
                site);
        }

        public async Task UpdateSite(
            Guid id,
            Site site)
        {
            await _repository.UpdateSite(
                id,
                site);
        }

        public async Task<DTO.OverseasReprocessingSite> GetOverseasSite(
            Guid id,
            Guid overseasSiteid)
        {
            return await _repository.GetOverseasSite(id, overseasSiteid);
        }

        public async Task<Guid> CreateOverseasSite(
            Guid id,
            Guid materialId,
            OverseasReprocessingSite overseasReprocessingSite)
        {
            return await _repository.CreateOverseasSite(
                id,
                materialId,
                overseasReprocessingSite);
        }

        public async Task UpdateOverseasSite(
            Guid id,
            Guid overseasSiteId,
            OverseasReprocessingSite overseasReprocessingSite)
        {
            await _repository.UpdateOverseasSite(
                id,
                overseasSiteId,
                overseasReprocessingSite);
        }

        public async Task<SaveAndComeBack> GetSaveAndComeBack(Guid id)
        {
            return await _repository.GetSaveAndComeBack(id);
        }

        public async Task DeleteSaveAndComeBack(Guid id)
        {
            await _repository.DeleteSaveAndComeBack(id);
        }

        public async Task AddSaveAndComeBack(
            Guid id,
            DTO.SaveAndComeBack saveAndComeBack)
        {
            await _repository.AddSaveAndComeBack(id, saveAndComeBack);
        }

        /// <summary>
        /// Gets the data for the relevant CYA page
        /// </summary>
        /// <param name="id">Accreditation ID</param>
        /// <param name="materialId">Site material ID</param>
        /// <param name="siteId">Reprocessor site ID</param>
        /// <param name="overseasSiteId">Exporter site ID</param>
        /// <param name="section">Required CYA section</param>
        /// <returns>Completed Task with the CheckAnswers DTO</returns>
        /// <exception cref="ArgumentException">Returns exception if operatorType ID condition isn't met</exception>
        public async Task<CheckAnswers> GetCheckAnswers(
            Guid id,
            Guid materialId,
            Guid? siteId,
            Guid? overseasSiteId,
            CheckAnswersSection section)
        {
            var accreditation = await GetAccreditation(id);
            var accreditationMaterial = await GetMaterial(id, materialId);
            var site = await GetSite(id);
            var checkAnswersDto = new CheckAnswers();

            switch (section)
            {
                case CheckAnswersSection.AboutMaterialReprocessorActuals:

                    if (accreditation.OperatorTypeId != OperatorType.Reprocessor)
                    {
                        throw new ArgumentException("Can only be for a reprocessor");
                    }
                    else
                    {
                        return BuildCyaAboutMaterialReprocessorActuals(
                            id,
                            materialId,
                            site,
                            accreditationMaterial);
                    }

                case CheckAnswersSection.AboutMaterialExporter:
                    if (accreditation.OperatorTypeId != Common.Enums.OperatorType.Exporter)
                    {
                        throw new ArgumentException("Can only be for a exporter");
                    }
                    else
                    {
                        return BuildCyaAboutMaterialExporter(
                            id,
                            materialId,
                            accreditationMaterial);
                    }
            }

            return checkAnswersDto;
        }

        /// <summary>
        /// Retrieves the data for the AboutMaterialReprocessorActuals enum
        /// </summary>
        /// <param name="id">Accreditation ID</param>
        /// <param name="materialId">Site Material ID</param>
        /// <param name="site">The reprocessor site object</param>
        /// <param name="accreditationMaterial">The material for the reprocessing site</param>
        /// <returns>CheckAnswers DTO with the data</returns>
        private CheckAnswers BuildCyaAboutMaterialReprocessorActuals(
            Guid id,
            Guid materialId,
            Site site,
            AccreditationMaterial accreditationMaterial)
        {
            var siteAddress = site.Address;

            var materialName = accreditationMaterial.Material.English;
            var wasteSource = accreditationMaterial.WasteSource;
            var annualCapacity = accreditationMaterial.AnnualCapacity;
            var weeklyCapacity = accreditationMaterial.WeeklyCapacity;

            var ukPackagingWaste = accreditationMaterial.MaterialReprocessorDetails.UkPackagingWaste;
            var nonUkPackagingWaste = accreditationMaterial.MaterialReprocessorDetails.NonUkPackagingWaste;
            var nonPackagingWaste = accreditationMaterial.MaterialReprocessorDetails.NonPackagingWaste;
            var totalWasteInputsLastCalendarYear = ukPackagingWaste + nonPackagingWaste + nonUkPackagingWaste;

            var materialsNotProcessedOnSite = accreditationMaterial.MaterialReprocessorDetails.MaterialsNotProcessedOnSite;
            var contaminents = accreditationMaterial.MaterialReprocessorDetails.Contaminents;
            var processLoss = accreditationMaterial.MaterialReprocessorDetails.ProcessLoss;
            var totalWasteOutputsLastCalendarYear = materialsNotProcessedOnSite + contaminents + processLoss;

            var materialRow = BuildRow(id, materialId, "Material", materialName.ToListSingle(), "Material");
            var ukSourceOfWasteRow = BuildRow(id, materialId, "Uk source of the waste", wasteSource.ToListSingle(), "WasteSource");
            var annualCapacityRow = BuildRow(id, materialId, "Annual total processing capacity", annualCapacity.ToListSingle(), "ProcessingCapacity");
            var weeklyCapacityRow = BuildRow(id, materialId, "Average weekly processing capacity", weeklyCapacity.ToListSingle(), "ProcessingCapacity");
            var detailsSectionRows = new List<CheckAnswersSectionRow>
            {
                materialRow,
                ukSourceOfWasteRow,
                annualCapacityRow,
                weeklyCapacityRow
            };
            var detailsSection = BuildSection(
                "Details",
                detailsSectionRows);

            var ukPackagingWasteRow = BuildRow(id, materialId, "Uk packaging waste", ukPackagingWaste.ToListSingle(), "MaterialWasteInputs");
            var nonUkPackagingWasteRow = BuildRow(id, materialId, "Non-UK packaging waste", nonUkPackagingWaste.ToListSingle(), "MaterialWasteInputs");
            var nonPackagingWasteRow = BuildRow(id, materialId, "Non-packaging waste", nonPackagingWaste.ToListSingle(), "MaterialWasteInputs");
            var totalWasteInputsLastCalendarYearRow = BuildRow(id, materialId, "Total", totalWasteInputsLastCalendarYear.ToListSingle(), string.Empty);
            var wasteInputsforLastYearRows = new List<CheckAnswersSectionRow>
            {
                ukPackagingWasteRow,
                nonUkPackagingWasteRow,
                nonPackagingWasteRow,
                totalWasteInputsLastCalendarYearRow
            };
            var wasteInputsLastYearSection = BuildSection(
                "Waste inputs for last calendar year",
                wasteInputsforLastYearRows);

            var materialsNotProcessedOnSiteRow = BuildRow(id, materialId, "Material not processed on site", materialsNotProcessedOnSite.ToListSingle(), "MaterialOutputs");
            var contaminentsRow = BuildRow(id, materialId, "Contaminants", contaminents.ToListSingle(), "MaterialOutputs");
            var processLossRow = BuildRow(id, materialId, "Process loss", processLoss.ToListSingle(), "MaterialOutputs");
            var totalWasteOutputsLastCalendarYearRow = BuildRow(id, materialId, "Total", totalWasteOutputsLastCalendarYear.ToListSingle(), string.Empty);
            var wasteOutputsforLastYearRows = new List<CheckAnswersSectionRow>
            {
                materialsNotProcessedOnSiteRow,
                contaminentsRow,
                processLossRow,
                totalWasteOutputsLastCalendarYearRow
            };
            var wasteOutputsLastYearSection = BuildSection(
                "Non-waste inputs for last calendar year",
                wasteOutputsforLastYearRows);

            var listOfSections = new List<CheckAnswersSectionDto>
            {
                detailsSection,
                wasteInputsLastYearSection,
                wasteOutputsLastYearSection
            };

            return new CheckAnswers
            {
                Completed = IsComplete(listOfSections),
                SiteAddress = siteAddress,
                Sections = listOfSections
            };
        }

        /// <summary>
        /// Retrieves the data for the AboutMaterialExporter enum
        /// </summary>
        /// <param name="id">Accreditation ID</param>
        /// <param name="materialId">Overseas site material ID</param>
        /// <param name="accreditationMaterial">The overseas site material</param>
        /// <returns>CheckAnswers DTO with the data</returns>
        private CheckAnswers BuildCyaAboutMaterialExporter(
            Guid id,
            Guid materialId,
            AccreditationMaterial accreditationMaterial)
        {
            var materialName = accreditationMaterial.Material.English;
            var wasteSource = accreditationMaterial.WasteSource;
            var listOfCommodityCodes = accreditationMaterial.WasteCodes.Select(x => x.Code).ToList();
            var listOfSignatories = new List<string>
            {
                "Andrew Shey, Management Accountant",
                "Gary Law, Strategic Buyer",
                "Scott McAllister, PRN signatory",
                "Shehzad Ismail, Software Developer"
            };

            var materialRow = BuildRow(id, materialId, "Material", new List<string> { materialName }, "Material");
            var ukSourceOfWasteRow = BuildRow(id, materialId, "Uk source of the waste", new List<string> { wasteSource }, "WasteSource");
            var commodityCodesRow = BuildRow(id, materialId, "Commodity codes", listOfCommodityCodes, "CommodityCodes");
            var peopleAuthorityRow = BuildRow(id, materialId, "People who have authority to issue PERNs", listOfSignatories, "Authority");
            var detailsSectionRows = new List<CheckAnswersSectionRow>
            {
                materialRow,
                ukSourceOfWasteRow,
                commodityCodesRow,
                peopleAuthorityRow
            };

            var detailsSection = BuildSection("Details", detailsSectionRows);

            var listOfSections = new List<CheckAnswersSectionDto>
            {
                detailsSection
            };

            return new CheckAnswers
            {
                Completed = IsComplete(listOfSections),
                SiteAddress = new Address(),
                Sections = listOfSections
            };
        }

        /// <summary>
        /// Builds a row for the CYA page within a section
        /// </summary>
        /// <param name="id">Accreditation ID</param>
        /// <param name="materialId">Site material ID</param>
        /// <param name="titleKey">Key of the row title e.g Commodity codes</param>
        /// <param name="value">The value of the answer</param>
        /// <param name="actionName">The name of the action that the link needs to point to</param>
        /// <returns>CheckAnswersSectionRow DTO</returns>
        private static CheckAnswersSectionRow BuildRow(
            Guid id,
            Guid? materialId,
            string titleKey,
            List<string> value,
            string actionName)
        {
            var changeLink = $"/Accreditation/{id}/Site/Material/{materialId}/{actionName}?rtap=y";

            return new CheckAnswersSectionRow
            {
                TitleKey = titleKey,
                Value = value,
                ChangeLink = changeLink
            };
        }

        /// <summary>
        /// Builds a section within a CYA page
        /// </summary>
        /// <param name="sectionTitle">The title of the section e.g. Waste inputs for last calendar year</param>
        /// <param name="rows">The row DTOs for the section</param>
        /// <returns>CheckAnswersSectionDto</returns>
        private static CheckAnswersSectionDto BuildSection(
            string sectionTitle,
            List<CheckAnswersSectionRow> rows)
        {
            var isCompletedSection = IsComplete(rows);

            return new CheckAnswersSectionDto
            {
                Title = sectionTitle,
                Completed = isCompletedSection,
                SectionRows = rows
            };
        }

        /// <summary>
        /// Checks if each question has been answered
        /// </summary>
        /// <param name="rows">The list of rows for a given section</param>
        /// <returns>True or False</returns>
        private static bool IsComplete(List<CheckAnswersSectionRow> rows)
        {
            foreach (var row in rows)
            {
                if (row.Value != null)
                {
                    continue;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Checks if each section has been completed
        /// </summary>
        /// <param name="sections">The list of sections for the CYA page</param>
        /// <returns>True or False</returns>
        private static bool IsComplete(List<CheckAnswersSectionDto> sections)
        {
            foreach (var section in sections)
            {
                if (section.Completed)
                {
                    continue;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
    }
}
