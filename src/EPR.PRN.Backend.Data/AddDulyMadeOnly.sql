BEGIN TRANSACTION;
GO

ALTER TABLE [Public.RegistrationMaterial] ADD [ApplicationReferenceNumber] nvarchar(20) NULL;
GO

ALTER TABLE [Public.RegistrationMaterial] ADD [CreatedDate] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';
GO

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
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ApplicationTypeId', N'IsMaterialSpecific', N'JourneyTypeId', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup.RegulatorTask]'))
    SET IDENTITY_INSERT [Lookup.RegulatorTask] ON;
INSERT INTO [Lookup.RegulatorTask] ([Id], [ApplicationTypeId], [IsMaterialSpecific], [JourneyTypeId], [Name])
VALUES (15, 1, CAST(1 AS bit), 1, N'CheckRegistrationStatus'),
(16, 2, CAST(1 AS bit), 1, N'CheckRegistrationStatus');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ApplicationTypeId', N'IsMaterialSpecific', N'JourneyTypeId', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup.RegulatorTask]'))
    SET IDENTITY_INSERT [Lookup.RegulatorTask] OFF;
GO

CREATE INDEX [IX_Public.DulyMade_RegistrationMaterialId] ON [Public.DulyMade] ([RegistrationMaterialId]);
GO

CREATE INDEX [IX_Public.DulyMade_TaskStatusId] ON [Public.DulyMade] ([TaskStatusId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250513183016_Adddulymadetable2', N'8.0.8');
GO

COMMIT;
GO

