CREATE TABLE [dbo].[CartPizza]
(
  [Id] INT NOT NULL IDENTITY,
  [CartId] INT NOT NULL,
  [PizzaId] INT NOT NULL,
  [PricePerItem] DECIMAL(20,2) NOT NULL,
  [Quantity] INT NOT NULL,
  PRIMARY KEY ([Id])
);