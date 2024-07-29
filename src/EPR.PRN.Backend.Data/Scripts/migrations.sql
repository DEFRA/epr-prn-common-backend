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
    VALUES (N'20240717145045_InitialCreate', N'8.0.7');
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
    VALUES (N'20240728110950_AddedEnumForStatus', N'8.0.7');
END;
GO

COMMIT;
GO

