CREATE TABLE [dbo].[PizzaCategory]
(
  [Id] INT NOT NULL IDENTITY,
  [PizzaId] INT NOT NULL,
  [CategoryName] VARCHAR(50) NOT NULL,
  [AvailableForPurchase] BIT NOT NULL,
  [PizzaName] VARCHAR(100) NOT NULL,
  [Description] VARCHAR(512) NOT NULL,
  PRIMARY KEY ([Id])
);