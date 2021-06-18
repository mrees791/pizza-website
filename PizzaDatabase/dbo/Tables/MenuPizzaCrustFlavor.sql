CREATE TABLE dbo.MenuPizzaCrustFlavor
(
  Id INT NOT NULL IDENTITY,
  SortOrder INT NOT NULL,
  AvailableForPurchase BIT NOT NULL,
  Name NVARCHAR(100) NOT NULL,
  Description NVARCHAR(512) NOT NULL,
  PRIMARY KEY (Id)
)