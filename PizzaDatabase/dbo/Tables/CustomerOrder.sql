﻿CREATE TABLE dbo.CustomerOrder
(
  Id INT NOT NULL IDENTITY,
  UserId NVARCHAR(256) NOT NULL,
  StoreId INT NOT NULL,
  CartId INT NOT NULL,
  OrderSubtotal DECIMAL(20,2) NOT NULL,
  OrderTax DECIMAL(20,2) NOT NULL,
  OrderTotal DECIMAL(20,2) NOT NULL,
  OrderStatus INT NOT NULL,
  DateOrderPlaced DATETIME NOT NULL,
  DateOrderCompleted DATETIME,
  CustomerFirstName NVARCHAR(100),
  CustomerLastName NVARCHAR(100),
  CustomerPhoneNumber NVARCHAR(MAX),
  IsDelivery BIT NOT NULL,
  DeliveryInfoId INT,
  PRIMARY KEY (Id),
  CONSTRAINT FK_CustomerOrder_SiteUser FOREIGN KEY (UserId) REFERENCES SiteUser(Id),
  CONSTRAINT FK_CustomerOrder_StoreLocation FOREIGN KEY (StoreId) REFERENCES StoreLocation(Id),
  CONSTRAINT FK_CustomerOrder_Cart FOREIGN KEY (CartId) REFERENCES Cart(Id),
  CONSTRAINT FK_CustomerOrder_DeliveryInfo FOREIGN KEY (DeliveryInfoId) REFERENCES DeliveryInfo(Id)
)