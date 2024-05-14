IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240319093911_initial')
BEGIN
    IF SCHEMA_ID(N'Lookup') IS NULL EXEC(N'CREATE SCHEMA [Lookup];');
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240319093911_initial')
BEGIN
    CREATE TABLE [Lookup].[AccreditationStatus] (
        [Id] int NOT NULL,
        [Name] nvarchar(30) NOT NULL,
        CONSTRAINT [PK_AccreditationStatus] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240319093911_initial')
BEGIN
    CREATE TABLE [Lookup].[Country] (
        [CountryId] int NOT NULL IDENTITY,
        [CountryCode] nvarchar(2) NULL,
        [Name] nvarchar(200) NULL,
        CONSTRAINT [PK_Country] PRIMARY KEY ([CountryId])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240319093911_initial')
BEGIN
    CREATE TABLE [Lookup].[FileUploadStatus] (
        [Id] int NOT NULL,
        [Name] nvarchar(30) NOT NULL,
        CONSTRAINT [PK_FileUploadStatus] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240319093911_initial')
BEGIN
    CREATE TABLE [Lookup].[FileUploadType] (
        [Id] int NOT NULL,
        [Name] nvarchar(30) NOT NULL,
        CONSTRAINT [PK_FileUploadType] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240319093911_initial')
BEGIN
    CREATE TABLE [Lookup].[OperatorType] (
        [Id] int NOT NULL,
        [Name] nvarchar(12) NOT NULL,
        CONSTRAINT [PK_OperatorType] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240319093911_initial')
BEGIN
    CREATE TABLE [Lookup].[ReprocessorSupportingInformationType] (
        [Id] int NOT NULL,
        [Name] nvarchar(30) NULL,
        CONSTRAINT [PK_ReprocessorSupportingInformationType] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240319093911_initial')
BEGIN
    CREATE TABLE [Site] (
        [Id] int NOT NULL IDENTITY,
        [ExternalId] uniqueidentifier NOT NULL,
        [Address1] nvarchar(100) NOT NULL,
        [Address2] nvarchar(100) NULL,
        [Town] nvarchar(100) NOT NULL,
        [County] nvarchar(100) NULL,
        [Postcode] nvarchar(10) NOT NULL,
        [OrganisationId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_Site] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240319093911_initial')
BEGIN
    CREATE TABLE [Lookup].[TaskName] (
        [Id] int NOT NULL,
        [Name] nvarchar(30) NOT NULL,
        CONSTRAINT [PK_TaskName] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240319093911_initial')
BEGIN
    CREATE TABLE [Lookup].[TaskStatus] (
        [Id] int NOT NULL,
        [Name] nvarchar(30) NOT NULL,
        CONSTRAINT [PK_TaskStatus] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240319093911_initial')
BEGIN
    CREATE TABLE [Lookup].[WasteCodeType] (
        [Id] int NOT NULL,
        [Name] nvarchar(30) NOT NULL,
        CONSTRAINT [PK_WasteCodeType] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240319093911_initial')
BEGIN
    CREATE TABLE [OverseasAddress] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(100) NULL,
        [CountryId] int NOT NULL,
        [Address] nvarchar(500) NULL,
        CONSTRAINT [PK_OverseasAddress] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_OverseasAddress_Country_CountryId] FOREIGN KEY ([CountryId]) REFERENCES [Lookup].[Country] ([CountryId]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240319093911_initial')
BEGIN
    CREATE TABLE [Accreditation] (
        [Id] int NOT NULL IDENTITY,
        [ExternalId] uniqueidentifier NOT NULL,
        [OperatorTypeId] int NOT NULL,
        [ReferenceNumber] nvarchar(12) NULL,
        [OrganisationId] uniqueidentifier NOT NULL,
        [Large] bit NULL,
        [AccreditationStatusId] int NOT NULL,
        [SiteId] int NULL,
        [CreatedBy] uniqueidentifier NOT NULL,
        [CreatedOn] datetime2 NOT NULL,
        [UpdatedBy] uniqueidentifier NULL,
        [UpdatedOn] datetime2 NULL,
        CONSTRAINT [PK_Accreditation] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Accreditation_AccreditationStatus_AccreditationStatusId] FOREIGN KEY ([AccreditationStatusId]) REFERENCES [Lookup].[AccreditationStatus] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_Accreditation_OperatorType_OperatorTypeId] FOREIGN KEY ([OperatorTypeId]) REFERENCES [Lookup].[OperatorType] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_Accreditation_Site_SiteId] FOREIGN KEY ([SiteId]) REFERENCES [Site] ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240319093911_initial')
BEGIN
    CREATE TABLE [ExemptionReference] (
        [Id] int NOT NULL IDENTITY,
        [SiteId] int NOT NULL,
        [Reference] nvarchar(max) NULL,
        CONSTRAINT [PK_ExemptionReference] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ExemptionReference_Site_SiteId] FOREIGN KEY ([SiteId]) REFERENCES [Site] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240319093911_initial')
BEGIN
    CREATE TABLE [SiteAuthority] (
        [Id] int NOT NULL IDENTITY,
        [UserId] uniqueidentifier NOT NULL,
        [SiteId] int NOT NULL,
        CONSTRAINT [PK_SiteAuthority] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_SiteAuthority_Site_SiteId] FOREIGN KEY ([SiteId]) REFERENCES [Site] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240319093911_initial')
BEGIN
    CREATE TABLE [AccreditationTaskProgress] (
        [Id] int NOT NULL IDENTITY,
        [TaskStatusId] int NOT NULL,
        [TaskNameId] int NOT NULL,
        [AccreditationId] int NOT NULL,
        CONSTRAINT [PK_AccreditationTaskProgress] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AccreditationTaskProgress_Accreditation_AccreditationId] FOREIGN KEY ([AccreditationId]) REFERENCES [Accreditation] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_AccreditationTaskProgress_TaskName_TaskNameId] FOREIGN KEY ([TaskNameId]) REFERENCES [Lookup].[TaskName] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_AccreditationTaskProgress_TaskStatus_TaskStatusId] FOREIGN KEY ([TaskStatusId]) REFERENCES [Lookup].[TaskStatus] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240319093911_initial')
BEGIN
    CREATE TABLE [FileUpload] (
        [Id] int NOT NULL IDENTITY,
        [AccreditationId] int NOT NULL,
        [Filename] nvarchar(50) NOT NULL,
        [FileId] uniqueidentifier NULL,
        [DateUploaded] datetime2 NOT NULL,
        [UploadedBy] nvarchar(50) NOT NULL,
        [FileUploadTypeId] int NOT NULL,
        [Status] int NOT NULL,
        CONSTRAINT [PK_FileUpload] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_FileUpload_Accreditation_AccreditationId] FOREIGN KEY ([AccreditationId]) REFERENCES [Accreditation] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_FileUpload_FileUploadStatus_Status] FOREIGN KEY ([Status]) REFERENCES [Lookup].[FileUploadStatus] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_FileUpload_FileUploadType_FileUploadTypeId] FOREIGN KEY ([FileUploadTypeId]) REFERENCES [Lookup].[FileUploadType] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240319093911_initial')
BEGIN
    CREATE TABLE [OverseasReprocessingSite] (
        [Id] int NOT NULL IDENTITY,
        [ExternalId] uniqueidentifier NOT NULL,
        [AccreditationId] int NOT NULL,
        [OverseasAddressId] int NULL,
        [UkPorts] nvarchar(500) NULL,
        [Outputs] nvarchar(500) NULL,
        [RejectedPlans] nvarchar(500) NULL,
        CONSTRAINT [PK_OverseasReprocessingSite] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_OverseasReprocessingSite_Accreditation_AccreditationId] FOREIGN KEY ([AccreditationId]) REFERENCES [Accreditation] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_OverseasReprocessingSite_OverseasAddress_OverseasAddressId] FOREIGN KEY ([OverseasAddressId]) REFERENCES [OverseasAddress] ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240319093911_initial')
BEGIN
    CREATE TABLE [AccreditationMaterial] (
        [Id] int NOT NULL IDENTITY,
        [ExternalId] uniqueidentifier NOT NULL,
        [MaterialId] int NOT NULL,
        [AnnualCapacity] decimal(10,3) NOT NULL,
        [WeeklyCapacity] decimal(10,3) NOT NULL,
        [WasteSource] nvarchar(max) NULL,
        [SiteId] int NULL,
        [OverseasReprocessingSiteId] int NULL,
        CONSTRAINT [PK_AccreditationMaterial] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AccreditationMaterial_OverseasReprocessingSite_OverseasReprocessingSiteId] FOREIGN KEY ([OverseasReprocessingSiteId]) REFERENCES [OverseasReprocessingSite] ([Id]),
        CONSTRAINT [FK_AccreditationMaterial_Site_SiteId] FOREIGN KEY ([SiteId]) REFERENCES [Site] ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240319093911_initial')
BEGIN
    CREATE TABLE [OverseasAgent] (
        [Id] int NOT NULL IDENTITY,
        [OverseasAddressId] int NOT NULL,
        [OverseasReprocessingSiteId] int NOT NULL,
        [Fullname] nvarchar(100) NULL,
        [Position] nvarchar(100) NULL,
        [Telephone] nvarchar(30) NULL,
        [Email] nvarchar(50) NULL,
        CONSTRAINT [PK_OverseasAgent] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_OverseasAgent_OverseasAddress_OverseasAddressId] FOREIGN KEY ([OverseasAddressId]) REFERENCES [OverseasAddress] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_OverseasAgent_OverseasReprocessingSite_OverseasReprocessingSiteId] FOREIGN KEY ([OverseasReprocessingSiteId]) REFERENCES [OverseasReprocessingSite] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240319093911_initial')
BEGIN
    CREATE TABLE [WastePermit] (
        [Id] int NOT NULL IDENTITY,
        [AccreditationId] int NOT NULL,
        [OverseasReprocessingSiteId] int NULL,
        [DealerRegistrationNumber] nvarchar(100) NULL,
        [EnvironmentalPermitNumber] nvarchar(100) NULL,
        [PartAActivityReferenceNumber] nvarchar(10) NULL,
        [PartBActivityReferenceNumber] nvarchar(10) NULL,
        [DischargeConsentNumber] nvarchar(50) NULL,
        [WastePermitExemption] bit NULL,
        CONSTRAINT [PK_WastePermit] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_WastePermit_Accreditation_AccreditationId] FOREIGN KEY ([AccreditationId]) REFERENCES [Accreditation] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_WastePermit_OverseasReprocessingSite_OverseasReprocessingSiteId] FOREIGN KEY ([OverseasReprocessingSiteId]) REFERENCES [OverseasReprocessingSite] ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240319093911_initial')
BEGIN
    CREATE TABLE [AccreditationTaskProgressMaterial] (
        [Id] int NOT NULL IDENTITY,
        [AccreditationTaskProgressId] int NOT NULL,
        [AccreditationMaterialId] int NOT NULL,
        [TaskStatusId] int NOT NULL,
        CONSTRAINT [PK_AccreditationTaskProgressMaterial] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AccreditationTaskProgressMaterial_AccreditationMaterial_AccreditationMaterialId] FOREIGN KEY ([AccreditationMaterialId]) REFERENCES [AccreditationMaterial] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_AccreditationTaskProgressMaterial_AccreditationTaskProgress_AccreditationTaskProgressId] FOREIGN KEY ([AccreditationTaskProgressId]) REFERENCES [AccreditationTaskProgress] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240319093911_initial')
