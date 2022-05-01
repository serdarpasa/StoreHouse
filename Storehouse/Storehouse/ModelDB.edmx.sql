
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 01/18/2022 17:10:14
-- Generated from EDMX file: F:\Ivan\Storehouse20220116\Storehouse\Storehouse\ModelDB.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [Storehouse];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_Goods_Categories]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Goods] DROP CONSTRAINT [FK_Goods_Categories];
GO
IF OBJECT_ID(N'[dbo].[FK_TransactionDetails_Goods]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TransactionDetails] DROP CONSTRAINT [FK_TransactionDetails_Goods];
GO
IF OBJECT_ID(N'[dbo].[FK_TransactionDetails_Transactions]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TransactionDetails] DROP CONSTRAINT [FK_TransactionDetails_Transactions];
GO
IF OBJECT_ID(N'[dbo].[FK_Transactions_Employeers]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Transactions] DROP CONSTRAINT [FK_Transactions_Employeers];
GO
IF OBJECT_ID(N'[dbo].[FK_Transactions_User]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Transactions] DROP CONSTRAINT [FK_Transactions_User];
GO
IF OBJECT_ID(N'[dbo].[FK_Users_Departments]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Employeers] DROP CONSTRAINT [FK_Users_Departments];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Categories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Categories];
GO
IF OBJECT_ID(N'[dbo].[Departments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Departments];
GO
IF OBJECT_ID(N'[dbo].[Employeers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Employeers];
GO
IF OBJECT_ID(N'[dbo].[Goods]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Goods];
GO
IF OBJECT_ID(N'[dbo].[TransactionDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TransactionDetails];
GO
IF OBJECT_ID(N'[dbo].[Transactions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Transactions];
GO
IF OBJECT_ID(N'[StorehouseModelStoreContainer].[Balance]', 'U') IS NOT NULL
    DROP TABLE [StorehouseModelStoreContainer].[Balance];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Departments'
CREATE TABLE [dbo].[Departments] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(80)  NULL,
    [Floor] nvarchar(50)  NULL,
    [Number] int  NULL
);
GO

-- Creating table 'Categories'
CREATE TABLE [dbo].[Categories] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(150)  NULL
);
GO

-- Creating table 'Goods'
CREATE TABLE [dbo].[Goods] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(80)  NULL,
    [Code] nvarchar(50)  NULL,
    [CategoryId] int  NULL,
    [Unit] nvarchar(20)  NULL,
    [Price] decimal(19,4)  NOT NULL
);
GO

-- Creating table 'TransactionDetails'
CREATE TABLE [dbo].[TransactionDetails] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [TransactionId] int  NOT NULL,
    [GoodsId] int  NOT NULL,
    [Amount] real  NOT NULL
);
GO

-- Creating table 'Employeers'
CREATE TABLE [dbo].[Employeers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [LastName] nvarchar(100)  NULL,
    [FirstName] nvarchar(50)  NULL,
    [MiddleName] nvarchar(50)  NULL,
    [Password] nvarchar(50)  NULL,
    [Post] nvarchar(250)  NULL,
    [DepartmentId] int  NULL,
    [Phone] nvarchar(50)  NULL,
    [Email] nvarchar(50)  NULL,
    [Role] int  NOT NULL,
    [IsLocked] bit  NOT NULL
);
GO

-- Creating table 'Balances'
CREATE TABLE [dbo].[Balances] (
    [Type] int  NOT NULL,
    [GoodsId] int  NOT NULL,
    [Amount] real  NOT NULL,
    [Code] nvarchar(50)  NULL,
    [CategoryId] int  NULL,
    [Unit] nvarchar(20)  NULL,
    [Price] decimal(19,4)  NOT NULL,
    [TotalPrice] real  NULL,
    [Name] nvarchar(80)  NULL,
    [EmployeerId] int  NOT NULL,
    [Date] datetime  NULL,
    [CategoryName] nvarchar(150)  NULL,
    [DepartmentId] int  NULL
);
GO

-- Creating table 'Transactions'
CREATE TABLE [dbo].[Transactions] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Date] datetime  NULL,
    [UserId] int  NOT NULL,
    [Type] int  NOT NULL,
    [EmployeerId] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Departments'
ALTER TABLE [dbo].[Departments]
ADD CONSTRAINT [PK_Departments]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Categories'
ALTER TABLE [dbo].[Categories]
ADD CONSTRAINT [PK_Categories]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Goods'
ALTER TABLE [dbo].[Goods]
ADD CONSTRAINT [PK_Goods]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TransactionDetails'
ALTER TABLE [dbo].[TransactionDetails]
ADD CONSTRAINT [PK_TransactionDetails]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Employeers'
ALTER TABLE [dbo].[Employeers]
ADD CONSTRAINT [PK_Employeers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Type], [GoodsId], [Amount], [Price], [EmployeerId] in table 'Balances'
ALTER TABLE [dbo].[Balances]
ADD CONSTRAINT [PK_Balances]
    PRIMARY KEY CLUSTERED ([Type], [GoodsId], [Amount], [Price], [EmployeerId] ASC);
GO

-- Creating primary key on [Id] in table 'Transactions'
ALTER TABLE [dbo].[Transactions]
ADD CONSTRAINT [PK_Transactions]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [CategoryId] in table 'Goods'
ALTER TABLE [dbo].[Goods]
ADD CONSTRAINT [FK_Goods_Categories]
    FOREIGN KEY ([CategoryId])
    REFERENCES [dbo].[Categories]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Goods_Categories'
CREATE INDEX [IX_FK_Goods_Categories]
ON [dbo].[Goods]
    ([CategoryId]);
GO

-- Creating foreign key on [GoodsId] in table 'TransactionDetails'
ALTER TABLE [dbo].[TransactionDetails]
ADD CONSTRAINT [FK_TransactionDetails_Goods]
    FOREIGN KEY ([GoodsId])
    REFERENCES [dbo].[Goods]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TransactionDetails_Goods'
CREATE INDEX [IX_FK_TransactionDetails_Goods]
ON [dbo].[TransactionDetails]
    ([GoodsId]);
GO

-- Creating foreign key on [DepartmentId] in table 'Employeers'
ALTER TABLE [dbo].[Employeers]
ADD CONSTRAINT [FK_Users_Departments]
    FOREIGN KEY ([DepartmentId])
    REFERENCES [dbo].[Departments]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Users_Departments'
CREATE INDEX [IX_FK_Users_Departments]
ON [dbo].[Employeers]
    ([DepartmentId]);
GO

-- Creating foreign key on [EmployeerId] in table 'Transactions'
ALTER TABLE [dbo].[Transactions]
ADD CONSTRAINT [FK_Transactions_Employeers]
    FOREIGN KEY ([EmployeerId])
    REFERENCES [dbo].[Employeers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Transactions_Employeers'
CREATE INDEX [IX_FK_Transactions_Employeers]
ON [dbo].[Transactions]
    ([EmployeerId]);
GO

-- Creating foreign key on [UserId] in table 'Transactions'
ALTER TABLE [dbo].[Transactions]
ADD CONSTRAINT [FK_Transactions_User]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[Employeers]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Transactions_User'
CREATE INDEX [IX_FK_Transactions_User]
ON [dbo].[Transactions]
    ([UserId]);
GO

-- Creating foreign key on [TransactionId] in table 'TransactionDetails'
ALTER TABLE [dbo].[TransactionDetails]
ADD CONSTRAINT [FK_TransactionDetails_Transactions]
    FOREIGN KEY ([TransactionId])
    REFERENCES [dbo].[Transactions]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TransactionDetails_Transactions'
CREATE INDEX [IX_FK_TransactionDetails_Transactions]
ON [dbo].[TransactionDetails]
    ([TransactionId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------