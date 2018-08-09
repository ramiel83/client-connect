
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 08/09/2018 20:26:04
-- Generated from EDMX file: C:\Users\ramis\Desktop\רמי\תכנות חדש\client-connect\DataTransfer\ClientConnectModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [ClientConnect];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_SwitchPbxConnection]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PbxConnectionSet] DROP CONSTRAINT [FK_SwitchPbxConnection];
GO
IF OBJECT_ID(N'[dbo].[FK_KolanConnectionSwitch]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[KolanConnectionSet] DROP CONSTRAINT [FK_KolanConnectionSwitch];
GO
IF OBJECT_ID(N'[dbo].[FK_SwitchTelnetConnection]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TelnetConnectionSet] DROP CONSTRAINT [FK_SwitchTelnetConnection];
GO
IF OBJECT_ID(N'[dbo].[FK_SwitchFile]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[FileSet] DROP CONSTRAINT [FK_SwitchFile];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[SwitchSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SwitchSet];
GO
IF OBJECT_ID(N'[dbo].[PbxConnectionSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PbxConnectionSet];
GO
IF OBJECT_ID(N'[dbo].[KolanConnectionSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[KolanConnectionSet];
GO
IF OBJECT_ID(N'[dbo].[TelnetConnectionSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TelnetConnectionSet];
GO
IF OBJECT_ID(N'[dbo].[FileSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[FileSet];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'SwitchSet'
CREATE TABLE [dbo].[SwitchSet] (
    [Id] int  NOT NULL,
    [CrmNum] nvarchar(max)  NULL,
    [Comments] nvarchar(max)  NULL,
    [Name] nvarchar(max)  NOT NULL,
    [SwRelease] nvarchar(max)  NULL,
    [MachineType] nvarchar(max)  NULL,
    [SiteId] int  NULL,
    [Tid] nvarchar(max)  NULL
);
GO

-- Creating table 'PbxConnectionSet'
CREATE TABLE [dbo].[PbxConnectionSet] (
    [SwitchId] int  NOT NULL,
    [DialNum] nvarchar(max)  NOT NULL,
    [LoginName] nvarchar(max)  NULL,
    [LoginPassword] nvarchar(max)  NOT NULL,
    [DebugPassword] nvarchar(max)  NULL,
    [BaudRate] int  NOT NULL,
    [ParDataStop] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'KolanConnectionSet'
CREATE TABLE [dbo].[KolanConnectionSet] (
    [SwitchId] int  NOT NULL,
    [DialNum] nvarchar(max)  NOT NULL,
    [BaudRate] int  NOT NULL,
    [ParDataStop] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'TelnetConnectionSet'
CREATE TABLE [dbo].[TelnetConnectionSet] (
    [SwitchId] int  NOT NULL,
    [IpAddress] nvarchar(max)  NOT NULL,
    [Script] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'FileSet'
CREATE TABLE [dbo].[FileSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [DateTime] datetime  NOT NULL,
    [Content] varbinary(max)  NOT NULL,
    [SwitchId] int  NOT NULL
);
GO

-- Creating table 'UserSet'
CREATE TABLE [dbo].[UserSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Username] nvarchar(max)  NOT NULL,
    [PasswordHash] nvarchar(max)  NOT NULL,
    [AccessLevel] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'SwitchSet'
ALTER TABLE [dbo].[SwitchSet]
ADD CONSTRAINT [PK_SwitchSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [SwitchId] in table 'PbxConnectionSet'
ALTER TABLE [dbo].[PbxConnectionSet]
ADD CONSTRAINT [PK_PbxConnectionSet]
    PRIMARY KEY CLUSTERED ([SwitchId] ASC);
GO

-- Creating primary key on [SwitchId] in table 'KolanConnectionSet'
ALTER TABLE [dbo].[KolanConnectionSet]
ADD CONSTRAINT [PK_KolanConnectionSet]
    PRIMARY KEY CLUSTERED ([SwitchId] ASC);
GO

-- Creating primary key on [SwitchId] in table 'TelnetConnectionSet'
ALTER TABLE [dbo].[TelnetConnectionSet]
ADD CONSTRAINT [PK_TelnetConnectionSet]
    PRIMARY KEY CLUSTERED ([SwitchId] ASC);
GO

-- Creating primary key on [Id] in table 'FileSet'
ALTER TABLE [dbo].[FileSet]
ADD CONSTRAINT [PK_FileSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'UserSet'
ALTER TABLE [dbo].[UserSet]
ADD CONSTRAINT [PK_UserSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [SwitchId] in table 'PbxConnectionSet'
ALTER TABLE [dbo].[PbxConnectionSet]
ADD CONSTRAINT [FK_SwitchPbxConnection]
    FOREIGN KEY ([SwitchId])
    REFERENCES [dbo].[SwitchSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [SwitchId] in table 'KolanConnectionSet'
ALTER TABLE [dbo].[KolanConnectionSet]
ADD CONSTRAINT [FK_KolanConnectionSwitch]
    FOREIGN KEY ([SwitchId])
    REFERENCES [dbo].[SwitchSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [SwitchId] in table 'TelnetConnectionSet'
ALTER TABLE [dbo].[TelnetConnectionSet]
ADD CONSTRAINT [FK_SwitchTelnetConnection]
    FOREIGN KEY ([SwitchId])
    REFERENCES [dbo].[SwitchSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [SwitchId] in table 'FileSet'
ALTER TABLE [dbo].[FileSet]
ADD CONSTRAINT [FK_SwitchFile]
    FOREIGN KEY ([SwitchId])
    REFERENCES [dbo].[SwitchSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SwitchFile'
CREATE INDEX [IX_FK_SwitchFile]
ON [dbo].[FileSet]
    ([SwitchId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------