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
    DECLARE @var27 sysname;
    SELECT @var27 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Public.DulyMade]') AND [c].[name] = N'DeterminationNote');
    IF @var27 IS NOT NULL EXEC(N'ALTER TABLE [Public.DulyMade] DROP CONSTRAINT [' + @var27 + '];');
    ALTER TABLE [Public.DulyMade] DROP COLUMN [DeterminationNote];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250612110818_RemoveunusedDulyMadeFields'
)
BEGIN
    DECLARE @var28 sysname;
    SELECT @var28 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Public.DulyMade]') AND [c].[name] = N'DeterminationUpdatedBy');
    IF @var28 IS NOT NULL EXEC(N'ALTER TABLE [Public.DulyMade] DROP CONSTRAINT [' + @var28 + '];');
    ALTER TABLE [Public.DulyMade] DROP COLUMN [DeterminationUpdatedBy];
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
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Public.DulyMade]') AND [c].[name] = N'DeterminationUpdatedDate');
    IF @var29 IS NOT NULL EXEC(N'ALTER TABLE [Public.DulyMade] DROP CONSTRAINT [' + @var29 + '];');
    ALTER TABLE [Public.DulyMade] DROP COLUMN [DeterminationUpdatedDate];
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
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Public.DulyMade]') AND [c].[name] = N'DulyMadeNote');
    IF @var30 IS NOT NULL EXEC(N'ALTER TABLE [Public.DulyMade] DROP CONSTRAINT [' + @var30 + '];');
    ALTER TABLE [Public.DulyMade] DROP COLUMN [DulyMadeNote];
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
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Public.DulyMade]') AND [c].[name] = N'TaskStatusId');
    IF @var31 IS NOT NULL EXEC(N'ALTER TABLE [Public.DulyMade] DROP CONSTRAINT [' + @var31 + '];');
    ALTER TABLE [Public.DulyMade] DROP COLUMN [TaskStatusId];
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
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Public.DulyMade]') AND [c].[name] = N'DulyMadeDate');
    IF @var32 IS NOT NULL EXEC(N'ALTER TABLE [Public.DulyMade] DROP CONSTRAINT [' + @var32 + '];');
    EXEC(N'UPDATE [Public.DulyMade] SET [DulyMadeDate] = ''0001-01-01T00:00:00.0000000'' WHERE [DulyMadeDate] IS NULL');
    ALTER TABLE [Public.DulyMade] ALTER COLUMN [DulyMadeDate] datetime2 NOT NULL;
    ALTER TABLE [Public.DulyMade] ADD DEFAULT '0001-01-01T00:00:00.0000000' FOR [DulyMadeDate];
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
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Public.DulyMade]') AND [c].[name] = N'DulyMadeBy');
    IF @var33 IS NOT NULL EXEC(N'ALTER TABLE [Public.DulyMade] DROP CONSTRAINT [' + @var33 + '];');
    EXEC(N'UPDATE [Public.DulyMade] SET [DulyMadeBy] = ''00000000-0000-0000-0000-000000000000'' WHERE [DulyMadeBy] IS NULL');
    ALTER TABLE [Public.DulyMade] ALTER COLUMN [DulyMadeBy] uniqueidentifier NOT NULL;
    ALTER TABLE [Public.DulyMade] ADD DEFAULT '00000000-0000-0000-0000-000000000000' FOR [DulyMadeBy];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250612110818_RemoveunusedDulyMadeFields'
)
BEGIN
    DECLARE @var34 sysname;
    SELECT @var34 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Public.DeterminationDate]') AND [c].[name] = N'DeterminateDate');
    IF @var34 IS NOT NULL EXEC(N'ALTER TABLE [Public.DeterminationDate] DROP CONSTRAINT [' + @var34 + '];');
    EXEC(N'UPDATE [Public.DeterminationDate] SET [DeterminateDate] = ''0001-01-01T00:00:00.0000000'' WHERE [DeterminateDate] IS NULL');
    ALTER TABLE [Public.DeterminationDate] ALTER COLUMN [DeterminateDate] datetime2 NOT NULL;
    ALTER TABLE [Public.DeterminationDate] ADD DEFAULT '0001-01-01T00:00:00.0000000' FOR [DeterminateDate];
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

