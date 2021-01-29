CREATE TABLE [dbo].[DeliveryAddress]
(
  [Id] INT NOT NULL IDENTITY,
  [UserId] INT NOT NULL,
  [AddressType] VARCHAR(50) NOT NULL,
  [Name] VARCHAR(50) NOT NULL,
  [StreetAddress] VARCHAR(50) NOT NULL,
  [City] VARCHAR(50) NOT NULL,
  [State] VARCHAR(2) NOT NULL,
  [ZipCode] VARCHAR(5) NOT NULL,
  [PhoneNumber] VARCHAR(10) NOT NULL,
  PRIMARY KEY ([Id]),
  FOREIGN KEY ([UserId]) REFERENCES [dbo].[User](Id)
);