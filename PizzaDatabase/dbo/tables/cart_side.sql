CREATE TABLE [dbo].[cart_side]
(
  [cart_side_id] INT NOT NULL IDENTITY,
  [cart_id] INT NOT NULL,
  [menu_side_id] INT NOT NULL,
  [price_per_item] DECIMAL(20,2) NOT NULL,
  [quantity] INT NOT NULL,
  PRIMARY KEY ([cart_side_id]),
  FOREIGN KEY ([cart_id]) REFERENCES cart(cart_id),
  FOREIGN KEY ([menu_side_id]) REFERENCES menu_side(menu_side_id)
);