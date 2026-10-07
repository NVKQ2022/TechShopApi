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
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261007055428_InitialCreate'
)
BEGIN
    CREATE TABLE [USER_FCM] (
        [USER_ID] int NOT NULL,
        [FCM_TOKEN] nvarchar(255) NOT NULL,
        CONSTRAINT [PK_USER_FCM] PRIMARY KEY ([USER_ID])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261007055428_InitialCreate'
)
BEGIN
    CREATE TABLE [userId] (
        [ID] int NOT NULL,
        CONSTRAINT [PK_userId] PRIMARY KEY ([ID])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261007055428_InitialCreate'
)
BEGIN
    CREATE TABLE [Users] (
        [Id] int NOT NULL,
        [Email] nvarchar(450) NOT NULL,
        [Username] nvarchar(450) NOT NULL,
        [Password] nvarchar(max) NOT NULL,
        [Salt] nvarchar(max) NOT NULL,
        [IsAdmin] bit NOT NULL,
        [IsEmailVerified] bit NOT NULL,
        [GOOGLE_ID] nvarchar(max) NULL,
        CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261007055428_InitialCreate'
)
BEGIN
    CREATE TABLE [VERIFICATION_CODES] (
        [ID] uniqueidentifier NOT NULL,
        [USER_ID] int NOT NULL,
        [EMAIL] nvarchar(255) NOT NULL,
        [CODE] nvarchar(100) NOT NULL,
        [TYPE] nvarchar(50) NOT NULL,
        [EXPIRES_AT] datetime2 NOT NULL,
        [IS_USED] bit NOT NULL,
        [CREATED_AT] datetime2 NOT NULL,
        [USED_AT] datetime2 NULL,
        CONSTRAINT [PK_VERIFICATION_CODES] PRIMARY KEY ([ID])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261007055428_InitialCreate'
)
BEGIN
    CREATE TABLE [auth_providers] (
        [id] uniqueidentifier NOT NULL,
        [user_id] int NOT NULL,
        [provider] nvarchar(50) NOT NULL,
        [provider_user_id] nvarchar(255) NOT NULL,
        [provider_email] nvarchar(255) NULL,
        [access_token] nvarchar(max) NULL,
        [refresh_token] nvarchar(max) NULL,
        [token_expires_at] datetime2 NULL,
        [created_at] datetime2 NULL,
        [updated_at] datetime2 NULL,
        CONSTRAINT [PK_auth_providers] PRIMARY KEY ([id]),
        CONSTRAINT [FK_auth_providers_Users_user_id] FOREIGN KEY ([user_id]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261007055428_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ID') AND [object_id] = OBJECT_ID(N'[userId]'))
        SET IDENTITY_INSERT [userId] ON;
    EXEC(N'INSERT INTO [userId] ([ID])
    VALUES (10000)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ID') AND [object_id] = OBJECT_ID(N'[userId]'))
        SET IDENTITY_INSERT [userId] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261007055428_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_auth_providers_user_id] ON [auth_providers] ([user_id]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261007055428_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Users_Email] ON [Users] ([Email]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261007055428_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Users_Username] ON [Users] ([Username]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261007055428_InitialCreate'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20261007055428_InitialCreate', N'9.0.9');
END;

COMMIT;
GO