BEGIN
    CREATE TABLE [MaterialReprocessorDetails] (
        [Id] int NOT NULL IDENTITY,
        [AccreditationMaterialId] int NOT NULL,
        [WasteLastYear] bit NOT NULL,
        [UkPackagingWaste] decimal(10,3) NULL,
        [NonUkPackagingWaste] decimal(10,3) NULL,
        [NonPackagingWaste] decimal(10,3) NULL,
        [MaterialsNotProcessedOnSite] decimal(10,3) NULL,
        [Contaminents] decimal(10,3) NULL,
        [ProcessLoss] decimal(10,3) NULL,
        CONSTRAINT [PK_MaterialReprocessorDetails] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_MaterialReprocessorDetails_AccreditationMaterial_AccreditationMaterialId] FOREIGN KEY ([AccreditationMaterialId]) REFERENCES [AccreditationMaterial] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240319093911_initial')
BEGIN
    CREATE TABLE [WasteCode] (
        [Id] int NOT NULL IDENTITY,
        [AccreditationMaterialId] int NOT NULL,
        [Code] nvarchar(50) NULL,
        [WasteCodeTypeId] int NOT NULL,
        CONSTRAINT [PK_WasteCode] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_WasteCode_AccreditationMaterial_AccreditationMaterialId] FOREIGN KEY ([AccreditationMaterialId]) REFERENCES [AccreditationMaterial] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_WasteCode_WasteCodeType_WasteCodeTypeId] FOREIGN KEY ([WasteCodeTypeId]) REFERENCES [Lookup].[WasteCodeType] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240319093911_initial')
