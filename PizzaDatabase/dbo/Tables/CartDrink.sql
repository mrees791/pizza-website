CREATE TABLE [dbo].[CartDrink]
(
  [Id] INT NOT NULL IDENTITY,
  [CartId] INT NOT NULL,
  [MenuDrinkId] INT NOT NULL,
  [PricePerItem] DECIMAL(20,2) NOT NULL,
  [Size] VARCHAR(50) NOT NULL,
  [Quantity] INT NOT NULL,
  PRIMARY KEY ([Id]),
  FOREIGN KEY ([CartId]) REFERENCES Cart(Id),
  FOREIGN KEY ([MenuDrinkId]) REFERENCES MenuDrink(Id)
);