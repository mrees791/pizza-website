CREATE TABLE [dbo].[MenuPizzaTopping]
(
  [Id] INT NOT NULL IDENTITY,
  [AvailableForPurchase] BIT NOT NULL,
  [Name] VARCHAR(100) NOT NULL,
  [PriceLight] DECIMAL(20,2) NOT NULL,
  [PriceRegular] DECIMAL(20,2) NOT NULL,
  [PriceExtra] DECIMAL(20,2) NOT NULL,
  [PizzaToppingType] VARCHAR(50) NOT NULL,
  [Description] VARCHAR(512) NOT NULL,
  [HasMenuIcon] BIT NOT NULL,
  [MenuIconFile] VARCHAR(50) NOT NULL,
  [HasPizzaBuilderImage] BIT NOT NULL,
  [PizzaBuilderImageFile] VARCHAR(50) NOT NULL,
  PRIMARY KEY ([Id])
);