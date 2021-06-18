﻿CREATE TABLE dbo.MenuPizzaSauce
(
  Id INT NOT NULL IDENTITY,
  SortOrder INT NOT NULL,
  AvailableForPurchase BIT NOT NULL,
  Name NVARCHAR(100) NOT NULL,
  PriceLight DECIMAL(20,2) NOT NULL,
  PriceRegular DECIMAL(20,2) NOT NULL,
  PriceExtra DECIMAL(20,2) NOT NULL,
  Description NVARCHAR(512) NOT NULL,
  PRIMARY KEY (Id)
)