BEGIN
    CREATE TABLE [ReprocessorSupportingInformation] (
        [Id] int NOT NULL IDENTITY,
        [MaterialReprocessorDetailsId] int NOT NULL,
        [ReprocessorSupportingInformationTypeId] int NOT NULL,
        [Type] nvarchar(20) NULL,
        [Tonnes] decimal(10,3) NOT NULL,
        CONSTRAINT [PK_ReprocessorSupportingInformation] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ReprocessorSupportingInformation_MaterialReprocessorDetails_MaterialReprocessorDetailsId] FOREIGN KEY ([MaterialReprocessorDetailsId]) REFERENCES [MaterialReprocessorDetails] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_ReprocessorSupportingInformation_ReprocessorSupportingInformationType_ReprocessorSupportingInformationTypeId] FOREIGN KEY ([ReprocessorSupportingInformationTypeId]) REFERENCES [Lookup].[ReprocessorSupportingInformationType] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240319093911_initial')
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup].[AccreditationStatus]'))
        SET IDENTITY_INSERT [Lookup].[AccreditationStatus] ON;
    EXEC(N'INSERT INTO [Lookup].[AccreditationStatus] ([Id], [Name])
    VALUES (0, N''None''),
    (1, N''Started''),
    (2, N''Submitted''),
    (3, N''Accepted''),
    (4, N''Queried''),
    (5, N''Updated''),
    (6, N''Granted''),
    (7, N''Refused''),
    (8, N''Withdrawn''),
    (9, N''Suspended''),
    (10, N''Cancelled'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup].[AccreditationStatus]'))
        SET IDENTITY_INSERT [Lookup].[AccreditationStatus] OFF;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240319093911_initial')
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'CountryId', N'CountryCode', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup].[Country]'))
        SET IDENTITY_INSERT [Lookup].[Country] ON;
    EXEC(N'INSERT INTO [Lookup].[Country] ([CountryId], [CountryCode], [Name])
    VALUES (4, N''AF'', N''Afghanistan''),
    (8, N''AL'', N''Albania''),
    (10, N''AQ'', N''Antarctica''),
    (12, N''DZ'', N''Algeria''),
    (16, N''AS'', N''American Samoa''),
    (20, N''AD'', N''Andorra''),
    (24, N''AO'', N''Angola''),
    (28, N''AG'', N''Antigua and Barbuda''),
    (31, N''AZ'', N''Azerbaijan''),
    (32, N''AR'', N''Argentina''),
    (36, N''AU'', N''Australia''),
    (40, N''AT'', N''Austria''),
    (44, N''BS'', N''Bahamas''),
    (48, N''BH'', N''Bahrain''),
    (50, N''BD'', N''Bangladesh''),
    (51, N''AM'', N''Armenia''),
    (52, N''BB'', N''Barbados''),
    (56, N''BE'', N''Belgium''),
    (60, N''BM'', N''Bermuda''),
    (64, N''BT'', N''Bhutan''),
    (68, N''BO'', N''Bolivia (Plurinational State of)''),
    (70, N''BA'', N''Bosnia and Herzegovina''),
    (72, N''BW'', N''Botswana''),
    (74, N''BV'', N''Bouvet Island''),
    (76, N''BR'', N''Brazil''),
    (84, N''BZ'', N''Belize''),
    (86, N''IO'', N''British Indian Ocean Territory''),
    (90, N''SB'', N''Solomon Islands''),
    (92, N''VG'', N''Virgin Islands (British)''),
    (96, N''BN'', N''Brunei Darussalam''),
    (100, N''BG'', N''Bulgaria'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'CountryId', N'CountryCode', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup].[Country]'))
        SET IDENTITY_INSERT [Lookup].[Country] OFF;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240319093911_initial')
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'CountryId', N'CountryCode', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup].[Country]'))
        SET IDENTITY_INSERT [Lookup].[Country] ON;
    EXEC(N'INSERT INTO [Lookup].[Country] ([CountryId], [CountryCode], [Name])
    VALUES (104, N''MM'', N''Myanmar''),
    (108, N''BI'', N''Burundi''),
    (112, N''BY'', N''Belarus''),
    (116, N''KH'', N''Cambodia''),
    (120, N''CM'', N''Cameroon''),
    (124, N''CA'', N''Canada''),
    (132, N''CV'', N''Cabo Verde''),
    (136, N''KY'', N''Cayman Islands''),
    (140, N''CF'', N''Central African Republic''),
    (144, N''LK'', N''Sri Lanka''),
    (148, N''TD'', N''Chad''),
    (152, N''CL'', N''Chile''),
    (156, N''CN'', N''China''),
    (158, N''TW'', N''Taiwan, Province of China''),
    (162, N''CX'', N''Christmas Island''),
    (166, N''CC'', N''Cocos (Keeling) Islands''),
    (170, N''CO'', N''Colombia''),
    (174, N''KM'', N''Comoros''),
    (175, N''YT'', N''Mayotte''),
    (178, N''CG'', N''Congo''),
    (180, N''CD'', N''Congo (Democratic Republic of the)''),
    (184, N''CK'', N''Cook Islands''),
    (188, N''CR'', N''Costa Rica''),
    (191, N''HR'', N''Croatia''),
    (192, N''CU'', N''Cuba''),
    (196, N''CY'', N''Cyprus''),
    (203, N''CZ'', N''Czech Republic''),
    (204, N''BJ'', N''Benin''),
    (208, N''DK'', N''Denmark''),
    (212, N''DM'', N''Dominica''),
    (214, N''DO'', N''Dominican Republic''),
    (218, N''EC'', N''Ecuador''),
    (222, N''SV'', N''El Salvador''),
    (226, N''GQ'', N''Equatorial Guinea''),
    (231, N''ET'', N''Ethiopia''),
    (232, N''ER'', N''Eritrea''),
    (233, N''EE'', N''Estonia''),
    (234, N''FO'', N''Faroe Islands''),
    (238, N''FK'', N''Falkland Islands (Malvinas)''),
    (239, N''GS'', N''South Georgia and the South Sandwich Islands''),
    (242, N''FJ'', N''Fiji''),
    (246, N''FI'', N''Finland'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'CountryId', N'CountryCode', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup].[Country]'))
        SET IDENTITY_INSERT [Lookup].[Country] OFF;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240319093911_initial')
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'CountryId', N'CountryCode', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup].[Country]'))
        SET IDENTITY_INSERT [Lookup].[Country] ON;
    EXEC(N'INSERT INTO [Lookup].[Country] ([CountryId], [CountryCode], [Name])
    VALUES (248, N''AX'', N''Åland Islands''),
    (250, N''FR'', N''France''),
    (254, N''GF'', N''French Guiana''),
    (258, N''PF'', N''French Polynesia''),
    (260, N''TF'', N''French Southern Territories''),
    (262, N''DJ'', N''Djibouti''),
    (266, N''GA'', N''Gabon''),
    (268, N''GE'', N''Georgia''),
    (270, N''GM'', N''Gambia''),
    (275, N''PS'', N''Palestine, State of''),
    (276, N''DE'', N''Germany''),
    (288, N''GH'', N''Ghana''),
    (292, N''GI'', N''Gibraltar''),
    (296, N''KI'', N''Kiribati''),
    (300, N''GR'', N''Greece''),
    (304, N''GL'', N''Greenland''),
    (308, N''GD'', N''Grenada''),
    (312, N''GP'', N''Guadeloupe''),
    (316, N''GU'', N''Guam''),
    (320, N''GT'', N''Guatemala''),
    (324, N''GN'', N''Guinea''),
    (328, N''GY'', N''Guyana''),
    (332, N''HT'', N''Haiti''),
    (334, N''HM'', N''Heard Island and McDonald Islands''),
    (336, N''VA'', N''Holy See''),
    (340, N''HN'', N''Honduras''),
    (344, N''HK'', N''Hong Kong''),
    (348, N''HU'', N''Hungary''),
    (352, N''IS'', N''Iceland''),
    (356, N''IN'', N''India''),
    (360, N''ID'', N''Indonesia''),
    (364, N''IR'', N''Iran (Islamic Republic of)''),
    (368, N''IQ'', N''Iraq''),
    (372, N''IE'', N''Ireland''),
    (376, N''IL'', N''Israel''),
    (380, N''IT'', N''Italy''),
    (384, N''CI'', N''Côte d''''Ivoire''),
    (388, N''JM'', N''Jamaica''),
    (392, N''JP'', N''Japan''),
    (398, N''KZ'', N''Kazakhstan''),
    (400, N''JO'', N''Jordan''),
    (404, N''KE'', N''Kenya'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'CountryId', N'CountryCode', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup].[Country]'))
        SET IDENTITY_INSERT [Lookup].[Country] OFF;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240319093911_initial')
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'CountryId', N'CountryCode', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup].[Country]'))
        SET IDENTITY_INSERT [Lookup].[Country] ON;
    EXEC(N'INSERT INTO [Lookup].[Country] ([CountryId], [CountryCode], [Name])
    VALUES (408, N''KP'', N''Korea (Democratic People''''s Republic of)''),
    (410, N''KR'', N''Korea (Republic of)''),
    (414, N''KW'', N''Kuwait''),
    (417, N''KG'', N''Kyrgyzstan''),
    (418, N''LA'', N''Lao People''''s Democratic Republic''),
    (422, N''LB'', N''Lebanon''),
    (426, N''LS'', N''Lesotho''),
    (428, N''LV'', N''Latvia''),
    (430, N''LR'', N''Liberia''),
    (434, N''LY'', N''Libya''),
    (438, N''LI'', N''Liechtenstein''),
    (440, N''LT'', N''Lithuania''),
    (442, N''LU'', N''Luxembourg''),
    (446, N''MO'', N''Macao''),
    (450, N''MG'', N''Madagascar''),
    (454, N''MW'', N''Malawi''),
    (458, N''MY'', N''Malaysia''),
    (462, N''MV'', N''Maldives''),
    (466, N''ML'', N''Mali''),
    (470, N''MT'', N''Malta''),
    (474, N''MQ'', N''Martinique''),
    (478, N''MR'', N''Mauritania''),
    (480, N''MU'', N''Mauritius''),
    (484, N''MX'', N''Mexico''),
    (492, N''MN'', N''Mongolia''),
    (498, N''MC'', N''Monaco''),
    (499, N''ME'', N''Montenegro''),
    (500, N''MS'', N''Montserrat''),
    (504, N''MA'', N''Morocco''),
    (508, N''MZ'', N''Mozambique''),
    (512, N''OM'', N''Oman''),
    (516, N''NA'', N''Namibia''),
    (520, N''NR'', N''Nauru''),
    (524, N''NP'', N''Nepal''),
    (528, N''NL'', N''Netherlands''),
    (531, N''CW'', N''Curaçao''),
    (533, N''AW'', N''Aruba''),
    (534, N''SX'', N''Sint Maarten (Dutch part)''),
    (535, N''BQ'', N''Bonaire, Sint Eustatius and Saba''),
    (540, N''NC'', N''New Caledonia''),
    (548, N''VU'', N''Vanuatu''),
    (554, N''NZ'', N''New Zealand'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'CountryId', N'CountryCode', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup].[Country]'))
        SET IDENTITY_INSERT [Lookup].[Country] OFF;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240319093911_initial')
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'CountryId', N'CountryCode', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup].[Country]'))
        SET IDENTITY_INSERT [Lookup].[Country] ON;
    EXEC(N'INSERT INTO [Lookup].[Country] ([CountryId], [CountryCode], [Name])
    VALUES (558, N''NI'', N''Nicaragua''),
    (562, N''NE'', N''Niger''),
    (566, N''NG'', N''Nigeria''),
    (570, N''NU'', N''Niue''),
    (574, N''NF'', N''Norfolk Island''),
    (578, N''NO'', N''Norway''),
    (580, N''MP'', N''Northern Mariana Islands''),
    (581, N''UM'', N''United States Minor Outlying Islands''),
    (583, N''FM'', N''Micronesia (Federated States of)''),
    (584, N''MH'', N''Marshall Islands''),
    (585, N''PW'', N''Palau''),
    (586, N''PK'', N''Pakistan''),
    (591, N''PA'', N''Panama''),
    (598, N''PG'', N''Papua New Guinea''),
    (600, N''PY'', N''Paraguay''),
    (604, N''PE'', N''Peru''),
    (608, N''PH'', N''Philippines''),
    (612, N''PN'', N''Pitcairn''),
    (616, N''PL'', N''Poland''),
    (620, N''PT'', N''Portugal''),
    (624, N''GW'', N''Guinea-Bissau''),
    (626, N''TL'', N''Timor-Leste''),
    (630, N''PR'', N''Puerto Rico''),
    (634, N''QA'', N''Qatar''),
    (638, N''RE'', N''Réunion''),
    (642, N''RO'', N''Romania''),
    (643, N''RU'', N''Russian Federation''),
    (646, N''RW'', N''Rwanda''),
    (652, N''BL'', N''Saint Barthélemy''),
    (654, N''SH'', N''Saint Helena, Ascension and Tristan da Cunha''),
    (659, N''KN'', N''Saint Kitts and Nevis''),
    (660, N''AI'', N''Anguilla''),
    (662, N''LC'', N''Saint Lucia''),
    (663, N''MF'', N''Saint Martin (French part)''),
    (666, N''PM'', N''Saint Pierre and Miquelon''),
    (670, N''VC'', N''Saint Vincent and the Grenadines''),
    (674, N''SM'', N''San Marino''),
    (678, N''ST'', N''Sao Tome and Principe''),
    (682, N''SA'', N''Saudi Arabia''),
    (686, N''SN'', N''Senegal''),
    (688, N''RS'', N''Serbia''),
    (690, N''SC'', N''Seychelles'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'CountryId', N'CountryCode', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup].[Country]'))
        SET IDENTITY_INSERT [Lookup].[Country] OFF;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240319093911_initial')
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'CountryId', N'CountryCode', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup].[Country]'))
        SET IDENTITY_INSERT [Lookup].[Country] ON;
    EXEC(N'INSERT INTO [Lookup].[Country] ([CountryId], [CountryCode], [Name])
    VALUES (694, N''SL'', N''Sierra Leone''),
    (702, N''SG'', N''Singapore''),
    (703, N''SK'', N''Slovakia''),
    (704, N''VN'', N''Viet Nam''),
    (705, N''SI'', N''Slovenia''),
    (706, N''SO'', N''Somalia''),
    (710, N''ZA'', N''South Africa''),
    (716, N''ZW'', N''Zimbabwe''),
    (724, N''ES'', N''Spain''),
    (728, N''SS'', N''South Sudan''),
    (729, N''SD'', N''Sudan''),
    (732, N''EH'', N''Western Sahara''),
    (740, N''SR'', N''Suriname''),
    (744, N''SJ'', N''Svalbard and Jan Mayen''),
    (748, N''SZ'', N''Swaziland''),
    (752, N''SE'', N''Sweden''),
    (756, N''CH'', N''Switzerland''),
    (760, N''SY'', N''Syrian Arab Republic''),
    (762, N''TJ'', N''Tajikistan''),
    (764, N''TH'', N''Thailand''),
    (768, N''TG'', N''Togo''),
    (772, N''TK'', N''Tokelau''),
    (776, N''TO'', N''Tonga''),
    (780, N''TT'', N''Trinidad and Tobago''),
    (784, N''AE'', N''United Arab Emirates''),
    (788, N''TN'', N''Tunisia''),
    (792, N''TR'', N''Turkey''),
    (795, N''TM'', N''Turkmenistan''),
    (796, N''TC'', N''Turks and Caicos Islands''),
    (798, N''TV'', N''Tuvalu''),
    (800, N''UG'', N''Uganda''),
    (804, N''UA'', N''Ukraine''),
    (807, N''MK'', N''Macedonia (the former Yugoslav Republic of)''),
    (818, N''EG'', N''Egypt''),
    (826, N''GB'', N''United Kingdom of Great Britain and Northern Ireland''),
    (831, N''GG'', N''Guernsey''),
    (832, N''JE'', N''Jersey''),
    (833, N''IM'', N''Isle of Man''),
    (834, N''TZ'', N''Tanzania, United Republic of''),
    (840, N''US'', N''United States of America''),
    (850, N''VI'', N''Virgin Islands (U.S.)''),
    (854, N''BF'', N''Burkina Faso'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'CountryId', N'CountryCode', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup].[Country]'))
        SET IDENTITY_INSERT [Lookup].[Country] OFF;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240319093911_initial')
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'CountryId', N'CountryCode', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup].[Country]'))
        SET IDENTITY_INSERT [Lookup].[Country] ON;
    EXEC(N'INSERT INTO [Lookup].[Country] ([CountryId], [CountryCode], [Name])
    VALUES (858, N''UY'', N''Uruguay''),
    (860, N''UZ'', N''Uzbekistan''),
    (862, N''VE'', N''Venezuela (Bolivarian Republic of)''),
    (876, N''WF'', N''Wallis and Futuna''),
    (882, N''WS'', N''Samoa''),
    (887, N''YE'', N''Yemen''),
    (894, N''ZM'', N''Zambia'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'CountryId', N'CountryCode', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup].[Country]'))
        SET IDENTITY_INSERT [Lookup].[Country] OFF;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240319093911_initial')
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup].[FileUploadStatus]'))
        SET IDENTITY_INSERT [Lookup].[FileUploadStatus] ON;
    EXEC(N'INSERT INTO [Lookup].[FileUploadStatus] ([Id], [Name])
    VALUES (0, N''Undefined''),
    (1, N''Virus check success''),
    (2, N''Upload complete''),
    (3, N''Virus check failed''),
    (4, N''Upload failed''),
    (5, N''Deleted'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup].[FileUploadStatus]'))
        SET IDENTITY_INSERT [Lookup].[FileUploadStatus] OFF;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240319093911_initial')
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup].[FileUploadType]'))
        SET IDENTITY_INSERT [Lookup].[FileUploadType] ON;
    EXEC(N'INSERT INTO [Lookup].[FileUploadType] ([Id], [Name])
    VALUES (0, N''Undefined''),
    (1, N''Business plan''),
    (2, N''Recording system''),
    (3, N''Sampling plan''),
    (4, N''Broadly eviquivalent evidence''),
    (5, N''Flow diagram'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup].[FileUploadType]'))
        SET IDENTITY_INSERT [Lookup].[FileUploadType] OFF;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240319093911_initial')
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup].[OperatorType]'))
        SET IDENTITY_INSERT [Lookup].[OperatorType] ON;
    EXEC(N'INSERT INTO [Lookup].[OperatorType] ([Id], [Name])
    VALUES (0, N''Reprocessor''),
    (1, N''Exporter'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup].[OperatorType]'))
        SET IDENTITY_INSERT [Lookup].[OperatorType] OFF;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240319093911_initial')
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup].[ReprocessorSupportingInformationType]'))
        SET IDENTITY_INSERT [Lookup].[ReprocessorSupportingInformationType] ON;
    EXEC(N'INSERT INTO [Lookup].[ReprocessorSupportingInformationType] ([Id], [Name])
    VALUES (0, N''Undefined''),
    (1, N''Non waste inputs''),
    (2, N''Products produced last year'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup].[ReprocessorSupportingInformationType]'))
        SET IDENTITY_INSERT [Lookup].[ReprocessorSupportingInformationType] OFF;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240319093911_initial')
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup].[TaskName]'))
        SET IDENTITY_INSERT [Lookup].[TaskName] ON;
    EXEC(N'INSERT INTO [Lookup].[TaskName] ([Id], [Name])
    VALUES (0, N''Undefined''),
    (1, N''Waste licence and PRNs''),
    (2, N''Upload a business plan''),
    (3, N''About the material''),
    (4, N''Upload supporting documents'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup].[TaskName]'))
        SET IDENTITY_INSERT [Lookup].[TaskName] OFF;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240319093911_initial')
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup].[TaskStatus]'))
        SET IDENTITY_INSERT [Lookup].[TaskStatus] ON;
    EXEC(N'INSERT INTO [Lookup].[TaskStatus] ([Id], [Name])
    VALUES (0, N''Undefined''),
    (1, N''Not started''),
    (2, N''Started''),
    (3, N''Completed''),
    (4, N''Cannot start yet'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup].[TaskStatus]'))
        SET IDENTITY_INSERT [Lookup].[TaskStatus] OFF;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240319093911_initial')
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup].[WasteCodeType]'))
        SET IDENTITY_INSERT [Lookup].[WasteCodeType] ON;
    EXEC(N'INSERT INTO [Lookup].[WasteCodeType] ([Id], [Name])
    VALUES (0, N''Undefined''),
    (1, N''Material commodity code''),
    (2, N''Waste description code'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup].[WasteCodeType]'))
        SET IDENTITY_INSERT [Lookup].[WasteCodeType] OFF;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240319093911_initial')
BEGIN
    CREATE INDEX [IX_Accreditation_AccreditationStatusId] ON [Accreditation] ([AccreditationStatusId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240319093911_initial')
BEGIN
    CREATE UNIQUE INDEX [IX_Accreditation_ExternalId] ON [Accreditation] ([ExternalId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240319093911_initial')
BEGIN
    CREATE INDEX [IX_Accreditation_OperatorTypeId] ON [Accreditation] ([OperatorTypeId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240319093911_initial')
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_Accreditation_ReferenceNumber] ON [Accreditation] ([ReferenceNumber]) WHERE [ReferenceNumber] IS NOT NULL');
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240319093911_initial')
BEGIN
    CREATE INDEX [IX_Accreditation_SiteId] ON [Accreditation] ([SiteId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240319093911_initial')
BEGIN
    CREATE UNIQUE INDEX [IX_AccreditationMaterial_ExternalId] ON [AccreditationMaterial] ([ExternalId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240319093911_initial')
BEGIN
    CREATE INDEX [IX_AccreditationMaterial_OverseasReprocessingSiteId] ON [AccreditationMaterial] ([OverseasReprocessingSiteId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240319093911_initial')
BEGIN
    CREATE INDEX [IX_AccreditationMaterial_SiteId] ON [AccreditationMaterial] ([SiteId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240319093911_initial')
BEGIN
    CREATE INDEX [IX_AccreditationTaskProgress_AccreditationId] ON [AccreditationTaskProgress] ([AccreditationId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240319093911_initial')
BEGIN
    CREATE INDEX [IX_AccreditationTaskProgress_TaskNameId] ON [AccreditationTaskProgress] ([TaskNameId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240319093911_initial')
BEGIN
    CREATE INDEX [IX_AccreditationTaskProgress_TaskStatusId] ON [AccreditationTaskProgress] ([TaskStatusId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240319093911_initial')
BEGIN
    CREATE INDEX [IX_AccreditationTaskProgressMaterial_AccreditationMaterialId] ON [AccreditationTaskProgressMaterial] ([AccreditationMaterialId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240319093911_initial')
BEGIN
    CREATE INDEX [IX_AccreditationTaskProgressMaterial_AccreditationTaskProgressId] ON [AccreditationTaskProgressMaterial] ([AccreditationTaskProgressId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240319093911_initial')
BEGIN
    CREATE INDEX [IX_ExemptionReference_SiteId] ON [ExemptionReference] ([SiteId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240319093911_initial')
BEGIN
    CREATE INDEX [IX_FileUpload_AccreditationId] ON [FileUpload] ([AccreditationId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240319093911_initial')
BEGIN
    CREATE INDEX [IX_FileUpload_FileUploadTypeId] ON [FileUpload] ([FileUploadTypeId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240319093911_initial')
BEGIN
    CREATE INDEX [IX_FileUpload_Status] ON [FileUpload] ([Status]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240319093911_initial')
BEGIN
    CREATE UNIQUE INDEX [IX_MaterialReprocessorDetails_AccreditationMaterialId] ON [MaterialReprocessorDetails] ([AccreditationMaterialId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240319093911_initial')
BEGIN
    CREATE INDEX [IX_OverseasAddress_CountryId] ON [OverseasAddress] ([CountryId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240319093911_initial')
BEGIN
    CREATE UNIQUE INDEX [IX_OverseasAgent_OverseasAddressId] ON [OverseasAgent] ([OverseasAddressId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240319093911_initial')
BEGIN
    CREATE UNIQUE INDEX [IX_OverseasAgent_OverseasReprocessingSiteId] ON [OverseasAgent] ([OverseasReprocessingSiteId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240319093911_initial')
BEGIN
    CREATE INDEX [IX_OverseasReprocessingSite_AccreditationId] ON [OverseasReprocessingSite] ([AccreditationId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240319093911_initial')
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_OverseasReprocessingSite_OverseasAddressId] ON [OverseasReprocessingSite] ([OverseasAddressId]) WHERE [OverseasAddressId] IS NOT NULL');
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240319093911_initial')
BEGIN
    CREATE INDEX [IX_ReprocessorSupportingInformation_MaterialReprocessorDetailsId] ON [ReprocessorSupportingInformation] ([MaterialReprocessorDetailsId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240319093911_initial')
BEGIN
    CREATE INDEX [IX_ReprocessorSupportingInformation_ReprocessorSupportingInformationTypeId] ON [ReprocessorSupportingInformation] ([ReprocessorSupportingInformationTypeId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240319093911_initial')
BEGIN
    CREATE UNIQUE INDEX [IX_Site_ExternalId] ON [Site] ([ExternalId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240319093911_initial')
BEGIN
    CREATE UNIQUE INDEX [IX_Site_Postcode_OrganisationId] ON [Site] ([Postcode], [OrganisationId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240319093911_initial')
BEGIN
    CREATE INDEX [IX_SiteAuthority_SiteId] ON [SiteAuthority] ([SiteId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240319093911_initial')
BEGIN
    CREATE INDEX [IX_WasteCode_AccreditationMaterialId] ON [WasteCode] ([AccreditationMaterialId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240319093911_initial')
BEGIN
    CREATE INDEX [IX_WasteCode_WasteCodeTypeId] ON [WasteCode] ([WasteCodeTypeId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240319093911_initial')
BEGIN
    CREATE UNIQUE INDEX [IX_WastePermit_AccreditationId] ON [WastePermit] ([AccreditationId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240319093911_initial')
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_WastePermit_OverseasReprocessingSiteId] ON [WastePermit] ([OverseasReprocessingSiteId]) WHERE [OverseasReprocessingSiteId] IS NOT NULL');
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240319093911_initial')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240319093911_initial', N'6.0.28');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240320124025_SaveAndContinue')
BEGIN
    ALTER TABLE [WasteCode] DROP CONSTRAINT [FK_WasteCode_AccreditationMaterial_AccreditationMaterialId];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240320124025_SaveAndContinue')
BEGIN
    ALTER TABLE [WasteCode] DROP CONSTRAINT [FK_WasteCode_WasteCodeType_WasteCodeTypeId];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240320124025_SaveAndContinue')
BEGIN
    ALTER TABLE [WasteCode] DROP CONSTRAINT [PK_WasteCode];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240320124025_SaveAndContinue')
BEGIN
    EXEC sp_rename N'[WasteCode]', N'WasteCodes';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240320124025_SaveAndContinue')
BEGIN
    EXEC sp_rename N'[WasteCodes].[IX_WasteCode_WasteCodeTypeId]', N'IX_WasteCodes_WasteCodeTypeId', N'INDEX';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240320124025_SaveAndContinue')
BEGIN
    EXEC sp_rename N'[WasteCodes].[IX_WasteCode_AccreditationMaterialId]', N'IX_WasteCodes_AccreditationMaterialId', N'INDEX';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240320124025_SaveAndContinue')
BEGIN
    ALTER TABLE [WasteCodes] ADD CONSTRAINT [PK_WasteCodes] PRIMARY KEY ([Id]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240320124025_SaveAndContinue')
BEGIN
    CREATE TABLE [SaveAndContinue] (
        [Id] int NOT NULL IDENTITY,
        [Area] nvarchar(30) NULL,
        [Controller] nvarchar(30) NULL,
        [Action] nvarchar(30) NULL,
        [Parameters] nvarchar(60) NULL,
        [AccreditationId] int NOT NULL,
        CONSTRAINT [PK_SaveAndContinue] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_SaveAndContinue_Accreditation_AccreditationId] FOREIGN KEY ([AccreditationId]) REFERENCES [Accreditation] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240320124025_SaveAndContinue')
BEGIN
    CREATE UNIQUE INDEX [IX_SaveAndContinue_AccreditationId] ON [SaveAndContinue] ([AccreditationId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240320124025_SaveAndContinue')
BEGIN
    ALTER TABLE [WasteCodes] ADD CONSTRAINT [FK_WasteCodes_AccreditationMaterial_AccreditationMaterialId] FOREIGN KEY ([AccreditationMaterialId]) REFERENCES [AccreditationMaterial] ([Id]) ON DELETE CASCADE;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240320124025_SaveAndContinue')
BEGIN
    ALTER TABLE [WasteCodes] ADD CONSTRAINT [FK_WasteCodes_WasteCodeType_WasteCodeTypeId] FOREIGN KEY ([WasteCodeTypeId]) REFERENCES [Lookup].[WasteCodeType] ([Id]) ON DELETE CASCADE;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240320124025_SaveAndContinue')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240320124025_SaveAndContinue', N'6.0.28');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240321140215_waste-source-limit')
BEGIN
    DECLARE @var0 sysname;
    SELECT @var0 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AccreditationMaterial]') AND [c].[name] = N'WasteSource');
    IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [AccreditationMaterial] DROP CONSTRAINT [' + @var0 + '];');
    ALTER TABLE [AccreditationMaterial] ALTER COLUMN [WasteSource] nvarchar(200) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240321140215_waste-source-limit')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240321140215_waste-source-limit', N'6.0.28');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240322110418_Add-Material')
BEGIN
    CREATE TABLE [Material] (
        [Id] int NOT NULL IDENTITY,
        [English] nvarchar(max) NULL,
        [Welsh] nvarchar(max) NULL,
        CONSTRAINT [PK_Material] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240322110418_Add-Material')
BEGIN
    CREATE INDEX [IX_AccreditationMaterial_MaterialId] ON [AccreditationMaterial] ([MaterialId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240322110418_Add-Material')
BEGIN
    ALTER TABLE [AccreditationMaterial] ADD CONSTRAINT [FK_AccreditationMaterial_Material_MaterialId] FOREIGN KEY ([MaterialId]) REFERENCES [Material] ([Id]) ON DELETE CASCADE;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240322110418_Add-Material')
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'English', N'Welsh') AND [object_id] = OBJECT_ID(N'[Material]'))
        SET IDENTITY_INSERT [Material] ON;
    EXEC(N'INSERT INTO [Material] ([English], [Welsh])
    VALUES (N''Paper/Board'', N''[Welsh]Paper/Board'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'English', N'Welsh') AND [object_id] = OBJECT_ID(N'[Material]'))
        SET IDENTITY_INSERT [Material] OFF;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240322110418_Add-Material')
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'English', N'Welsh') AND [object_id] = OBJECT_ID(N'[Material]'))
        SET IDENTITY_INSERT [Material] ON;
    EXEC(N'INSERT INTO [Material] ([English], [Welsh])
    VALUES (N''Paper Composting'', N''[Welsh]Paper Composting'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'English', N'Welsh') AND [object_id] = OBJECT_ID(N'[Material]'))
        SET IDENTITY_INSERT [Material] OFF;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240322110418_Add-Material')
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'English', N'Welsh') AND [object_id] = OBJECT_ID(N'[Material]'))
        SET IDENTITY_INSERT [Material] ON;
    EXEC(N'INSERT INTO [Material] ([English], [Welsh])
    VALUES (N''Glass Remelt'', N''[Welsh]Glass Remelt'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'English', N'Welsh') AND [object_id] = OBJECT_ID(N'[Material]'))
        SET IDENTITY_INSERT [Material] OFF;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240322110418_Add-Material')
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'English', N'Welsh') AND [object_id] = OBJECT_ID(N'[Material]'))
        SET IDENTITY_INSERT [Material] ON;
    EXEC(N'INSERT INTO [Material] ([English], [Welsh])
    VALUES (N''Glass Other'', N''[Welsh]Glass Other'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'English', N'Welsh') AND [object_id] = OBJECT_ID(N'[Material]'))
        SET IDENTITY_INSERT [Material] OFF;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240322110418_Add-Material')
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'English', N'Welsh') AND [object_id] = OBJECT_ID(N'[Material]'))
        SET IDENTITY_INSERT [Material] ON;
    EXEC(N'INSERT INTO [Material] ([English], [Welsh])
    VALUES (N''Aluminium'', N''[Welsh]Aluminium'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'English', N'Welsh') AND [object_id] = OBJECT_ID(N'[Material]'))
        SET IDENTITY_INSERT [Material] OFF;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240322110418_Add-Material')
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'English', N'Welsh') AND [object_id] = OBJECT_ID(N'[Material]'))
        SET IDENTITY_INSERT [Material] ON;
    EXEC(N'INSERT INTO [Material] ([English], [Welsh])
    VALUES (N''Steel'', N''[Welsh]Steel'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'English', N'Welsh') AND [object_id] = OBJECT_ID(N'[Material]'))
        SET IDENTITY_INSERT [Material] OFF;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240322110418_Add-Material')
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'English', N'Welsh') AND [object_id] = OBJECT_ID(N'[Material]'))
        SET IDENTITY_INSERT [Material] ON;
    EXEC(N'INSERT INTO [Material] ([English], [Welsh])
    VALUES (N''Plastic'', N''[Welsh]Plastic'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'English', N'Welsh') AND [object_id] = OBJECT_ID(N'[Material]'))
        SET IDENTITY_INSERT [Material] OFF;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240322110418_Add-Material')
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'English', N'Welsh') AND [object_id] = OBJECT_ID(N'[Material]'))
        SET IDENTITY_INSERT [Material] ON;
    EXEC(N'INSERT INTO [Material] ([English], [Welsh])
    VALUES (N''Wood'', N''[Welsh]Wood'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'English', N'Welsh') AND [object_id] = OBJECT_ID(N'[Material]'))
        SET IDENTITY_INSERT [Material] OFF;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240322110418_Add-Material')
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'English', N'Welsh') AND [object_id] = OBJECT_ID(N'[Material]'))
        SET IDENTITY_INSERT [Material] ON;
    EXEC(N'INSERT INTO [Material] ([English], [Welsh])
    VALUES (N''Wood Composting'', N''[Welsh]Wood Composting'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'English', N'Welsh') AND [object_id] = OBJECT_ID(N'[Material]'))
        SET IDENTITY_INSERT [Material] OFF;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240322110418_Add-Material')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240322110418_Add-Material', N'6.0.28');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240325111241_save-and-come-back-rename')
BEGIN
    DROP TABLE [SaveAndContinue];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240325111241_save-and-come-back-rename')
BEGIN
    CREATE TABLE [SaveAndComeBack] (
        [Id] int NOT NULL IDENTITY,
        [Area] nvarchar(30) NULL,
        [Controller] nvarchar(30) NULL,
        [Action] nvarchar(30) NULL,
        [Parameters] nvarchar(60) NULL,
        [AccreditationId] int NOT NULL,
        CONSTRAINT [PK_SaveAndComeBack] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_SaveAndComeBack_Accreditation_AccreditationId] FOREIGN KEY ([AccreditationId]) REFERENCES [Accreditation] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240325111241_save-and-come-back-rename')
BEGIN
    CREATE UNIQUE INDEX [IX_SaveAndComeBack_AccreditationId] ON [SaveAndComeBack] ([AccreditationId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240325111241_save-and-come-back-rename')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240325111241_save-and-come-back-rename', N'6.0.28');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240325115713_save-and-continue-field-length')
BEGIN
    DECLARE @var1 sysname;
    SELECT @var1 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[SaveAndComeBack]') AND [c].[name] = N'Parameters');
    IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [SaveAndComeBack] DROP CONSTRAINT [' + @var1 + '];');
    ALTER TABLE [SaveAndComeBack] ALTER COLUMN [Parameters] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240325115713_save-and-continue-field-length')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240325115713_save-and-continue-field-length', N'6.0.28');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240326124940_overseas-person-type')
BEGIN
    DROP TABLE [OverseasAgent];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240326124940_overseas-person-type')
BEGIN
    CREATE TABLE [Lookup].[OverseasPersonType] (
        [Id] int NOT NULL,
        [Name] nvarchar(30) NOT NULL,
        CONSTRAINT [PK_OverseasPersonType] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240326124940_overseas-person-type')
BEGIN
    CREATE TABLE [OverseasContactPerson] (
        [Id] int NOT NULL IDENTITY,
        [OverseasAddressId] int NOT NULL,
        [OverseasReprocessingSiteId] int NOT NULL,
        [Fullname] nvarchar(100) NULL,
        [Position] nvarchar(100) NULL,
        [Telephone] nvarchar(30) NULL,
        [Email] nvarchar(50) NULL,
        [OverseasPersonTypeId] int NOT NULL,
        CONSTRAINT [PK_OverseasContactPerson] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_OverseasContactPerson_OverseasAddress_OverseasAddressId] FOREIGN KEY ([OverseasAddressId]) REFERENCES [OverseasAddress] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_OverseasContactPerson_OverseasPersonType_OverseasPersonTypeId] FOREIGN KEY ([OverseasPersonTypeId]) REFERENCES [Lookup].[OverseasPersonType] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_OverseasContactPerson_OverseasReprocessingSite_OverseasReprocessingSiteId] FOREIGN KEY ([OverseasReprocessingSiteId]) REFERENCES [OverseasReprocessingSite] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240326124940_overseas-person-type')
BEGIN
    CREATE UNIQUE INDEX [IX_OverseasContactPerson_OverseasAddressId] ON [OverseasContactPerson] ([OverseasAddressId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240326124940_overseas-person-type')
BEGIN
    CREATE INDEX [IX_OverseasContactPerson_OverseasPersonTypeId] ON [OverseasContactPerson] ([OverseasPersonTypeId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240326124940_overseas-person-type')
BEGIN
    CREATE UNIQUE INDEX [IX_OverseasContactPerson_OverseasReprocessingSiteId] ON [OverseasContactPerson] ([OverseasReprocessingSiteId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240326124940_overseas-person-type')
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup].[OverseasPersonType]'))
        SET IDENTITY_INSERT [Lookup].[OverseasPersonType] ON;
    EXEC(N'INSERT INTO [Lookup].[OverseasPersonType] ([Id], [Name])
    VALUES (0, N''Undefined'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup].[OverseasPersonType]'))
        SET IDENTITY_INSERT [Lookup].[OverseasPersonType] OFF;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240326124940_overseas-person-type')
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup].[OverseasPersonType]'))
        SET IDENTITY_INSERT [Lookup].[OverseasPersonType] ON;
    EXEC(N'INSERT INTO [Lookup].[OverseasPersonType] ([Id], [Name])
    VALUES (1, N''OverseasAgent'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup].[OverseasPersonType]'))
        SET IDENTITY_INSERT [Lookup].[OverseasPersonType] OFF;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240326124940_overseas-person-type')
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup].[OverseasPersonType]'))
        SET IDENTITY_INSERT [Lookup].[OverseasPersonType] ON;
    EXEC(N'INSERT INTO [Lookup].[OverseasPersonType] ([Id], [Name])
    VALUES (2, N''ReprocessorContact'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup].[OverseasPersonType]'))
        SET IDENTITY_INSERT [Lookup].[OverseasPersonType] OFF;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240326124940_overseas-person-type')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240326124940_overseas-person-type', N'6.0.28');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240404154923_Move-WasteLastYear')
BEGIN
    DECLARE @var2 sysname;
    SELECT @var2 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[MaterialReprocessorDetails]') AND [c].[name] = N'WasteLastYear');
    IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [MaterialReprocessorDetails] DROP CONSTRAINT [' + @var2 + '];');
    ALTER TABLE [MaterialReprocessorDetails] DROP COLUMN [WasteLastYear];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240404154923_Move-WasteLastYear')
BEGIN
    ALTER TABLE [AccreditationMaterial] ADD [WasteLastYear] bit NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240404154923_Move-WasteLastYear')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240404154923_Move-WasteLastYear', N'6.0.28');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240418092230_Making-countryId-nullable')
BEGIN
    ALTER TABLE [OverseasAddress] DROP CONSTRAINT [FK_OverseasAddress_Country_CountryId];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240418092230_Making-countryId-nullable')
BEGIN
    DECLARE @var3 sysname;
    SELECT @var3 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[OverseasAddress]') AND [c].[name] = N'CountryId');
    IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [OverseasAddress] DROP CONSTRAINT [' + @var3 + '];');
    ALTER TABLE [OverseasAddress] ALTER COLUMN [CountryId] int NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240418092230_Making-countryId-nullable')
BEGIN
    ALTER TABLE [OverseasAddress] ADD CONSTRAINT [FK_OverseasAddress_Country_CountryId] FOREIGN KEY ([CountryId]) REFERENCES [Lookup].[Country] ([CountryId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240418092230_Making-countryId-nullable')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240418092230_Making-countryId-nullable', N'6.0.28');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240424114109_Add-Large-Fee-To-Accreditation')
BEGIN
    ALTER TABLE [Accreditation] ADD [LargeFee] decimal(10,3) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240424114109_Add-Large-Fee-To-Accreditation')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240424114109_Add-Large-Fee-To-Accreditation', N'6.0.28');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240425091719_Added-HasNpwdAccreditationNumber-column')
BEGIN
    ALTER TABLE [Accreditation] ADD [HasNpwdAccreditationNumber] bit NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240425091719_Added-HasNpwdAccreditationNumber-column')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240425091719_Added-HasNpwdAccreditationNumber-column', N'6.0.28');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240425112326_relocated-HasNpwdAccreditationNumber-column')
BEGIN
    DECLARE @var4 sysname;
    SELECT @var4 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Accreditation]') AND [c].[name] = N'HasNpwdAccreditationNumber');
    IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [Accreditation] DROP CONSTRAINT [' + @var4 + '];');
    ALTER TABLE [Accreditation] DROP COLUMN [HasNpwdAccreditationNumber];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240425112326_relocated-HasNpwdAccreditationNumber-column')
BEGIN
    ALTER TABLE [AccreditationMaterial] ADD [HasNpwdAccreditationNumber] bit NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240425112326_relocated-HasNpwdAccreditationNumber-column')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240425112326_relocated-HasNpwdAccreditationNumber-column', N'6.0.28');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240425123535_AccreditationMaterial-default-columns-to-null')
BEGIN
    DECLARE @var5 sysname;
    SELECT @var5 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AccreditationMaterial]') AND [c].[name] = N'WeeklyCapacity');
    IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [AccreditationMaterial] DROP CONSTRAINT [' + @var5 + '];');
    ALTER TABLE [AccreditationMaterial] ALTER COLUMN [WeeklyCapacity] decimal(10,3) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240425123535_AccreditationMaterial-default-columns-to-null')
BEGIN
    DECLARE @var6 sysname;
    SELECT @var6 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AccreditationMaterial]') AND [c].[name] = N'AnnualCapacity');
    IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [AccreditationMaterial] DROP CONSTRAINT [' + @var6 + '];');
    ALTER TABLE [AccreditationMaterial] ALTER COLUMN [AnnualCapacity] decimal(10,3) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240425123535_AccreditationMaterial-default-columns-to-null')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240425123535_AccreditationMaterial-default-columns-to-null', N'6.0.28');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240425123658_AccreditationMaterial-add-AccreditationId')
BEGIN
    DELETE FROM [Accreditation].[dbo].[AccreditationMaterial]
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240425123658_AccreditationMaterial-add-AccreditationId')
BEGIN
    ALTER TABLE [AccreditationMaterial] ADD [AccreditationId] int NOT NULL DEFAULT 0;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240425123658_AccreditationMaterial-add-AccreditationId')
BEGIN
    CREATE INDEX [IX_AccreditationMaterial_AccreditationId] ON [AccreditationMaterial] ([AccreditationId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240425123658_AccreditationMaterial-add-AccreditationId')
BEGIN
    ALTER TABLE [AccreditationMaterial] DROP CONSTRAINT [FK_AccreditationMaterial_OverseasReprocessingSite_OverseasReprocessingSiteId];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240425123658_AccreditationMaterial-add-AccreditationId')
BEGIN
    ALTER TABLE [AccreditationMaterial] DROP CONSTRAINT [FK_AccreditationMaterial_Site_SiteId];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240425123658_AccreditationMaterial-add-AccreditationId')
BEGIN
    ALTER TABLE [AccreditationMaterial] ADD CONSTRAINT [FK_AccreditationMaterial_Accreditation_AccreditationId] FOREIGN KEY ([AccreditationId]) REFERENCES [Accreditation] ([Id]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240425123658_AccreditationMaterial-add-AccreditationId')
BEGIN
    ALTER TABLE [AccreditationMaterial] ADD CONSTRAINT [FK_AccreditationMaterial_OverseasReprocessingSite_OverseasReprocessingSiteId] FOREIGN KEY ([OverseasReprocessingSiteId]) REFERENCES [OverseasReprocessingSite] ([Id]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240425123658_AccreditationMaterial-add-AccreditationId')
BEGIN
    ALTER TABLE [AccreditationMaterial] ADD CONSTRAINT [FK_AccreditationMaterial_Site_SiteId] FOREIGN KEY ([SiteId]) REFERENCES [Site] ([Id]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240425123658_AccreditationMaterial-add-AccreditationId')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240425123658_AccreditationMaterial-add-AccreditationId', N'6.0.28');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240429103222_Added-NpwdAccreditationNumber-column')
BEGIN
    ALTER TABLE [AccreditationMaterial] ADD [NpwdAccreditationNumber] nvarchar(12) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240429103222_Added-NpwdAccreditationNumber-column')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240429103222_Added-NpwdAccreditationNumber-column', N'6.0.28');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240429130212_Create-address-table')
BEGIN
    UPDATE [Accreditation].[dbo].[Accreditation] Set SiteId = null
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240429130212_Create-address-table')
BEGIN
    DELETE FROM [Accreditation].[dbo].[AccreditationMaterial]
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240429130212_Create-address-table')
BEGIN
    DELETE FROM [Accreditation].[dbo].[Site]
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240429130212_Create-address-table')
BEGIN
    DROP INDEX [IX_Site_ExternalId] ON [Site];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240429130212_Create-address-table')
BEGIN
    DROP INDEX [IX_Site_Postcode_OrganisationId] ON [Site];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240429130212_Create-address-table')
BEGIN
    DECLARE @var7 sysname;
    SELECT @var7 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Site]') AND [c].[name] = N'Address1');
    IF @var7 IS NOT NULL EXEC(N'ALTER TABLE [Site] DROP CONSTRAINT [' + @var7 + '];');
    ALTER TABLE [Site] DROP COLUMN [Address1];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240429130212_Create-address-table')
BEGIN
    DECLARE @var8 sysname;
    SELECT @var8 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Site]') AND [c].[name] = N'Address2');
    IF @var8 IS NOT NULL EXEC(N'ALTER TABLE [Site] DROP CONSTRAINT [' + @var8 + '];');
    ALTER TABLE [Site] DROP COLUMN [Address2];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240429130212_Create-address-table')
BEGIN
    DECLARE @var9 sysname;
    SELECT @var9 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Site]') AND [c].[name] = N'County');
    IF @var9 IS NOT NULL EXEC(N'ALTER TABLE [Site] DROP CONSTRAINT [' + @var9 + '];');
    ALTER TABLE [Site] DROP COLUMN [County];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240429130212_Create-address-table')
BEGIN
    DECLARE @var10 sysname;
    SELECT @var10 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Site]') AND [c].[name] = N'OrganisationId');
    IF @var10 IS NOT NULL EXEC(N'ALTER TABLE [Site] DROP CONSTRAINT [' + @var10 + '];');
    ALTER TABLE [Site] DROP COLUMN [OrganisationId];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240429130212_Create-address-table')
BEGIN
    DECLARE @var11 sysname;
    SELECT @var11 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Site]') AND [c].[name] = N'Postcode');
    IF @var11 IS NOT NULL EXEC(N'ALTER TABLE [Site] DROP CONSTRAINT [' + @var11 + '];');
    ALTER TABLE [Site] DROP COLUMN [Postcode];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240429130212_Create-address-table')
BEGIN
    DECLARE @var12 sysname;
    SELECT @var12 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Site]') AND [c].[name] = N'Town');
    IF @var12 IS NOT NULL EXEC(N'ALTER TABLE [Site] DROP CONSTRAINT [' + @var12 + '];');
    ALTER TABLE [Site] DROP COLUMN [Town];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240429130212_Create-address-table')
BEGIN
    ALTER TABLE [Site] ADD [AddressId] int NOT NULL DEFAULT 0;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240429130212_Create-address-table')
BEGIN
    ALTER TABLE [Accreditation] ADD [LegalAddressId] int NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240429130212_Create-address-table')
BEGIN
    CREATE TABLE [Address] (
        [Id] int NOT NULL IDENTITY,
        [Address1] nvarchar(max) NOT NULL,
        [Address2] nvarchar(max) NULL,
        [Town] nvarchar(max) NOT NULL,
        [County] nvarchar(max) NULL,
        [Postcode] nvarchar(8) NOT NULL,
        CONSTRAINT [PK_Address] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240429130212_Create-address-table')
BEGIN
    CREATE UNIQUE INDEX [IX_Site_AddressId] ON [Site] ([AddressId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240429130212_Create-address-table')
BEGIN
    ALTER TABLE [Site] ADD CONSTRAINT [FK_Site_Address_AddressId] FOREIGN KEY ([AddressId]) REFERENCES [Address] ([Id]) ON DELETE CASCADE;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240429130212_Create-address-table')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240429130212_Create-address-table', N'6.0.28');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240502094220_null-tonnes-field')
BEGIN
    ALTER TABLE [Address] ADD [AccreditationId] int NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240502094220_null-tonnes-field')
BEGIN
    ALTER TABLE [Address] ADD [OrganisationId] uniqueidentifier NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240502094220_null-tonnes-field')
BEGIN
    CREATE INDEX [IX_Address_AccreditationId] ON [Address] ([AccreditationId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240502094220_null-tonnes-field')
BEGIN
    ALTER TABLE [Address] ADD CONSTRAINT [FK_Address_Accreditation_AccreditationId] FOREIGN KEY ([AccreditationId]) REFERENCES [Accreditation] ([Id]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240502094220_null-tonnes-field')
BEGIN
    ALTER TABLE [Address] DROP CONSTRAINT [FK_Address_Accreditation_AccreditationId];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240502094220_null-tonnes-field')
BEGIN
    DROP INDEX [IX_Address_AccreditationId] ON [Address];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240502094220_null-tonnes-field')
BEGIN
    DECLARE @var13 sysname;
    SELECT @var13 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Address]') AND [c].[name] = N'AccreditationId');
    IF @var13 IS NOT NULL EXEC(N'ALTER TABLE [Address] DROP CONSTRAINT [' + @var13 + '];');
    ALTER TABLE [Address] DROP COLUMN [AccreditationId];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240502094220_null-tonnes-field')
BEGIN
    DECLARE @var14 sysname;
    SELECT @var14 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Address]') AND [c].[name] = N'OrganisationId');
    IF @var14 IS NOT NULL EXEC(N'ALTER TABLE [Address] DROP CONSTRAINT [' + @var14 + '];');
    ALTER TABLE [Address] DROP COLUMN [OrganisationId];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240502094220_null-tonnes-field')
BEGIN
    DECLARE @var15 sysname;
    SELECT @var15 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[ReprocessorSupportingInformation]') AND [c].[name] = N'Tonnes');
    IF @var15 IS NOT NULL EXEC(N'ALTER TABLE [ReprocessorSupportingInformation] DROP CONSTRAINT [' + @var15 + '];');
    ALTER TABLE [ReprocessorSupportingInformation] ALTER COLUMN [Tonnes] decimal(10,3) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240502094220_null-tonnes-field')
BEGIN
    CREATE UNIQUE INDEX [IX_Site_ExternalId] ON [Site] ([ExternalId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240502094220_null-tonnes-field')
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_Accreditation_LegalAddressId] ON [Accreditation] ([LegalAddressId]) WHERE [LegalAddressId] IS NOT NULL');
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240502094220_null-tonnes-field')
BEGIN
    ALTER TABLE [Accreditation] ADD CONSTRAINT [FK_Accreditation_Address_LegalAddressId] FOREIGN KEY ([LegalAddressId]) REFERENCES [Address] ([Id]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240502094220_null-tonnes-field')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240502094220_null-tonnes-field', N'6.0.28');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240503102508_Modify-Material-Names')
BEGIN
    update [Accreditation].[dbo].[Material] set English = 'paper/board', Welsh = '[Welsh]paper/board' where english = 'Paper/Board'update [Accreditation].[dbo].[Material] set English = 'paper composting', Welsh = '[Welsh]paper composting' where english = 'Paper Composting'update [Accreditation].[dbo].[Material] set English = 'glass remelt', Welsh = '[Welsh]glass remelt' where english = 'Glass Remelt'update [Accreditation].[dbo].[Material] set English = 'glass other', Welsh = '[Welsh]glass other' where english = 'Glass Other'update [Accreditation].[dbo].[Material] set English = 'aluminium', Welsh = '[Welsh]aluminium' where english = 'Aluminium'update [Accreditation].[dbo].[Material] set English = 'steel', Welsh = '[Welsh]steel' where english = 'Steel'update [Accreditation].[dbo].[Material] set English = 'plastic', Welsh = '[Welsh]plastic' where english = 'Plastic'update [Accreditation].[dbo].[Material] set English = 'wood', Welsh = '[Welsh]wood' where english = 'Wood'update [Accreditation].[dbo].[Material] set English = 'wood composting', Welsh = '[Welsh]wood composting' where english = 'Wood Composting'
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240503102508_Modify-Material-Names')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240503102508_Modify-Material-Names', N'6.0.28');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240503124041_AccreditionFee')
BEGIN
    ALTER TABLE [Accreditation] ADD [AccreditationFee] decimal(18,2) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240503124041_AccreditionFee')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240503124041_AccreditionFee', N'6.0.28');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240507124737_Address-remove-required-fields')
BEGIN
    DECLARE @var16 sysname;
    SELECT @var16 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Address]') AND [c].[name] = N'Town');
    IF @var16 IS NOT NULL EXEC(N'ALTER TABLE [Address] DROP CONSTRAINT [' + @var16 + '];');
    ALTER TABLE [Address] ALTER COLUMN [Town] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240507124737_Address-remove-required-fields')
BEGIN
    DECLARE @var17 sysname;
    SELECT @var17 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Address]') AND [c].[name] = N'Postcode');
    IF @var17 IS NOT NULL EXEC(N'ALTER TABLE [Address] DROP CONSTRAINT [' + @var17 + '];');
    ALTER TABLE [Address] ALTER COLUMN [Postcode] nvarchar(8) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240507124737_Address-remove-required-fields')
BEGIN
    DECLARE @var18 sysname;
    SELECT @var18 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Address]') AND [c].[name] = N'Address1');
    IF @var18 IS NOT NULL EXEC(N'ALTER TABLE [Address] DROP CONSTRAINT [' + @var18 + '];');
    ALTER TABLE [Address] ALTER COLUMN [Address1] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240507124737_Address-remove-required-fields')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240507124737_Address-remove-required-fields', N'6.0.28');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240509104644_Add-externalid-to-material')
BEGIN
    ALTER TABLE [Material] ADD [ExternalId] uniqueidentifier NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240509104644_Add-externalid-to-material')
BEGIN
    UPDATE Material SET ExternalId = NEWID() WHERE ExternalId is NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240509104644_Add-externalid-to-material')
BEGIN
    DECLARE @var19 sysname;
    SELECT @var19 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Material]') AND [c].[name] = N'ExternalId');
    IF @var19 IS NOT NULL EXEC(N'ALTER TABLE [Material] DROP CONSTRAINT [' + @var19 + '];');
    ALTER TABLE [Material] ALTER COLUMN [ExternalId] uniqueidentifier NOT NULL;
    ALTER TABLE [Material] ADD DEFAULT '6446591e-8213-452d-a422-a38f7c6c9336' FOR [ExternalId];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240509104644_Add-externalid-to-material')
BEGIN
    DECLARE @var20 sysname;
    SELECT @var20 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Accreditation]') AND [c].[name] = N'AccreditationFee');
    IF @var20 IS NOT NULL EXEC(N'ALTER TABLE [Accreditation] DROP CONSTRAINT [' + @var20 + '];');
    ALTER TABLE [Accreditation] ALTER COLUMN [AccreditationFee] decimal(19,2) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240509104644_Add-externalid-to-material')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240509104644_Add-externalid-to-material', N'6.0.28');
END;
GO

COMMIT;
GO

