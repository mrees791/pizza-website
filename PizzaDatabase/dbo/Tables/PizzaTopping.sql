CREATE TABLE [dbo].[PizzaTopping]
(
  [Id] INT NOT NULL IDENTITY,
  [PizzaId] INT NOT NULL,
  [ToppingHalf] VARCHAR(50) NOT NULL,
  [MenuPizzaToppingId] INT NOT NULL,
  PRIMARY KEY ([Id])
);