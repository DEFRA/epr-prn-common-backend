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

