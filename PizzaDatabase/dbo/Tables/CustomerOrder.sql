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
  [DeliveryInfoId] INT,
  PRIMARY KEY ([Id]),
  FOREIGN KEY ([UserId]) REFERENCES [dbo].[User](Id),
  FOREIGN KEY ([StoreId]) REFERENCES StoreLocation(Id),
  FOREIGN KEY ([CartId]) REFERENCES Cart(Id),
  FOREIGN KEY ([DeliveryInfoId]) REFERENCES DeliveryInfo(Id)
);