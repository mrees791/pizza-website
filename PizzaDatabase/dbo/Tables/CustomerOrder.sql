CREATE TABLE [dbo].[CustomerOrder]
(
  [Id] INT NOT NULL IDENTITY,
  [UserId] INT NOT NULL,
  [StoreId] INT NOT NULL,
  [CartId] INT NOT NULL,
  [OrderSubtotal] DECIMAL(20,2) NOT NULL,
  [OrderTax] DECIMAL(20,2) NOT NULL,
  [OrderTotal] DECIMAL(20,2) NOT NULL,
  [OrderPhase] INT NOT NULL,
  [OrderCompleted] BIT NOT NULL,
  [DateOfOrder] DATETIME NOT NULL,
  [IsDelivery] BIT NOT NULL,
  [DateOfDelivery] DATETIME,
  [DeliveryAddressType] VARCHAR(50),
  [DeliveryAddressName] VARCHAR(50),
  [DeliveryStreetAddress] VARCHAR(50),
  [DeliveryCity] VARCHAR(50),
  [DeliveryState] VARCHAR(2),
  [DeliveryZipCode] VARCHAR(5),
  [DeliveryPhoneNumber] VARCHAR(10),
  PRIMARY KEY ([Id])
);