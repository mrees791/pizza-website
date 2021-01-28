CREATE TABLE [dbo].[CartWings]
(
  [Id] INT NOT NULL IDENTITY,
  [CartId] INT NOT NULL,
  [MenuWingsId] INT NOT NULL,
  [MenuWingsSauceId] INT NOT NULL,
  [PieceAmount] INT NOT NULL,
  [PricePerItem] DECIMAL(20,2) NOT NULL,
  [Quantity] INT NOT NULL,
  PRIMARY KEY ([Id])
);