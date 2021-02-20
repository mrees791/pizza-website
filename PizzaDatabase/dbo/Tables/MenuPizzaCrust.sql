CREATE TABLE dbo.MenuPizzaCrust
(
  Id INT NOT NULL IDENTITY,
  SortOrder INT NOT NULL,
  AvailableForPurchase BIT NOT NULL,
  Name NVARCHAR(100) NOT NULL,
  PriceSmall DECIMAL(20,2) NOT NULL,
  PriceMedium DECIMAL(20,2) NOT NULL,
  PriceLarge DECIMAL(20,2) NOT NULL,
  Description NVARCHAR(512) NOT NULL,
  HasMenuIcon BIT NOT NULL,
  MenuIconFile NVARCHAR(50) NOT NULL,
  HasPizzaBuilderImage BIT NOT NULL,
  PizzaBuilderImageFile NVARCHAR(50) NOT NULL,
  PRIMARY KEY (Id)
)