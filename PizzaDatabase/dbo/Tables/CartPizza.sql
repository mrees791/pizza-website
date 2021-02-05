CREATE TABLE [dbo].[CartPizza]
(
  [Id] INT NOT NULL IDENTITY,
  [CartItemId] INT NOT NULL,
  [PizzaId] INT NOT NULL,
  PRIMARY KEY ([Id]),
  FOREIGN KEY (CartItemId) REFERENCES CartItem(Id),
  FOREIGN KEY (PizzaId) REFERENCES Pizza(Id)
);