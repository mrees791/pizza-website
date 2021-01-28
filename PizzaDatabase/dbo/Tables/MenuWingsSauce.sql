CREATE TABLE [dbo].[MenuWingsSauce]
(
  [Id] INT NOT NULL IDENTITY,
  [AvailableForPurchase] BIT NOT NULL,
  [Name] VARCHAR(100) NOT NULL,
  [Description] VARCHAR(512) NOT NULL,
  PRIMARY KEY ([Id])
);