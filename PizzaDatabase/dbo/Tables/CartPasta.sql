﻿CREATE TABLE [dbo].[CartPasta]
(
  [Id] INT NOT NULL IDENTITY,
  [CartId] INT NOT NULL,
  [MenuPastaId] INT NOT NULL,
  [PricePerItem] DECIMAL(20,2) NOT NULL,
  [Quantity] INT NOT NULL,
  [DateAddedToCart] DATETIME NOT NULL,
  PRIMARY KEY ([Id]),
  FOREIGN KEY ([CartId]) REFERENCES Cart(Id),
  FOREIGN KEY ([MenuPastaId]) REFERENCES MenuPasta(Id)
);