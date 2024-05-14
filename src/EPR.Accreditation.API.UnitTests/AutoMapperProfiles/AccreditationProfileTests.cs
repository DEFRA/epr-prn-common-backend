namespace EPR.Accreditation.API.UnitTests.AutoMapperProfiles
{
    using AutoFixture;
    using AutoMapper;
    using EPR.Accreditation.API.Common.Data.DataModels;
    using EPR.Accreditation.API.Profiles;
    using Data = EPR.Accreditation.API.Common.Data.DataModels;
    using Dto = EPR.Accreditation.API.Common.Dtos;

    [TestClass]
    public class AccreditationProfileTests
    {
        private IMapper _mapper;
        private Fixture _fixture;

        [TestInitialize]
        public void Init()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AccreditationProfile());
                cfg.AddProfile(new EnumProfile());
            });

            _mapper = new Mapper(config);
            _fixture = new Fixture();
        }

        [TestMethod]
        public void ConfigurationIsValid()
        {
            // Act
            _mapper.ConfigurationProvider.AssertConfigurationIsValid();

            // Assert
            // No exception should be thrown
        }

        [TestMethod]
        public void DtoAccreditationMaterial_MapsWasteDescriptionWasteCodes_ToDataAccreditationMaterial()
        {
            // Arrange
            // new waste codes
            var newWasteCode1 = new Dto.WasteCode
            {
                WasteCodeTypeId = Common.Enums.WasteCodeType.WasteDescriptionCode,
                Code = "ABC"
            };
            var newWasteCode2 = new Dto.WasteCode
            {
                WasteCodeTypeId = Common.Enums.WasteCodeType.WasteDescriptionCode,
                Code = "DEF"
            };

            // existing waste codes - not in dto
            var toBeDeletedWasteCode = new Data.WasteCode
            {
                WasteCodeTypeId = Common.Data.Enums.WasteCodeType.WasteDescriptionCode,
                Code = "GHI"
            };
            var willNotBeDeletedBecauseDifferentType = new Data.WasteCode
            {
                WasteCodeTypeId = Common.Data.Enums.WasteCodeType.MaterialCommodityCode,
                Code = "ZZZ"
            };

            // matching waste codes
            var dataSameAsDto = new Data.WasteCode
            {
                WasteCodeTypeId = Common.Data.Enums.WasteCodeType.WasteDescriptionCode,
                Code = "DEF"
            };

            var dtoWasteCodeList = new List<Dto.WasteCode>
            {
                newWasteCode1,
                newWasteCode2
            };

            var dataWasteCodeList = new List<Data.WasteCode>
            {
                toBeDeletedWasteCode,
                willNotBeDeletedBecauseDifferentType,
                dataSameAsDto
            };

            var dtoAccreditationMaterial = _fixture
                .Build<Dto.Request.AccreditationMaterial>()
                .Without(m => m.MaterialReprocessorDetails)
                .With(m =>
                    m.WasteCodes,
                    dtoWasteCodeList)
                .Create();

            var dataAccreditationMaterial = _fixture
                .Build<Data.AccreditationMaterial>()
                .Without(m => m.Accreditation)
                .Without(m => m.Site)
                .Without(m => m.OverseasReprocessingSite)
                .Without(m => m.MaterialReprocessorDetails)
                .Without(m => m.AccreditationTaskProgressMaterials)
                .Without(m => m.Material)
                .With(m => m.WasteCodes, dataWasteCodeList)
                .Create();

            // Act
            /* This should end with a list of waste codes that has one added (ABC), one that doesn't change (DEF), another
             * doesn't change because of a different type (ZZZ), and one that is removed (GHI) */
            _mapper.Map(dtoAccreditationMaterial, dataAccreditationMaterial);

            // Assert
            Assert.IsNotNull(dataAccreditationMaterial);
            Assert.IsTrue(dataAccreditationMaterial.WasteCodes.Count == 3);

            var dataList = dataAccreditationMaterial.WasteCodes.ToList();

            Assert.AreEqual("ZZZ", dataList[0].Code);
            Assert.AreEqual(Common.Data.Enums.WasteCodeType.MaterialCommodityCode, dataList[0].WasteCodeTypeId);
            Assert.AreEqual("DEF", dataList[1].Code);
            Assert.AreEqual(Common.Data.Enums.WasteCodeType.WasteDescriptionCode, dataList[1].WasteCodeTypeId);
            Assert.AreEqual("ABC", dataList[2].Code);
            Assert.AreEqual(Common.Data.Enums.WasteCodeType.WasteDescriptionCode, dataList[2].WasteCodeTypeId);
        }

        [TestMethod]
        public void DtoAccreditationMaterial_MapsMaterialCommodityCodeWasteCodes_ToDataAccreditationMaterial()
        {
            // Arrange
            // new waste codes
            var newWasteCode1 = new Dto.WasteCode
            {
                WasteCodeTypeId = Common.Enums.WasteCodeType.MaterialCommodityCode,
                Code = "ABC"
            };
            var newWasteCode2 = new Dto.WasteCode
            {
                WasteCodeTypeId = Common.Enums.WasteCodeType.MaterialCommodityCode,
                Code = "DEF"
            };

            // existing waste codes - not in dto
            var toBeDeletedWasteCode = new Data.WasteCode
            {
                WasteCodeTypeId = Common.Data.Enums.WasteCodeType.MaterialCommodityCode,
                Code = "GHI"
            };
            var willNotBeDeletedBecauseDifferentType = new Data.WasteCode
            {
                WasteCodeTypeId = Common.Data.Enums.WasteCodeType.WasteDescriptionCode,
                Code = "ZZZ"
            };

            // matching waste codes
            var dataSameAsDto = new Data.WasteCode
            {
                WasteCodeTypeId = Common.Data.Enums.WasteCodeType.MaterialCommodityCode,
                Code = "DEF"
            };

            var dtoWasteCodeList = new List<Dto.WasteCode>
            {
                newWasteCode1,
                newWasteCode2
            };

            var dataWasteCodeList = new List<Data.WasteCode>
            {
                toBeDeletedWasteCode,
                willNotBeDeletedBecauseDifferentType,
                dataSameAsDto
            };

            var dtoAccreditationMaterial = _fixture
                .Build<Dto.Request.AccreditationMaterial>()
                .Without(m => m.MaterialReprocessorDetails)
                .With(m =>
                    m.WasteCodes,
                    dtoWasteCodeList)
                .Create();

            var dataAccreditationMaterial = _fixture
                .Build<Data.AccreditationMaterial>()
                .Without(m => m.Accreditation)
                .Without(m => m.Site)
                .Without(m => m.OverseasReprocessingSite)
                .Without(m => m.MaterialReprocessorDetails)
                .Without(m => m.AccreditationTaskProgressMaterials)
                .Without(m => m.Material)
                .With(m => m.WasteCodes, dataWasteCodeList)
                .Create();

            // Act
            /* This should end with a list of waste codes that has one added (ABC), one that doesn't change (DEF), another
             * doesn't change because of a different type (ZZZ), and one that is removed (GHI) */
            _mapper.Map(dtoAccreditationMaterial, dataAccreditationMaterial);

            // Assert
            Assert.IsNotNull(dataAccreditationMaterial);
            Assert.IsTrue(dataAccreditationMaterial.WasteCodes.Count == 3);

            var dataList = dataAccreditationMaterial.WasteCodes.ToList();

            Assert.AreEqual("ZZZ", dataList[0].Code);
            Assert.AreEqual(Common.Data.Enums.WasteCodeType.WasteDescriptionCode, dataList[0].WasteCodeTypeId);
            Assert.AreEqual("DEF", dataList[1].Code);
            Assert.AreEqual(Common.Data.Enums.WasteCodeType.MaterialCommodityCode, dataList[1].WasteCodeTypeId);
            Assert.AreEqual("ABC", dataList[2].Code);
            Assert.AreEqual(Common.Data.Enums.WasteCodeType.MaterialCommodityCode, dataList[2].WasteCodeTypeId);
        }

        [TestMethod]
        public void DtoAccreditationMaterial_WithNoDestinationList_MapsAllDtoToDataList()
        {
            // Arrange
            // new waste codes
            var newWasteCode1 = new Dto.WasteCode
            {
                WasteCodeTypeId = Common.Enums.WasteCodeType.MaterialCommodityCode,
                Code = "ABC"
            };
            var newWasteCode2 = new Dto.WasteCode
            {
                WasteCodeTypeId = Common.Enums.WasteCodeType.MaterialCommodityCode,
                Code = "DEF"
            };

            var dtoWasteCodeList = new List<Dto.WasteCode>
            {
                newWasteCode1,
                newWasteCode2
            };

            var dtoAccreditationMaterial = _fixture
                .Build<Dto.Request.AccreditationMaterial>()
                .Without(m => m.MaterialReprocessorDetails)
                .With(m =>
                    m.WasteCodes,
                    dtoWasteCodeList)
                .Create();

            var dataAccreditationMaterial = _fixture
                .Build<Data.AccreditationMaterial>()
                .Without(m => m.Accreditation)
                .Without(m => m.Site)
                .Without(m => m.OverseasReprocessingSite)
                .Without(m => m.MaterialReprocessorDetails)
                .Without(m => m.AccreditationTaskProgressMaterials)
                .Without(m => m.Material)
                .Without(m => m.WasteCodes) // ensures a null list
                .Create();

            // Act
            /* Destination list does not exist, so will result in both dtos being added */
            _mapper.Map(dtoAccreditationMaterial, dataAccreditationMaterial);

            // Assert
            Assert.IsNotNull(dataAccreditationMaterial);
            Assert.IsTrue(dataAccreditationMaterial.WasteCodes.Count == 2);

            var dataList = dataAccreditationMaterial.WasteCodes.ToList();

            Assert.AreEqual("ABC", dataList[0].Code);
            Assert.AreEqual(Common.Data.Enums.WasteCodeType.MaterialCommodityCode, dataList[0].WasteCodeTypeId);
            Assert.AreEqual("DEF", dataList[1].Code);
            Assert.AreEqual(Common.Data.Enums.WasteCodeType.MaterialCommodityCode, dataList[1].WasteCodeTypeId);
        }

        [TestMethod]
        public void DtoAccreditationMaterial_KeepsDestinationListUntouchedWhenSourceListDoesNotExist()
        {
            // Arrange
            
            // existing waste codes - not in dto
            var existingData1 = new Data.WasteCode
            {
                WasteCodeTypeId = Common.Data.Enums.WasteCodeType.MaterialCommodityCode,
                Code = "GHI"
            };
            var existingData2 = new Data.WasteCode
            {
                WasteCodeTypeId = Common.Data.Enums.WasteCodeType.WasteDescriptionCode,
                Code = "ZZZ"
            };

            // matching waste codes
            var existingData3 = new Data.WasteCode
            {
                WasteCodeTypeId = Common.Data.Enums.WasteCodeType.MaterialCommodityCode,
                Code = "DEF"
            };

            var dataWasteCodeList = new List<Data.WasteCode>
            {
                existingData1,
                existingData2,
                existingData3
            };

            var dtoAccreditationMaterial = _fixture
                .Build<Dto.Request.AccreditationMaterial>()
                .Without(m => m.MaterialReprocessorDetails)
                .Without(m => m.WasteCodes)
                .Create();

            var dataAccreditationMaterial = _fixture
                .Build<Data.AccreditationMaterial>()
                .Without(m => m.Accreditation)
                .Without(m => m.Site)
                .Without(m => m.OverseasReprocessingSite)
                .Without(m => m.MaterialReprocessorDetails)
                .Without(m => m.AccreditationTaskProgressMaterials)
                .Without(m => m.Material)
                .With(m => m.WasteCodes, dataWasteCodeList)
                .Create();

            // Act
            /* This should end with a list of waste codes that has one added (ABC), one that doesn't change (DEF), another
             * doesn't change because of a different type (ZZZ), and one that is removed (GHI) */
            _mapper.Map(dtoAccreditationMaterial, dataAccreditationMaterial);

            // Assert
            Assert.IsNotNull(dataAccreditationMaterial);
            Assert.IsTrue(dataAccreditationMaterial.WasteCodes.Count == 3);

            var dataList = dataAccreditationMaterial.WasteCodes.ToList();

            Assert.AreEqual("GHI", dataList[0].Code);
            Assert.AreEqual(Common.Data.Enums.WasteCodeType.MaterialCommodityCode, dataList[0].WasteCodeTypeId);
            Assert.AreEqual("ZZZ", dataList[1].Code);
            Assert.AreEqual(Common.Data.Enums.WasteCodeType.WasteDescriptionCode, dataList[1].WasteCodeTypeId);
            Assert.AreEqual("DEF", dataList[2].Code);
            Assert.AreEqual(Common.Data.Enums.WasteCodeType.MaterialCommodityCode, dataList[2].WasteCodeTypeId);
        }

        [TestMethod]
        public void DataSiteExemptionReferences_MapsToDtoObject()
        {
            // Arrange
            var exemptionReferences = _fixture.Build<Data.ExemptionReference>()
                .Without(er => er.Site)
                .CreateMany()
                .ToList();

            var dataSite = _fixture
                .Build<Data.Site>()
                .Without(s => s.Address)
                .Without(s => s.Accreditations)
                .Without(s => s.AccreditationMaterials)
                .Without(s => s.SiteAuthorities)
                .With(s => s.ExemptionReferences, exemptionReferences)
                .Create();

            var dtoSite = _fixture
                .Build<Dto.Site>()
                .Without(s => s.Address)
                .Without(s => s.SiteAuthorities)
                .Without(s => s.ExemptionReferences)
                .Create();

            // Act
            _mapper.Map(dataSite, dtoSite);

            // Arrange
            Assert.IsNotNull(dtoSite.ExemptionReferences);
            Assert.IsTrue(dtoSite.ExemptionReferences.Count() == exemptionReferences.Count);

            var dtoList = dtoSite.ExemptionReferences.ToList();

            for (int i = 0; i < exemptionReferences.Count; i++)
            {
                Assert.AreEqual(exemptionReferences[i].Reference, dtoList[i]);
            }
        }

        [TestMethod]
        public void DataSiteExemptionReferences_MapsToDtoObject_WhenDataListNonExistant()
        {
            // Arrange
            var dataSite = _fixture
                .Build<Data.Site>()
                .Without(s => s.Address)
                .Without(s => s.Accreditations)
                .Without(s => s.AccreditationMaterials)
                .Without(s => s.SiteAuthorities)
                .Without(s => s.ExemptionReferences)
                .Create();

            var dtoSite = _fixture
                .Build<Dto.Site>()
                .Without(s => s.Address)
                .Without(s => s.SiteAuthorities)
                .Without(s => s.ExemptionReferences)
                .Create();

            // Act
            _mapper.Map(dataSite, dtoSite);

            // Arrange
            Assert.IsNotNull(dtoSite.ExemptionReferences);
            Assert.IsFalse(dtoSite.ExemptionReferences.Any());
        }

        [TestMethod]
        public void DtoSiteExemptionReferences_MapsToDataObject()
        {
            // Arrange
            var exemptionReferences = _fixture.Build<string>()
                .CreateMany()
                .ToList();

            var dataSite = _fixture
                .Build<Data.Site>()
                .Without(s => s.Address)
                .Without(s => s.Accreditations)
                .Without(s => s.AccreditationMaterials)
                .Without(s => s.SiteAuthorities)
                .Without(s => s.ExemptionReferences)
                .Create();

            var dtoSite = _fixture
                .Build<Dto.Site>()
                .Without(s => s.Address)
                .Without(s => s.SiteAuthorities)
                .With(s => s.ExemptionReferences, exemptionReferences)
                .Create();

            // Act
            _mapper.Map(dtoSite, dataSite);

            // Assert
            Assert.IsNotNull(dataSite.ExemptionReferences);
            Assert.AreEqual(exemptionReferences.Count, dataSite.ExemptionReferences.Count);

            var dataList = dataSite.ExemptionReferences.ToList();

            for (int i = 0; i < exemptionReferences.Count; i++)
            {
                Assert.AreEqual(exemptionReferences[i], dataList[i].Reference);
            }
        }
    }
}
