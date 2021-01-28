CREATE TABLE [dbo].[MenuWings]
(
  [Id] INT NOT NULL IDENTITY,
  [AvailableForPurchase] BIT NOT NULL,
  [Name] VARCHAR(100) NOT NULL,
  [Price6Piece] DECIMAL(20,2) NOT NULL,
  [Price12Piece] DECIMAL(20,2) NOT NULL,
  [Price18Piece] DECIMAL(20,2) NOT NULL,
  [Description] VARCHAR(512) NOT NULL,
  [HasMenuIcon] BIT NOT NULL,
  [MenuIconFile] VARCHAR(50) NOT NULL,
  PRIMARY KEY ([Id])
);