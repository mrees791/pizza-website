﻿CREATE TABLE dbo.CartItem
(
  Id INT NOT NULL IDENTITY,
  CartId INT NOT NULL,
  UserId NVARCHAR(256) NOT NULL,
  Price DECIMAL(20,2) NOT NULL,
  PricePerItem DECIMAL(20,2) NOT NULL,
  Quantity INT NOT NULL,
  ProductCategory NVARCHAR(50) NOT NULL,
  PRIMARY KEY (Id),
  CONSTRAINT FK_CartItem_Cart FOREIGN KEY (CartId) REFERENCES Cart(Id),
  CONSTRAINT FK_CartItem_SiteUser FOREIGN KEY (UserId) REFERENCES SiteUser(Id)
)