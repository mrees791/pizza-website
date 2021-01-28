CREATE TABLE [dbo].[StoreLocation]
(
  [Id] INT NOT NULL IDENTITY,
  [Name] VARCHAR(50) NOT NULL,
  [StreetAddress] VARCHAR(50) NOT NULL,
  [City] VARCHAR(50) NOT NULL,
  [State] VARCHAR(2) NOT NULL,
  [ZipCode] VARCHAR(5) NOT NULL,
  [PhoneNumber] VARCHAR(10) NOT NULL,
  [IsActiveLocation] BIT NOT NULL,
  PRIMARY KEY ([Id])
);