BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250613113347_Add-table-CarrierBrokerDealerPermits'
)
BEGIN
    CREATE TABLE [Public.CarrierBrokerDealerPermits] (
        [Id] int NOT NULL IDENTITY,
        [ExternalId] uniqueidentifier NOT NULL DEFAULT (NEWID()),
        [RegistrationId] int NOT NULL,
        [WasteCarrierBrokerDealerRegistrstion] varchar(20) NULL,
        [WasteManagementorEnvironmentPermitNumber] varchar(20) NULL,
        [InstallationPermitorPPCNumber] varchar(20) NULL,
        [WasteExemptionReference] varchar(150) NULL,
        [RegisteredWasteCarrierBrokerDealerFlag] bit NULL,
        [CreatedBy] uniqueidentifier NOT NULL,
        [CreatedOn] datetime2 NOT NULL,
        [UpdatedBy] uniqueidentifier NOT NULL,
        [UpdatedOn] datetime2 NULL,
        CONSTRAINT [PK_Public.CarrierBrokerDealerPermits] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Public.CarrierBrokerDealerPermits_Public.Registration_RegistrationId] FOREIGN KEY ([RegistrationId]) REFERENCES [Public.Registration] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250613113347_Add-table-CarrierBrokerDealerPermits'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Public.CarrierBrokerDealerPermits_RegistrationId] ON [Public.CarrierBrokerDealerPermits] ([RegistrationId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250613113347_Add-table-CarrierBrokerDealerPermits'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250613113347_Add-table-CarrierBrokerDealerPermits', N'8.0.8');
END;
GO

COMMIT;
GO

