CREATE TABLE [dbo].[MenuPasta]
(
  [Id] INT NOT NULL IDENTITY,
  [AvailableForPurchase] BIT NOT NULL,
  [Name] VARCHAR(100) NOT NULL,
  [Price] DECIMAL(20,2) NOT NULL,
  [Description] VARCHAR(512) NOT NULL,
  [ItemDetails] VARCHAR(512) NOT NULL,
  [HasMenuIcon] BIT NOT NULL,
  [MenuIconFile] VARCHAR(50) NOT NULL,
  PRIMARY KEY ([Id])
);