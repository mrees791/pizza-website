CREATE TABLE [dbo].[CartItem]
(
  [Id] INT NOT NULL IDENTITY,
  [CartId] INT NOT NULL,
  [PricePerItem] DECIMAL(20,2) NOT NULL,
  [Quantity] INT NOT NULL
  PRIMARY KEY ([Id]),
  FOREIGN KEY ([CartId]) REFERENCES Cart(Id)
);