CREATE TABLE [dbo].[cart_drink]
(
  [cart_drink_id] INT NOT NULL IDENTITY,
  [cart_id] INT NOT NULL,
  [menu_drink_id] INT NOT NULL,
  [price_per_item] DECIMAL(20,2) NOT NULL,
  [size] VARCHAR(50) NOT NULL,
  [quantity] INT NOT NULL,
  PRIMARY KEY ([cart_drink_id]),
  FOREIGN KEY ([cart_id]) REFERENCES cart(cart_id),
  FOREIGN KEY ([menu_drink_id]) REFERENCES menu_drink(menu_drink_id)
);