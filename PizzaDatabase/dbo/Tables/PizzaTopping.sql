CREATE TABLE [dbo].[PizzaTopping]
(
  [Id] INT NOT NULL IDENTITY,
  [PizzaId] INT NOT NULL,
  [ToppingHalf] VARCHAR(50) NOT NULL,
  [ToppingAmount] VARCHAR(50) NOT NULL,
  [MenuPizzaToppingId] INT NOT NULL,
  PRIMARY KEY ([Id]),
  FOREIGN KEY ([PizzaId]) REFERENCES Pizza(Id),
  FOREIGN KEY ([MenuPizzaToppingId]) REFERENCES MenuPizzaTopping(Id)
);