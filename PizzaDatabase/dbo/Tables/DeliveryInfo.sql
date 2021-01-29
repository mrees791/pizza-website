CREATE TABLE [dbo].[DeliveryInfo]
(
  [Id] INT NOT NULL IDENTITY,
  [DateOfDelivery] DATETIME NOT NULL,
  [DeliveryAddressType] VARCHAR(50) NOT NULL,
  [DeliveryAddressName] VARCHAR(50) NOT NULL,
  [DeliveryStreetAddress] VARCHAR(50) NOT NULL,
  [DeliveryCity] VARCHAR(50) NOT NULL,
  [DeliveryState] VARCHAR(2) NOT NULL,
  [DeliveryZipCode] VARCHAR(5) NOT NULL,
  [DeliveryPhoneNumber] VARCHAR(10) NOT NULL,
  PRIMARY KEY ([Id])
);