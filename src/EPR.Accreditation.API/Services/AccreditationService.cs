﻿namespace EPR.Accreditation.API.Services
{
    using EPR.Accreditation.API.Common.Dtos;
    using EPR.Accreditation.API.Common.Enums;
    using EPR.Accreditation.API.Helpers.Comparers;
    using EPR.Accreditation.API.Repositories.Interfaces;
    using EPR.Accreditation.API.Services.Interfaces;
    using EPR.Accreditation.Facade.Common.Dtos;
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

        public async Task<CheckAnswers> GetCheckAnswers(
            Guid id,
            Guid materialId,
            Guid? siteId,
            Guid? overseasSiteId,
            Common.Enums.CheckAnswersSection section)
        {
            var accreditation = await GetAccreditation(id);
            var checkAnswersDto = new CheckAnswers();

            switch (section)
            {
                case Common.Enums.CheckAnswersSection.AboutMaterialReprocessorActuals:

                    if (accreditation.OperatorTypeId != Common.Enums.OperatorType.Reprocessor)
                    {
                        throw new ArgumentException("Can only be for a reprocessor");
                    }
                    else
                    {

                        var accreditationMaterial = await GetMaterial(id, materialId);

                        var materialNameEn = accreditationMaterial.Material.English;
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


                        var materialRow = new CheckAnswersSectionRow
                        {
                            TitleKey = "Material",
                            Value = materialNameEn,
                            ChangeLink = "/"
                        };

                        var ukSourceOfWasteRow = new CheckAnswersSectionRow
                        {
                            TitleKey = "Uk source of the waste",
                            Value = wasteSource,
                            ChangeLink = "/"
                        };

                        var annualCapacityRow = new CheckAnswersSectionRow
                        {
                            TitleKey = "Annual total processing capacity",
                            Value = annualCapacity,
                            ChangeLink = "/"
                        };

                        var weeklyCapacityRow = new CheckAnswersSectionRow
                        {
                            TitleKey = "Average weekly processing capacity",
                            Value = weeklyCapacity,
                            ChangeLink = "/"
                        };

                        var detailsSectionRows = new List<CheckAnswersSectionRow>
                        {
                            materialRow,
                            ukSourceOfWasteRow,
                            annualCapacityRow,
                            weeklyCapacityRow
                        };

                        var isCompletedDetailsSection = false;

                        foreach (var row in detailsSectionRows)
                        {
                            if (row.Value != null)
                            {
                                isCompletedDetailsSection = true;
                            }
                            else
                            {
                                isCompletedDetailsSection = false;
                            }
                        }

                        var detailsSection = new Common.Dtos.CheckAnswersSectionDto
                        {
                            Completed = isCompletedDetailsSection,
                            SectionRows = detailsSectionRows
                        };

                        var ukPackagingWasteRow = new CheckAnswersSectionRow
                        {
                            TitleKey = "Uk packaging waste",
                            Value = ukPackagingWaste,
                            ChangeLink = "/"
                        };

                        var nonUkPackagingWasteRow = new CheckAnswersSectionRow
                        {
                            TitleKey = "Non-UK packaging waste",
                            Value = nonUkPackagingWaste,
                            ChangeLink = "/"
                        };

                        var nonPackagingWasteRow = new CheckAnswersSectionRow
                        {
                            TitleKey = "Non-packaging waste",
                            Value = nonPackagingWaste,
                            ChangeLink = "/"
                        };

                        var totalWasteInputsLastCalendarYearRow = new CheckAnswersSectionRow
                        {
                            TitleKey = "Total",
                            Value = totalWasteInputsLastCalendarYear,
                        };

                        var wasteInputsforLastYearRows = new List<CheckAnswersSectionRow>
                        {
                            ukPackagingWasteRow,
                            nonUkPackagingWasteRow,
                            nonPackagingWasteRow,
                            totalWasteInputsLastCalendarYearRow
                        };

                        var isCompletedWasteInputsLastYearSection = false;

                        foreach (var row in wasteInputsforLastYearRows)
                        {
                            if (row.Value != null)
                            {
                                isCompletedWasteInputsLastYearSection = true;
                            }
                            else
                            {
                                isCompletedWasteInputsLastYearSection = false;
                            }
                        }

                        var wasteInputsLastYearSection = new Common.Dtos.CheckAnswersSectionDto
                        {
                            Completed = isCompletedWasteInputsLastYearSection,
                            SectionRows = wasteInputsforLastYearRows
                        };

                        var materialsNotProcessedOnSiteRow = new CheckAnswersSectionRow
                        {

                        };

                        var contaminentsRow = new CheckAnswersSectionRow
                        {

                        };

                        var processLossRow = new CheckAnswersSectionRow
                        {

                        };

                        var totalWasteOutputsLastCalendarYearRow = new CheckAnswersSectionRow
                        {

                        };

                        var wasteOutputsforLastYearRows = new List<CheckAnswersSectionRow>
                        {
                            materialsNotProcessedOnSiteRow,
                            contaminentsRow,
                            processLossRow,
                            totalWasteOutputsLastCalendarYearRow
                        };

                        var wasteOutputsLastYearSection = new Common.Dtos.CheckAnswersSectionDto
                        {
                            SectionRows = wasteOutputsforLastYearRows
                        };


                        var listOfSections = new List<CheckAnswersSectionDto>
                        {
                            detailsSection,
                            wasteInputsLastYearSection,
                            wasteOutputsLastYearSection
                        };

                        return new CheckAnswers
                        {
                            Sections = listOfSections
                        };
                    }
            }

            return checkAnswersDto;
        }
    }
}
