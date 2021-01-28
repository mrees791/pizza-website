CREATE TABLE [dbo].[MenuDrink]
(
  [Id] INT NOT NULL IDENTITY,
  [AvailableForPurchase] BIT NOT NULL,
  [Name] VARCHAR(100) NOT NULL,
  [AvailableIn20Oz] BIT NOT NULL,
  [AvailableIn2Liter] BIT NOT NULL,
  [AvailableIn2Pack12Oz] BIT NOT NULL,
  [AvailableIn6Pack12Oz] BIT NOT NULL,
  [Price20Oz] DECIMAL(20,2) NOT NULL,
  [Price2Liter] DECIMAL(20,2) NOT NULL,
  [Price2Pack12Oz] DECIMAL(20,2) NOT NULL,
  [Price6Pack12Oz] DECIMAL(20,2) NOT NULL,
  [Description] VARCHAR(512) NOT NULL,
  [HasMenuIcon] BIT NOT NULL,
  [MenuIconFile] VARCHAR(50) NOT NULL,
  PRIMARY KEY ([Id])
);