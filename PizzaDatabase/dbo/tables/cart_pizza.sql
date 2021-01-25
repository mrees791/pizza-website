CREATE TABLE [dbo].[cart_pizza]
(
  [cart_pizza_id] INT NOT NULL IDENTITY,
  [cart_id] INT NOT NULL,
  [pizza_id] INT NOT NULL,
  [price_per_item] DECIMAL(20,2) NOT NULL,
  [quantity] INT NOT NULL,
  PRIMARY KEY ([cart_pizza_id]),
  FOREIGN KEY ([cart_id]) REFERENCES cart(cart_id),
  FOREIGN KEY ([pizza_id]) REFERENCES pizza(pizza_id)
);