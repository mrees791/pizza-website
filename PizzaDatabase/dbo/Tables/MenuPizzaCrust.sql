CREATE TABLE [dbo].[MenuPizzaCrust]
(
  [Id] INT NOT NULL IDENTITY,
  [AvailableForPurchase] BIT NOT NULL,
  [Name] VARCHAR(100) NOT NULL,
  [PriceSmall] DECIMAL(20,2) NOT NULL,
  [PriceMedium] DECIMAL(20,2) NOT NULL,
  [PriceLarge] DECIMAL(20,2) NOT NULL,
  [Description] VARCHAR(512) NOT NULL,
  [HasMenuIcon] BIT NOT NULL,
  [MenuIconFile] VARCHAR(50) NOT NULL,
  [HasPizzaBuilderImage] BIT NOT NULL,
  [PizzaBuilderImageFile] VARCHAR(50) NOT NULL,
  PRIMARY KEY ([Id])
);