﻿IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
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

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240717145045_InitialCreate'
)
BEGIN
    CREATE TABLE [Prn] (
        [Id] int NOT NULL IDENTITY,
        [ExternalId] uniqueidentifier NOT NULL,
        [PrnNumber] nvarchar(20) NOT NULL,
        [OrganisationId] uniqueidentifier NOT NULL,
        [OrganisationName] nvarchar(50) NOT NULL,
        [ProducerAgency] nvarchar(50) NOT NULL,
        [ReprocessorExporterAgency] nvarchar(50) NOT NULL,
        [PrnStatusId] int NOT NULL,
        [TonnageValue] int NOT NULL,
        [MaterialName] nvarchar(20) NOT NULL,
        [IssuerNotes] nvarchar(200) NULL,
        [IssuerReference] nvarchar(200) NOT NULL,
        [PrnSignatory] nvarchar(50) NULL,
        [PrnSignatoryPosition] nvarchar(50) NULL,
        [Signature] nvarchar(100) NULL,
        [IssueDate] datetime2 NOT NULL,
        [ProcessToBeUsed] nvarchar(20) NULL,
        [DecemberWaste] bit NOT NULL,
        [CancelledDate] datetime2 NULL,
        [IssuedByOrg] nvarchar(50) NOT NULL,
        [AccreditationNumber] nvarchar(20) NOT NULL,
        [ReprocessingSite] nvarchar(100) NULL,
        [AccreditationYear] nvarchar(10) NOT NULL,
        [ObligationYear] nvarchar(10) NOT NULL,
        [PackagingProducer] nvarchar(100) NOT NULL,
        [CreatedBy] nvarchar(20) NULL,
        [CreatedOn] datetime2 NOT NULL,
        [LastUpdatedBy] uniqueidentifier NOT NULL,
        [LastUpdatedDate] datetime2 NOT NULL,
        [IsExport] bit NOT NULL,
        CONSTRAINT [PK_Prn] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240717145045_InitialCreate'
)
BEGIN
    CREATE TABLE [PrnStatus] (
        [Id] int NOT NULL IDENTITY,
        [StatusName] nvarchar(20) NOT NULL,
        [StatusDescription] nvarchar(50) NULL,
        CONSTRAINT [PK_PrnStatus] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240717145045_InitialCreate'
)
BEGIN
    CREATE TABLE [PrnStatusHistory] (
        [Id] int NOT NULL IDENTITY,
        [CreatedOn] datetime2 NOT NULL,
        [CreatedByUser] uniqueidentifier NOT NULL,
        [CreatedByOrganisationId] uniqueidentifier NOT NULL,
        [PrnStatusIdFk] int NOT NULL,
        [PrnIdFk] int NOT NULL,
        [Comment] nvarchar(1000) NULL,
        CONSTRAINT [PK_PrnStatusHistory] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240717145045_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Prn_ExternalId] ON [Prn] ([ExternalId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240717145045_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Prn_PrnNumber] ON [Prn] ([PrnNumber]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240717145045_InitialCreate'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240717145045_InitialCreate', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240728110950_AddedEnumForStatus'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'StatusDescription', N'StatusName') AND [object_id] = OBJECT_ID(N'[PrnStatus]'))
        SET IDENTITY_INSERT [PrnStatus] ON;
    EXEC(N'INSERT INTO [PrnStatus] ([Id], [StatusDescription], [StatusName])
    VALUES (1, N''Prn Accepted'', N''ACCEPTED''),
    (2, N''Prn Rejected'', N''REJECTED''),
    (3, N''Prn Cancelled'', N''CANCELED''),
    (4, N''Prn Awaiting Acceptance'', N''AWAITINGACCEPTANCE'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'StatusDescription', N'StatusName') AND [object_id] = OBJECT_ID(N'[PrnStatus]'))
        SET IDENTITY_INSERT [PrnStatus] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240728110950_AddedEnumForStatus'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240728110950_AddedEnumForStatus', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240730132550_InitialRecyclingTargets'
)
BEGIN
    CREATE TABLE [RecyclingTargets] (
        [Year] int NOT NULL IDENTITY,
        [PaperTarget] decimal(5,2) NOT NULL,
        [GlassTarget] decimal(5,2) NOT NULL,
        [AluminiumTarget] decimal(5,2) NOT NULL,
        [SteelTarget] decimal(5,2) NOT NULL,
        [PlasticTarget] decimal(5,2) NOT NULL,
        [WoodTarget] decimal(5,2) NOT NULL,
        [GlassRemeltTarget] decimal(5,2) NOT NULL,
        CONSTRAINT [PK_RecyclingTargets] PRIMARY KEY ([Year])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240730132550_InitialRecyclingTargets'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Year', N'AluminiumTarget', N'GlassRemeltTarget', N'GlassTarget', N'PaperTarget', N'PlasticTarget', N'SteelTarget', N'WoodTarget') AND [object_id] = OBJECT_ID(N'[RecyclingTargets]'))
        SET IDENTITY_INSERT [RecyclingTargets] ON;
    EXEC(N'INSERT INTO [RecyclingTargets] ([Year], [AluminiumTarget], [GlassRemeltTarget], [GlassTarget], [PaperTarget], [PlasticTarget], [SteelTarget], [WoodTarget])
    VALUES (2025, 0.61, 0.75, 0.74, 0.75, 0.55, 0.8, 0.45),
    (2026, 0.62, 0.76, 0.76, 0.77, 0.57, 0.81, 0.46),
    (2027, 0.63, 0.77, 0.78, 0.79, 0.59, 0.82, 0.47),
    (2028, 0.64, 0.78, 0.8, 0.81, 0.61, 0.83, 0.48),
    (2029, 0.65, 0.79, 0.82, 0.83, 0.63, 0.84, 0.49),
    (2030, 0.67, 0.8, 0.85, 0.85, 0.65, 0.85, 0.5)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Year', N'AluminiumTarget', N'GlassRemeltTarget', N'GlassTarget', N'PaperTarget', N'PlasticTarget', N'SteelTarget', N'WoodTarget') AND [object_id] = OBJECT_ID(N'[RecyclingTargets]'))
        SET IDENTITY_INSERT [RecyclingTargets] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240730132550_InitialRecyclingTargets'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240730132550_InitialRecyclingTargets', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240813152307_InitialObligationCalculation'
)
BEGIN
    CREATE TABLE [ObligationCalculations] (
        [Id] int NOT NULL IDENTITY,
        [OrganisationId] int NOT NULL,
        [MaterialName] nvarchar(20) NOT NULL,
        [MaterialObligationValue] int NOT NULL,
        [Year] int NOT NULL,
        [CalculatedOn] datetime2 NOT NULL,
        CONSTRAINT [PK_ObligationCalculations] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240813152307_InitialObligationCalculation'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240813152307_InitialObligationCalculation', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240924172133_UpdateEprnStatusCancelledCorrection'
)
BEGIN
    EXEC(N'UPDATE [PrnStatus] SET [StatusName] = N''CANCELLED''
    WHERE [Id] = 3;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240924172133_UpdateEprnStatusCancelledCorrection'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240924172133_UpdateEprnStatusCancelledCorrection', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241010152706_AddMaterialTable'
)
BEGIN
    CREATE TABLE [Material] (
        [MaterialName] nvarchar(20) NOT NULL,
        [MaterialCode] nvarchar(3) NOT NULL,
        CONSTRAINT [PK_Material] PRIMARY KEY ([MaterialName])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241010152706_AddMaterialTable'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'MaterialName', N'MaterialCode') AND [object_id] = OBJECT_ID(N'[Material]'))
        SET IDENTITY_INSERT [Material] ON;
    EXEC(N'INSERT INTO [Material] ([MaterialName], [MaterialCode])
    VALUES (N''Aluminium'', N''AL''),
    (N''Glass'', N''GL''),
    (N''Paper'', N''PC''),
    (N''Plastic'', N''PL''),
    (N''Steel'', N''ST''),
    (N''Wood'', N''WD'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'MaterialName', N'MaterialCode') AND [object_id] = OBJECT_ID(N'[Material]'))
        SET IDENTITY_INSERT [Material] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241010152706_AddMaterialTable'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20241010152706_AddMaterialTable', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241010153947_DropObligationCalculation'
)
BEGIN
    DROP TABLE [ObligationCalculations];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241010153947_DropObligationCalculation'
)
BEGIN
    CREATE TABLE [ObligationCalculations] (
        [Id] int NOT NULL IDENTITY,
        [OrganisationId] uniqueidentifier NOT NULL,
        [MaterialName] nvarchar(20) NOT NULL,
        [MaterialObligationValue] int NOT NULL,
        [Year] int NOT NULL,
        [CalculatedOn] datetime2 NOT NULL,
        [MaterialWeight] float NOT NULL DEFAULT 0.0E0,
        CONSTRAINT [PK_ObligationCalculations] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241010153947_DropObligationCalculation'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20241010153947_DropObligationCalculation', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241018113908_UpdatedCancelledDateToStatusUpdatedOn'
)
BEGIN
    EXEC sp_rename N'[Prn].[CancelledDate]', N'StatusUpdatedOn', N'COLUMN';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241018113908_UpdatedCancelledDateToStatusUpdatedOn'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20241018113908_UpdatedCancelledDateToStatusUpdatedOn', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241120151024_RenameMaterialNameToTonnage'
)
BEGIN
    EXEC sp_rename N'[ObligationCalculations].[MaterialWeight]', N'Tonnage', N'COLUMN';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241120151024_RenameMaterialNameToTonnage'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20241120151024_RenameMaterialNameToTonnage', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241202142542_AddPEprNpwdSyncTable'
)
BEGIN
    CREATE TABLE [PEprNpwdSync] (
        [Id] int NOT NULL IDENTITY,
        [PRNId] int NOT NULL,
        [PRNStatusId] int NOT NULL,
        [CreatedOn] datetime2 NOT NULL,
        CONSTRAINT [PK_PEprNpwdSync] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241202142542_AddPEprNpwdSyncTable'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20241202142542_AddPEprNpwdSyncTable', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241219235202_RecreateRecyclingTargetTable'
)
BEGIN
    DROP TABLE [RecyclingTargets];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241219235202_RecreateRecyclingTargetTable'
)
BEGIN
    CREATE TABLE [RecyclingTargets] (
        [Id] int NOT NULL IDENTITY,
        [Year] int NOT NULL,
        [MaterialNameRT] nvarchar(20) NOT NULL,
        [Target] decimal(5,2) NOT NULL,
        CONSTRAINT [PK_RecyclingTargets] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241219235202_RecreateRecyclingTargetTable'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'MaterialNameRT', N'Target', N'Year') AND [object_id] = OBJECT_ID(N'[RecyclingTargets]'))
        SET IDENTITY_INSERT [RecyclingTargets] ON;
    EXEC(N'INSERT INTO [RecyclingTargets] ([Id], [MaterialNameRT], [Target], [Year])
    VALUES (1, N''Paper'', 0.75, 2025),
    (2, N''Paper'', 0.77, 2026),
    (3, N''Paper'', 0.79, 2027),
    (4, N''Paper'', 0.81, 2028),
    (5, N''Paper'', 0.83, 2029),
    (6, N''Paper'', 0.85, 2030),
    (7, N''Glass'', 0.74, 2025),
    (8, N''Glass'', 0.76, 2026),
    (9, N''Glass'', 0.78, 2027),
    (10, N''Glass'', 0.8, 2028),
    (11, N''Glass'', 0.82, 2029),
    (12, N''Glass'', 0.85, 2030),
    (13, N''Aluminium'', 0.61, 2025),
    (14, N''Aluminium'', 0.62, 2026),
    (15, N''Aluminium'', 0.63, 2027),
    (16, N''Aluminium'', 0.64, 2028),
    (17, N''Aluminium'', 0.65, 2029),
    (18, N''Aluminium'', 0.67, 2030),
    (19, N''Steel'', 0.8, 2025),
    (20, N''Steel'', 0.81, 2026),
    (21, N''Steel'', 0.82, 2027),
    (22, N''Steel'', 0.83, 2028),
    (23, N''Steel'', 0.84, 2029),
    (24, N''Steel'', 0.85, 2030),
    (25, N''Plastic'', 0.55, 2025),
    (26, N''Plastic'', 0.57, 2026),
    (27, N''Plastic'', 0.59, 2027),
    (28, N''Plastic'', 0.61, 2028),
    (29, N''Plastic'', 0.63, 2029),
    (30, N''Plastic'', 0.65, 2030),
    (31, N''Wood'', 0.45, 2025),
    (32, N''Wood'', 0.46, 2026),
    (33, N''Wood'', 0.47, 2027),
    (34, N''Wood'', 0.48, 2028),
    (35, N''Wood'', 0.49, 2029),
    (36, N''Wood'', 0.5, 2030),
    (37, N''GlassRemelt'', 0.75, 2025),
    (38, N''GlassRemelt'', 0.76, 2026),
    (39, N''GlassRemelt'', 0.77, 2027),
    (40, N''GlassRemelt'', 0.78, 2028),
    (41, N''GlassRemelt'', 0.79, 2029),
    (42, N''GlassRemelt'', 0.8, 2030)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'MaterialNameRT', N'Target', N'Year') AND [object_id] = OBJECT_ID(N'[RecyclingTargets]'))
        SET IDENTITY_INSERT [RecyclingTargets] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241219235202_RecreateRecyclingTargetTable'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20241219235202_RecreateRecyclingTargetTable', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250121153034_PrnStatusHistoryForeignKeyToPrn'
)
BEGIN
    CREATE INDEX [IX_PrnStatusHistory_PrnIdFk] ON [PrnStatusHistory] ([PrnIdFk]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250121153034_PrnStatusHistoryForeignKeyToPrn'
)
BEGIN
    ALTER TABLE [PrnStatusHistory] ADD CONSTRAINT [FK_PrnStatusHistory_Prn_PrnIdFk] FOREIGN KEY ([PrnIdFk]) REFERENCES [Prn] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250121153034_PrnStatusHistoryForeignKeyToPrn'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250121153034_PrnStatusHistoryForeignKeyToPrn', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250210141835_AddFiberCompositetothelistofmaterialsandrecyclingtargets'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'MaterialNameRT', N'Target', N'Year') AND [object_id] = OBJECT_ID(N'[RecyclingTargets]'))
        SET IDENTITY_INSERT [RecyclingTargets] ON;
    EXEC(N'INSERT INTO [RecyclingTargets] ([Id], [MaterialNameRT], [Target], [Year])
    VALUES (43, N''FibreComposite'', 0.75, 2025),
    (44, N''FibreComposite'', 0.77, 2026),
    (45, N''FibreComposite'', 0.79, 2027),
    (46, N''FibreComposite'', 0.81, 2028),
    (47, N''FibreComposite'', 0.83, 2029),
    (48, N''FibreComposite'', 0.85, 2030)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'MaterialNameRT', N'Target', N'Year') AND [object_id] = OBJECT_ID(N'[RecyclingTargets]'))
        SET IDENTITY_INSERT [RecyclingTargets] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250210141835_AddFiberCompositetothelistofmaterialsandrecyclingtargets'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250210141835_AddFiberCompositetothelistofmaterialsandrecyclingtargets', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250226182508_AlterObligationCalculationsTableTonnageColumnFromFloatToInteger'
)
BEGIN
    DECLARE @var0 sysname;
    SELECT @var0 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[ObligationCalculations]') AND [c].[name] = N'Tonnage');
    IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [ObligationCalculations] DROP CONSTRAINT [' + @var0 + '];');
    ALTER TABLE [ObligationCalculations] ALTER COLUMN [Tonnage] int NOT NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250226182508_AlterObligationCalculationsTableTonnageColumnFromFloatToInteger'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250226182508_AlterObligationCalculationsTableTonnageColumnFromFloatToInteger', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250228120631_AddFibreCompisiteMaterial'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'MaterialName', N'MaterialCode') AND [object_id] = OBJECT_ID(N'[Material]'))
        SET IDENTITY_INSERT [Material] ON;
    EXEC(N'INSERT INTO [Material] ([MaterialName], [MaterialCode])
    VALUES (N''FibreComposite'', N''FC'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'MaterialName', N'MaterialCode') AND [object_id] = OBJECT_ID(N'[Material]'))
        SET IDENTITY_INSERT [Material] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250228120631_AddFibreCompisiteMaterial'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250228120631_AddFibreCompisiteMaterial', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250310094044_AmendedMaterialAndAddedMaterialMappingAndUpdatedObligationCalculation'
)
BEGIN
    ALTER TABLE [Material] DROP CONSTRAINT [PK_Material];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250310094044_AmendedMaterialAndAddedMaterialMappingAndUpdatedObligationCalculation'
)
BEGIN
    EXEC(N'DELETE FROM [Material]
    WHERE [MaterialName] = N''Aluminium'';
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250310094044_AmendedMaterialAndAddedMaterialMappingAndUpdatedObligationCalculation'
)
BEGIN
    EXEC(N'DELETE FROM [Material]
    WHERE [MaterialName] = N''FibreComposite'';
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250310094044_AmendedMaterialAndAddedMaterialMappingAndUpdatedObligationCalculation'
)
BEGIN
    EXEC(N'DELETE FROM [Material]
    WHERE [MaterialName] = N''Glass'';
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250310094044_AmendedMaterialAndAddedMaterialMappingAndUpdatedObligationCalculation'
)
BEGIN
    EXEC(N'DELETE FROM [Material]
    WHERE [MaterialName] = N''Paper'';
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250310094044_AmendedMaterialAndAddedMaterialMappingAndUpdatedObligationCalculation'
)
BEGIN
    EXEC(N'DELETE FROM [Material]
    WHERE [MaterialName] = N''Plastic'';
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250310094044_AmendedMaterialAndAddedMaterialMappingAndUpdatedObligationCalculation'
)
BEGIN
    EXEC(N'DELETE FROM [Material]
    WHERE [MaterialName] = N''Steel'';
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250310094044_AmendedMaterialAndAddedMaterialMappingAndUpdatedObligationCalculation'
)
BEGIN
    EXEC(N'DELETE FROM [Material]
    WHERE [MaterialName] = N''Wood'';
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250310094044_AmendedMaterialAndAddedMaterialMappingAndUpdatedObligationCalculation'
)
BEGIN
    ALTER TABLE [Material] ADD [Id] int NOT NULL IDENTITY;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250310094044_AmendedMaterialAndAddedMaterialMappingAndUpdatedObligationCalculation'
)
BEGIN
    ALTER TABLE [Material] ADD [IsCaculable] bit NOT NULL DEFAULT CAST(0 AS bit);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250310094044_AmendedMaterialAndAddedMaterialMappingAndUpdatedObligationCalculation'
)
BEGIN
    ALTER TABLE [Material] ADD [IsVisibleToObligation] bit NOT NULL DEFAULT CAST(0 AS bit);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250310094044_AmendedMaterialAndAddedMaterialMappingAndUpdatedObligationCalculation'
)
BEGIN
    ALTER TABLE [Material] ADD CONSTRAINT [PK_Material] PRIMARY KEY ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250310094044_AmendedMaterialAndAddedMaterialMappingAndUpdatedObligationCalculation'
)
BEGIN
    CREATE TABLE [PrnMaterialMapping] (
        [Id] int NOT NULL IDENTITY,
        [PRNMaterialId] int NOT NULL,
        [NPWDMaterialName] nvarchar(50) NOT NULL,
        CONSTRAINT [PK_PrnMaterialMapping] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_PrnMaterialMapping_Material_PRNMaterialId] FOREIGN KEY ([PRNMaterialId]) REFERENCES [Material] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250310094044_AmendedMaterialAndAddedMaterialMappingAndUpdatedObligationCalculation'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'IsCaculable', N'IsVisibleToObligation', N'MaterialCode', N'MaterialName') AND [object_id] = OBJECT_ID(N'[Material]'))
        SET IDENTITY_INSERT [Material] ON;
    EXEC(N'INSERT INTO [Material] ([Id], [IsCaculable], [IsVisibleToObligation], [MaterialCode], [MaterialName])
    VALUES (1, CAST(1 AS bit), CAST(1 AS bit), N''PL'', N''Plastic''),
    (2, CAST(1 AS bit), CAST(1 AS bit), N''WD'', N''Wood''),
    (3, CAST(1 AS bit), CAST(1 AS bit), N''AL'', N''Aluminium''),
    (4, CAST(1 AS bit), CAST(1 AS bit), N''ST'', N''Steel''),
    (5, CAST(1 AS bit), CAST(1 AS bit), N''PC'', N''Paper''),
    (6, CAST(1 AS bit), CAST(1 AS bit), N''GL'', N''Glass''),
    (7, CAST(0 AS bit), CAST(1 AS bit), N''GR'', N''GlassRemelt''),
    (8, CAST(1 AS bit), CAST(0 AS bit), N''FC'', N''FibreComposite'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'IsCaculable', N'IsVisibleToObligation', N'MaterialCode', N'MaterialName') AND [object_id] = OBJECT_ID(N'[Material]'))
        SET IDENTITY_INSERT [Material] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250310094044_AmendedMaterialAndAddedMaterialMappingAndUpdatedObligationCalculation'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'NPWDMaterialName', N'PRNMaterialId') AND [object_id] = OBJECT_ID(N'[PrnMaterialMapping]'))
        SET IDENTITY_INSERT [PrnMaterialMapping] ON;
    EXEC(N'INSERT INTO [PrnMaterialMapping] ([Id], [NPWDMaterialName], [PRNMaterialId])
    VALUES (1, N''Plastic'', 1),
    (2, N''Wood'', 2),
    (3, N''Wood Composting'', 2),
    (4, N''Aluminium'', 3),
    (5, N''Steel'', 4),
    (6, N''Paper/board'', 5),
    (7, N''Paper Composting'', 5),
    (8, N''Glass Other'', 6),
    (9, N''Glass Re-melt'', 7)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'NPWDMaterialName', N'PRNMaterialId') AND [object_id] = OBJECT_ID(N'[PrnMaterialMapping]'))
        SET IDENTITY_INSERT [PrnMaterialMapping] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250310094044_AmendedMaterialAndAddedMaterialMappingAndUpdatedObligationCalculation'
)
BEGIN
    ALTER TABLE [ObligationCalculations] ADD [MaterialId] int NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250310094044_AmendedMaterialAndAddedMaterialMappingAndUpdatedObligationCalculation'
)
BEGIN

                    BEGIN
                        IF COL_LENGTH('ObligationCalculations', 'MaterialName') IS NOT NULL
                        BEGIN
                            EXEC sp_executesql N'
                                UPDATE ObligationCalculations
                                SET MaterialId = (SELECT Id FROM Material WHERE Material.MaterialName = ObligationCalculations.MaterialName)
                                WHERE EXISTS (SELECT 1 FROM Material WHERE Material.MaterialName = ObligationCalculations.MaterialName)
                            ';
                        END
                    END;
                
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250310094044_AmendedMaterialAndAddedMaterialMappingAndUpdatedObligationCalculation'
)
BEGIN
    DECLARE @var1 sysname;
    SELECT @var1 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[ObligationCalculations]') AND [c].[name] = N'MaterialId');
    IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [ObligationCalculations] DROP CONSTRAINT [' + @var1 + '];');
    ALTER TABLE [ObligationCalculations] ALTER COLUMN [MaterialId] int NOT NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250310094044_AmendedMaterialAndAddedMaterialMappingAndUpdatedObligationCalculation'
)
BEGIN
    DECLARE @var2 sysname;
    SELECT @var2 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[ObligationCalculations]') AND [c].[name] = N'MaterialName');
    IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [ObligationCalculations] DROP CONSTRAINT [' + @var2 + '];');
    ALTER TABLE [ObligationCalculations] DROP COLUMN [MaterialName];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250310094044_AmendedMaterialAndAddedMaterialMappingAndUpdatedObligationCalculation'
)
BEGIN
    CREATE INDEX [IX_ObligationCalculations_MaterialId] ON [ObligationCalculations] ([MaterialId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250310094044_AmendedMaterialAndAddedMaterialMappingAndUpdatedObligationCalculation'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Material_MaterialCode] ON [Material] ([MaterialCode]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250310094044_AmendedMaterialAndAddedMaterialMappingAndUpdatedObligationCalculation'
)
BEGIN
    CREATE INDEX [IX_PrnMaterialMapping_PRNMaterialId] ON [PrnMaterialMapping] ([PRNMaterialId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250310094044_AmendedMaterialAndAddedMaterialMappingAndUpdatedObligationCalculation'
)
BEGIN
    ALTER TABLE [ObligationCalculations] ADD CONSTRAINT [FK_ObligationCalculations_Material_MaterialId] FOREIGN KEY ([MaterialId]) REFERENCES [Material] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250310094044_AmendedMaterialAndAddedMaterialMappingAndUpdatedObligationCalculation'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250310094044_AmendedMaterialAndAddedMaterialMappingAndUpdatedObligationCalculation', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250310175126_AmendedMaterialToRemoveBoolColumns'
)
BEGIN
    DECLARE @var3 sysname;
    SELECT @var3 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Material]') AND [c].[name] = N'IsCaculable');
    IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [Material] DROP CONSTRAINT [' + @var3 + '];');
    ALTER TABLE [Material] DROP COLUMN [IsCaculable];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250310175126_AmendedMaterialToRemoveBoolColumns'
)
BEGIN
    DECLARE @var4 sysname;
    SELECT @var4 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Material]') AND [c].[name] = N'IsVisibleToObligation');
    IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [Material] DROP CONSTRAINT [' + @var4 + '];');
    ALTER TABLE [Material] DROP COLUMN [IsVisibleToObligation];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250310175126_AmendedMaterialToRemoveBoolColumns'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250310175126_AmendedMaterialToRemoveBoolColumns', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250512144436_AddReprocessorRegistrationTables'
)
BEGIN
    CREATE TABLE [Lookup.ApplicationType] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(100) NOT NULL,
        CONSTRAINT [PK_Lookup.ApplicationType] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250512144436_AddReprocessorRegistrationTables'
)
BEGIN
    CREATE TABLE [Lookup.FileUploadStatus] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(200) NOT NULL,
        CONSTRAINT [PK_Lookup.FileUploadStatus] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250512144436_AddReprocessorRegistrationTables'
)
BEGIN
    CREATE TABLE [Lookup.FileUploadType] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(200) NOT NULL,
        CONSTRAINT [PK_Lookup.FileUploadType] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250512144436_AddReprocessorRegistrationTables'
)
BEGIN
    CREATE TABLE [Lookup.JourneyType] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(30) NOT NULL,
        CONSTRAINT [PK_Lookup.JourneyType] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250512144436_AddReprocessorRegistrationTables'
)
BEGIN
    CREATE TABLE [Lookup.Material] (
        [Id] int NOT NULL IDENTITY,
        [MaterialName] nvarchar(250) NOT NULL,
        [MaterialCode] nvarchar(250) NOT NULL,
        CONSTRAINT [PK_Lookup.Material] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250512144436_AddReprocessorRegistrationTables'
)
BEGIN
    CREATE TABLE [Lookup.MaterialPermit] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(200) NOT NULL,
        CONSTRAINT [PK_Lookup.MaterialPermit] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250512144436_AddReprocessorRegistrationTables'
)
BEGIN
    CREATE TABLE [Lookup.Period] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(100) NOT NULL,
        CONSTRAINT [PK_Lookup.Period] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250512144436_AddReprocessorRegistrationTables'
)
BEGIN
    CREATE TABLE [Lookup.RegistrationMaterialStatus] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(200) NOT NULL,
        CONSTRAINT [PK_Lookup.RegistrationMaterialStatus] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250512144436_AddReprocessorRegistrationTables'
)
BEGIN
    CREATE TABLE [Lookup.RegulatorTask] (
        [Id] int NOT NULL IDENTITY,
        [IsMaterialSpecific] bit NOT NULL,
        [ApplicationTypeId] int NOT NULL,
        [JourneyTypeId] int NOT NULL,
        [Name] nvarchar(200) NOT NULL,
        CONSTRAINT [PK_Lookup.RegulatorTask] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250512144436_AddReprocessorRegistrationTables'
)
BEGIN
    CREATE TABLE [Lookup.TaskStatus] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(100) NOT NULL,
        CONSTRAINT [PK_Lookup.TaskStatus] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250512144436_AddReprocessorRegistrationTables'
)
BEGIN
    CREATE TABLE [Public.Address] (
        [Id] int NOT NULL IDENTITY,
        [AddressLine1] nvarchar(200) NOT NULL,
        [AddressLine2] nvarchar(200) NOT NULL,
        [TownCity] nvarchar(70) NOT NULL,
        [County] nvarchar(50) NULL,
        [PostCode] nvarchar(10) NOT NULL,
        [NationId] int NULL,
        [GridReference] nvarchar(20) NOT NULL,
        CONSTRAINT [PK_Public.Address] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250512144436_AddReprocessorRegistrationTables'
)
BEGIN
    CREATE TABLE [Public.Registration] (
        [Id] int NOT NULL IDENTITY,
        [ExternalId] uniqueidentifier NOT NULL,
        [ApplicationTypeId] int NOT NULL,
        [OrganisationId] int NOT NULL,
        [RegistrationStatusId] int NOT NULL,
        [BusinessAddressId] int NULL,
        [ReprocessingSiteAddressId] int NULL,
        [LegalDocumentAddressId] int NULL,
        [AssignedOfficerId] int NOT NULL,
        [CreatedBy] uniqueidentifier NOT NULL,
        [CreatedDate] datetime2 NOT NULL,
        [UpdatedBy] uniqueidentifier NOT NULL,
        [UpdatedDate] datetime2 NULL,
        CONSTRAINT [PK_Public.Registration] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Public.Registration_Public.Address_BusinessAddressId] FOREIGN KEY ([BusinessAddressId]) REFERENCES [Public.Address] ([Id]),
        CONSTRAINT [FK_Public.Registration_Public.Address_LegalDocumentAddressId] FOREIGN KEY ([LegalDocumentAddressId]) REFERENCES [Public.Address] ([Id]),
        CONSTRAINT [FK_Public.Registration_Public.Address_ReprocessingSiteAddressId] FOREIGN KEY ([ReprocessingSiteAddressId]) REFERENCES [Public.Address] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250512144436_AddReprocessorRegistrationTables'
)
BEGIN
    CREATE TABLE [Public.RegistrationMaterial] (
        [Id] int NOT NULL IDENTITY,
        [ExternalId] uniqueidentifier NOT NULL,
        [RegistrationId] int NOT NULL,
        [MaterialId] int NOT NULL,
        [StatusId] int NULL,
        [ReferenceNumber] nvarchar(max) NULL,
        [Comments] nvarchar(max) NULL,
        [StatusUpdatedDate] datetime2 NOT NULL,
        [StatusUpdatedBy] uniqueidentifier NULL,
        [ReasonforNotreg] nvarchar(max) NULL,
        [PermitTypeId] int NOT NULL,
        [PPCPermitNumber] nvarchar(20) NULL,
        [WasteManagementLicenceNumber] nvarchar(20) NULL,
        [InstallationPermitNumber] nvarchar(20) NULL,
        [EnvironmentalPermitWasteManagementNumber] nvarchar(20) NULL,
        [PPCReprocessingCapacityTonne] decimal(18,2) NOT NULL,
        [WasteManagementReprocessingCapacityTonne] decimal(18,2) NOT NULL,
        [InstallationReprocessingTonne] decimal(18,2) NOT NULL,
        [EnvironmentalPermitWasteManagementTonne] decimal(18,2) NOT NULL,
        [PPCPeriodId] int NULL,
        [WasteManagementPeriodId] int NULL,
        [InstallationPeriodId] int NULL,
        [EnvironmentalPermitWasteManagementPeriodId] int NULL,
        [MaximumReprocessingCapacityTonne] decimal(18,2) NOT NULL,
        [MaximumReprocessingPeriodId] int NULL,
        [IsMaterialRegistered] bit NOT NULL,
        CONSTRAINT [PK_Public.RegistrationMaterial] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Public.RegistrationMaterial_Lookup.MaterialPermit_PermitTypeId] FOREIGN KEY ([PermitTypeId]) REFERENCES [Lookup.MaterialPermit] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_Public.RegistrationMaterial_Lookup.Material_MaterialId] FOREIGN KEY ([MaterialId]) REFERENCES [Lookup.Material] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_Public.RegistrationMaterial_Lookup.Period_EnvironmentalPermitWasteManagementPeriodId] FOREIGN KEY ([EnvironmentalPermitWasteManagementPeriodId]) REFERENCES [Lookup.Period] ([Id]),
        CONSTRAINT [FK_Public.RegistrationMaterial_Lookup.Period_InstallationPeriodId] FOREIGN KEY ([InstallationPeriodId]) REFERENCES [Lookup.Period] ([Id]),
        CONSTRAINT [FK_Public.RegistrationMaterial_Lookup.Period_MaximumReprocessingPeriodId] FOREIGN KEY ([MaximumReprocessingPeriodId]) REFERENCES [Lookup.Period] ([Id]),
        CONSTRAINT [FK_Public.RegistrationMaterial_Lookup.Period_PPCPeriodId] FOREIGN KEY ([PPCPeriodId]) REFERENCES [Lookup.Period] ([Id]),
        CONSTRAINT [FK_Public.RegistrationMaterial_Lookup.Period_WasteManagementPeriodId] FOREIGN KEY ([WasteManagementPeriodId]) REFERENCES [Lookup.Period] ([Id]),
        CONSTRAINT [FK_Public.RegistrationMaterial_Lookup.RegistrationMaterialStatus_StatusId] FOREIGN KEY ([StatusId]) REFERENCES [Lookup.RegistrationMaterialStatus] ([Id]),
        CONSTRAINT [FK_Public.RegistrationMaterial_Public.Registration_RegistrationId] FOREIGN KEY ([RegistrationId]) REFERENCES [Public.Registration] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250512144436_AddReprocessorRegistrationTables'
)
BEGIN
    CREATE TABLE [Public.RegulatorRegistrationTaskStatus] (
        [Id] int NOT NULL IDENTITY,
        [RegistrationId] int NULL,
        [ExternalId] uniqueidentifier NOT NULL,
        [TaskId] int NULL,
        [TaskStatusId] int NULL,
        [Comments] nvarchar(500) NULL,
        [StatusCreatedBy] uniqueidentifier NOT NULL,
        [StatusCreatedDate] datetime2 NOT NULL,
        [StatusUpdatedBy] uniqueidentifier NULL,
        [StatusUpdatedDate] datetime2 NULL,
        CONSTRAINT [PK_Public.RegulatorRegistrationTaskStatus] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Public.RegulatorRegistrationTaskStatus_Lookup.RegulatorTask_TaskId] FOREIGN KEY ([TaskId]) REFERENCES [Lookup.RegulatorTask] ([Id]),
        CONSTRAINT [FK_Public.RegulatorRegistrationTaskStatus_Lookup.TaskStatus_TaskStatusId] FOREIGN KEY ([TaskStatusId]) REFERENCES [Lookup.TaskStatus] ([Id]),
        CONSTRAINT [FK_Public.RegulatorRegistrationTaskStatus_Public.Registration_RegistrationId] FOREIGN KEY ([RegistrationId]) REFERENCES [Public.Registration] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250512144436_AddReprocessorRegistrationTables'
)
BEGIN
    CREATE TABLE [public.FileUpload] (
        [Id] int NOT NULL IDENTITY,
        [ExternalId] uniqueidentifier NOT NULL,
        [RegistrationMaterialId] int NULL,
        [Filename] nvarchar(50) NULL,
        [FileId] uniqueidentifier NOT NULL,
        [DateUploaded] datetime2 NULL,
        [UpdatedBy] uniqueidentifier NOT NULL,
        [FileUploadTypeId] int NULL,
        [FileUploadStatusId] int NULL,
        [Comments] nvarchar(500) NULL,
        CONSTRAINT [PK_public.FileUpload] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_public.FileUpload_Lookup.FileUploadStatus_FileUploadStatusId] FOREIGN KEY ([FileUploadStatusId]) REFERENCES [Lookup.FileUploadStatus] ([Id]),
        CONSTRAINT [FK_public.FileUpload_Lookup.FileUploadType_FileUploadTypeId] FOREIGN KEY ([FileUploadTypeId]) REFERENCES [Lookup.FileUploadType] ([Id]),
        CONSTRAINT [FK_public.FileUpload_Public.RegistrationMaterial_RegistrationMaterialId] FOREIGN KEY ([RegistrationMaterialId]) REFERENCES [Public.RegistrationMaterial] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250512144436_AddReprocessorRegistrationTables'
)
BEGIN
    CREATE TABLE [Public.MaterialExemptionReference] (
        [Id] int NOT NULL IDENTITY,
        [ExternalId] uniqueidentifier NOT NULL,
        [RegistrationMaterialId] int NOT NULL,
        [ReferenceNo] nvarchar(100) NOT NULL,
        CONSTRAINT [PK_Public.MaterialExemptionReference] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Public.MaterialExemptionReference_Public.RegistrationMaterial_RegistrationMaterialId] FOREIGN KEY ([RegistrationMaterialId]) REFERENCES [Public.RegistrationMaterial] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250512144436_AddReprocessorRegistrationTables'
)
BEGIN
    CREATE TABLE [Public.RegistrationReprocessingIO] (
        [Id] int NOT NULL IDENTITY,
        [ExternalId] uniqueidentifier NOT NULL,
        [RegistrationMaterialId] int NOT NULL,
        [TypeOfSupplier] nvarchar(2000) NULL,
        [PlantEquipmentUsed] nvarchar(2000) NULL,
        [ReprocessingPackagingWasteLastYearFlag] bit NOT NULL,
        [UKPackagingWasteTonne] decimal(18,2) NOT NULL,
        [NonUKPackagingWasteTonne] decimal(18,2) NOT NULL,
        [NotPackingWasteTonne] decimal(18,2) NOT NULL,
        [SenttoOtherSiteTonne] decimal(18,2) NOT NULL,
        [ContaminantsTonne] decimal(18,2) NOT NULL,
        [ProcessLossTonne] decimal(18,2) NOT NULL,
        [TotalInputs] decimal(18,2) NOT NULL,
        [TotalOutputs] decimal(18,2) NOT NULL,
        CONSTRAINT [PK_Public.RegistrationReprocessingIO] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Public.RegistrationReprocessingIO_Public.RegistrationMaterial_RegistrationMaterialId] FOREIGN KEY ([RegistrationMaterialId]) REFERENCES [Public.RegistrationMaterial] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250512144436_AddReprocessorRegistrationTables'
)
BEGIN
    CREATE TABLE [Public.RegulatorApplicationTaskStatus] (
        [Id] int NOT NULL IDENTITY,
        [RegistrationMaterialId] int NULL,
        [ExternalId] uniqueidentifier NOT NULL,
        [TaskId] int NULL,
        [TaskStatusId] int NULL,
        [Comments] nvarchar(500) NULL,
        [StatusCreatedBy] uniqueidentifier NOT NULL,
        [StatusCreatedDate] datetime2 NOT NULL,
        [StatusUpdatedBy] uniqueidentifier NULL,
        [StatusUpdatedDate] datetime2 NULL,
        CONSTRAINT [PK_Public.RegulatorApplicationTaskStatus] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Public.RegulatorApplicationTaskStatus_Lookup.RegulatorTask_TaskId] FOREIGN KEY ([TaskId]) REFERENCES [Lookup.RegulatorTask] ([Id]),
        CONSTRAINT [FK_Public.RegulatorApplicationTaskStatus_Lookup.TaskStatus_TaskStatusId] FOREIGN KEY ([TaskStatusId]) REFERENCES [Lookup.TaskStatus] ([Id]),
        CONSTRAINT [FK_Public.RegulatorApplicationTaskStatus_Public.RegistrationMaterial_RegistrationMaterialId] FOREIGN KEY ([RegistrationMaterialId]) REFERENCES [Public.RegistrationMaterial] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250512144436_AddReprocessorRegistrationTables'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup.ApplicationType]'))
        SET IDENTITY_INSERT [Lookup.ApplicationType] ON;
    EXEC(N'INSERT INTO [Lookup.ApplicationType] ([Id], [Name])
    VALUES (1, N''Reprocessor''),
    (2, N''Exporter'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup.ApplicationType]'))
        SET IDENTITY_INSERT [Lookup.ApplicationType] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250512144436_AddReprocessorRegistrationTables'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup.FileUploadStatus]'))
        SET IDENTITY_INSERT [Lookup.FileUploadStatus] ON;
    EXEC(N'INSERT INTO [Lookup.FileUploadStatus] ([Id], [Name])
    VALUES (1, N''Virus check failed''),
    (2, N''Virus check succeeded''),
    (3, N''Upload complete''),
    (4, N''Upload failed''),
    (5, N''File deleted(Soft delete of record in database – will physically remove from blob storage)'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup.FileUploadStatus]'))
        SET IDENTITY_INSERT [Lookup.FileUploadStatus] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250512144436_AddReprocessorRegistrationTables'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup.FileUploadType]'))
        SET IDENTITY_INSERT [Lookup.FileUploadType] ON;
    EXEC(N'INSERT INTO [Lookup.FileUploadType] ([Id], [Name])
    VALUES (1, N''SamplingAndInspectionPlan'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup.FileUploadType]'))
        SET IDENTITY_INSERT [Lookup.FileUploadType] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250512144436_AddReprocessorRegistrationTables'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup.JourneyType]'))
        SET IDENTITY_INSERT [Lookup.JourneyType] ON;
    EXEC(N'INSERT INTO [Lookup.JourneyType] ([Id], [Name])
    VALUES (1, N''Registration''),
    (2, N''Accreditation'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup.JourneyType]'))
        SET IDENTITY_INSERT [Lookup.JourneyType] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250512144436_AddReprocessorRegistrationTables'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'MaterialCode', N'MaterialName') AND [object_id] = OBJECT_ID(N'[Lookup.Material]'))
        SET IDENTITY_INSERT [Lookup.Material] ON;
    EXEC(N'INSERT INTO [Lookup.Material] ([Id], [MaterialCode], [MaterialName])
    VALUES (1, N''PL'', N''Plastic''),
    (2, N''GL'', N''Steel''),
    (3, N''AL'', N''Aluminium'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'MaterialCode', N'MaterialName') AND [object_id] = OBJECT_ID(N'[Lookup.Material]'))
        SET IDENTITY_INSERT [Lookup.Material] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250512144436_AddReprocessorRegistrationTables'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup.MaterialPermit]'))
        SET IDENTITY_INSERT [Lookup.MaterialPermit] ON;
    EXEC(N'INSERT INTO [Lookup.MaterialPermit] ([Id], [Name])
    VALUES (1, N''Waste Exemption''),
    (2, N''Pollution,Prevention and Control(PPC) permit''),
    (3, N''Waste Management Licence''),
    (4, N''Installation Permit''),
    (5, N''Environmental permit or waste management licence'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup.MaterialPermit]'))
        SET IDENTITY_INSERT [Lookup.MaterialPermit] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250512144436_AddReprocessorRegistrationTables'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup.Period]'))
        SET IDENTITY_INSERT [Lookup.Period] ON;
    EXEC(N'INSERT INTO [Lookup.Period] ([Id], [Name])
    VALUES (1, N''Per Year''),
    (2, N''Per Month''),
    (3, N''Per Week'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup.Period]'))
        SET IDENTITY_INSERT [Lookup.Period] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250512144436_AddReprocessorRegistrationTables'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup.RegistrationMaterialStatus]'))
        SET IDENTITY_INSERT [Lookup.RegistrationMaterialStatus] ON;
    EXEC(N'INSERT INTO [Lookup.RegistrationMaterialStatus] ([Id], [Name])
    VALUES (1, N''Granted''),
    (2, N''Refused'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup.RegistrationMaterialStatus]'))
        SET IDENTITY_INSERT [Lookup.RegistrationMaterialStatus] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250512144436_AddReprocessorRegistrationTables'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ApplicationTypeId', N'IsMaterialSpecific', N'JourneyTypeId', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup.RegulatorTask]'))
        SET IDENTITY_INSERT [Lookup.RegulatorTask] ON;
    EXEC(N'INSERT INTO [Lookup.RegulatorTask] ([Id], [ApplicationTypeId], [IsMaterialSpecific], [JourneyTypeId], [Name])
    VALUES (1, 1, CAST(0 AS bit), 1, N''SiteAddressAndContactDetails''),
    (2, 1, CAST(0 AS bit), 1, N''MaterialsAuthorisedOnSite''),
    (3, 1, CAST(0 AS bit), 1, N''RegistrationDulyMade''),
    (4, 1, CAST(1 AS bit), 1, N''WasteLicensesPermitsAndExemptions''),
    (5, 1, CAST(1 AS bit), 1, N''ReprocessingInputsAndOutputs''),
    (6, 1, CAST(1 AS bit), 1, N''SamplingAndInspectionPlan''),
    (7, 1, CAST(1 AS bit), 1, N''AssignOfficer''),
    (8, 2, CAST(0 AS bit), 1, N''BusinessAddress''),
    (9, 2, CAST(0 AS bit), 1, N''WasteLicensesPermitsAndExemptions''),
    (10, 2, CAST(0 AS bit), 1, N''RegistrationDulyMade''),
    (11, 2, CAST(1 AS bit), 1, N''SamplingAndInspectionPlan''),
    (12, 2, CAST(1 AS bit), 1, N''AssignOfficer''),
    (13, 2, CAST(1 AS bit), 1, N''MaterialDetailsAndContact''),
    (14, 2, CAST(1 AS bit), 1, N''OverseasReprocessorAndInterimSiteDetails'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ApplicationTypeId', N'IsMaterialSpecific', N'JourneyTypeId', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup.RegulatorTask]'))
        SET IDENTITY_INSERT [Lookup.RegulatorTask] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250512144436_AddReprocessorRegistrationTables'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup.TaskStatus]'))
        SET IDENTITY_INSERT [Lookup.TaskStatus] ON;
    EXEC(N'INSERT INTO [Lookup.TaskStatus] ([Id], [Name])
    VALUES (1, N''NotStarted''),
    (2, N''Started''),
    (3, N''CannotStartYet''),
    (4, N''Queried''),
    (5, N''Completed'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup.TaskStatus]'))
        SET IDENTITY_INSERT [Lookup.TaskStatus] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250512144436_AddReprocessorRegistrationTables'
)
BEGIN
    CREATE INDEX [IX_public.FileUpload_FileUploadStatusId] ON [public.FileUpload] ([FileUploadStatusId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250512144436_AddReprocessorRegistrationTables'
)
BEGIN
    CREATE INDEX [IX_public.FileUpload_FileUploadTypeId] ON [public.FileUpload] ([FileUploadTypeId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250512144436_AddReprocessorRegistrationTables'
)
BEGIN
    CREATE INDEX [IX_public.FileUpload_RegistrationMaterialId] ON [public.FileUpload] ([RegistrationMaterialId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250512144436_AddReprocessorRegistrationTables'
)
BEGIN
    CREATE INDEX [IX_Public.MaterialExemptionReference_RegistrationMaterialId] ON [Public.MaterialExemptionReference] ([RegistrationMaterialId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250512144436_AddReprocessorRegistrationTables'
)
BEGIN
    CREATE INDEX [IX_Public.Registration_BusinessAddressId] ON [Public.Registration] ([BusinessAddressId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250512144436_AddReprocessorRegistrationTables'
)
BEGIN
    CREATE INDEX [IX_Public.Registration_LegalDocumentAddressId] ON [Public.Registration] ([LegalDocumentAddressId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250512144436_AddReprocessorRegistrationTables'
)
BEGIN
    CREATE INDEX [IX_Public.Registration_ReprocessingSiteAddressId] ON [Public.Registration] ([ReprocessingSiteAddressId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250512144436_AddReprocessorRegistrationTables'
)
BEGIN
    CREATE INDEX [IX_Public.RegistrationMaterial_EnvironmentalPermitWasteManagementPeriodId] ON [Public.RegistrationMaterial] ([EnvironmentalPermitWasteManagementPeriodId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250512144436_AddReprocessorRegistrationTables'
)
BEGIN
    CREATE INDEX [IX_Public.RegistrationMaterial_InstallationPeriodId] ON [Public.RegistrationMaterial] ([InstallationPeriodId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250512144436_AddReprocessorRegistrationTables'
)
BEGIN
    CREATE INDEX [IX_Public.RegistrationMaterial_MaterialId] ON [Public.RegistrationMaterial] ([MaterialId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250512144436_AddReprocessorRegistrationTables'
)
BEGIN
    CREATE INDEX [IX_Public.RegistrationMaterial_MaximumReprocessingPeriodId] ON [Public.RegistrationMaterial] ([MaximumReprocessingPeriodId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250512144436_AddReprocessorRegistrationTables'
)
BEGIN
    CREATE INDEX [IX_Public.RegistrationMaterial_PermitTypeId] ON [Public.RegistrationMaterial] ([PermitTypeId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250512144436_AddReprocessorRegistrationTables'
)
BEGIN
    CREATE INDEX [IX_Public.RegistrationMaterial_PPCPeriodId] ON [Public.RegistrationMaterial] ([PPCPeriodId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250512144436_AddReprocessorRegistrationTables'
)
BEGIN
    CREATE INDEX [IX_Public.RegistrationMaterial_RegistrationId] ON [Public.RegistrationMaterial] ([RegistrationId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250512144436_AddReprocessorRegistrationTables'
)
BEGIN
    CREATE INDEX [IX_Public.RegistrationMaterial_StatusId] ON [Public.RegistrationMaterial] ([StatusId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250512144436_AddReprocessorRegistrationTables'
)
BEGIN
    CREATE INDEX [IX_Public.RegistrationMaterial_WasteManagementPeriodId] ON [Public.RegistrationMaterial] ([WasteManagementPeriodId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250512144436_AddReprocessorRegistrationTables'
)
BEGIN
    CREATE INDEX [IX_Public.RegistrationReprocessingIO_RegistrationMaterialId] ON [Public.RegistrationReprocessingIO] ([RegistrationMaterialId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250512144436_AddReprocessorRegistrationTables'
)
BEGIN
    CREATE INDEX [IX_Public.RegulatorApplicationTaskStatus_RegistrationMaterialId] ON [Public.RegulatorApplicationTaskStatus] ([RegistrationMaterialId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250512144436_AddReprocessorRegistrationTables'
)
BEGIN
    CREATE INDEX [IX_Public.RegulatorApplicationTaskStatus_TaskId] ON [Public.RegulatorApplicationTaskStatus] ([TaskId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250512144436_AddReprocessorRegistrationTables'
)
BEGIN
    CREATE INDEX [IX_Public.RegulatorApplicationTaskStatus_TaskStatusId] ON [Public.RegulatorApplicationTaskStatus] ([TaskStatusId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250512144436_AddReprocessorRegistrationTables'
)
BEGIN
    CREATE INDEX [IX_Public.RegulatorRegistrationTaskStatus_RegistrationId] ON [Public.RegulatorRegistrationTaskStatus] ([RegistrationId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250512144436_AddReprocessorRegistrationTables'
)
BEGIN
    CREATE INDEX [IX_Public.RegulatorRegistrationTaskStatus_TaskId] ON [Public.RegulatorRegistrationTaskStatus] ([TaskId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250512144436_AddReprocessorRegistrationTables'
)
BEGIN
    CREATE INDEX [IX_Public.RegulatorRegistrationTaskStatus_TaskStatusId] ON [Public.RegulatorRegistrationTaskStatus] ([TaskStatusId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250512144436_AddReprocessorRegistrationTables'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250512144436_AddReprocessorRegistrationTables', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250514185141_AddDulyMadeTableAndUpdatedLookUpMaterial'
)
BEGIN
    ALTER TABLE [Public.RegistrationMaterial] ADD [ApplicationReferenceNumber] nvarchar(20) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250514185141_AddDulyMadeTableAndUpdatedLookUpMaterial'
)
BEGIN
    ALTER TABLE [Public.RegistrationMaterial] ADD [CreatedDate] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250514185141_AddDulyMadeTableAndUpdatedLookUpMaterial'
)
BEGIN
    CREATE TABLE [Public.DulyMade] (
        [Id] int NOT NULL IDENTITY,
        [ExternalId] uniqueidentifier NOT NULL,
        [RegistrationMaterialId] int NOT NULL,
        [TaskStatusId] int NOT NULL,
        [DulyMadeDate] datetime2 NULL,
        [DulyMadeBy] uniqueidentifier NULL,
        [DeterminationDate] datetime2 NULL,
        CONSTRAINT [PK_Public.DulyMade] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Public.DulyMade_Lookup.TaskStatus_TaskStatusId] FOREIGN KEY ([TaskStatusId]) REFERENCES [Lookup.TaskStatus] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_Public.DulyMade_Public.RegistrationMaterial_RegistrationMaterialId] FOREIGN KEY ([RegistrationMaterialId]) REFERENCES [Public.RegistrationMaterial] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250514185141_AddDulyMadeTableAndUpdatedLookUpMaterial'
)
BEGIN
    EXEC(N'UPDATE [Lookup.Material] SET [MaterialCode] = N''ST''
    WHERE [Id] = 2;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250514185141_AddDulyMadeTableAndUpdatedLookUpMaterial'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'MaterialCode', N'MaterialName') AND [object_id] = OBJECT_ID(N'[Lookup.Material]'))
        SET IDENTITY_INSERT [Lookup.Material] ON;
    EXEC(N'INSERT INTO [Lookup.Material] ([Id], [MaterialCode], [MaterialName])
    VALUES (4, N''GL'', N''Glass''),
    (5, N''PA'', N''Paper/Board''),
    (6, N''WO'', N''Wood'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'MaterialCode', N'MaterialName') AND [object_id] = OBJECT_ID(N'[Lookup.Material]'))
        SET IDENTITY_INSERT [Lookup.Material] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250514185141_AddDulyMadeTableAndUpdatedLookUpMaterial'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ApplicationTypeId', N'IsMaterialSpecific', N'JourneyTypeId', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup.RegulatorTask]'))
        SET IDENTITY_INSERT [Lookup.RegulatorTask] ON;
    EXEC(N'INSERT INTO [Lookup.RegulatorTask] ([Id], [ApplicationTypeId], [IsMaterialSpecific], [JourneyTypeId], [Name])
    VALUES (15, 1, CAST(1 AS bit), 1, N''CheckRegistrationStatus''),
    (16, 2, CAST(1 AS bit), 1, N''CheckRegistrationStatus'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ApplicationTypeId', N'IsMaterialSpecific', N'JourneyTypeId', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup.RegulatorTask]'))
        SET IDENTITY_INSERT [Lookup.RegulatorTask] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250514185141_AddDulyMadeTableAndUpdatedLookUpMaterial'
)
BEGIN
    CREATE INDEX [IX_Public.DulyMade_RegistrationMaterialId] ON [Public.DulyMade] ([RegistrationMaterialId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250514185141_AddDulyMadeTableAndUpdatedLookUpMaterial'
)
BEGIN
    CREATE INDEX [IX_Public.DulyMade_TaskStatusId] ON [Public.DulyMade] ([TaskStatusId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250514185141_AddDulyMadeTableAndUpdatedLookUpMaterial'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250514185141_AddDulyMadeTableAndUpdatedLookUpMaterial', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250516163946_AlterTableDulyMadeAndRegistrationMaterial'
)
BEGIN
    EXEC sp_rename N'[Public.RegistrationMaterial].[ReferenceNumber]', N'RegistrationReferenceNumber', N'COLUMN';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250516163946_AlterTableDulyMadeAndRegistrationMaterial'
)
BEGIN
    ALTER TABLE [Public.DulyMade] ADD [DeterminationNote] nvarchar(500) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250516163946_AlterTableDulyMadeAndRegistrationMaterial'
)
BEGIN
    ALTER TABLE [Public.DulyMade] ADD [DeterminationUpdatedBy] uniqueidentifier NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250516163946_AlterTableDulyMadeAndRegistrationMaterial'
)
BEGIN
    ALTER TABLE [Public.DulyMade] ADD [DeterminationUpdatedDate] datetime2 NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250516163946_AlterTableDulyMadeAndRegistrationMaterial'
)
BEGIN
    ALTER TABLE [Public.DulyMade] ADD [DulyMadeNote] nvarchar(500) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250516163946_AlterTableDulyMadeAndRegistrationMaterial'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250516163946_AlterTableDulyMadeAndRegistrationMaterial', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250522135103_AddRegistrationTaskStatus'
)
BEGIN
    CREATE TABLE [Public.RegistrationTaskStatus] (
        [Id] int NOT NULL IDENTITY,
        [ExternalId] uniqueidentifier NOT NULL,
        [TaskId] int NULL,
        [TaskStatusId] int NULL,
        [RegistrationId] int NULL,
        CONSTRAINT [PK_Public.RegistrationTaskStatus] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Public.RegistrationTaskStatus_Lookup.RegulatorTask_TaskId] FOREIGN KEY ([TaskId]) REFERENCES [Lookup.RegulatorTask] ([Id]),
        CONSTRAINT [FK_Public.RegistrationTaskStatus_Lookup.TaskStatus_TaskStatusId] FOREIGN KEY ([TaskStatusId]) REFERENCES [Lookup.TaskStatus] ([Id]),
        CONSTRAINT [FK_Public.RegistrationTaskStatus_Public.Registration_RegistrationId] FOREIGN KEY ([RegistrationId]) REFERENCES [Public.Registration] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250522135103_AddRegistrationTaskStatus'
)
BEGIN
    CREATE INDEX [IX_Public.RegistrationTaskStatus_RegistrationId] ON [Public.RegistrationTaskStatus] ([RegistrationId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250522135103_AddRegistrationTaskStatus'
)
BEGIN
    CREATE INDEX [IX_Public.RegistrationTaskStatus_TaskId] ON [Public.RegistrationTaskStatus] ([TaskId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250522135103_AddRegistrationTaskStatus'
)
BEGIN
    CREATE INDEX [IX_Public.RegistrationTaskStatus_TaskStatusId] ON [Public.RegistrationTaskStatus] ([TaskStatusId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250522135103_AddRegistrationTaskStatus'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250522135103_AddRegistrationTaskStatus', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250522160155_ChangeTaskStatusTables'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250522160155_ChangeTaskStatusTables', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250523091809_DropOrgIdFromRegistration'
)
BEGIN
    DECLARE @var5 sysname;
    SELECT @var5 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Public.Registration]') AND [c].[name] = N'OrganisationId');
    IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [Public.Registration] DROP CONSTRAINT [' + @var5 + '];');
    ALTER TABLE [Public.Registration] DROP COLUMN [OrganisationId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250523091809_DropOrgIdFromRegistration'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250523091809_DropOrgIdFromRegistration', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250523092133_AddGuidOrganisationId'
)
BEGIN
    ALTER TABLE [Public.Registration] ADD [OrganisationId] uniqueidentifier NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250523092133_AddGuidOrganisationId'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250523092133_AddGuidOrganisationId', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250528090955_Accreditation'
)
BEGIN
    CREATE TABLE [Lookup.AccreditationStatus] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(100) NOT NULL,
        CONSTRAINT [PK_Lookup.AccreditationStatus] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250528090955_Accreditation'
)
BEGIN
    CREATE TABLE [Public.RegulatorAccreditationRegistrationTaskStatus] (
        [Id] int NOT NULL IDENTITY,
        [RegistrationId] int NOT NULL,
        [AccreditationYear] int NOT NULL,
        [ExternalId] uniqueidentifier NOT NULL,
        [TaskId] int NULL,
        [TaskStatusId] int NULL,
        [Comments] nvarchar(500) NULL,
        [StatusCreatedBy] uniqueidentifier NOT NULL,
        [StatusCreatedDate] datetime2 NOT NULL,
        [StatusUpdatedBy] uniqueidentifier NULL,
        [StatusUpdatedDate] datetime2 NULL,
        CONSTRAINT [PK_Public.RegulatorAccreditationRegistrationTaskStatus] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Public.RegulatorAccreditationRegistrationTaskStatus_Lookup.RegulatorTask_TaskId] FOREIGN KEY ([TaskId]) REFERENCES [Lookup.RegulatorTask] ([Id]),
        CONSTRAINT [FK_Public.RegulatorAccreditationRegistrationTaskStatus_Lookup.TaskStatus_TaskStatusId] FOREIGN KEY ([TaskStatusId]) REFERENCES [Lookup.TaskStatus] ([Id]),
        CONSTRAINT [FK_Public.RegulatorAccreditationRegistrationTaskStatus_Public.Registration_RegistrationId] FOREIGN KEY ([RegistrationId]) REFERENCES [Public.Registration] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250528090955_Accreditation'
)
BEGIN
    CREATE TABLE [Public.Accreditation] (
        [Id] int NOT NULL IDENTITY,
        [ExternalId] uniqueidentifier NOT NULL,
        [RegistrationMaterialId] int NOT NULL,
        [AccreditationYear] int NOT NULL,
        [ApplicationReferenceNumber] nvarchar(12) NOT NULL,
        [AccreditationStatusId] int NOT NULL,
        CONSTRAINT [PK_Public.Accreditation] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Public.Accreditation_Lookup.AccreditationStatus_AccreditationStatusId] FOREIGN KEY ([AccreditationStatusId]) REFERENCES [Lookup.AccreditationStatus] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_Public.Accreditation_Public.RegistrationMaterial_RegistrationMaterialId] FOREIGN KEY ([RegistrationMaterialId]) REFERENCES [Public.RegistrationMaterial] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250528090955_Accreditation'
)
BEGIN
    CREATE TABLE [Public.AccreditationDeterminationDate] (
        [Id] int NOT NULL IDENTITY,
        [ExternalId] uniqueidentifier NOT NULL,
        [AccreditationId] int NOT NULL,
        [DeterminationDate] datetime2 NULL,
        CONSTRAINT [PK_Public.AccreditationDeterminationDate] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Public.AccreditationDeterminationDate_Public.Accreditation_AccreditationId] FOREIGN KEY ([AccreditationId]) REFERENCES [Public.Accreditation] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250528090955_Accreditation'
)
BEGIN
    CREATE TABLE [Public.RegulatorAccreditationTaskStatus] (
        [Id] int NOT NULL IDENTITY,
        [AccreditationId] int NULL,
        [ExternalId] uniqueidentifier NOT NULL,
        [TaskId] int NULL,
        [TaskStatusId] int NULL,
        [Comments] nvarchar(500) NULL,
        [StatusCreatedBy] uniqueidentifier NOT NULL,
        [StatusCreatedDate] datetime2 NOT NULL,
        [StatusUpdatedBy] uniqueidentifier NULL,
        [StatusUpdatedDate] datetime2 NULL,
        CONSTRAINT [PK_Public.RegulatorAccreditationTaskStatus] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Public.RegulatorAccreditationTaskStatus_Lookup.RegulatorTask_TaskId] FOREIGN KEY ([TaskId]) REFERENCES [Lookup.RegulatorTask] ([Id]),
        CONSTRAINT [FK_Public.RegulatorAccreditationTaskStatus_Lookup.TaskStatus_TaskStatusId] FOREIGN KEY ([TaskStatusId]) REFERENCES [Lookup.TaskStatus] ([Id]),
        CONSTRAINT [FK_Public.RegulatorAccreditationTaskStatus_Public.Accreditation_AccreditationId] FOREIGN KEY ([AccreditationId]) REFERENCES [Public.Accreditation] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250528090955_Accreditation'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup.AccreditationStatus]'))
        SET IDENTITY_INSERT [Lookup.AccreditationStatus] ON;
    EXEC(N'INSERT INTO [Lookup.AccreditationStatus] ([Id], [Name])
    VALUES (1, N''Started''),
    (2, N''Submitted''),
    (3, N''RegulatorReviewing''),
    (4, N''Queried''),
    (5, N''Updated''),
    (6, N''Granted''),
    (7, N''Refused''),
    (8, N''Withdrawn''),
    (9, N''Suspended''),
    (10, N''Cancelled''),
    (11, N''ReadyToSubmit'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup.AccreditationStatus]'))
        SET IDENTITY_INSERT [Lookup.AccreditationStatus] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250528090955_Accreditation'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup.RegistrationMaterialStatus]'))
        SET IDENTITY_INSERT [Lookup.RegistrationMaterialStatus] ON;
    EXEC(N'INSERT INTO [Lookup.RegistrationMaterialStatus] ([Id], [Name])
    VALUES (3, N''Started''),
    (4, N''Submitted''),
    (5, N''RegulatorReviewing''),
    (6, N''Queried''),
    (8, N''Withdrawn''),
    (9, N''Suspended''),
    (10, N''Cancelled''),
    (11, N''ReadyToSubmit'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup.RegistrationMaterialStatus]'))
        SET IDENTITY_INSERT [Lookup.RegistrationMaterialStatus] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250528090955_Accreditation'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ApplicationTypeId', N'IsMaterialSpecific', N'JourneyTypeId', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup.RegulatorTask]'))
        SET IDENTITY_INSERT [Lookup.RegulatorTask] ON;
    EXEC(N'INSERT INTO [Lookup.RegulatorTask] ([Id], [ApplicationTypeId], [IsMaterialSpecific], [JourneyTypeId], [Name])
    VALUES (17, 1, CAST(0 AS bit), 2, N''AssignOfficer''),
    (18, 1, CAST(1 AS bit), 2, N''PRNs tonnage and authority to issue PRNs''),
    (19, 1, CAST(1 AS bit), 2, N''Business Plan''),
    (20, 1, CAST(1 AS bit), 2, N''Accreditation sampling and inspection plan''),
    (21, 1, CAST(1 AS bit), 2, N''Overseas reprocessing sites and broadly equivalent evidence''),
    (22, 2, CAST(0 AS bit), 2, N''AssignOfficer''),
    (23, 2, CAST(1 AS bit), 2, N''PRNs tonnage and authority to issue PRNs''),
    (24, 2, CAST(1 AS bit), 2, N''Business Plan''),
    (25, 2, CAST(1 AS bit), 2, N''Accreditation sampling and inspection plan''),
    (26, 2, CAST(1 AS bit), 2, N''Overseas reprocessing sites and broadly equivalent evidence'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ApplicationTypeId', N'IsMaterialSpecific', N'JourneyTypeId', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup.RegulatorTask]'))
        SET IDENTITY_INSERT [Lookup.RegulatorTask] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250528090955_Accreditation'
)
BEGIN
    CREATE INDEX [IX_Public.Accreditation_AccreditationStatusId] ON [Public.Accreditation] ([AccreditationStatusId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250528090955_Accreditation'
)
BEGIN
    CREATE INDEX [IX_Public.Accreditation_RegistrationMaterialId] ON [Public.Accreditation] ([RegistrationMaterialId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250528090955_Accreditation'
)
BEGIN
    CREATE INDEX [IX_Public.AccreditationDeterminationDate_AccreditationId] ON [Public.AccreditationDeterminationDate] ([AccreditationId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250528090955_Accreditation'
)
BEGIN
    CREATE INDEX [IX_Public.RegulatorAccreditationRegistrationTaskStatus_RegistrationId] ON [Public.RegulatorAccreditationRegistrationTaskStatus] ([RegistrationId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250528090955_Accreditation'
)
BEGIN
    CREATE INDEX [IX_Public.RegulatorAccreditationRegistrationTaskStatus_TaskId] ON [Public.RegulatorAccreditationRegistrationTaskStatus] ([TaskId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250528090955_Accreditation'
)
BEGIN
    CREATE INDEX [IX_Public.RegulatorAccreditationRegistrationTaskStatus_TaskStatusId] ON [Public.RegulatorAccreditationRegistrationTaskStatus] ([TaskStatusId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250528090955_Accreditation'
)
BEGIN
    CREATE INDEX [IX_Public.RegulatorAccreditationTaskStatus_AccreditationId] ON [Public.RegulatorAccreditationTaskStatus] ([AccreditationId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250528090955_Accreditation'
)
BEGIN
    CREATE INDEX [IX_Public.RegulatorAccreditationTaskStatus_TaskId] ON [Public.RegulatorAccreditationTaskStatus] ([TaskId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250528090955_Accreditation'
)
BEGIN
    CREATE INDEX [IX_Public.RegulatorAccreditationTaskStatus_TaskStatusId] ON [Public.RegulatorAccreditationTaskStatus] ([TaskStatusId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250528090955_Accreditation'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250528090955_Accreditation', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250528152956_accreditationFileUploadTable'
)
BEGIN
    DECLARE @var6 sysname;
    SELECT @var6 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[public.FileUpload]') AND [c].[name] = N'UpdatedBy');
    IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [public.FileUpload] DROP CONSTRAINT [' + @var6 + '];');
    ALTER TABLE [public.FileUpload] ALTER COLUMN [UpdatedBy] nvarchar(50) NOT NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250528152956_accreditationFileUploadTable'
)
BEGIN
    CREATE TABLE [public.AccreditationFileUpload] (
        [Id] int NOT NULL IDENTITY,
        [AccreditationId] int NULL,
        [ExternalId] uniqueidentifier NOT NULL,
        [Filename] nvarchar(50) NULL,
        [FileId] uniqueidentifier NOT NULL,
        [DateUploaded] datetime2 NULL,
        [UpdatedBy] nvarchar(50) NOT NULL,
        [FileUploadTypeId] int NULL,
        [FileUploadStatusId] int NULL,
        CONSTRAINT [PK_public.AccreditationFileUpload] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_public.AccreditationFileUpload_Lookup.FileUploadStatus_FileUploadStatusId] FOREIGN KEY ([FileUploadStatusId]) REFERENCES [Lookup.FileUploadStatus] ([Id]),
        CONSTRAINT [FK_public.AccreditationFileUpload_Lookup.FileUploadType_FileUploadTypeId] FOREIGN KEY ([FileUploadTypeId]) REFERENCES [Lookup.FileUploadType] ([Id]),
        CONSTRAINT [FK_public.AccreditationFileUpload_Public.Accreditation_AccreditationId] FOREIGN KEY ([AccreditationId]) REFERENCES [Public.Accreditation] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250528152956_accreditationFileUploadTable'
)
BEGIN
    CREATE INDEX [IX_public.AccreditationFileUpload_AccreditationId] ON [public.AccreditationFileUpload] ([AccreditationId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250528152956_accreditationFileUploadTable'
)
BEGIN
    CREATE INDEX [IX_public.AccreditationFileUpload_FileUploadStatusId] ON [public.AccreditationFileUpload] ([FileUploadStatusId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250528152956_accreditationFileUploadTable'
)
BEGIN
    CREATE INDEX [IX_public.AccreditationFileUpload_FileUploadTypeId] ON [public.AccreditationFileUpload] ([FileUploadTypeId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250528152956_accreditationFileUploadTable'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250528152956_accreditationFileUploadTable', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530121131_Accreditation_Updates'
)
BEGIN
    ALTER TABLE [Public.RegulatorAccreditationRegistrationTaskStatus] DROP CONSTRAINT [FK_Public.RegulatorAccreditationRegistrationTaskStatus_Lookup.RegulatorTask_TaskId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530121131_Accreditation_Updates'
)
BEGIN
    ALTER TABLE [Public.RegulatorAccreditationRegistrationTaskStatus] DROP CONSTRAINT [FK_Public.RegulatorAccreditationRegistrationTaskStatus_Lookup.TaskStatus_TaskStatusId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530121131_Accreditation_Updates'
)
BEGIN
    ALTER TABLE [Public.RegulatorAccreditationTaskStatus] DROP CONSTRAINT [FK_Public.RegulatorAccreditationTaskStatus_Lookup.RegulatorTask_TaskId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530121131_Accreditation_Updates'
)
BEGIN
    ALTER TABLE [Public.RegulatorAccreditationTaskStatus] DROP CONSTRAINT [FK_Public.RegulatorAccreditationTaskStatus_Lookup.TaskStatus_TaskStatusId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530121131_Accreditation_Updates'
)
BEGIN
    ALTER TABLE [Public.RegulatorAccreditationTaskStatus] DROP CONSTRAINT [FK_Public.RegulatorAccreditationTaskStatus_Public.Accreditation_AccreditationId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530121131_Accreditation_Updates'
)
BEGIN
    ALTER TABLE [Public.RegulatorApplicationTaskStatus] DROP CONSTRAINT [FK_Public.RegulatorApplicationTaskStatus_Lookup.RegulatorTask_TaskId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530121131_Accreditation_Updates'
)
BEGIN
    ALTER TABLE [Public.RegulatorApplicationTaskStatus] DROP CONSTRAINT [FK_Public.RegulatorApplicationTaskStatus_Lookup.TaskStatus_TaskStatusId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530121131_Accreditation_Updates'
)
BEGIN
    ALTER TABLE [Public.RegulatorRegistrationTaskStatus] DROP CONSTRAINT [FK_Public.RegulatorRegistrationTaskStatus_Lookup.RegulatorTask_TaskId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530121131_Accreditation_Updates'
)
BEGIN
    ALTER TABLE [Public.RegulatorRegistrationTaskStatus] DROP CONSTRAINT [FK_Public.RegulatorRegistrationTaskStatus_Lookup.TaskStatus_TaskStatusId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530121131_Accreditation_Updates'
)
BEGIN
    DROP INDEX [IX_Public.RegulatorRegistrationTaskStatus_TaskId] ON [Public.RegulatorRegistrationTaskStatus];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530121131_Accreditation_Updates'
)
BEGIN
    DROP INDEX [IX_Public.RegulatorRegistrationTaskStatus_TaskStatusId] ON [Public.RegulatorRegistrationTaskStatus];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530121131_Accreditation_Updates'
)
BEGIN
    DROP INDEX [IX_Public.RegulatorApplicationTaskStatus_TaskId] ON [Public.RegulatorApplicationTaskStatus];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530121131_Accreditation_Updates'
)
BEGIN
    DROP INDEX [IX_Public.RegulatorApplicationTaskStatus_TaskStatusId] ON [Public.RegulatorApplicationTaskStatus];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530121131_Accreditation_Updates'
)
BEGIN
    DROP INDEX [IX_Public.RegulatorAccreditationTaskStatus_TaskId] ON [Public.RegulatorAccreditationTaskStatus];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530121131_Accreditation_Updates'
)
BEGIN
    DROP INDEX [IX_Public.RegulatorAccreditationTaskStatus_TaskStatusId] ON [Public.RegulatorAccreditationTaskStatus];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530121131_Accreditation_Updates'
)
BEGIN
    DROP INDEX [IX_Public.RegulatorAccreditationRegistrationTaskStatus_TaskId] ON [Public.RegulatorAccreditationRegistrationTaskStatus];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530121131_Accreditation_Updates'
)
BEGIN
    DROP INDEX [IX_Public.RegulatorAccreditationRegistrationTaskStatus_TaskStatusId] ON [Public.RegulatorAccreditationRegistrationTaskStatus];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530121131_Accreditation_Updates'
)
BEGIN
    EXEC(N'DELETE FROM [Lookup.RegulatorTask]
    WHERE [Id] = 21;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530121131_Accreditation_Updates'
)
BEGIN
    DECLARE @var7 sysname;
    SELECT @var7 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Public.RegulatorRegistrationTaskStatus]') AND [c].[name] = N'TaskId');
    IF @var7 IS NOT NULL EXEC(N'ALTER TABLE [Public.RegulatorRegistrationTaskStatus] DROP CONSTRAINT [' + @var7 + '];');
    ALTER TABLE [Public.RegulatorRegistrationTaskStatus] DROP COLUMN [TaskId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530121131_Accreditation_Updates'
)
BEGIN
    DECLARE @var8 sysname;
    SELECT @var8 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Public.RegulatorRegistrationTaskStatus]') AND [c].[name] = N'TaskStatusId');
    IF @var8 IS NOT NULL EXEC(N'ALTER TABLE [Public.RegulatorRegistrationTaskStatus] DROP CONSTRAINT [' + @var8 + '];');
    ALTER TABLE [Public.RegulatorRegistrationTaskStatus] DROP COLUMN [TaskStatusId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530121131_Accreditation_Updates'
)
BEGIN
    DECLARE @var9 sysname;
    SELECT @var9 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Public.RegulatorApplicationTaskStatus]') AND [c].[name] = N'TaskId');
    IF @var9 IS NOT NULL EXEC(N'ALTER TABLE [Public.RegulatorApplicationTaskStatus] DROP CONSTRAINT [' + @var9 + '];');
    ALTER TABLE [Public.RegulatorApplicationTaskStatus] DROP COLUMN [TaskId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530121131_Accreditation_Updates'
)
BEGIN
    DECLARE @var10 sysname;
    SELECT @var10 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Public.RegulatorApplicationTaskStatus]') AND [c].[name] = N'TaskStatusId');
    IF @var10 IS NOT NULL EXEC(N'ALTER TABLE [Public.RegulatorApplicationTaskStatus] DROP CONSTRAINT [' + @var10 + '];');
    ALTER TABLE [Public.RegulatorApplicationTaskStatus] DROP COLUMN [TaskStatusId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530121131_Accreditation_Updates'
)
BEGIN
    DECLARE @var11 sysname;
    SELECT @var11 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Public.RegulatorAccreditationTaskStatus]') AND [c].[name] = N'Comments');
    IF @var11 IS NOT NULL EXEC(N'ALTER TABLE [Public.RegulatorAccreditationTaskStatus] DROP CONSTRAINT [' + @var11 + '];');
    ALTER TABLE [Public.RegulatorAccreditationTaskStatus] DROP COLUMN [Comments];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530121131_Accreditation_Updates'
)
BEGIN
    DECLARE @var12 sysname;
    SELECT @var12 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Public.RegulatorAccreditationTaskStatus]') AND [c].[name] = N'TaskId');
    IF @var12 IS NOT NULL EXEC(N'ALTER TABLE [Public.RegulatorAccreditationTaskStatus] DROP CONSTRAINT [' + @var12 + '];');
    ALTER TABLE [Public.RegulatorAccreditationTaskStatus] DROP COLUMN [TaskId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530121131_Accreditation_Updates'
)
BEGIN
    DECLARE @var13 sysname;
    SELECT @var13 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Public.RegulatorAccreditationTaskStatus]') AND [c].[name] = N'TaskStatusId');
    IF @var13 IS NOT NULL EXEC(N'ALTER TABLE [Public.RegulatorAccreditationTaskStatus] DROP CONSTRAINT [' + @var13 + '];');
    ALTER TABLE [Public.RegulatorAccreditationTaskStatus] DROP COLUMN [TaskStatusId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530121131_Accreditation_Updates'
)
BEGIN
    DECLARE @var14 sysname;
    SELECT @var14 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Public.RegulatorAccreditationRegistrationTaskStatus]') AND [c].[name] = N'Comments');
    IF @var14 IS NOT NULL EXEC(N'ALTER TABLE [Public.RegulatorAccreditationRegistrationTaskStatus] DROP CONSTRAINT [' + @var14 + '];');
    ALTER TABLE [Public.RegulatorAccreditationRegistrationTaskStatus] DROP COLUMN [Comments];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530121131_Accreditation_Updates'
)
BEGIN
    DECLARE @var15 sysname;
    SELECT @var15 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Public.RegulatorAccreditationRegistrationTaskStatus]') AND [c].[name] = N'TaskId');
    IF @var15 IS NOT NULL EXEC(N'ALTER TABLE [Public.RegulatorAccreditationRegistrationTaskStatus] DROP CONSTRAINT [' + @var15 + '];');
    ALTER TABLE [Public.RegulatorAccreditationRegistrationTaskStatus] DROP COLUMN [TaskId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530121131_Accreditation_Updates'
)
BEGIN
    DECLARE @var16 sysname;
    SELECT @var16 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Public.RegulatorAccreditationRegistrationTaskStatus]') AND [c].[name] = N'TaskStatusId');
    IF @var16 IS NOT NULL EXEC(N'ALTER TABLE [Public.RegulatorAccreditationRegistrationTaskStatus] DROP CONSTRAINT [' + @var16 + '];');
    ALTER TABLE [Public.RegulatorAccreditationRegistrationTaskStatus] DROP COLUMN [TaskStatusId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530121131_Accreditation_Updates'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ApplicationTypeId', N'IsMaterialSpecific', N'JourneyTypeId', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup.RegulatorTask]'))
        SET IDENTITY_INSERT [Lookup.RegulatorTask] ON;
    EXEC(N'INSERT INTO [Lookup.RegulatorTask] ([Id], [ApplicationTypeId], [IsMaterialSpecific], [JourneyTypeId], [Name])
    VALUES (27, 1, CAST(0 AS bit), 2, N''DulyMade''),
    (28, 2, CAST(0 AS bit), 2, N''DulyMade'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ApplicationTypeId', N'IsMaterialSpecific', N'JourneyTypeId', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup.RegulatorTask]'))
        SET IDENTITY_INSERT [Lookup.RegulatorTask] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530121131_Accreditation_Updates'
)
BEGIN
    ALTER TABLE [Public.RegulatorAccreditationTaskStatus] ADD CONSTRAINT [FK_Public.RegulatorAccreditationTaskStatus_Public.Accreditation_AccreditationId] FOREIGN KEY ([AccreditationId]) REFERENCES [Public.Accreditation] ([Id]) ON DELETE CASCADE;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530121131_Accreditation_Updates'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250530121131_Accreditation_Updates', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530121218_Accreditation_Updates_2'
)
BEGIN
    ALTER TABLE [Public.RegulatorRegistrationTaskStatus] ADD [RegulatorTaskId] int NOT NULL DEFAULT 0;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530121218_Accreditation_Updates_2'
)
BEGIN
    ALTER TABLE [Public.RegulatorRegistrationTaskStatus] ADD [TaskStatusId] int NOT NULL DEFAULT 0;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530121218_Accreditation_Updates_2'
)
BEGIN
    ALTER TABLE [Public.RegulatorApplicationTaskStatus] ADD [RegulatorTaskId] int NOT NULL DEFAULT 0;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530121218_Accreditation_Updates_2'
)
BEGIN
    ALTER TABLE [Public.RegulatorApplicationTaskStatus] ADD [TaskStatusId] int NOT NULL DEFAULT 0;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530121218_Accreditation_Updates_2'
)
BEGIN
    ALTER TABLE [Public.RegulatorAccreditationTaskStatus] ADD [RegulatorTaskId] int NOT NULL DEFAULT 0;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530121218_Accreditation_Updates_2'
)
BEGIN
    ALTER TABLE [Public.RegulatorAccreditationTaskStatus] ADD [TaskStatusId] int NOT NULL DEFAULT 0;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530121218_Accreditation_Updates_2'
)
BEGIN
    ALTER TABLE [Public.RegulatorAccreditationRegistrationTaskStatus] ADD [RegulatorTaskId] int NOT NULL DEFAULT 0;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530121218_Accreditation_Updates_2'
)
BEGIN
    ALTER TABLE [Public.RegulatorAccreditationRegistrationTaskStatus] ADD [TaskStatusId] int NOT NULL DEFAULT 0;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530121218_Accreditation_Updates_2'
)
BEGIN
    CREATE INDEX [IX_Public.RegulatorRegistrationTaskStatus_RegulatorTaskId] ON [Public.RegulatorRegistrationTaskStatus] ([RegulatorTaskId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530121218_Accreditation_Updates_2'
)
BEGIN
    CREATE INDEX [IX_Public.RegulatorRegistrationTaskStatus_TaskStatusId] ON [Public.RegulatorRegistrationTaskStatus] ([TaskStatusId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530121218_Accreditation_Updates_2'
)
BEGIN
    CREATE INDEX [IX_Public.RegulatorApplicationTaskStatus_RegulatorTaskId] ON [Public.RegulatorApplicationTaskStatus] ([RegulatorTaskId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530121218_Accreditation_Updates_2'
)
BEGIN
    CREATE INDEX [IX_Public.RegulatorApplicationTaskStatus_TaskStatusId] ON [Public.RegulatorApplicationTaskStatus] ([TaskStatusId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530121218_Accreditation_Updates_2'
)
BEGIN
    CREATE INDEX [IX_Public.RegulatorAccreditationTaskStatus_RegulatorTaskId] ON [Public.RegulatorAccreditationTaskStatus] ([RegulatorTaskId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530121218_Accreditation_Updates_2'
)
BEGIN
    CREATE INDEX [IX_Public.RegulatorAccreditationTaskStatus_TaskStatusId] ON [Public.RegulatorAccreditationTaskStatus] ([TaskStatusId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530121218_Accreditation_Updates_2'
)
BEGIN
    CREATE INDEX [IX_Public.RegulatorAccreditationRegistrationTaskStatus_RegulatorTaskId] ON [Public.RegulatorAccreditationRegistrationTaskStatus] ([RegulatorTaskId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530121218_Accreditation_Updates_2'
)
BEGIN
    CREATE INDEX [IX_Public.RegulatorAccreditationRegistrationTaskStatus_TaskStatusId] ON [Public.RegulatorAccreditationRegistrationTaskStatus] ([TaskStatusId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530121218_Accreditation_Updates_2'
)
BEGIN
    ALTER TABLE [Public.RegulatorAccreditationRegistrationTaskStatus] ADD CONSTRAINT [FK_Public.RegulatorAccreditationRegistrationTaskStatus_Lookup.RegulatorTask_RegulatorTaskId] FOREIGN KEY ([RegulatorTaskId]) REFERENCES [Lookup.RegulatorTask] ([Id]) ON DELETE CASCADE;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530121218_Accreditation_Updates_2'
)
BEGIN
    ALTER TABLE [Public.RegulatorAccreditationRegistrationTaskStatus] ADD CONSTRAINT [FK_Public.RegulatorAccreditationRegistrationTaskStatus_Lookup.TaskStatus_TaskStatusId] FOREIGN KEY ([TaskStatusId]) REFERENCES [Lookup.TaskStatus] ([Id]) ON DELETE CASCADE;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530121218_Accreditation_Updates_2'
)
BEGIN
    ALTER TABLE [Public.RegulatorAccreditationTaskStatus] ADD CONSTRAINT [FK_Public.RegulatorAccreditationTaskStatus_Lookup.RegulatorTask_RegulatorTaskId] FOREIGN KEY ([RegulatorTaskId]) REFERENCES [Lookup.RegulatorTask] ([Id]) ON DELETE CASCADE;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530121218_Accreditation_Updates_2'
)
BEGIN
    ALTER TABLE [Public.RegulatorAccreditationTaskStatus] ADD CONSTRAINT [FK_Public.RegulatorAccreditationTaskStatus_Lookup.TaskStatus_TaskStatusId] FOREIGN KEY ([TaskStatusId]) REFERENCES [Lookup.TaskStatus] ([Id]) ON DELETE CASCADE;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530121218_Accreditation_Updates_2'
)
BEGIN
    ALTER TABLE [Public.RegulatorApplicationTaskStatus] ADD CONSTRAINT [FK_Public.RegulatorApplicationTaskStatus_Lookup.RegulatorTask_RegulatorTaskId] FOREIGN KEY ([RegulatorTaskId]) REFERENCES [Lookup.RegulatorTask] ([Id]) ON DELETE CASCADE;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530121218_Accreditation_Updates_2'
)
BEGIN
    ALTER TABLE [Public.RegulatorApplicationTaskStatus] ADD CONSTRAINT [FK_Public.RegulatorApplicationTaskStatus_Lookup.TaskStatus_TaskStatusId] FOREIGN KEY ([TaskStatusId]) REFERENCES [Lookup.TaskStatus] ([Id]) ON DELETE CASCADE;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530121218_Accreditation_Updates_2'
)
BEGIN
    ALTER TABLE [Public.RegulatorRegistrationTaskStatus] ADD CONSTRAINT [FK_Public.RegulatorRegistrationTaskStatus_Lookup.RegulatorTask_RegulatorTaskId] FOREIGN KEY ([RegulatorTaskId]) REFERENCES [Lookup.RegulatorTask] ([Id]) ON DELETE CASCADE;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530121218_Accreditation_Updates_2'
)
BEGIN
    ALTER TABLE [Public.RegulatorRegistrationTaskStatus] ADD CONSTRAINT [FK_Public.RegulatorRegistrationTaskStatus_Lookup.TaskStatus_TaskStatusId] FOREIGN KEY ([TaskStatusId]) REFERENCES [Lookup.TaskStatus] ([Id]) ON DELETE CASCADE;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530121218_Accreditation_Updates_2'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250530121218_Accreditation_Updates_2', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530134136_Set-DulyMade-Lookup-IsMaterialSpecific'
)
BEGIN
    EXEC(N'UPDATE [Lookup.RegulatorTask] SET [IsMaterialSpecific] = CAST(1 AS bit)
    WHERE [Id] = 27;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530134136_Set-DulyMade-Lookup-IsMaterialSpecific'
)
BEGIN
    EXEC(N'UPDATE [Lookup.RegulatorTask] SET [IsMaterialSpecific] = CAST(1 AS bit)
    WHERE [Id] = 28;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530134136_Set-DulyMade-Lookup-IsMaterialSpecific'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250530134136_Set-DulyMade-Lookup-IsMaterialSpecific', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250602134517_CheckRegistrationStatus'
)
BEGIN
    DROP INDEX [IX_Public.DulyMade_RegistrationMaterialId] ON [Public.DulyMade];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250602134517_CheckRegistrationStatus'
)
BEGIN
    DECLARE @var17 sysname;
    SELECT @var17 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Public.DulyMade]') AND [c].[name] = N'DeterminationDate');
    IF @var17 IS NOT NULL EXEC(N'ALTER TABLE [Public.DulyMade] DROP CONSTRAINT [' + @var17 + '];');
    ALTER TABLE [Public.DulyMade] DROP COLUMN [DeterminationDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250602134517_CheckRegistrationStatus'
)
BEGIN
    DECLARE @var18 sysname;
    SELECT @var18 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[public.FileUpload]') AND [c].[name] = N'UpdatedBy');
    IF @var18 IS NOT NULL EXEC(N'ALTER TABLE [public.FileUpload] DROP CONSTRAINT [' + @var18 + '];');
    ALTER TABLE [public.FileUpload] ALTER COLUMN [UpdatedBy] nvarchar(50) NOT NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250602134517_CheckRegistrationStatus'
)
BEGIN
    CREATE TABLE [Public.DeterminationDate] (
        [Id] int NOT NULL IDENTITY,
        [ExternalId] uniqueidentifier NOT NULL,
        [RegistrationMaterialId] int NOT NULL,
        [DeterminateDate] datetime2 NULL,
        CONSTRAINT [PK_Public.DeterminationDate] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Public.DeterminationDate_Public.RegistrationMaterial_RegistrationMaterialId] FOREIGN KEY ([RegistrationMaterialId]) REFERENCES [Public.RegistrationMaterial] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250602134517_CheckRegistrationStatus'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Public.DulyMade_RegistrationMaterialId] ON [Public.DulyMade] ([RegistrationMaterialId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250602134517_CheckRegistrationStatus'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Public.DeterminationDate_RegistrationMaterialId] ON [Public.DeterminationDate] ([RegistrationMaterialId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250602134517_CheckRegistrationStatus'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250602134517_CheckRegistrationStatus', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250603090928_AddNoteTable'
)
BEGIN
    DECLARE @var19 sysname;
    SELECT @var19 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Public.RegulatorRegistrationTaskStatus]') AND [c].[name] = N'Comments');
    IF @var19 IS NOT NULL EXEC(N'ALTER TABLE [Public.RegulatorRegistrationTaskStatus] DROP CONSTRAINT [' + @var19 + '];');
    ALTER TABLE [Public.RegulatorRegistrationTaskStatus] DROP COLUMN [Comments];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250603090928_AddNoteTable'
)
BEGIN
    DECLARE @var20 sysname;
    SELECT @var20 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Public.RegulatorApplicationTaskStatus]') AND [c].[name] = N'Comments');
    IF @var20 IS NOT NULL EXEC(N'ALTER TABLE [Public.RegulatorApplicationTaskStatus] DROP CONSTRAINT [' + @var20 + '];');
    ALTER TABLE [Public.RegulatorApplicationTaskStatus] DROP COLUMN [Comments];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250603090928_AddNoteTable'
)
BEGIN
    CREATE TABLE [Public.Note] (
        [Id] int NOT NULL IDENTITY,
        [Notes] nvarchar(500) NOT NULL,
        [CreatedBy] uniqueidentifier NOT NULL,
        [CreatedDate] datetime2 NOT NULL,
        CONSTRAINT [PK_Public.Note] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250603090928_AddNoteTable'
)
BEGIN
    CREATE TABLE [Public.ApplicationTaskStatusQueryNote] (
        [Id] int NOT NULL IDENTITY,
        [QueryNoteId] int NOT NULL,
        [RegulatorApplicationTaskStatusId] int NOT NULL,
        CONSTRAINT [PK_Public.ApplicationTaskStatusQueryNote] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Public.ApplicationTaskStatusQueryNote_Public.Note_QueryNoteId] FOREIGN KEY ([QueryNoteId]) REFERENCES [Public.Note] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_Public.ApplicationTaskStatusQueryNote_Public.RegulatorApplicationTaskStatus_RegulatorApplicationTaskStatusId] FOREIGN KEY ([RegulatorApplicationTaskStatusId]) REFERENCES [Public.RegulatorApplicationTaskStatus] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250603090928_AddNoteTable'
)
BEGIN
    CREATE TABLE [Public.RegistrationTaskStatusQueryNote] (
        [Id] int NOT NULL IDENTITY,
        [QueryNoteId] int NOT NULL,
        [RegulatorRegistrationTaskStatusId] int NOT NULL,
        [RegistrationTaskStatusId] int NOT NULL,
        CONSTRAINT [PK_Public.RegistrationTaskStatusQueryNote] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Public.RegistrationTaskStatusQueryNote_Public.Note_QueryNoteId] FOREIGN KEY ([QueryNoteId]) REFERENCES [Public.Note] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_Public.RegistrationTaskStatusQueryNote_Public.RegulatorRegistrationTaskStatus_RegulatorRegistrationTaskStatusId] FOREIGN KEY ([RegulatorRegistrationTaskStatusId]) REFERENCES [Public.RegulatorRegistrationTaskStatus] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250603090928_AddNoteTable'
)
BEGIN
    CREATE INDEX [IX_Public.ApplicationTaskStatusQueryNote_QueryNoteId] ON [Public.ApplicationTaskStatusQueryNote] ([QueryNoteId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250603090928_AddNoteTable'
)
BEGIN
    CREATE INDEX [IX_Public.ApplicationTaskStatusQueryNote_RegulatorApplicationTaskStatusId] ON [Public.ApplicationTaskStatusQueryNote] ([RegulatorApplicationTaskStatusId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250603090928_AddNoteTable'
)
BEGIN
    CREATE INDEX [IX_Public.RegistrationTaskStatusQueryNote_QueryNoteId] ON [Public.RegistrationTaskStatusQueryNote] ([QueryNoteId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250603090928_AddNoteTable'
)
BEGIN
    CREATE INDEX [IX_Public.RegistrationTaskStatusQueryNote_RegulatorRegistrationTaskStatusId] ON [Public.RegistrationTaskStatusQueryNote] ([RegulatorRegistrationTaskStatusId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250603090928_AddNoteTable'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250603090928_AddNoteTable', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250603161139_RemoveRegistrationTaskStatusId'
)
BEGIN
    DECLARE @var21 sysname;
    SELECT @var21 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Public.RegistrationTaskStatusQueryNote]') AND [c].[name] = N'RegistrationTaskStatusId');
    IF @var21 IS NOT NULL EXEC(N'ALTER TABLE [Public.RegistrationTaskStatusQueryNote] DROP CONSTRAINT [' + @var21 + '];');
    ALTER TABLE [Public.RegistrationTaskStatusQueryNote] DROP COLUMN [RegistrationTaskStatusId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250603161139_RemoveRegistrationTaskStatusId'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250603161139_RemoveRegistrationTaskStatusId', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250604103449_AccreditationTaskStatusQueryNote'
)
BEGIN
    ALTER TABLE [Public.Accreditation] ADD [CreatedOn] datetime2 NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250604103449_AccreditationTaskStatusQueryNote'
)
BEGIN
    ALTER TABLE [Public.Accreditation] ADD [PRNTonnage] int NOT NULL DEFAULT 0;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250604103449_AccreditationTaskStatusQueryNote'
)
BEGIN
    CREATE TABLE [Public.AccreditationDulyMade] (
        [Id] int NOT NULL IDENTITY,
        [ExternalId] uniqueidentifier NOT NULL,
        [AccreditationId] int NOT NULL,
        [TaskStatusId] int NOT NULL,
        [DulyMadeDate] datetime2 NULL,
        [DulyMadeBy] uniqueidentifier NULL,
        [DeterminationDate] datetime2 NULL,
        [DulyMadeNote] nvarchar(500) NULL,
        [DeterminationNote] nvarchar(500) NULL,
        [DeterminationUpdatedBy] uniqueidentifier NULL,
        [DeterminationUpdatedDate] datetime2 NULL,
        CONSTRAINT [PK_Public.AccreditationDulyMade] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Public.AccreditationDulyMade_Lookup.TaskStatus_TaskStatusId] FOREIGN KEY ([TaskStatusId]) REFERENCES [Lookup.TaskStatus] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250604103449_AccreditationTaskStatusQueryNote'
)
BEGIN
    CREATE TABLE [Public.AccreditationTaskStatusQueryNote] (
        [Id] int NOT NULL IDENTITY,
        [QueryNoteId] int NOT NULL,
        [RegulatorAccreditationTaskStatusId] int NOT NULL,
        CONSTRAINT [PK_Public.AccreditationTaskStatusQueryNote] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Public.AccreditationTaskStatusQueryNote_Public.Note_QueryNoteId] FOREIGN KEY ([QueryNoteId]) REFERENCES [Public.Note] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_Public.AccreditationTaskStatusQueryNote_Public.RegulatorApplicationTaskStatus_RegulatorAccreditationTaskStatusId] FOREIGN KEY ([RegulatorAccreditationTaskStatusId]) REFERENCES [Public.RegulatorApplicationTaskStatus] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250604103449_AccreditationTaskStatusQueryNote'
)
BEGIN
    CREATE INDEX [IX_Public.AccreditationDulyMade_TaskStatusId] ON [Public.AccreditationDulyMade] ([TaskStatusId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250604103449_AccreditationTaskStatusQueryNote'
)
BEGIN
    CREATE INDEX [IX_Public.AccreditationTaskStatusQueryNote_QueryNoteId] ON [Public.AccreditationTaskStatusQueryNote] ([QueryNoteId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250604103449_AccreditationTaskStatusQueryNote'
)
BEGIN
    CREATE INDEX [IX_Public.AccreditationTaskStatusQueryNote_RegulatorAccreditationTaskStatusId] ON [Public.AccreditationTaskStatusQueryNote] ([RegulatorAccreditationTaskStatusId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250604103449_AccreditationTaskStatusQueryNote'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250604103449_AccreditationTaskStatusQueryNote', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250604113009_AccreditationTaskStatusQueryNote2'
)
BEGIN
    ALTER TABLE [Public.AccreditationTaskStatusQueryNote] DROP CONSTRAINT [FK_Public.AccreditationTaskStatusQueryNote_Public.RegulatorApplicationTaskStatus_RegulatorAccreditationTaskStatusId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250604113009_AccreditationTaskStatusQueryNote2'
)
BEGIN
    ALTER TABLE [Public.AccreditationTaskStatusQueryNote] ADD CONSTRAINT [FK_Public.AccreditationTaskStatusQueryNote_Public.RegulatorAccreditationTaskStatus_RegulatorAccreditationTaskStatusId] FOREIGN KEY ([RegulatorAccreditationTaskStatusId]) REFERENCES [Public.RegulatorAccreditationTaskStatus] ([Id]) ON DELETE CASCADE;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250604113009_AccreditationTaskStatusQueryNote2'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250604113009_AccreditationTaskStatusQueryNote2', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250605124846_RemoveColumnsFromAccreditationDulyMade'
)
BEGIN
    DECLARE @var22 sysname;
    SELECT @var22 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Public.AccreditationDulyMade]') AND [c].[name] = N'DeterminationDate');
    IF @var22 IS NOT NULL EXEC(N'ALTER TABLE [Public.AccreditationDulyMade] DROP CONSTRAINT [' + @var22 + '];');
    ALTER TABLE [Public.AccreditationDulyMade] DROP COLUMN [DeterminationDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250605124846_RemoveColumnsFromAccreditationDulyMade'
)
BEGIN
    DECLARE @var23 sysname;
    SELECT @var23 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Public.AccreditationDulyMade]') AND [c].[name] = N'DeterminationNote');
    IF @var23 IS NOT NULL EXEC(N'ALTER TABLE [Public.AccreditationDulyMade] DROP CONSTRAINT [' + @var23 + '];');
    ALTER TABLE [Public.AccreditationDulyMade] DROP COLUMN [DeterminationNote];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250605124846_RemoveColumnsFromAccreditationDulyMade'
)
BEGIN
    DECLARE @var24 sysname;
    SELECT @var24 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Public.AccreditationDulyMade]') AND [c].[name] = N'DeterminationUpdatedBy');
    IF @var24 IS NOT NULL EXEC(N'ALTER TABLE [Public.AccreditationDulyMade] DROP CONSTRAINT [' + @var24 + '];');
    ALTER TABLE [Public.AccreditationDulyMade] DROP COLUMN [DeterminationUpdatedBy];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250605124846_RemoveColumnsFromAccreditationDulyMade'
)
BEGIN
    DECLARE @var25 sysname;
    SELECT @var25 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Public.AccreditationDulyMade]') AND [c].[name] = N'DeterminationUpdatedDate');
    IF @var25 IS NOT NULL EXEC(N'ALTER TABLE [Public.AccreditationDulyMade] DROP CONSTRAINT [' + @var25 + '];');
    ALTER TABLE [Public.AccreditationDulyMade] DROP COLUMN [DeterminationUpdatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250605124846_RemoveColumnsFromAccreditationDulyMade'
)
BEGIN
    DECLARE @var26 sysname;
    SELECT @var26 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Public.AccreditationDulyMade]') AND [c].[name] = N'DulyMadeNote');
    IF @var26 IS NOT NULL EXEC(N'ALTER TABLE [Public.AccreditationDulyMade] DROP CONSTRAINT [' + @var26 + '];');
    ALTER TABLE [Public.AccreditationDulyMade] DROP COLUMN [DulyMadeNote];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250605124846_RemoveColumnsFromAccreditationDulyMade'
)
BEGIN
    ALTER TABLE [Public.AccreditationDulyMade] ADD [DeterminationDateId] int NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250605124846_RemoveColumnsFromAccreditationDulyMade'
)
BEGIN
    CREATE INDEX [IX_Public.AccreditationDulyMade_DeterminationDateId] ON [Public.AccreditationDulyMade] ([DeterminationDateId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250605124846_RemoveColumnsFromAccreditationDulyMade'
)
BEGIN
    ALTER TABLE [Public.AccreditationDulyMade] ADD CONSTRAINT [FK_Public.AccreditationDulyMade_Public.AccreditationDeterminationDate_DeterminationDateId] FOREIGN KEY ([DeterminationDateId]) REFERENCES [Public.AccreditationDeterminationDate] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250605124846_RemoveColumnsFromAccreditationDulyMade'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250605124846_RemoveColumnsFromAccreditationDulyMade', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250610104645_AddCarrierBrokerDealerPermitsTable'
)
BEGIN
    CREATE TABLE [Public.CarrierBrokerDealerPermits] (
        [Id] int NOT NULL IDENTITY,
        [ExternalId] uniqueidentifier NOT NULL,
        [RegistrationId] int NOT NULL,
        [WasteCarrierBrokerDealerRegistration] nvarchar(max) NULL,
        [WasteManagementEnvironmentPermitNumber] nvarchar(max) NULL,
        [InstatallationPermitOrPPCNumber] nvarchar(max) NULL,
        [RegisteredWasteCarrierBrokerDealerFlag] bit NOT NULL,
        [CreatedBy] uniqueidentifier NOT NULL,
        [CreatedDate] datetime2 NOT NULL,
        [UpdatedBy] uniqueidentifier NULL,
        [UpdatedDate] datetime2 NULL,
        CONSTRAINT [PK_Public.CarrierBrokerDealerPermits] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Public.CarrierBrokerDealerPermits_Public.Registration_RegistrationId] FOREIGN KEY ([RegistrationId]) REFERENCES [Public.Registration] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250610104645_AddCarrierBrokerDealerPermitsTable'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ApplicationTypeId', N'IsMaterialSpecific', N'JourneyTypeId', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup.RegulatorTask]'))
        SET IDENTITY_INSERT [Lookup.RegulatorTask] ON;
    EXEC(N'INSERT INTO [Lookup.RegulatorTask] ([Id], [ApplicationTypeId], [IsMaterialSpecific], [JourneyTypeId], [Name])
    VALUES (29, 1, CAST(0 AS bit), 1, N''WasteCarrierBrokerDealerNumber'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ApplicationTypeId', N'IsMaterialSpecific', N'JourneyTypeId', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup.RegulatorTask]'))
        SET IDENTITY_INSERT [Lookup.RegulatorTask] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250610104645_AddCarrierBrokerDealerPermitsTable'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Public.CarrierBrokerDealerPermits_ExternalId] ON [Public.CarrierBrokerDealerPermits] ([ExternalId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250610104645_AddCarrierBrokerDealerPermitsTable'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Public.CarrierBrokerDealerPermits_RegistrationId] ON [Public.CarrierBrokerDealerPermits] ([RegistrationId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250610104645_AddCarrierBrokerDealerPermitsTable'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250610104645_AddCarrierBrokerDealerPermitsTable', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250610165559_ModifyObligationCalculationsTableToAddNewCoulmnAndRelatedTable'
)
BEGIN
    DELETE FROM ObligationCalculations
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250610165559_ModifyObligationCalculationsTableToAddNewCoulmnAndRelatedTable'
)
BEGIN
    ALTER TABLE [ObligationCalculations] ADD [SubmitterId] uniqueidentifier NOT NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250610165559_ModifyObligationCalculationsTableToAddNewCoulmnAndRelatedTable'
)
BEGIN
    ALTER TABLE [ObligationCalculations] ADD [SubmitterTypeId] int NOT NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250610165559_ModifyObligationCalculationsTableToAddNewCoulmnAndRelatedTable'
)
BEGIN
    CREATE TABLE [ObligationCalculationOrganisationSubmitterType] (
        [Id] int NOT NULL IDENTITY,
        [TypeName] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_ObligationCalculationOrganisationSubmitterType] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250610165559_ModifyObligationCalculationsTableToAddNewCoulmnAndRelatedTable'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'TypeName') AND [object_id] = OBJECT_ID(N'[ObligationCalculationOrganisationSubmitterType]'))
        SET IDENTITY_INSERT [ObligationCalculationOrganisationSubmitterType] ON;
    EXEC(N'INSERT INTO [ObligationCalculationOrganisationSubmitterType] ([Id], [TypeName])
    VALUES (1, N''ComplianceScheme''),
    (2, N''DirectRegistrant'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'TypeName') AND [object_id] = OBJECT_ID(N'[ObligationCalculationOrganisationSubmitterType]'))
        SET IDENTITY_INSERT [ObligationCalculationOrganisationSubmitterType] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250610165559_ModifyObligationCalculationsTableToAddNewCoulmnAndRelatedTable'
)
BEGIN
    CREATE INDEX [IX_ObligationCalculations_SubmitterTypeId] ON [ObligationCalculations] ([SubmitterTypeId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250610165559_ModifyObligationCalculationsTableToAddNewCoulmnAndRelatedTable'
)
BEGIN
    CREATE UNIQUE INDEX [IX_ObligationCalculationOrganisationSubmitterType_TypeName] ON [ObligationCalculationOrganisationSubmitterType] ([TypeName]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250610165559_ModifyObligationCalculationsTableToAddNewCoulmnAndRelatedTable'
)
BEGIN
    ALTER TABLE [ObligationCalculations] ADD CONSTRAINT [FK_ObligationCalculations_ObligationCalculationOrganisationSubmitterType_SubmitterTypeId] FOREIGN KEY ([SubmitterTypeId]) REFERENCES [ObligationCalculationOrganisationSubmitterType] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250610165559_ModifyObligationCalculationsTableToAddNewCoulmnAndRelatedTable'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250610165559_ModifyObligationCalculationsTableToAddNewCoulmnAndRelatedTable', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250611140921_AddressFieldsNullable'
)
BEGIN
    DECLARE @var27 sysname;
    SELECT @var27 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Public.Address]') AND [c].[name] = N'GridReference');
    IF @var27 IS NOT NULL EXEC(N'ALTER TABLE [Public.Address] DROP CONSTRAINT [' + @var27 + '];');
    ALTER TABLE [Public.Address] ALTER COLUMN [GridReference] nvarchar(20) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250611140921_AddressFieldsNullable'
)
BEGIN
    DECLARE @var28 sysname;
    SELECT @var28 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Public.Address]') AND [c].[name] = N'AddressLine2');
    IF @var28 IS NOT NULL EXEC(N'ALTER TABLE [Public.Address] DROP CONSTRAINT [' + @var28 + '];');
    ALTER TABLE [Public.Address] ALTER COLUMN [AddressLine2] nvarchar(200) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250611140921_AddressFieldsNullable'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250611140921_AddressFieldsNullable', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250612110818_RemoveunusedDulyMadeFields'
)
BEGIN
    ALTER TABLE [Public.DulyMade] DROP CONSTRAINT [FK_Public.DulyMade_Lookup.TaskStatus_TaskStatusId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250612110818_RemoveunusedDulyMadeFields'
)
BEGIN
    DROP INDEX [IX_Public.DulyMade_TaskStatusId] ON [Public.DulyMade];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250612110818_RemoveunusedDulyMadeFields'
)
BEGIN
    DECLARE @var29 sysname;
    SELECT @var29 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Public.DulyMade]') AND [c].[name] = N'DeterminationNote');
    IF @var29 IS NOT NULL EXEC(N'ALTER TABLE [Public.DulyMade] DROP CONSTRAINT [' + @var29 + '];');
    ALTER TABLE [Public.DulyMade] DROP COLUMN [DeterminationNote];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250612110818_RemoveunusedDulyMadeFields'
)
BEGIN
    DECLARE @var30 sysname;
    SELECT @var30 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Public.DulyMade]') AND [c].[name] = N'DeterminationUpdatedBy');
    IF @var30 IS NOT NULL EXEC(N'ALTER TABLE [Public.DulyMade] DROP CONSTRAINT [' + @var30 + '];');
    ALTER TABLE [Public.DulyMade] DROP COLUMN [DeterminationUpdatedBy];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250612110818_RemoveunusedDulyMadeFields'
)
BEGIN
    DECLARE @var31 sysname;
    SELECT @var31 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Public.DulyMade]') AND [c].[name] = N'DeterminationUpdatedDate');
    IF @var31 IS NOT NULL EXEC(N'ALTER TABLE [Public.DulyMade] DROP CONSTRAINT [' + @var31 + '];');
    ALTER TABLE [Public.DulyMade] DROP COLUMN [DeterminationUpdatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250612110818_RemoveunusedDulyMadeFields'
)
BEGIN
    DECLARE @var32 sysname;
    SELECT @var32 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Public.DulyMade]') AND [c].[name] = N'DulyMadeNote');
    IF @var32 IS NOT NULL EXEC(N'ALTER TABLE [Public.DulyMade] DROP CONSTRAINT [' + @var32 + '];');
    ALTER TABLE [Public.DulyMade] DROP COLUMN [DulyMadeNote];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250612110818_RemoveunusedDulyMadeFields'
)
BEGIN
    DECLARE @var33 sysname;
    SELECT @var33 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Public.DulyMade]') AND [c].[name] = N'TaskStatusId');
    IF @var33 IS NOT NULL EXEC(N'ALTER TABLE [Public.DulyMade] DROP CONSTRAINT [' + @var33 + '];');
    ALTER TABLE [Public.DulyMade] DROP COLUMN [TaskStatusId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250612110818_RemoveunusedDulyMadeFields'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250612110818_RemoveunusedDulyMadeFields', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250612190957_Accreditation-Exporter-And-Duly-Made-Cleanup'
)
BEGIN
    ALTER TABLE [Public.AccreditationDulyMade] DROP CONSTRAINT [FK_Public.AccreditationDulyMade_Lookup.TaskStatus_TaskStatusId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250612190957_Accreditation-Exporter-And-Duly-Made-Cleanup'
)
BEGIN
    ALTER TABLE [Public.AccreditationDulyMade] DROP CONSTRAINT [FK_Public.AccreditationDulyMade_Public.AccreditationDeterminationDate_DeterminationDateId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250612190957_Accreditation-Exporter-And-Duly-Made-Cleanup'
)
BEGIN
    DROP INDEX [IX_Public.AccreditationDulyMade_DeterminationDateId] ON [Public.AccreditationDulyMade];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250612190957_Accreditation-Exporter-And-Duly-Made-Cleanup'
)
BEGIN
    DROP INDEX [IX_Public.AccreditationDulyMade_TaskStatusId] ON [Public.AccreditationDulyMade];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250612190957_Accreditation-Exporter-And-Duly-Made-Cleanup'
)
BEGIN
    DECLARE @var34 sysname;
    SELECT @var34 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Public.AccreditationDulyMade]') AND [c].[name] = N'DeterminationDateId');
    IF @var34 IS NOT NULL EXEC(N'ALTER TABLE [Public.AccreditationDulyMade] DROP CONSTRAINT [' + @var34 + '];');
    ALTER TABLE [Public.AccreditationDulyMade] DROP COLUMN [DeterminationDateId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250612190957_Accreditation-Exporter-And-Duly-Made-Cleanup'
)
BEGIN
    DECLARE @var35 sysname;
    SELECT @var35 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Public.AccreditationDulyMade]') AND [c].[name] = N'TaskStatusId');
    IF @var35 IS NOT NULL EXEC(N'ALTER TABLE [Public.AccreditationDulyMade] DROP CONSTRAINT [' + @var35 + '];');
    ALTER TABLE [Public.AccreditationDulyMade] DROP COLUMN [TaskStatusId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250612190957_Accreditation-Exporter-And-Duly-Made-Cleanup'
)
BEGIN
    EXEC(N'UPDATE [Lookup.RegulatorTask] SET [Name] = N''PRNsTonnageAndAuthorityToIssuePRNs''
    WHERE [Id] = 18;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250612190957_Accreditation-Exporter-And-Duly-Made-Cleanup'
)
BEGIN
    EXEC(N'UPDATE [Lookup.RegulatorTask] SET [Name] = N''BusinessPlan''
    WHERE [Id] = 19;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250612190957_Accreditation-Exporter-And-Duly-Made-Cleanup'
)
BEGIN
    EXEC(N'UPDATE [Lookup.RegulatorTask] SET [Name] = N''AccreditationSamplingAndInspectionPlan''
    WHERE [Id] = 20;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250612190957_Accreditation-Exporter-And-Duly-Made-Cleanup'
)
BEGIN
    EXEC(N'UPDATE [Lookup.RegulatorTask] SET [Name] = N''PERNsTonnageAndAuthorityToIssuePERNs''
    WHERE [Id] = 23;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250612190957_Accreditation-Exporter-And-Duly-Made-Cleanup'
)
BEGIN
    EXEC(N'UPDATE [Lookup.RegulatorTask] SET [Name] = N''BusinessPlan''
    WHERE [Id] = 24;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250612190957_Accreditation-Exporter-And-Duly-Made-Cleanup'
)
BEGIN
    EXEC(N'UPDATE [Lookup.RegulatorTask] SET [Name] = N''AccreditationSamplingAndInspectionPlan''
    WHERE [Id] = 25;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250612190957_Accreditation-Exporter-And-Duly-Made-Cleanup'
)
BEGIN
    EXEC(N'UPDATE [Lookup.RegulatorTask] SET [Name] = N''OverseasReprocessingSitesAndEvidenceOfBroadlyEquivalentStandards''
    WHERE [Id] = 26;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250612190957_Accreditation-Exporter-And-Duly-Made-Cleanup'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Public.RegulatorRegistrationTaskStatus_ExternalId] ON [Public.RegulatorRegistrationTaskStatus] ([ExternalId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250612190957_Accreditation-Exporter-And-Duly-Made-Cleanup'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Public.RegulatorApplicationTaskStatus_ExternalId] ON [Public.RegulatorApplicationTaskStatus] ([ExternalId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250612190957_Accreditation-Exporter-And-Duly-Made-Cleanup'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Public.RegulatorAccreditationTaskStatus_ExternalId] ON [Public.RegulatorAccreditationTaskStatus] ([ExternalId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250612190957_Accreditation-Exporter-And-Duly-Made-Cleanup'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Public.RegulatorAccreditationRegistrationTaskStatus_ExternalId] ON [Public.RegulatorAccreditationRegistrationTaskStatus] ([ExternalId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250612190957_Accreditation-Exporter-And-Duly-Made-Cleanup'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Public.RegistrationTaskStatus_ExternalId] ON [Public.RegistrationTaskStatus] ([ExternalId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250612190957_Accreditation-Exporter-And-Duly-Made-Cleanup'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Public.RegistrationReprocessingIO_ExternalId] ON [Public.RegistrationReprocessingIO] ([ExternalId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250612190957_Accreditation-Exporter-And-Duly-Made-Cleanup'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Public.RegistrationMaterial_ExternalId] ON [Public.RegistrationMaterial] ([ExternalId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250612190957_Accreditation-Exporter-And-Duly-Made-Cleanup'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Public.Registration_ExternalId] ON [Public.Registration] ([ExternalId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250612190957_Accreditation-Exporter-And-Duly-Made-Cleanup'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Public.MaterialExemptionReference_ExternalId] ON [Public.MaterialExemptionReference] ([ExternalId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250612190957_Accreditation-Exporter-And-Duly-Made-Cleanup'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Public.DeterminationDate_ExternalId] ON [Public.DeterminationDate] ([ExternalId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250612190957_Accreditation-Exporter-And-Duly-Made-Cleanup'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Public.AccreditationDulyMade_ExternalId] ON [Public.AccreditationDulyMade] ([ExternalId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250612190957_Accreditation-Exporter-And-Duly-Made-Cleanup'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Public.AccreditationDeterminationDate_ExternalId] ON [Public.AccreditationDeterminationDate] ([ExternalId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250612190957_Accreditation-Exporter-And-Duly-Made-Cleanup'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Public.Accreditation_ExternalId] ON [Public.Accreditation] ([ExternalId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250612190957_Accreditation-Exporter-And-Duly-Made-Cleanup'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250612190957_Accreditation-Exporter-And-Duly-Made-Cleanup', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250612221733_AddIsDelectedColoumnToObligationCalculations'
)
BEGIN
    ALTER TABLE [ObligationCalculations] ADD [IsDeleted] bit NOT NULL DEFAULT CAST(0 AS bit);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250612221733_AddIsDelectedColoumnToObligationCalculations'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250612221733_AddIsDelectedColoumnToObligationCalculations', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250615135732_UpdateAccreditationBusinessPlan'
)
BEGIN
    ALTER TABLE [Public.Accreditation] ADD [BusinessCollectionsNotes] varchar(500) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250615135732_UpdateAccreditationBusinessPlan'
)
BEGIN
    ALTER TABLE [Public.Accreditation] ADD [BusinessCollectionsPercentage] decimal(10,2) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250615135732_UpdateAccreditationBusinessPlan'
)
BEGIN
    ALTER TABLE [Public.Accreditation] ADD [CommunicationsNotes] varchar(500) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250615135732_UpdateAccreditationBusinessPlan'
)
BEGIN
    ALTER TABLE [Public.Accreditation] ADD [CommunicationsPercentage] decimal(10,2) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250615135732_UpdateAccreditationBusinessPlan'
)
BEGIN
    ALTER TABLE [Public.Accreditation] ADD [InfrastructureNotes] varchar(500) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250615135732_UpdateAccreditationBusinessPlan'
)
BEGIN
    ALTER TABLE [Public.Accreditation] ADD [InfrastructurePercentage] decimal(10,2) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250615135732_UpdateAccreditationBusinessPlan'
)
BEGIN
    ALTER TABLE [Public.Accreditation] ADD [NewMarketsNotes] varchar(500) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250615135732_UpdateAccreditationBusinessPlan'
)
BEGIN
    ALTER TABLE [Public.Accreditation] ADD [NewMarketsPercentage] decimal(10,2) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250615135732_UpdateAccreditationBusinessPlan'
)
BEGIN
    ALTER TABLE [Public.Accreditation] ADD [NewUsersRecycledPackagingWasteNotes] varchar(500) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250615135732_UpdateAccreditationBusinessPlan'
)
BEGIN
    ALTER TABLE [Public.Accreditation] ADD [NewUsersRecycledPackagingWastePercentage] decimal(10,2) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250615135732_UpdateAccreditationBusinessPlan'
)
BEGIN
    ALTER TABLE [Public.Accreditation] ADD [NotCoveredOtherCategoriesNotes] varchar(500) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250615135732_UpdateAccreditationBusinessPlan'
)
BEGIN
    ALTER TABLE [Public.Accreditation] ADD [NotCoveredOtherCategoriesPercentage] decimal(10,2) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250615135732_UpdateAccreditationBusinessPlan'
)
BEGIN
    ALTER TABLE [Public.Accreditation] ADD [RecycledWasteNotes] varchar(500) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250615135732_UpdateAccreditationBusinessPlan'
)
BEGIN
    ALTER TABLE [Public.Accreditation] ADD [RecycledWastePercentage] decimal(10,2) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250615135732_UpdateAccreditationBusinessPlan'
)
BEGIN
    ALTER TABLE [Public.Accreditation] ADD [TotalPercentage] decimal(10,2) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250615135732_UpdateAccreditationBusinessPlan'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250615135732_UpdateAccreditationBusinessPlan', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250617105930_AddIsOverdueColumn'
)
BEGIN

                        ALTER TABLE [Public.DeterminationDate]
                        ADD [IsOverdue] AS CASE 
                            WHEN [DeterminateDate] > GETUTCDATE() THEN CAST(0 AS BIT)
                            ELSE CAST(1 AS BIT)
                        END
                    
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250617105930_AddIsOverdueColumn'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250617105930_AddIsOverdueColumn', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250623170955_UpdateCarrierBrokerPermitsTable'
)
BEGIN
    EXEC sp_rename N'[Public.CarrierBrokerDealerPermits].[InstatallationPermitOrPPCNumber]', N'InstallationPermitOrPPCNumber', N'COLUMN';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250623170955_UpdateCarrierBrokerPermitsTable'
)
BEGIN
    DECLARE @var36 sysname;
    SELECT @var36 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Public.CarrierBrokerDealerPermits]') AND [c].[name] = N'InstallationPermitOrPPCNumber');
    IF @var36 IS NOT NULL EXEC(N'ALTER TABLE [Public.CarrierBrokerDealerPermits] DROP CONSTRAINT [' + @var36 + '];');
    ALTER TABLE [Public.CarrierBrokerDealerPermits] ALTER COLUMN [InstallationPermitOrPPCNumber] varchar(20) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250623170955_UpdateCarrierBrokerPermitsTable'
)
BEGIN
    DECLARE @var37 sysname;
    SELECT @var37 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Public.CarrierBrokerDealerPermits]') AND [c].[name] = N'WasteManagementEnvironmentPermitNumber');
    IF @var37 IS NOT NULL EXEC(N'ALTER TABLE [Public.CarrierBrokerDealerPermits] DROP CONSTRAINT [' + @var37 + '];');
    ALTER TABLE [Public.CarrierBrokerDealerPermits] ALTER COLUMN [WasteManagementEnvironmentPermitNumber] varchar(20) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250623170955_UpdateCarrierBrokerPermitsTable'
)
BEGIN
    DECLARE @var38 sysname;
    SELECT @var38 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Public.CarrierBrokerDealerPermits]') AND [c].[name] = N'WasteCarrierBrokerDealerRegistration');
    IF @var38 IS NOT NULL EXEC(N'ALTER TABLE [Public.CarrierBrokerDealerPermits] DROP CONSTRAINT [' + @var38 + '];');
    ALTER TABLE [Public.CarrierBrokerDealerPermits] ALTER COLUMN [WasteCarrierBrokerDealerRegistration] varchar(20) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250623170955_UpdateCarrierBrokerPermitsTable'
)
BEGIN
    ALTER TABLE [Public.CarrierBrokerDealerPermits] ADD [WasteExemptionReference] varchar(150) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250623170955_UpdateCarrierBrokerPermitsTable'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250623170955_UpdateCarrierBrokerPermitsTable', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250624084527_AddDefaultForExternalIdOnCarrierBrokerDealerPermits'
)
BEGIN
    DECLARE @var39 sysname;
    SELECT @var39 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Public.CarrierBrokerDealerPermits]') AND [c].[name] = N'ExternalId');
    IF @var39 IS NOT NULL EXEC(N'ALTER TABLE [Public.CarrierBrokerDealerPermits] DROP CONSTRAINT [' + @var39 + '];');
    ALTER TABLE [Public.CarrierBrokerDealerPermits] ADD DEFAULT (NEWID()) FOR [ExternalId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250624084527_AddDefaultForExternalIdOnCarrierBrokerDealerPermits'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250624084527_AddDefaultForExternalIdOnCarrierBrokerDealerPermits', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250627081758_AddInProgressRegistrationMaterialStatus'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup.RegistrationMaterialStatus]'))
        SET IDENTITY_INSERT [Lookup.RegistrationMaterialStatus] ON;
    EXEC(N'INSERT INTO [Lookup.RegistrationMaterialStatus] ([Id], [Name])
    VALUES (12, N''InProgress'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup.RegistrationMaterialStatus]'))
        SET IDENTITY_INSERT [Lookup.RegistrationMaterialStatus] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250627081758_AddInProgressRegistrationMaterialStatus'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250627081758_AddInProgressRegistrationMaterialStatus', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250627113549_RenamingTypeOfSuppliers'
)
BEGIN
    EXEC sp_rename N'[Public.RegistrationReprocessingIO].[TypeOfSupplier]', N'TypeOfSuppliers', N'COLUMN';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250627113549_RenamingTypeOfSuppliers'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250627113549_RenamingTypeOfSuppliers', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250627230610_Countries-Lookup'
)
BEGIN
    CREATE TABLE [Lookup.Country] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(100) NOT NULL,
        [CountryCode] nvarchar(3) NULL,
        CONSTRAINT [PK_Lookup.Country] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250627230610_Countries-Lookup'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CountryCode', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup.Country]'))
        SET IDENTITY_INSERT [Lookup.Country] ON;
    EXEC(N'INSERT INTO [Lookup.Country] ([Id], [CountryCode], [Name])
    VALUES (1, N''ad'', N''Andorra''),
    (2, N''ae'', N''United Arab Emirates''),
    (3, N''af'', N''Afghanistan''),
    (4, N''ag'', N''Antigua and Barbuda''),
    (5, N''ai'', N''Anguilla''),
    (6, N''al'', N''Albania''),
    (7, N''am'', N''Armenia''),
    (8, N''an'', N''Netherlands Antilles''),
    (9, N''ao'', N''Angola''),
    (10, N''aq'', N''Antarctica''),
    (11, N''ar'', N''Argentina''),
    (13, N''as'', N''American Samoa''),
    (14, N''at'', N''Austria''),
    (15, N''au'', N''Australia''),
    (16, N''aw'', N''Aruba''),
    (17, N''az'', N''Azerbaidjan''),
    (18, N''ba'', N''Bosnia-Herzegovina''),
    (19, N''bb'', N''Barbados''),
    (20, N''bd'', N''Bangladesh''),
    (21, N''be'', N''Belgium''),
    (22, N''bf'', N''Burkina Faso''),
    (23, N''bg'', N''Bulgaria''),
    (24, N''bh'', N''Bahrain''),
    (25, N''bi'', N''Burundi''),
    (26, N''bj'', N''Benin''),
    (27, N''bm'', N''Bermuda''),
    (28, N''bn'', N''Brunei Darussalam''),
    (29, N''bo'', N''Bolivia''),
    (30, N''br'', N''Brazil''),
    (31, N''bs'', N''Bahamas''),
    (32, N''bt'', N''Bhutan''),
    (33, N''bv'', N''Bouvet Island''),
    (34, N''bw'', N''Botswana''),
    (35, N''by'', N''Belarus''),
    (36, N''bz'', N''Belize''),
    (37, N''ca'', N''Canada''),
    (38, N''cc'', N''Cocos (Keeling) Islands''),
    (39, N''cf'', N''Central African Republic''),
    (40, N''cg'', N''Congo''),
    (41, N''ch'', N''Switzerland''),
    (42, N''ci'', N''Ivory Coast (Cote D''''Ivoire)''),
    (43, N''ck'', N''Cook Islands'');
    INSERT INTO [Lookup.Country] ([Id], [CountryCode], [Name])
    VALUES (44, N''cl'', N''Chile''),
    (45, N''cm'', N''Cameroon''),
    (46, N''cn'', N''China''),
    (47, N''co'', N''Colombia''),
    (48, N''com'', N''Commercial''),
    (49, N''cr'', N''Costa Rica''),
    (50, N''cs'', N''Former Czechoslovakia''),
    (51, N''cu'', N''Cuba''),
    (52, N''cv'', N''Cape Verde''),
    (53, N''cx'', N''Christmas Island''),
    (54, N''cy'', N''Cyprus''),
    (55, N''cz'', N''Czech Republic''),
    (56, N''de'', N''Germany''),
    (57, N''dj'', N''Djibouti''),
    (58, N''dk'', N''Denmark''),
    (59, N''dm'', N''Dominica''),
    (60, N''do'', N''Dominican Republic''),
    (61, N''dz'', N''Algeria''),
    (62, N''ec'', N''Ecuador''),
    (64, N''ee'', N''Estonia''),
    (65, N''eg'', N''Egypt''),
    (66, N''eh'', N''Western Sahara''),
    (67, N''er'', N''Eritrea''),
    (68, N''es'', N''Spain''),
    (69, N''et'', N''Ethiopia''),
    (70, N''fi'', N''Finland''),
    (71, N''fj'', N''Fiji''),
    (72, N''fk'', N''Falkland Islands''),
    (73, N''fm'', N''Micronesia''),
    (74, N''fo'', N''Faroe Islands''),
    (75, N''fr'', N''France''),
    (76, N''fx'', N''France (European Territory)''),
    (77, N''ga'', N''Gabon''),
    (79, N''gd'', N''Grenada''),
    (80, N''ge'', N''Georgia''),
    (81, N''gf'', N''French Guyana''),
    (82, N''gh'', N''Ghana''),
    (83, N''gi'', N''Gibraltar''),
    (84, N''gl'', N''Greenland''),
    (85, N''gm'', N''Gambia''),
    (86, N''gn'', N''Guinea''),
    (88, N''gp'', N''Guadeloupe (French)'');
    INSERT INTO [Lookup.Country] ([Id], [CountryCode], [Name])
    VALUES (89, N''gq'', N''Equatorial Guinea''),
    (90, N''gr'', N''Greece''),
    (91, N''gs'', N''S. Georgia & S. Sandwich Isls.''),
    (92, N''gt'', N''Guatemala''),
    (93, N''gu'', N''Guam (USA)''),
    (94, N''gw'', N''Guinea Bissau''),
    (95, N''gy'', N''Guyana''),
    (96, N''hk'', N''Hong Kong''),
    (97, N''hm'', N''Heard and McDonald Islands''),
    (98, N''hn'', N''Honduras''),
    (99, N''hr'', N''Croatia''),
    (100, N''ht'', N''Haiti''),
    (101, N''hu'', N''Hungary''),
    (102, N''id'', N''Indonesia''),
    (103, N''ie'', N''Ireland''),
    (104, N''il'', N''Israel''),
    (105, N''in'', N''India''),
    (106, N''int'', N''International''),
    (107, N''io'', N''British Indian Ocean Territory''),
    (108, N''iq'', N''Iraq''),
    (109, N''ir'', N''Iran''),
    (110, N''is'', N''Iceland''),
    (111, N''it'', N''Italy''),
    (112, N''jm'', N''Jamaica''),
    (113, N''jo'', N''Jordan''),
    (114, N''jp'', N''Japan''),
    (115, N''ke'', N''Kenya''),
    (116, N''kg'', N''Kyrgyzstan''),
    (117, N''kh'', N''Cambodia''),
    (118, N''ki'', N''Kiribati''),
    (119, N''km'', N''Comoros''),
    (120, N''kn'', N''Saint Kitts & Nevis Anguilla''),
    (121, N''kp'', N''North Korea''),
    (122, N''kr'', N''South Korea''),
    (123, N''kw'', N''Kuwait''),
    (124, N''ky'', N''Cayman Islands''),
    (125, N''kz'', N''Kazakhstan''),
    (126, N''la'', N''Laos''),
    (127, N''lb'', N''Lebanon''),
    (128, N''lc'', N''Saint Lucia''),
    (129, N''li'', N''Liechtenstein''),
    (130, N''lk'', N''Sri Lanka'');
    INSERT INTO [Lookup.Country] ([Id], [CountryCode], [Name])
    VALUES (131, N''lr'', N''Liberia''),
    (132, N''ls'', N''Lesotho''),
    (133, N''lt'', N''Lithuania''),
    (134, N''lu'', N''Luxembourg''),
    (135, N''lv'', N''Latvia''),
    (136, N''ly'', N''Libya''),
    (137, N''ma'', N''Morocco''),
    (138, N''mc'', N''Monaco''),
    (139, N''md'', N''Moldavia''),
    (140, N''mg'', N''Madagascar''),
    (141, N''mh'', N''Marshall Islands''),
    (143, N''mk'', N''Macedonia''),
    (144, N''ml'', N''Mali''),
    (145, N''mm'', N''Myanmar''),
    (146, N''mn'', N''Mongolia''),
    (147, N''mo'', N''Macau''),
    (148, N''mp'', N''Northern Mariana Islands''),
    (149, N''mq'', N''Martinique (French)''),
    (150, N''mr'', N''Mauritania''),
    (151, N''ms'', N''Montserrat''),
    (152, N''mt'', N''Malta''),
    (153, N''mu'', N''Mauritius''),
    (154, N''mv'', N''Maldives''),
    (155, N''mw'', N''Malawi''),
    (156, N''mx'', N''Mexico''),
    (157, N''my'', N''Malaysia''),
    (158, N''mz'', N''Mozambique''),
    (159, N''na'', N''Namibia''),
    (161, N''nc'', N''New Caledonia (French)''),
    (162, N''ne'', N''Niger''),
    (163, N''net'', N''Network''),
    (164, N''nf'', N''Norfolk Island''),
    (165, N''ng'', N''Nigeria''),
    (166, N''ni'', N''Nicaragua''),
    (167, N''nl'', N''Netherlands''),
    (168, N''no'', N''Norway''),
    (169, N''np'', N''Nepal''),
    (170, N''nr'', N''Nauru''),
    (171, N''nt'', N''Neutral Zone''),
    (172, N''nu'', N''Niue''),
    (173, N''nz'', N''New Zealand''),
    (174, N''om'', N''Oman'');
    INSERT INTO [Lookup.Country] ([Id], [CountryCode], [Name])
    VALUES (176, N''pa'', N''Panama''),
    (177, N''pe'', N''Peru''),
    (178, N''pf'', N''Polynesia (French)''),
    (179, N''pg'', N''Papua New Guinea''),
    (180, N''ph'', N''Philippines''),
    (181, N''pk'', N''Pakistan''),
    (182, N''pl'', N''Poland''),
    (183, N''pm'', N''Saint Pierre and Miquelon''),
    (184, N''pn'', N''Pitcairn Island''),
    (185, N''pr'', N''Puerto Rico''),
    (186, N''pt'', N''Portugal''),
    (187, N''pw'', N''Palau''),
    (188, N''py'', N''Paraguay''),
    (189, N''qa'', N''Qatar''),
    (190, N''re'', N''Reunion (French)''),
    (191, N''ro'', N''Romania''),
    (192, N''ru'', N''Russian Federation''),
    (193, N''rw'', N''Rwanda''),
    (194, N''sa'', N''Saudi Arabia''),
    (195, N''sb'', N''Solomon Islands''),
    (196, N''sc'', N''Seychelles''),
    (197, N''sd'', N''Sudan''),
    (198, N''se'', N''Sweden''),
    (199, N''sg'', N''Singapore''),
    (200, N''sh'', N''Saint Helena''),
    (201, N''si'', N''Slovenia''),
    (202, N''sj'', N''Svalbard and Jan Mayen Islands''),
    (203, N''sk'', N''Slovak Republic''),
    (204, N''sl'', N''Sierra Leone''),
    (205, N''sm'', N''San Marino''),
    (206, N''sn'', N''Senegal''),
    (207, N''so'', N''Somalia''),
    (208, N''sr'', N''Suriname''),
    (209, N''st'', N''Saint Tome (Sao Tome) and Principe''),
    (210, N''su'', N''Former USSR''),
    (211, N''sv'', N''El Salvador''),
    (212, N''sy'', N''Syria''),
    (213, N''sz'', N''Swaziland''),
    (214, N''tc'', N''Turks and Caicos Islands''),
    (215, N''td'', N''Chad''),
    (216, N''tf'', N''French Southern Territories''),
    (217, N''tg'', N''Togo'');
    INSERT INTO [Lookup.Country] ([Id], [CountryCode], [Name])
    VALUES (218, N''th'', N''Thailand''),
    (219, N''tj'', N''Tadjikistan''),
    (220, N''tk'', N''Tokelau''),
    (221, N''tm'', N''Turkmenistan''),
    (222, N''tn'', N''Tunisia''),
    (223, N''to'', N''Tonga''),
    (224, N''tp'', N''East Timor''),
    (225, N''tr'', N''Turkey''),
    (226, N''tt'', N''Trinidad and Tobago''),
    (227, N''tv'', N''Tuvalu''),
    (228, N''tw'', N''Taiwan''),
    (229, N''tz'', N''Tanzania''),
    (230, N''ua'', N''Ukraine''),
    (231, N''ug'', N''Uganda''),
    (233, N''um'', N''USA Minor Outlying Islands''),
    (234, N''us'', N''United States''),
    (235, N''uy'', N''Uruguay''),
    (236, N''uz'', N''Uzbekistan''),
    (237, N''va'', N''Vatican City State''),
    (238, N''vc'', N''Saint Vincent & Grenadines''),
    (239, N''ve'', N''Venezuela''),
    (240, N''vg'', N''Virgin Islands (British)''),
    (241, N''vi'', N''Virgin Islands (USA)''),
    (242, N''vn'', N''Vietnam''),
    (243, N''vu'', N''Vanuatu''),
    (244, N''wf'', N''Wallis and Futuna Islands''),
    (245, N''ws'', N''Samoa''),
    (246, N''ye'', N''Yemen''),
    (247, N''yt'', N''Mayotte''),
    (248, N''yu'', N''Yugoslavia''),
    (249, N''za'', N''South Africa''),
    (250, N''zm'', N''Zambia''),
    (251, N''zr'', N''Zaire''),
    (252, N''zw'', N''Zimbabwe'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CountryCode', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup.Country]'))
        SET IDENTITY_INSERT [Lookup.Country] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250627230610_Countries-Lookup'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250627230610_Countries-Lookup', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250629092851_ApplicantTaskLookupTable'
)
BEGIN
    ALTER TABLE [Public.RegistrationTaskStatus] DROP CONSTRAINT [FK_Public.RegistrationTaskStatus_Lookup.RegulatorTask_TaskId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250629092851_ApplicantTaskLookupTable'
)
BEGIN
    ALTER TABLE [Public.RegistrationTaskStatus] ADD [RegistrationMaterialId] int NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250629092851_ApplicantTaskLookupTable'
)
BEGIN
    CREATE TABLE [Lookup.Task] (
        [Id] int NOT NULL IDENTITY,
        [IsMaterialSpecific] bit NOT NULL,
        [ApplicationTypeId] int NOT NULL,
        [JourneyTypeId] int NOT NULL,
        [Name] nvarchar(200) NOT NULL,
        CONSTRAINT [PK_Lookup.Task] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250629092851_ApplicantTaskLookupTable'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ApplicationTypeId', N'IsMaterialSpecific', N'JourneyTypeId', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup.Task]'))
        SET IDENTITY_INSERT [Lookup.Task] ON;
    EXEC(N'INSERT INTO [Lookup.Task] ([Id], [ApplicationTypeId], [IsMaterialSpecific], [JourneyTypeId], [Name])
    VALUES (1, 1, CAST(0 AS bit), 1, N''SiteAddressAndContactDetails''),
    (2, 1, CAST(1 AS bit), 1, N''WasteLicensesPermitsAndExemptions''),
    (3, 1, CAST(1 AS bit), 1, N''ReprocessingInputsAndOutputs''),
    (4, 1, CAST(1 AS bit), 1, N''SamplingAndInspectionPlan''),
    (5, 2, CAST(0 AS bit), 1, N''WasteLicensesPermitsAndExemptions''),
    (6, 2, CAST(1 AS bit), 1, N''SamplingAndInspectionPlan''),
    (7, 2, CAST(1 AS bit), 1, N''OverseasReprocessingSites''),
    (8, 2, CAST(1 AS bit), 1, N''InterimSites''),
    (9, 1, CAST(1 AS bit), 2, N''PRNsTonnageAndAuthorityToIssuePRNs''),
    (10, 1, CAST(1 AS bit), 2, N''BusinessPlan''),
    (11, 1, CAST(1 AS bit), 2, N''AccreditationSamplingAndInspectionPlan''),
    (12, 2, CAST(1 AS bit), 2, N''PERNsTonnageAndAuthorityToIssuePERNs''),
    (13, 2, CAST(1 AS bit), 2, N''BusinessPlan''),
    (14, 2, CAST(1 AS bit), 2, N''AccreditationSamplingAndInspectionPlan''),
    (15, 2, CAST(1 AS bit), 2, N''OverseasReprocessingSitesAndEvidenceOfBroadlyEquivalentStandards''),
    (16, 2, CAST(0 AS bit), 1, N''WasteCarrierBrokerDealerNumber'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ApplicationTypeId', N'IsMaterialSpecific', N'JourneyTypeId', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup.Task]'))
        SET IDENTITY_INSERT [Lookup.Task] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250629092851_ApplicantTaskLookupTable'
)
BEGIN
    CREATE INDEX [IX_Public.RegistrationTaskStatus_RegistrationMaterialId] ON [Public.RegistrationTaskStatus] ([RegistrationMaterialId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250629092851_ApplicantTaskLookupTable'
)
BEGIN
    ALTER TABLE [Public.RegistrationTaskStatus] ADD CONSTRAINT [FK_Public.RegistrationTaskStatus_Lookup.Task_TaskId] FOREIGN KEY ([TaskId]) REFERENCES [Lookup.Task] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250629092851_ApplicantTaskLookupTable'
)
BEGIN
    ALTER TABLE [Public.RegistrationTaskStatus] ADD CONSTRAINT [FK_Public.RegistrationTaskStatus_Public.RegistrationMaterial_RegistrationMaterialId] FOREIGN KEY ([RegistrationMaterialId]) REFERENCES [Public.RegistrationMaterial] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250629092851_ApplicantTaskLookupTable'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250629092851_ApplicantTaskLookupTable', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250629130952_RegistratinoMaterialNullableFields'
)
BEGIN
    ALTER TABLE [Public.RegistrationMaterial] DROP CONSTRAINT [FK_Public.RegistrationMaterial_Lookup.MaterialPermit_PermitTypeId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250629130952_RegistratinoMaterialNullableFields'
)
BEGIN
    DECLARE @var40 sysname;
    SELECT @var40 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Public.RegistrationMaterial]') AND [c].[name] = N'WasteManagementReprocessingCapacityTonne');
    IF @var40 IS NOT NULL EXEC(N'ALTER TABLE [Public.RegistrationMaterial] DROP CONSTRAINT [' + @var40 + '];');
    ALTER TABLE [Public.RegistrationMaterial] ALTER COLUMN [WasteManagementReprocessingCapacityTonne] decimal(18,2) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250629130952_RegistratinoMaterialNullableFields'
)
BEGIN
    DECLARE @var41 sysname;
    SELECT @var41 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Public.RegistrationMaterial]') AND [c].[name] = N'PermitTypeId');
    IF @var41 IS NOT NULL EXEC(N'ALTER TABLE [Public.RegistrationMaterial] DROP CONSTRAINT [' + @var41 + '];');
    ALTER TABLE [Public.RegistrationMaterial] ALTER COLUMN [PermitTypeId] int NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250629130952_RegistratinoMaterialNullableFields'
)
BEGIN
    DECLARE @var42 sysname;
    SELECT @var42 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Public.RegistrationMaterial]') AND [c].[name] = N'PPCReprocessingCapacityTonne');
    IF @var42 IS NOT NULL EXEC(N'ALTER TABLE [Public.RegistrationMaterial] DROP CONSTRAINT [' + @var42 + '];');
    ALTER TABLE [Public.RegistrationMaterial] ALTER COLUMN [PPCReprocessingCapacityTonne] decimal(18,2) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250629130952_RegistratinoMaterialNullableFields'
)
BEGIN
    DECLARE @var43 sysname;
    SELECT @var43 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Public.RegistrationMaterial]') AND [c].[name] = N'MaximumReprocessingCapacityTonne');
    IF @var43 IS NOT NULL EXEC(N'ALTER TABLE [Public.RegistrationMaterial] DROP CONSTRAINT [' + @var43 + '];');
    ALTER TABLE [Public.RegistrationMaterial] ALTER COLUMN [MaximumReprocessingCapacityTonne] decimal(18,2) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250629130952_RegistratinoMaterialNullableFields'
)
BEGIN
    DECLARE @var44 sysname;
    SELECT @var44 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Public.RegistrationMaterial]') AND [c].[name] = N'InstallationReprocessingTonne');
    IF @var44 IS NOT NULL EXEC(N'ALTER TABLE [Public.RegistrationMaterial] DROP CONSTRAINT [' + @var44 + '];');
    ALTER TABLE [Public.RegistrationMaterial] ALTER COLUMN [InstallationReprocessingTonne] decimal(18,2) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250629130952_RegistratinoMaterialNullableFields'
)
BEGIN
    DECLARE @var45 sysname;
    SELECT @var45 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Public.RegistrationMaterial]') AND [c].[name] = N'EnvironmentalPermitWasteManagementTonne');
    IF @var45 IS NOT NULL EXEC(N'ALTER TABLE [Public.RegistrationMaterial] DROP CONSTRAINT [' + @var45 + '];');
    ALTER TABLE [Public.RegistrationMaterial] ALTER COLUMN [EnvironmentalPermitWasteManagementTonne] decimal(18,2) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250629130952_RegistratinoMaterialNullableFields'
)
BEGIN
    ALTER TABLE [Public.RegistrationMaterial] ADD CONSTRAINT [FK_Public.RegistrationMaterial_Lookup.MaterialPermit_PermitTypeId] FOREIGN KEY ([PermitTypeId]) REFERENCES [Lookup.MaterialPermit] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250629130952_RegistratinoMaterialNullableFields'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250629130952_RegistratinoMaterialNullableFields', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250701083202_AddRegistrationMaterialContact'
)
BEGIN
    CREATE TABLE [Public.RegistrationMaterialContact] (
        [Id] int NOT NULL IDENTITY,
        [ExternalId] uniqueidentifier NOT NULL,
        [RegistrationMaterialId] int NOT NULL,
        [UserId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_Public.RegistrationMaterialContact] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Public.RegistrationMaterialContact_Public.RegistrationMaterial_RegistrationMaterialId] FOREIGN KEY ([RegistrationMaterialId]) REFERENCES [Public.RegistrationMaterial] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250701083202_AddRegistrationMaterialContact'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Public.RegistrationMaterialContact_ExternalId] ON [Public.RegistrationMaterialContact] ([ExternalId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250701083202_AddRegistrationMaterialContact'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Public.RegistrationMaterialContact_RegistrationMaterialId] ON [Public.RegistrationMaterialContact] ([RegistrationMaterialId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250701083202_AddRegistrationMaterialContact'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250701083202_AddRegistrationMaterialContact', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250704143807_RegistrationReprocessingIORawMaterialOrProducts'
)
BEGIN
    CREATE TABLE [Public.RegistrationReprocessingIORawMaterialOrProducts] (
        [Id] int NOT NULL IDENTITY,
        [ExternalId] uniqueidentifier NOT NULL,
        [RegistrationReprocessingIOId] int NOT NULL,
        [RawMaterialOrProductName] nvarchar(max) NOT NULL,
        [TonneValue] decimal(18,2) NOT NULL,
        [IsInput] bit NOT NULL,
        CONSTRAINT [PK_Public.RegistrationReprocessingIORawMaterialOrProducts] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Public.RegistrationReprocessingIORawMaterialOrProducts_Public.RegistrationReprocessingIO_RegistrationReprocessingIOId] FOREIGN KEY ([RegistrationReprocessingIOId]) REFERENCES [Public.RegistrationReprocessingIO] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250704143807_RegistrationReprocessingIORawMaterialOrProducts'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Public.RegistrationReprocessingIORawMaterialOrProducts_ExternalId] ON [Public.RegistrationReprocessingIORawMaterialOrProducts] ([ExternalId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250704143807_RegistrationReprocessingIORawMaterialOrProducts'
)
BEGIN
    CREATE INDEX [IX_Public.RegistrationReprocessingIORawMaterialOrProducts_RegistrationReprocessingIOId] ON [Public.RegistrationReprocessingIORawMaterialOrProducts] ([RegistrationReprocessingIOId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250704143807_RegistrationReprocessingIORawMaterialOrProducts'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250704143807_RegistrationReprocessingIORawMaterialOrProducts', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250709141930_AddOverseasTables'
)
BEGIN
    CREATE TABLE [Public.OverseasAddress] (
        [Id] int NOT NULL IDENTITY,
        [ExternalId] uniqueidentifier NOT NULL,
        [RegistrationId] int NOT NULL,
        [OrganisationName] nvarchar(100) NOT NULL,
        [CountryId] int NOT NULL,
        [AddressLine1] nvarchar(100) NOT NULL,
        [AddressLine2] nvarchar(100) NULL,
        [CityOrTown] nvarchar(70) NOT NULL,
        [StateProvince] nvarchar(70) NULL,
        [PostCode] nvarchar(20) NULL,
        [CreatedBy] uniqueidentifier NOT NULL,
        [UpdatedBy] uniqueidentifier NOT NULL,
        [CreatedOn] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedOn] datetime2 NOT NULL,
        [SiteCoordinates] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Public.OverseasAddress] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Public.OverseasAddress_Lookup.Country_CountryId] FOREIGN KEY ([CountryId]) REFERENCES [Lookup.Country] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_Public.OverseasAddress_Public.Registration_RegistrationId] FOREIGN KEY ([RegistrationId]) REFERENCES [Public.Registration] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250709141930_AddOverseasTables'
)
BEGIN
    CREATE TABLE [Public.InterimOverseasConnections] (
        [Id] int NOT NULL IDENTITY,
        [ExternalId] uniqueidentifier NOT NULL,
        [InterimSiteId] int NOT NULL,
        [ParentOverseasAddressId] int NOT NULL,
        CONSTRAINT [PK_Public.InterimOverseasConnections] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Public.InterimOverseasConnections_Public.OverseasAddress_ParentOverseasAddressId] FOREIGN KEY ([ParentOverseasAddressId]) REFERENCES [Public.OverseasAddress] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250709141930_AddOverseasTables'
)
BEGIN
    CREATE TABLE [Public.OverseasAddressContact] (
        [Id] int NOT NULL IDENTITY,
        [OverseasAddressId] int NOT NULL,
        [FullName] nvarchar(100) NOT NULL,
        [Email] nvarchar(100) NOT NULL,
        [PhoneNumber] nvarchar(25) NOT NULL,
        [CreatedOn] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [CreatedBy] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_Public.OverseasAddressContact] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Public.OverseasAddressContact_Public.OverseasAddress_OverseasAddressId] FOREIGN KEY ([OverseasAddressId]) REFERENCES [Public.OverseasAddress] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250709141930_AddOverseasTables'
)
BEGIN
    CREATE TABLE [Public.OverseasAddressWasteCode] (
        [Id] int NOT NULL IDENTITY,
        [ExternalId] uniqueidentifier NOT NULL,
        [OverseasAddressId] int NOT NULL,
        [CodeName] nvarchar(10) NOT NULL,
        CONSTRAINT [PK_Public.OverseasAddressWasteCode] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Public.OverseasAddressWasteCode_Public.OverseasAddress_OverseasAddressId] FOREIGN KEY ([OverseasAddressId]) REFERENCES [Public.OverseasAddress] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250709141930_AddOverseasTables'
)
BEGIN
    CREATE TABLE [Public.OverseasMaterialReprocessingSite] (
        [Id] int NOT NULL IDENTITY,
        [ExternalId] uniqueidentifier NOT NULL,
        [OverseasAddressId] int NOT NULL,
        [RegistrationMaterialId] int NOT NULL,
        CONSTRAINT [PK_Public.OverseasMaterialReprocessingSite] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Public.OverseasMaterialReprocessingSite_Public.OverseasAddress_OverseasAddressId] FOREIGN KEY ([OverseasAddressId]) REFERENCES [Public.OverseasAddress] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_Public.OverseasMaterialReprocessingSite_Public.RegistrationMaterial_RegistrationMaterialId] FOREIGN KEY ([RegistrationMaterialId]) REFERENCES [Public.RegistrationMaterial] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250709141930_AddOverseasTables'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Public.InterimOverseasConnections_ExternalId] ON [Public.InterimOverseasConnections] ([ExternalId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250709141930_AddOverseasTables'
)
BEGIN
    CREATE INDEX [IX_Public.InterimOverseasConnections_ParentOverseasAddressId] ON [Public.InterimOverseasConnections] ([ParentOverseasAddressId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250709141930_AddOverseasTables'
)
BEGIN
    CREATE INDEX [IX_Public.OverseasAddress_CountryId] ON [Public.OverseasAddress] ([CountryId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250709141930_AddOverseasTables'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Public.OverseasAddress_ExternalId] ON [Public.OverseasAddress] ([ExternalId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250709141930_AddOverseasTables'
)
BEGIN
    CREATE INDEX [IX_Public.OverseasAddress_RegistrationId] ON [Public.OverseasAddress] ([RegistrationId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250709141930_AddOverseasTables'
)
BEGIN
    CREATE INDEX [IX_Public.OverseasAddressContact_OverseasAddressId] ON [Public.OverseasAddressContact] ([OverseasAddressId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250709141930_AddOverseasTables'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Public.OverseasAddressWasteCode_ExternalId] ON [Public.OverseasAddressWasteCode] ([ExternalId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250709141930_AddOverseasTables'
)
BEGIN
    CREATE INDEX [IX_Public.OverseasAddressWasteCode_OverseasAddressId] ON [Public.OverseasAddressWasteCode] ([OverseasAddressId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250709141930_AddOverseasTables'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Public.OverseasMaterialReprocessingSite_ExternalId] ON [Public.OverseasMaterialReprocessingSite] ([ExternalId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250709141930_AddOverseasTables'
)
BEGIN
    CREATE INDEX [IX_Public.OverseasMaterialReprocessingSite_OverseasAddressId] ON [Public.OverseasMaterialReprocessingSite] ([OverseasAddressId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250709141930_AddOverseasTables'
)
BEGIN
    CREATE INDEX [IX_Public.OverseasMaterialReprocessingSite_RegistrationMaterialId] ON [Public.OverseasMaterialReprocessingSite] ([RegistrationMaterialId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250709141930_AddOverseasTables'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250709141930_AddOverseasTables', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250721141702_UpdateAccreditationPrns'
)
BEGIN
    DECLARE @var46 sysname;
    SELECT @var46 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Public.OverseasAddress]') AND [c].[name] = N'SiteCoordinates');
    IF @var46 IS NOT NULL EXEC(N'ALTER TABLE [Public.OverseasAddress] DROP CONSTRAINT [' + @var46 + '];');
    ALTER TABLE [Public.OverseasAddress] ALTER COLUMN [SiteCoordinates] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250721141702_UpdateAccreditationPrns'
)
BEGIN
    ALTER TABLE [public.AccreditationFileUpload] ADD [OverseasSiteId] int NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250721141702_UpdateAccreditationPrns'
)
BEGIN
    ALTER TABLE [public.AccreditationFileUpload] ADD [SubmissionId] uniqueidentifier NOT NULL DEFAULT ('00000000-0000-0000-0000-000000000000');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250721141702_UpdateAccreditationPrns'
)
BEGIN
    DECLARE @var47 sysname;
    SELECT @var47 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Public.Accreditation]') AND [c].[name] = N'PRNTonnage');
    IF @var47 IS NOT NULL EXEC(N'ALTER TABLE [Public.Accreditation] DROP CONSTRAINT [' + @var47 + '];');
    ALTER TABLE [Public.Accreditation] ALTER COLUMN [PRNTonnage] int NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250721141702_UpdateAccreditationPrns'
)
BEGIN
    DECLARE @var48 sysname;
    SELECT @var48 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Public.Accreditation]') AND [c].[name] = N'ApplicationReferenceNumber');
    IF @var48 IS NOT NULL EXEC(N'ALTER TABLE [Public.Accreditation] DROP CONSTRAINT [' + @var48 + '];');
    ALTER TABLE [Public.Accreditation] ALTER COLUMN [ApplicationReferenceNumber] nvarchar(12) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250721141702_UpdateAccreditationPrns'
)
BEGIN
    DECLARE @var49 sysname;
    SELECT @var49 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Public.Accreditation]') AND [c].[name] = N'AccreditationYear');
    IF @var49 IS NOT NULL EXEC(N'ALTER TABLE [Public.Accreditation] DROP CONSTRAINT [' + @var49 + '];');
    ALTER TABLE [Public.Accreditation] ALTER COLUMN [AccreditationYear] int NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250721141702_UpdateAccreditationPrns'
)
BEGIN
    ALTER TABLE [Public.Accreditation] ADD [CreatedBy] uniqueidentifier NOT NULL DEFAULT ('00000000-0000-0000-0000-000000000000');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250721141702_UpdateAccreditationPrns'
)
BEGIN
    ALTER TABLE [Public.Accreditation] ADD [DecFullName] nvarchar(50) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250721141702_UpdateAccreditationPrns'
)
BEGIN
    ALTER TABLE [Public.Accreditation] ADD [DecJobTitle] nvarchar(50) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250721141702_UpdateAccreditationPrns'
)
BEGIN
    ALTER TABLE [Public.Accreditation] ADD [UpdatedBy] uniqueidentifier NOT NULL DEFAULT ('00000000-0000-0000-0000-000000000000');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250721141702_UpdateAccreditationPrns'
)
BEGIN
    ALTER TABLE [Public.Accreditation] ADD [UpdatedOn] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250721141702_UpdateAccreditationPrns'
)
BEGIN
    CREATE TABLE [Lookup.MeetConditionsOfExport] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(50) NOT NULL,
        CONSTRAINT [PK_Lookup.MeetConditionsOfExport] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250721141702_UpdateAccreditationPrns'
)
BEGIN
    CREATE TABLE [Lookup.SiteCheckStatus] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(10) NOT NULL,
        CONSTRAINT [PK_Lookup.SiteCheckStatus] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250721141702_UpdateAccreditationPrns'
)
BEGIN
    CREATE TABLE [Public.AccreditationPrnIssueAuth] (
        [Id] int NOT NULL IDENTITY,
        [ExternalId] uniqueidentifier NOT NULL,
        [AccreditationExternalId] uniqueidentifier NOT NULL,
        [AccreditationId] int NOT NULL,
        [PersonExternalId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_Public.AccreditationPrnIssueAuth] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Public.AccreditationPrnIssueAuth_Public.Accreditation_AccreditationId] FOREIGN KEY ([AccreditationId]) REFERENCES [Public.Accreditation] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250721141702_UpdateAccreditationPrns'
)
BEGIN
    CREATE TABLE [Public.AccreditationTaskStatus] (
        [Id] int NOT NULL IDENTITY,
        [TaskId] int NOT NULL,
        [AccreditationId] int NOT NULL,
        [ExternalId] uniqueidentifier NOT NULL,
        [TaskStatusId] int NOT NULL,
        CONSTRAINT [PK_Public.AccreditationTaskStatus] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Public.AccreditationTaskStatus_Lookup.TaskStatus_TaskStatusId] FOREIGN KEY ([TaskStatusId]) REFERENCES [Lookup.TaskStatus] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_Public.AccreditationTaskStatus_Lookup.Task_TaskId] FOREIGN KEY ([TaskId]) REFERENCES [Lookup.Task] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_Public.AccreditationTaskStatus_Public.Accreditation_AccreditationId] FOREIGN KEY ([AccreditationId]) REFERENCES [Public.Accreditation] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250721141702_UpdateAccreditationPrns'
)
BEGIN
    CREATE TABLE [Public.OverseasAccreditationSite] (
        [Id] int NOT NULL IDENTITY,
        [ExternalId] uniqueidentifier NOT NULL,
        [AccreditationId] int NOT NULL,
        [OverseasAddressId] int NOT NULL,
        [OrganisationName] nvarchar(max) NOT NULL,
        [MeetConditionsOfExportId] int NOT NULL,
        [StartDay] int NOT NULL,
        [StartMonth] int NOT NULL,
        [StartYear] int NOT NULL,
        [ExpiryDay] int NOT NULL,
        [ExpiryMonth] int NOT NULL,
        [ExpiryYear] int NOT NULL,
        [CreatedBy] uniqueidentifier NOT NULL,
        [CreatedOn] datetime2 NOT NULL,
        [SiteCheckStatusId] int NOT NULL,
        CONSTRAINT [PK_Public.OverseasAccreditationSite] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Public.OverseasAccreditationSite_Lookup.MeetConditionsOfExport_MeetConditionsOfExportId] FOREIGN KEY ([MeetConditionsOfExportId]) REFERENCES [Lookup.MeetConditionsOfExport] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_Public.OverseasAccreditationSite_Lookup.SiteCheckStatus_SiteCheckStatusId] FOREIGN KEY ([SiteCheckStatusId]) REFERENCES [Lookup.SiteCheckStatus] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_Public.OverseasAccreditationSite_Public.Accreditation_AccreditationId] FOREIGN KEY ([AccreditationId]) REFERENCES [Public.Accreditation] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_Public.OverseasAccreditationSite_Public.OverseasAddress_OverseasAddressId] FOREIGN KEY ([OverseasAddressId]) REFERENCES [Public.OverseasAddress] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250721141702_UpdateAccreditationPrns'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup.FileUploadType]'))
        SET IDENTITY_INSERT [Lookup.FileUploadType] ON;
    EXEC(N'INSERT INTO [Lookup.FileUploadType] ([Id], [Name])
    VALUES (2, N''OverseasSiteEvidence'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup.FileUploadType]'))
        SET IDENTITY_INSERT [Lookup.FileUploadType] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250721141702_UpdateAccreditationPrns'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup.MeetConditionsOfExport]'))
        SET IDENTITY_INSERT [Lookup.MeetConditionsOfExport] ON;
    EXEC(N'INSERT INTO [Lookup.MeetConditionsOfExport] ([Id], [Name])
    VALUES (1, N''Yes (Don''''t Upload)''),
    (2, N''Yes (upload)''),
    (3, N''No'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup.MeetConditionsOfExport]'))
        SET IDENTITY_INSERT [Lookup.MeetConditionsOfExport] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250721141702_UpdateAccreditationPrns'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup.SiteCheckStatus]'))
        SET IDENTITY_INSERT [Lookup.SiteCheckStatus] ON;
    EXEC(N'INSERT INTO [Lookup.SiteCheckStatus] ([Id], [Name])
    VALUES (1, N''NotStarted''),
    (2, N''InProgress''),
    (3, N''Completed'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup.SiteCheckStatus]'))
        SET IDENTITY_INSERT [Lookup.SiteCheckStatus] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250721141702_UpdateAccreditationPrns'
)
BEGIN
    CREATE INDEX [IX_Public.AccreditationPrnIssueAuth_AccreditationId] ON [Public.AccreditationPrnIssueAuth] ([AccreditationId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250721141702_UpdateAccreditationPrns'
)
BEGIN
    CREATE INDEX [IX_Public.AccreditationTaskStatus_AccreditationId] ON [Public.AccreditationTaskStatus] ([AccreditationId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250721141702_UpdateAccreditationPrns'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Public.AccreditationTaskStatus_ExternalId] ON [Public.AccreditationTaskStatus] ([ExternalId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250721141702_UpdateAccreditationPrns'
)
BEGIN
    CREATE INDEX [IX_Public.AccreditationTaskStatus_TaskId] ON [Public.AccreditationTaskStatus] ([TaskId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250721141702_UpdateAccreditationPrns'
)
BEGIN
    CREATE INDEX [IX_Public.AccreditationTaskStatus_TaskStatusId] ON [Public.AccreditationTaskStatus] ([TaskStatusId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250721141702_UpdateAccreditationPrns'
)
BEGIN
    CREATE INDEX [IX_Public.OverseasAccreditationSite_AccreditationId] ON [Public.OverseasAccreditationSite] ([AccreditationId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250721141702_UpdateAccreditationPrns'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Public.OverseasAccreditationSite_ExternalId] ON [Public.OverseasAccreditationSite] ([ExternalId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250721141702_UpdateAccreditationPrns'
)
BEGIN
    CREATE INDEX [IX_Public.OverseasAccreditationSite_MeetConditionsOfExportId] ON [Public.OverseasAccreditationSite] ([MeetConditionsOfExportId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250721141702_UpdateAccreditationPrns'
)
BEGIN
    CREATE INDEX [IX_Public.OverseasAccreditationSite_OverseasAddressId] ON [Public.OverseasAccreditationSite] ([OverseasAddressId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250721141702_UpdateAccreditationPrns'
)
BEGIN
    CREATE INDEX [IX_Public.OverseasAccreditationSite_SiteCheckStatusId] ON [Public.OverseasAccreditationSite] ([SiteCheckStatusId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250721141702_UpdateAccreditationPrns'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250721141702_UpdateAccreditationPrns', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250721155337_UpdateAccreditationRefLength'
)
BEGIN
    DECLARE @var50 sysname;
    SELECT @var50 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Public.Accreditation]') AND [c].[name] = N'ApplicationReferenceNumber');
    IF @var50 IS NOT NULL EXEC(N'ALTER TABLE [Public.Accreditation] DROP CONSTRAINT [' + @var50 + '];');
    ALTER TABLE [Public.Accreditation] ALTER COLUMN [ApplicationReferenceNumber] nvarchar(18) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250721155337_UpdateAccreditationRefLength'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250721155337_UpdateAccreditationRefLength', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250714163811_InterimOverSeasConnectionRelationships'
)
BEGIN
    DECLARE @var46 sysname;
    SELECT @var46 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Public.OverseasAddress]') AND [c].[name] = N'SiteCoordinates');
    IF @var46 IS NOT NULL EXEC(N'ALTER TABLE [Public.OverseasAddress] DROP CONSTRAINT [' + @var46 + '];');
    ALTER TABLE [Public.OverseasAddress] ALTER COLUMN [SiteCoordinates] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250714163811_InterimOverSeasConnectionRelationships'
)
BEGIN
    CREATE INDEX [IX_Public.InterimOverseasConnections_InterimSiteId] ON [Public.InterimOverseasConnections] ([InterimSiteId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250714163811_InterimOverSeasConnectionRelationships'
)
BEGIN
    ALTER TABLE [Public.InterimOverseasConnections] ADD CONSTRAINT [FK_Public.InterimOverseasConnections_Public.OverseasAddress_InterimSiteId] FOREIGN KEY ([InterimSiteId]) REFERENCES [Public.OverseasAddress] ([Id]) ON DELETE NO ACTION;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250714163811_InterimOverSeasConnectionRelationships'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250714163811_InterimOverSeasConnectionRelationships', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250714164749_IsInterimSiteFlag'
)
BEGIN
    ALTER TABLE [Public.OverseasAddress] ADD [IsInterimSite] bit NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250714164749_IsInterimSiteFlag'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250714164749_IsInterimSiteFlag', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250717100928_UpdateOverseasTables'
)
BEGIN
    ALTER TABLE [Public.OverseasAddressWasteCode] DROP CONSTRAINT [FK_Public.OverseasAddressWasteCode_Public.OverseasAddress_OverseasAddressId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250717100928_UpdateOverseasTables'
)
BEGIN
    ALTER TABLE [Public.OverseasAddressWasteCode] DROP CONSTRAINT [PK_Public.OverseasAddressWasteCode];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250717100928_UpdateOverseasTables'
)
BEGIN
    EXEC sp_rename N'[Public.OverseasAddressWasteCode]', N'Public.OverseasAddressWasteCodes';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250717100928_UpdateOverseasTables'
)
BEGIN
    EXEC sp_rename N'[Public.OverseasAddressWasteCodes].[IX_Public.OverseasAddressWasteCode_OverseasAddressId]', N'IX_Public.OverseasAddressWasteCodes_OverseasAddressId', N'INDEX';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250717100928_UpdateOverseasTables'
)
BEGIN
    EXEC sp_rename N'[Public.OverseasAddressWasteCodes].[IX_Public.OverseasAddressWasteCode_ExternalId]', N'IX_Public.OverseasAddressWasteCodes_ExternalId', N'INDEX';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250717100928_UpdateOverseasTables'
)
BEGIN
    ALTER TABLE [Public.OverseasAddressContact] ADD [ExternalId] uniqueidentifier NOT NULL DEFAULT (NEWID());
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250717100928_UpdateOverseasTables'
)
BEGIN
    DECLARE @var47 sysname;
    SELECT @var47 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Public.OverseasAddress]') AND [c].[name] = N'SiteCoordinates');
    IF @var47 IS NOT NULL EXEC(N'ALTER TABLE [Public.OverseasAddress] DROP CONSTRAINT [' + @var47 + '];');
    ALTER TABLE [Public.OverseasAddress] ALTER COLUMN [SiteCoordinates] nvarchar(100) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250717100928_UpdateOverseasTables'
)
BEGIN
    ALTER TABLE [Public.OverseasAddress] ADD [OrganisationId] uniqueidentifier NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250717100928_UpdateOverseasTables'
)
BEGIN
    ALTER TABLE [Public.OverseasAddressWasteCodes] ADD CONSTRAINT [PK_Public.OverseasAddressWasteCodes] PRIMARY KEY ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250717100928_UpdateOverseasTables'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Public.OverseasAddressContact_ExternalId] ON [Public.OverseasAddressContact] ([ExternalId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250717100928_UpdateOverseasTables'
)
BEGIN
    ALTER TABLE [Public.OverseasAddressWasteCodes] ADD CONSTRAINT [FK_Public.OverseasAddressWasteCodes_Public.OverseasAddress_OverseasAddressId] FOREIGN KEY ([OverseasAddressId]) REFERENCES [Public.OverseasAddress] ([Id]) ON DELETE CASCADE;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250717100928_UpdateOverseasTables'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250717100928_UpdateOverseasTables', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250709113024_ResizeOrgName'
)
BEGIN
    DECLARE @var46 sysname;
    SELECT @var46 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Prn]') AND [c].[name] = N'OrganisationName');
    IF @var46 IS NOT NULL EXEC(N'ALTER TABLE [Prn] DROP CONSTRAINT [' + @var46 + '];');
    ALTER TABLE [Prn] ALTER COLUMN [OrganisationName] nvarchar(160) NOT NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250709113024_ResizeOrgName'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250709113024_ResizeOrgName', N'8.0.8');
END;
GO

COMMIT;
GO
