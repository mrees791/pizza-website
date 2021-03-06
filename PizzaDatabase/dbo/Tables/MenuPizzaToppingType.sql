﻿CREATE TABLE dbo.MenuPizzaToppingType
(
  Id INT NOT NULL IDENTITY,
  SortOrder INT NOT NULL,
  AvailableForPurchase BIT NOT NULL,
  Name NVARCHAR(100) NOT NULL,
  PriceLight DECIMAL(20,2) NOT NULL,
  PriceRegular DECIMAL(20,2) NOT NULL,
  PriceExtra DECIMAL(20,2) NOT NULL,
  CategoryName NVARCHAR(50) NOT NULL,
  Description NVARCHAR(512) NOT NULL,
  HasMenuIcon BIT NOT NULL,
  MenuIconFile NVARCHAR(50),
  HasPizzaBuilderImage BIT NOT NULL,
  PizzaBuilderImageFile NVARCHAR(50),
  PRIMARY KEY (Id)
)