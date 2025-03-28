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

